namespace SafeMapper.Configuration
{
    using System.Reflection;

    public class MethodWrapper
    {
        public MethodWrapper(MethodInfo method, object target, MemberInfo staticInstanceMember)
        {
            this.StaticInstanceMember = staticInstanceMember;
            this.Method = method;
            this.Target = target;
        }

        public MethodInfo Method { get; set; }

        public object Target { get; set; }

        public MemberInfo StaticInstanceMember { get; set; }
    }
}
