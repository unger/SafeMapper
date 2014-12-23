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

            if (this.HasDefaultConstructor)
            {
                this.CreateObject = Expression.Lambda<Func<object>>(Expression.New(currentType)).Compile();
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

                this.CreateObject = () => Activator.CreateInstance(this.ConcreteType);
            }
            else
            {
                this.CreateObject = () => this.GetDefaultValue(currentType);
            }

            if (!(currentType.IsPrimitive || this.IsCollection))
            {
                this.CacheMembers();
            }
        }

        public Type ActualType { get; private set; }

        public Type ConcreteType { get; private set; }

        public bool HasDefaultConstructor { get; private set; }

        public bool IsCollection { get; private set; }

        public Func<object> CreateObject { get; private set; }

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
    }
}