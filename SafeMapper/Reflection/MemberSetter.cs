namespace SafeMapper.Reflection
{
    using System;
    using System.Reflection;

    public class MemberSetter : Member
    {
        public MemberSetter(MemberInfo member, string key = null)
            : base(member, key)
        {
            this.Type = this.GetSetterType(member);
            this.NeedsStringIndex = this.CheckNeedsStringIndex(this.MemberInfo, this.MemberType);
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

                if (parameters.Length == 1)
                {
                    return parameters[0].ParameterType;
                }
            }

            return ReflectionUtils.GetMemberType(member);
        }
    }
}
