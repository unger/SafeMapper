namespace SafeMapper.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Reflection;

    public class ReflectionUtils
    {
        public static MemberInfo[] GetPublicFieldsAndProperties(Type type)
        {
            return type
                .GetMembers(BindingFlags.Instance | BindingFlags.Public)
                .Where(mi => mi.MemberType == MemberTypes.Property || mi.MemberType == MemberTypes.Field)
                .ToArray();
        }

        public static MemberSetter[] GetMemberSetters(Type type)
        {
            return GetPublicFieldsAndProperties(type).Select(m => new MemberSetter(m)).ToArray();
        }

        public static MemberGetter[] GetMemberGetters(Type type)
        {
            return GetPublicFieldsAndProperties(type).Select(m => new MemberGetter(m)).ToArray();
        }

        public static MemberGetter GetMemberGetter(Type type, string name, Type returnType = null)
        {
            if (IsStringKeyDictionary(type))
            {
                var itemIndexer = type.GetProperty("Item", new[] { typeof(string) });
                return new MemberGetter(itemIndexer, name);
            }

            if (type == typeof(NameValueCollection))
            {
                if (returnType != null && (IsCollection(returnType) || returnType != typeof(string)))
                {
                    var getValuesMethod = type.GetMethod("GetValues", new[] { typeof(string) });
                    return new MemberGetter(getValuesMethod, name);
                }

                var itemIndexer = type.GetProperty("Item", new[] { typeof(string) });
                return new MemberGetter(itemIndexer, name);
            }

            var propertyInfo = type.GetProperty(name);
            if (propertyInfo != null)
            {
                return new MemberGetter(propertyInfo);
            }

            var fieldInfo = type.GetField(name);
            if (fieldInfo != null)
            {
                return new MemberGetter(fieldInfo);
            }

            return null;
        }

        public static MemberSetter GetMemberSetter(Type type, string name, Type returnType = null)
        {
            if (IsStringKeyDictionary(type))
            {
                var itemIndexer = type.GetProperty("Item", new[] { typeof(string) });
                return new MemberSetter(itemIndexer, name);
            }

            if (type == typeof(NameValueCollection))
            {
                var addMethod = type.GetMethod("Add", new[] { typeof(string), typeof(string) });
                return new MemberSetter(addMethod, name);
            }

            var propertyInfo = type.GetProperty(name);
            if (propertyInfo != null)
            {
                return new MemberSetter(propertyInfo);
            }

            var fieldInfo = type.GetField(name);
            if (fieldInfo != null)
            {
                return new MemberSetter(fieldInfo);
            }

            return null;
        }
        /*
        public static MemberWrapper GetMember(Type type, string name, Type returnType = null)
        {
            if (type.IsGenericType)
            {
                if (IsStringKeyDictionary(type))
                {
                    var itemIndexer = type.GetProperty("Item", new[] { typeof(string) });
                    return new MemberWrapper(name, itemIndexer);
                }
            }
            else if (type == typeof(NameValueCollection))
            {
                var addMethod = type.GetMethod("Add", new[] { typeof(string), typeof(string) });
                if (returnType != null && (IsCollection(returnType) || returnType != typeof(string)))
                {
                    var getValuesMethod = type.GetMethod("GetValues", new[] { typeof(string) });
                    return new MemberWrapper(name, getValuesMethod, addMethod);
                }

                var itemIndexer = type.GetProperty("Item", new[] { typeof(string) });
                return new MemberWrapper(name, itemIndexer, addMethod);
            }

            var propertyInfo = type.GetProperty(name);
            if (propertyInfo != null)
            {
                return new MemberWrapper(propertyInfo);
            }

            var fieldInfo = type.GetField(name);
            if (fieldInfo != null)
            {
                return new MemberWrapper(fieldInfo);
            }

            return null;
        }*/

        public static bool IsCollection(Type type)
        {
            if (type.IsArray)
            {
                return true;
            }

            if (type == typeof(string) || IsDictionary(type))
            {
                return false;
            }

            return ImplementsGenericTypeDefinition(type, typeof(IEnumerable<>));
        }

        public static bool ImplementsGenericTypeDefinition(Type searchType, Type genericTypeDefinition)
        {
            var type = GetTypeWithGenericTypeDefinition(searchType, genericTypeDefinition);
            return type != null;
        }

        public static Type GetTypeWithGenericTypeDefinition(Type searchType, Type genericTypeDefinition)
        {
            if (!genericTypeDefinition.IsGenericTypeDefinition)
            {
                return null;
            }

            var interfaces = new List<Type>(searchType.GetInterfaces());
            interfaces.Insert(0, searchType);

            return interfaces.FirstOrDefault(intType => intType.IsGenericType && intType.GetGenericTypeDefinition() == genericTypeDefinition);
        }

        public static bool IsDictionary(Type type)
        {
            if (type == typeof(NameValueCollection))
            {
                return true;
            }

            return IsStringKeyDictionary(type);
        }

        public static bool IsStringKeyDictionary(Type type)
        {
            var dictType = GetTypeWithGenericTypeDefinition(type, typeof(IDictionary<,>));

            if (dictType != null)
            {
                return dictType.GetGenericArguments()[0] == typeof(string);
            }

            return false;
        }

        public static Type GetElementType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            if (IsCollection(type) && type.IsGenericType)
            {
                var types = type.GetGenericArguments();
                return types.Length > 0 ? types[0] : null;
            }

            return null;
        }

        public static Type GetConcreteType(Type type)
        {
            if (type.IsInterface)
            {
                if (type.IsGenericType)
                {
                    var genericTypeDefinition = type.GetGenericTypeDefinition();
                    var elementType = GetElementType(type);
                    var concreteTypeDefinition = GetConcreteTypeDefinition(genericTypeDefinition);
                    return concreteTypeDefinition.MakeGenericType(elementType);
                }

                return null;
            }

            return type;
        }

        public static Type GetConcreteTypeDefinition(Type typedefinition)
        {
            if (typedefinition.IsInterface)
            {
                if (typedefinition == typeof(IEnumerable<>) || typedefinition == typeof(IList<>))
                {
                    return typeof(List<>);
                }

                if (typedefinition == typeof(ICollection<>))
                {
                    return typeof(Collection<>);
                }

                if (typedefinition == typeof(ISet<>))
                {
                    return typeof(HashSet<>);
                }

                return null;
            }

            return typedefinition;
        }

        public static List<MemberMap> GetMemberMaps(Type fromType, Type toType)
        {
            var result = new List<MemberMap>();

            var fromIsDictionary = IsDictionary(fromType);
            var toIsDictionary = IsDictionary(toType);

            if (fromIsDictionary && toIsDictionary)
            {
                // Do nothing
            }
            else if (fromIsDictionary)
            {
                var toMembers = GetMemberSetters(toType);
                foreach (var toMember in toMembers)
                {
                    var fromMember = GetMemberGetter(fromType, toMember.Name, toMember.Type);
                    if (fromMember != null)
                    {
                        result.Add(new MemberMap(fromMember, toMember));
                    }
                }
            }
            else if (toIsDictionary)
            {
                var fromMembers = GetMemberGetters(fromType);

                foreach (var fromMember in fromMembers)
                {
                    var toMember = GetMemberSetter(toType, fromMember.Name);
                    result.Add(new MemberMap(fromMember, toMember));
                }
            }
            else
            {
                var toMembers = GetMemberSetters(toType);
                var fromMembers = GetMemberGetters(fromType);
                var toMembersDict = toMembers.ToDictionary(m => m.Name);
                foreach (var fromMember in fromMembers)
                {
                    if (toMembersDict.ContainsKey(fromMember.Name))
                    {
                        var toMember = toMembersDict[fromMember.Name];
                        result.Add(new MemberMap(fromMember, toMember));
                    }
                }
            }

            return result;
        }

        public static MethodInfo GetConvertMethod(Type fromType, Type toType, Type[] searchInTypes)
        {
            if (toType.IsAssignableFrom(fromType))
            {
                return null;
            }

            foreach (var convertType in searchInTypes)
            {
                MethodInfo methodInfoWithoutFormatProvider = null;
                var methods = convertType.GetMethods();
                foreach (var method in methods)
                {
                    if (method.ReturnType == toType)
                    {
                        var parameters = method.GetParameters();
                        if (parameters.Length >= 1)
                        {
                            if (parameters[0].ParameterType == fromType)
                            {
                                if (parameters.Length == 2 && parameters[1].ParameterType == typeof(IFormatProvider))
                                {
                                    return method;
                                }

                                if (parameters.Length == 1)
                                {
                                    methodInfoWithoutFormatProvider = method;
                                }
                            }
                        }
                    }
                }

                if (methodInfoWithoutFormatProvider != null)
                {
                    return methodInfoWithoutFormatProvider;
                }
            }

            return null;
        }

        public static Type GetMemberType(MemberInfo member)
        {
            if (member is PropertyInfo)
            {
                return (member as PropertyInfo).PropertyType;
            }

            if (member is FieldInfo)
            {
                return (member as FieldInfo).FieldType;
            }

            if (member is MethodInfo)
            {
                return (member as MethodInfo).ReturnType;
            }

            return typeof(void);
        }

        public static bool CanHaveCircularReference(Type type)
        {
            return CanHaveCircularReferenceRecursive(type, new HashSet<Type>());
        }

        private static bool CanHaveCircularReferenceRecursive(Type type, HashSet<Type> addedTypes)
        {
            var returnValue = false;

            if (addedTypes.Contains(type))
            {
                return true;
            }

            if (IsCollection(type))
            {
                return CanHaveCircularReferenceRecursive(GetElementType(type), new HashSet<Type>(addedTypes));
            }

            if (type.IsGenericType)
            {
                foreach (var genericArg in type.GetGenericArguments())
                {
                    returnValue |= CanHaveCircularReferenceRecursive(genericArg, new HashSet<Type>(addedTypes));
                }
            }

            if (!type.IsValueType && type != typeof(string))
            {
                addedTypes.Add(type);

                foreach (var prop in type.GetProperties())
                {
                    returnValue |= CanHaveCircularReferenceRecursive(prop.PropertyType, new HashSet<Type>(addedTypes));
                }

                foreach (var prop in type.GetFields())
                {
                    returnValue |= CanHaveCircularReferenceRecursive(prop.FieldType, new HashSet<Type>(addedTypes));
                }
            }

            return returnValue;
        }
    }
}
