namespace SafeMapper.Configuration
{
    using System.Collections.Generic;

    using SafeMapper.Reflection;

    public class TypeMap<TFrom, TTo>
    {
        private readonly List<MemberMap> memberMaps = new List<MemberMap>();

        public TypeMapping GetTypeMapping()
        {
            return new TypeMapping(typeof(TFrom), typeof(TTo), this.memberMaps);
        }

        protected void Map(string fromName, string toName)
        {
            var fromMember = ReflectionUtils.GetMemberGetter(typeof(TFrom), fromName);
            var toMember = ReflectionUtils.GetMemberSetter(typeof(TTo), toName);

            if (fromMember != null && toMember != null)
            {
                this.memberMaps.Add(new MemberMap(fromMember, toMember));
            }
        }
    }
}
