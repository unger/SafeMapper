namespace SafeMapper.Configuration
{
    using System.Reflection;

    public class MethodWrapper
    {
        public MethodWrapper(MethodInfo method, object target = null)
        {
            this.Method = method;
            this.Target = target;
        }

        public MethodInfo Method { get; set; }

        public object Target { get; set; }
    }
}
