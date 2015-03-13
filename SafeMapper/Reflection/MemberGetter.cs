namespace SafeMapper.Reflection
{
    using System.Reflection;

    public class MemberGetter : Member
    {
        public MemberGetter(MemberInfo member, string key = null) : base(member, key)
        {
            this.NeedsContainsCheck = ReflectionUtils.IsStringKeyDictionary(member.DeclaringType);
        }

        public bool NeedsContainsCheck { get; private set; }
    }
}
