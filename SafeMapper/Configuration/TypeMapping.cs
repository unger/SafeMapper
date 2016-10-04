namespace SafeMapper.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SafeMapper.Abstractions;
    using SafeMapper.Reflection;

    public class TypeMapping : ITypeMapping
    {
        public TypeMapping(Type fromType, Type toType) 
            : this(fromType, toType, null)
        {
        }

        public TypeMapping(Type fromType, Type toType, IEnumerable<MemberMap> memberMaps)
        {
            this.FromType = fromType;
            this.ToType = toType;
            this.MemberMaps = ReflectionUtils.GetMemberMaps(fromType, toType).ToList();
            if (memberMaps != null)
            {
                this.MemberMaps.AddRange(memberMaps);
            }
        }

        public Type FromType { get; private set; }

        public Type ToType { get; private set; }

        public List<MemberMap> MemberMaps { get; private set; }
    }
}
