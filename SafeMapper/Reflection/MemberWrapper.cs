namespace SafeMapper.Reflection
{
    using System;
    using System.Reflection;

    public enum MemberType
    {
        Undefined, Property, Field, StringIndexer, Method  
    }

    public class MemberWrapper
    {
        public MemberWrapper(MemberInfo member)
            : this(member.Name, member, member)
        {
        }

        public MemberWrapper(MemberInfo memberGetter, MemberInfo memberSetter)
            : this(memberGetter.Name, memberGetter, memberSetter)
        {
        }

        public MemberWrapper(string name, MemberInfo member)
            : this(name, member, member)
        {
        }
        
        public MemberWrapper(string name, MemberInfo memberGetter, MemberInfo memberSetter)
        {
            this.Name = name;
            this.MemberGetter = memberGetter;
            this.MemberSetter = memberSetter;
            this.Type = ReflectionUtils.GetMemberType(memberGetter);

            this.MemberGetterType = this.GetMemberTypeEnum(memberGetter);
            this.MemberSetterType = this.GetMemberTypeEnum(memberSetter);

            this.CanRead = this.CanReadMember(memberGetter);
            this.CanWrite = this.CanWriteMember(memberSetter);
        }

        public string Name { get; private set; }

        public Type Type { get; private set; }

        public bool CanRead { get; private set; }

        public bool CanWrite { get; private set; }

        public MemberInfo MemberGetter { get; private set; }

        public MemberType MemberGetterType { get; private set; }

        public MemberInfo MemberSetter { get; private set; }

        public MemberType MemberSetterType { get; private set; }

        private bool CanWriteMember(MemberInfo member)
        {
            if (member is PropertyInfo)
            {
                return (member as PropertyInfo).CanWrite;
            }

            if (member is FieldInfo)
            {
                return !(member as FieldInfo).IsInitOnly;
            }

            return false;
        }

        private bool CanReadMember(MemberInfo member)
        {
            if (member is PropertyInfo)
            {
                return (member as PropertyInfo).CanRead;
            }

            if (member is FieldInfo)
            {
                return true;
            }

            if (member is MethodInfo)
            {
                return (member as MethodInfo).ReturnType != typeof(void);
            }

            return false;
        }

        private MemberType GetMemberTypeEnum(MemberInfo member)
        {
            if (member is PropertyInfo)
            {
                var indexParams = (member as PropertyInfo).GetIndexParameters();
                if (indexParams.Length == 0)
                {
                    return MemberType.Property;
                }
                
                if (indexParams.Length == 1 && indexParams[0].ParameterType == typeof(string))
                {
                    return MemberType.StringIndexer;
                }
            }

            if (member is FieldInfo)
            {
                return MemberType.Field;
            }
                
            if (member is MethodInfo)
            {
                return MemberType.Method;
            }

            return MemberType.Undefined;
        }
    }
}
