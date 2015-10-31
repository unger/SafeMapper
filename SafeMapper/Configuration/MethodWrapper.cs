namespace SafeMapper.Configuration
{
    using System.Reflection;

    public class MethodWrapper
    {
        public MethodWrapper(MethodInfo method, MemberInfo staticInstanceMember = null)
        {
            this.StaticInstanceMember = staticInstanceMember;
            this.Method = method;
        }

        public MethodInfo Method { get; set; }

        public MemberInfo StaticInstanceMember { get; set; }
    }
}
