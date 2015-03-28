namespace SafeMapper
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;

    using SafeMapper.Configuration;
    using SafeMapper.Utils;

    public class SafeMap
    {
        private static readonly ConcurrentDictionary<string, object> ConverterCache = new ConcurrentDictionary<string, object>();

        private static ConverterFactory converterFactory;

        private static MapConfiguration mapConfiguration;

        static SafeMap()
        {
            mapConfiguration = new MapConfiguration();
            converterFactory = new ConverterFactory(mapConfiguration);
        }

        public static MapConfiguration Configuration 
        {
            get
            {
                return mapConfiguration;
            }

            set
            {
                mapConfiguration = value;
                converterFactory = new ConverterFactory(mapConfiguration);
            }
        }

        public static object Convert(object fromObject, Type fromType, Type toType)
        {
            return GetConverter(fromType, toType, CultureInfo.CurrentCulture)(fromObject);
        }

        public static object Convert(object fromObject, Type fromType, Type toType, IFormatProvider provider)
        {
            return GetConverter(fromType, toType, provider)(fromObject);
        }

        public static TTo Convert<TFrom, TTo>(TFrom fromObject)
        {
            return GetConverter<TFrom, TTo>(CultureInfo.CurrentCulture)(fromObject);
        }

        public static TTo Convert<TFrom, TTo>(TFrom fromObject, IFormatProvider provider)
        {
            return GetConverter<TFrom, TTo>(provider)(fromObject);
        }

        public static Func<object, object> GetConverter(Type fromType, Type toType)
        {
            return GetConverter(fromType, toType, CultureInfo.CurrentCulture);
        }

        public static Func<object, object> GetConverter(Type fromType, Type toType, IFormatProvider provider)
        {
            return (Func<object, object>)ConverterCache.GetOrAdd(
                string.Concat(toType.FullName, fromType.FullName, "NonGeneric"),
                k => converterFactory.CreateDelegate(fromType, toType, provider));
        }

        public static Converter<TFrom, TTo> GetConverter<TFrom, TTo>()
        {
            return GetConverter<TFrom, TTo>(CultureInfo.CurrentCulture);
        }

        public static Converter<TFrom, TTo> GetConverter<TFrom, TTo>(IFormatProvider provider)
        {
            return (Converter<TFrom, TTo>)ConverterCache.GetOrAdd(
                string.Concat(typeof(TTo).FullName, typeof(TFrom).FullName),
                k => converterFactory.CreateDelegate<TFrom, TTo>(provider));
        }

        public static void CreateMap<TFrom, TTo>(Action<ITypeMap<TFrom, TTo>> config)
        {
            var typeMap = new TypeMap<TFrom, TTo>();
            config(typeMap);
            mapConfiguration.SetTypeMapping(typeMap.GetTypeMapping());
        }
    }
}
