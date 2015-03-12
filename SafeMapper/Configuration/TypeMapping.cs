namespace SafeMapper.Configuration
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using SafeMapper.Reflection;

    public class TypeMapping
    {
        private static readonly ConcurrentDictionary<string, TypeMapping> TypeMappingCache = new ConcurrentDictionary<string, TypeMapping>();

        public TypeMapping(Type fromType, Type toType) 
            : this(fromType, toType, null)
        {
        }

        public TypeMapping(Type fromType, Type toType, IEnumerable<MemberMap> memberMaps)
        {
            this.FromType = fromType;
            this.ToType = toType;
            this.MemberMaps = ReflectionUtils.GetMemberMaps(fromType, toType).Select(tup => new MemberMap(tup.Item1, tup.Item2)).ToList();
            if (memberMaps != null)
            {
                this.MemberMaps.AddRange(memberMaps);
            }
        }

        public Type FromType { get; private set; }

        public Type ToType { get; private set; }

        public List<MemberMap> MemberMaps { get; private set; }

        public static TypeMapping GetTypeMapping(Type fromType, Type toType)
        {
            return TypeMappingCache.GetOrAdd(
                string.Concat(fromType.FullName, toType.FullName),
                k => new TypeMapping(fromType, toType));
        }

        public static void SetTypeMapping(TypeMapping typeMapping)
        {
            TypeMappingCache.AddOrUpdate(
                string.Concat(typeMapping.FromType.FullName, typeMapping.ToType.FullName), 
                typeMapping, 
                (s, o) => o);
        }
    }
}
