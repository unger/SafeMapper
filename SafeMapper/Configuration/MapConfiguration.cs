namespace SafeMapper.Configuration
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;

    using SafeMapper.Reflection;

    public class MapConfiguration
    {
        private readonly ConcurrentDictionary<string, ITypeMapping> typeMappings = new ConcurrentDictionary<string, ITypeMapping>();

        private readonly ConcurrentDictionary<string, MethodWrapper> convertMethods = new ConcurrentDictionary<string, MethodWrapper>();

        public ITypeMapping GetTypeMapping(Type fromType, Type toType)
        {
            return this.typeMappings.GetOrAdd(
                string.Concat(fromType.FullName, toType.FullName),
                k => new TypeMapping(fromType, toType));
        }

        public void SetTypeMapping(ITypeMapping typeMapping)
        {
            this.typeMappings.AddOrUpdate(
                string.Concat(typeMapping.FromType.FullName, typeMapping.ToType.FullName),
                typeMapping,
                (key, oldValue) => typeMapping);
        }

        public MethodWrapper GetConvertMethod(Type fromType, Type toType)
        {
            return this.convertMethods.GetOrAdd(
                string.Concat(fromType.FullName, toType.FullName),
                k =>
                    {
                        var method = ReflectionUtils.GetConvertMethod(fromType, toType, new[] { typeof(SafeConvert), typeof(SafeNullableConvert) });
                        return method != null ? new MethodWrapper(method) : null;
                    });
        }

        public void SetConvertMethod(Type fromType, Type toType, MethodWrapper convertMethod)
        {
            this.convertMethods.AddOrUpdate(
                string.Concat(fromType.FullName, toType.FullName),
                convertMethod,
                (key, oldValue) => convertMethod);
        }

        public void SetConvertMethod<TFrom, TTo>(Func<TFrom, TTo> converter)
        {
            if (!converter.Method.IsStatic)
            {
                throw new ArgumentException("Only static Func-lamdas are supported");    
            }

            this.SetConvertMethod(typeof(TFrom), typeof(TTo), new MethodWrapper(converter.Method, converter.Target));
        }
    }
}
