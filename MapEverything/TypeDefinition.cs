namespace MapEverything
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Fasterflect;

    public class TypeDefinition<T> : TypeDefinition
    {
        public TypeDefinition()
            : base(typeof(T))
        {
        }

        public Func<T, TProperty> GetPropertyGetter<TProperty>(string propertyName)
        {
            ParameterExpression paramExpression = Expression.Parameter(typeof(T), "value");
            Expression propertyGetterExpression = Expression.Property(paramExpression, propertyName);

            return Expression.Lambda<Func<T, TProperty>>(propertyGetterExpression, paramExpression).Compile();
        }
    }

    public class TypeDefinition
    {
        private Dictionary<string, MemberInfo> members = new Dictionary<string, MemberInfo>();
        private Dictionary<string, MemberGetter> memberGetters = new Dictionary<string, MemberGetter>();
        private Dictionary<string, MemberSetter> memberSetters = new Dictionary<string, MemberSetter>();

        public TypeDefinition(Type currentType)
        {
            this.ActualType = currentType;
            this.ConcreteType = currentType;

            var defaultConstructor = this.ActualType.GetConstructor(Type.EmptyTypes);
            this.HasDefaultConstructor = defaultConstructor != null;
            this.IsCollection = this.IsCollectionType(currentType);

            if (currentType.HasElementType)
            {
                this.ElementType = currentType.GetElementType();
            }
            else if (this.IsCollection && currentType.IsGenericType)
            {
                this.ElementType = currentType.GetGenericArguments()[0];
            }

            this.CreateInstanceDelegate = this.GenerateCreateInstanceDelegate(currentType);

            if (!(currentType.IsPrimitive || this.IsCollection))
            {
                this.CacheMembers();
            }
        }

        public Type ActualType { get; private set; }

        public Type ConcreteType { get; private set; }

        public Type ElementType { get; private set; }

        public bool HasDefaultConstructor { get; private set; }

        public bool IsCollection { get; private set; }

        public Func<object> CreateInstanceDelegate { get; protected set; }

        public dynamic GetPropertyGetter(PropertyInfo propertyInfo)
        {
            return this.GetPropertyGetter(propertyInfo.DeclaringType, propertyInfo.PropertyType, propertyInfo.Name);
        }

        public Action<object, object> GetPropertySetter(PropertyInfo propertyInfo)
        {
            return this.GetPropertySetter(propertyInfo.DeclaringType, propertyInfo.PropertyType, propertyInfo.Name);
        }

        private Func<object> GenerateCreateInstanceDelegate(Type currentType)
        {
            if (this.HasDefaultConstructor)
            {
                return Expression.Lambda<Func<object>>(Expression.New(currentType)).Compile();
            }
            else if (currentType.IsGenericType)
            {
                var genericType = currentType.GetGenericTypeDefinition();
                var elementTypes = currentType.GetGenericArguments();

                if (genericType.IsInterface)
                {
                    genericType = this.GetConcreteType(genericType);
                }

                this.ConcreteType = genericType.MakeGenericType(elementTypes);

                return () => Activator.CreateInstance(this.ConcreteType);
            }

            return () => this.GetDefaultValue(currentType);
        }

        private void CacheMembers()
        {
            foreach (var member in this.ActualType.GetMembers())
            {
                var propInfo = member as PropertyInfo;
                if (propInfo != null)
                {
                    this.members.Add(member.Name, propInfo);
                    if (propInfo.CanRead)
                    {
                        this.memberGetters.Add(member.Name, propInfo.DelegateForGetPropertyValue());
                    }

                    if (propInfo.CanWrite)
                    {
                        this.memberSetters.Add(member.Name, propInfo.DelegateForSetPropertyValue());
                    }

                    continue;
                }

                var fieldInfo = member as FieldInfo;
                if (fieldInfo != null)
                {
                    this.members.Add(member.Name, fieldInfo);
                    this.memberGetters.Add(member.Name, fieldInfo.DelegateForGetFieldValue());
                    this.memberSetters.Add(member.Name, fieldInfo.DelegateForSetFieldValue());
                }
            }            
        }

        private Type GetConcreteType(Type type)
        {
            if (type.IsInterface)
            {
                if (type == typeof(IEnumerable<>))
                {
                    return typeof(Collection<>);
                }

                if (type == typeof(IList<>))
                {
                    return typeof(List<>);
                }

                if (type == typeof(ICollection<>))
                {
                    return typeof(Collection<>);
                }

                if (type == typeof(ISet<>))
                {
                    return typeof(HashSet<>);
                }
            }

            return type;
        }

        private object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        private bool IsCollectionType(Type type)
        {
            return type.IsArray
                || (!type.IsAssignableFrom(typeof(string)) && type.GetInterfaces().Any(t => t == typeof(IEnumerable)));
        }

        private dynamic GetPropertyGetter(Type type, Type propertyType, string propertyName)
        {
            ParameterExpression paramExpression = Expression.Parameter(type, "value");

            Expression propertyGetterExpression = Expression.Property(paramExpression, propertyName);

            var func = typeof(Func<,>);
            var genericFunc = func.MakeGenericType(type, propertyType);

            return Expression.Lambda(genericFunc, propertyGetterExpression, paramExpression).Compile();
        }

        private Action<object, object> GetPropertySetter(Type type, Type propertyType, string propertyName)
        {
            ParameterExpression paramExpression = Expression.Parameter(type);

            ParameterExpression paramExpression2 = Expression.Parameter(propertyType, propertyName);

            MemberExpression propertyGetterExpression = Expression.Property(paramExpression, propertyName);

            return
                Expression.Lambda<Action<object, object>>(
                    Expression.Assign(propertyGetterExpression, paramExpression2),
                    paramExpression,
                    paramExpression2).Compile();
        }
    }
}