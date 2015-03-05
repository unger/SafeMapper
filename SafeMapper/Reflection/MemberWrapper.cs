namespace SafeMapper.Reflection
{
    using System;
    using System.Reflection;

    public enum MemberType
    {
        Undefined,

        Property,

        Field,

        StringIndexer,

        Method
    }

    public class MemberWrapper
    {
        public MemberWrapper(MemberInfo member)
            : this(member.Name, member)
        {
        }

        public MemberWrapper(MemberInfo memberGetter, MemberInfo memberSetter)
            : this(memberGetter.Name, memberGetter, memberSetter)
        {
        }

        public MemberWrapper(string name, MemberInfo member)
            : this(name, member, member)
        {
            if (member is MethodInfo)
            {
                this.CanWrite = false;
            }
        }

        public MemberWrapper(string name, MemberInfo memberGetter, MemberInfo memberSetter)
        {
            this.Name = name;
            this.MemberGetter = memberGetter;
            this.MemberSetter = memberSetter;
            this.GetterType = ReflectionUtils.GetMemberType(memberGetter);
            this.SetterType = this.GetSetterType(memberSetter);

            this.MemberGetterType = this.GetMemberTypeEnum(memberGetter);
            this.MemberSetterType = this.GetMemberTypeEnum(memberSetter);

            this.CanRead = this.CanReadMember(memberGetter);
            this.CanWrite = this.CanWriteMember(memberSetter);

            this.SetterNeedsStringIndex = this.CheckNeedsStringIndex(memberSetter, this.MemberSetterType);

            if (ReflectionUtils.IsStringKeyDictionary(memberGetter.DeclaringType))
            {
                this.GetterNeedsContainsCheck = true;
            }
        }

        public string Name { get; private set; }

        public Type GetterType { get; private set; }

        public Type SetterType { get; private set; }

        public bool CanRead { get; private set; }

        public bool CanWrite { get; private set; }

        public bool SetterNeedsStringIndex { get; private set; }

        public bool GetterNeedsContainsCheck { get; private set; }

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

            if (member is MethodInfo)
            {
                return true;
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

        private bool CheckNeedsStringIndex(MemberInfo member, MemberType memberType)
        {
            if (memberType == MemberType.StringIndexer)
            {
                return true;
            }

            var method = member as MethodInfo;
            if (method != null)
            {
                var parameters = method.GetParameters();
                if (parameters.Length == 2 && parameters[0].ParameterType == typeof(string))
                {
                    return true;
                }
            }

            return false;
        }

        private Type GetSetterType(MemberInfo member)
        {
            var method = member as MethodInfo;
            if (method != null)
            {
                var parameters = method.GetParameters();
                if (parameters.Length == 2 && parameters[0].ParameterType == typeof(string))
                {
                    return parameters[1].ParameterType;
                }
            }

            return ReflectionUtils.GetMemberType(member);
        }
    }
}
