namespace SafeMapper.Configuration
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;

    using SafeMapper.Reflection;

    public class MapConfiguration
    {
        private readonly ConcurrentDictionary<string, TypeMapping> typeMappings = new ConcurrentDictionary<string, TypeMapping>();

        private readonly ConcurrentDictionary<string, MethodInfo> convertMethodInfos = new ConcurrentDictionary<string, MethodInfo>();

        public TypeMapping GetTypeMapping(Type fromType, Type toType)
        {
            return this.typeMappings.GetOrAdd(
                string.Concat(fromType.FullName, toType.FullName),
                k => new TypeMapping(fromType, toType));
        }

        public void SetTypeMapping(TypeMapping typeMapping)
        {
            this.typeMappings.AddOrUpdate(
                string.Concat(typeMapping.FromType.FullName, typeMapping.ToType.FullName),
                typeMapping,
                (key, oldValue) => typeMapping);
        }

        public MethodInfo GetConvertMethod(Type fromType, Type toType)
        {
            return this.convertMethodInfos.GetOrAdd(
                string.Concat(fromType.FullName, toType.FullName),
                k => ReflectionUtils.GetConvertMethod(fromType, toType, new[] { typeof(SafeConvert) }));
        }

        public void SetConvertMethod(Type fromType, Type toType, MethodInfo convertMethod)
        {
            this.convertMethodInfos.AddOrUpdate(
                string.Concat(fromType.FullName, toType.FullName),
                convertMethod,
                (key, oldValue) => convertMethod);
        }
    }
}
