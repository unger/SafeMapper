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

        public static MemberWrapper[] GetMembers(Type type)
        {
            return GetPublicFieldsAndProperties(type).Select(m => new MemberWrapper(m)).ToArray();
        }

        public static MemberWrapper GetMember(Type type, string name)
        {
            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Dictionary<,>))
                {
                    var genericArguments = type.GetGenericArguments();
                    if (genericArguments[0] == typeof(string))
                    {
                        var itemIndexer = type.GetProperty("Item", new[] { typeof(string) });
                        if (itemIndexer != null)
                        {
                            return new MemberWrapper(name, itemIndexer);
                        }
                    }
                }
            }
            else if (type == typeof(NameValueCollection))
            {
                var getValuesMethod = type.GetMethod("GetValues", new[] { typeof(string) });
                if (getValuesMethod != null)
                {
                    return new MemberWrapper(name, getValuesMethod);
                }
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
        }

        public static bool IsCollection(Type type)
        {
            if (type.IsArray)
            {
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return true;
            }

            foreach (var intType in type.GetInterfaces())
            {
                if (intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return true;
                }
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
            }

            return typedefinition;
        }

        public static List<Tuple<MemberWrapper, MemberWrapper>> GetMemberMaps(Type fromType, Type toType)
        {
            var result = new List<Tuple<MemberWrapper, MemberWrapper>>();

            var fromMembers = GetMembers(fromType);
            var toMembers = GetMembers(toType).ToDictionary(m => m.Name);

            foreach (var fromMember in fromMembers)
            {
                if (fromMember.CanRead)
                {
                    if (toMembers.ContainsKey(fromMember.Name))
                    {
                        var toMember = toMembers[fromMember.Name];
                        if (toMember.CanWrite)
                        {
                            result.Add(new Tuple<MemberWrapper, MemberWrapper>(fromMember, toMember));
                        }
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
    }
}
