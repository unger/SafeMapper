namespace SafeMapper.Reflection
{
    using System;
    using System.Reflection;

    public class Member
    {
        public Member(MemberInfo member, string key = null)
        {
            this.Name = key ?? member.Name;
            this.Type = ReflectionUtils.GetMemberType(member);
            this.MemberType = this.GetMemberTypeEnum(member);
            this.MemberInfo = member;
            this.NeedsStringIndex = this.CheckNeedsStringIndex(this.MemberInfo, this.MemberType);
        }

        public string Name { get; protected set; }

        public MemberType MemberType { get; protected set; }

        public Type Type { get; protected set; }

        public MemberInfo MemberInfo { get; protected set; }

        public bool NeedsStringIndex { get; protected set; }

        protected MemberType GetMemberTypeEnum(MemberInfo member)
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
    }
}