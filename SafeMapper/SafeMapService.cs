namespace SafeMapper
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;

    using SafeMapper.Abstractions;
    using SafeMapper.Configuration;

    public class SafeMapService
    {
        private readonly ConcurrentDictionary<string, object> converterCache = new ConcurrentDictionary<string, object>();

        private readonly IConverterFactory converterFactory;

        public SafeMapService(IConverterFactory converterFactory)
        {
            this.converterFactory = converterFactory;
        }

        public IMapConfiguration Configuration 
        {
            get
            {
                return this.converterFactory.Configuration;
            }
        }

        public object Convert(object fromObject, Type fromType, Type toType)
        {
            return this.GetConverter(fromType, toType, CultureInfo.CurrentCulture)(fromObject);
        }

        public object Convert(object fromObject, Type fromType, Type toType, IFormatProvider provider)
        {
            return this.GetConverter(fromType, toType, provider)(fromObject);
        }

        public TTo Convert<TFrom, TTo>(TFrom fromObject)
        {
            return this.GetConverter<TFrom, TTo>(CultureInfo.CurrentCulture)(fromObject);
        }

        public TTo Convert<TFrom, TTo>(TFrom fromObject, IFormatProvider provider)
        {
            return this.GetConverter<TFrom, TTo>(provider)(fromObject);
        }

        public Func<object, object> GetConverter(Type fromType, Type toType)
        {
            return this.GetConverter(fromType, toType, CultureInfo.CurrentCulture);
        }

        public Func<object, object> GetConverter(Type fromType, Type toType, IFormatProvider provider)
        {
            return (Func<object, object>)this.converterCache.GetOrAdd(
                string.Concat(toType.FullName, fromType.FullName, "NonGeneric"),
                k => this.converterFactory.CreateDelegate(fromType, toType, provider));
        }

        public Func<TFrom, TTo> GetConverter<TFrom, TTo>()
        {
            return this.GetConverter<TFrom, TTo>(CultureInfo.CurrentCulture);
        }

        public Func<TFrom, TTo> GetConverter<TFrom, TTo>(IFormatProvider provider)
        {
            return (Func<TFrom, TTo>)this.converterCache.GetOrAdd(
                string.Concat(typeof(TTo).FullName, typeof(TFrom).FullName),
                k => this.converterFactory.CreateDelegate<TFrom, TTo>(provider));
        }

        public void CreateMap<TFrom, TTo>(Action<ITypeMap<TFrom, TTo>> config)
        {
            var typeMap = new TypeMap<TFrom, TTo>();
            config(typeMap);
            this.Configuration.SetTypeMapping(typeMap.GetTypeMapping());
        }
    }
}
