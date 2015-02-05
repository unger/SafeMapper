namespace SafeMapper
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;

    using SafeMapper.Utils;

    public class SafeMap
    {
        private static readonly ConcurrentDictionary<string, object> ConverterCache = new ConcurrentDictionary<string, object>();

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
                k => ConverterFactory.CreateDelegate(fromType, toType, provider));
        }

        public static Converter<TFrom, TTo> GetConverter<TFrom, TTo>()
        {
            return GetConverter<TFrom, TTo>(CultureInfo.CurrentCulture);
        }

        public static Converter<TFrom, TTo> GetConverter<TFrom, TTo>(IFormatProvider provider)
        {
            return (Converter<TFrom, TTo>)ConverterCache.GetOrAdd(
                string.Concat(typeof(TTo).FullName, typeof(TFrom).FullName),
                k => ConverterFactory.CreateDelegate<TFrom, TTo>(provider));
        }
    }
}
