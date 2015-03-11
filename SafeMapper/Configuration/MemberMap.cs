namespace SafeMapper.Configuration
{
    using SafeMapper.Reflection;

    public class MemberMap
    {
        public MemberMap(MemberWrapper fromMember, MemberWrapper toMember)
        {
            this.FromMember = fromMember;
            this.ToMember = toMember;
        }

        public MemberWrapper FromMember { get; private set; }

        public MemberWrapper ToMember { get; private set; }
    }
}
