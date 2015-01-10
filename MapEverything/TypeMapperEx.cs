namespace MapEverything
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;

    using Fasterflect;

    public class TypeMapperEx
    {
        private static readonly ConcurrentDictionary<string, object> TypeConvertInvokers = new ConcurrentDictionary<string, object>();

        public static TTo Convert<TFrom, TTo>(TFrom value)
        {
            return Convert<TFrom, TTo>(value, GetConverter<TFrom, TTo>(CultureInfo.CurrentCulture));
        }

        public static TTo Convert<TFrom, TTo>(TFrom value, IFormatProvider formatProvider)
        {
            return Convert<TFrom, TTo>(value, GetConverter<TFrom, TTo>(formatProvider));
        }

        public static TTo Convert<TFrom, TTo>(TFrom value, Converter<TFrom, TTo> converter)
        {
            return converter(value);
        }

        public static Converter<TFrom, TTo> GetConverter<TFrom, TTo>(IFormatProvider formatProvider)
        {
            return (Converter<TFrom, TTo>)TypeConvertInvokers.GetOrAdd(
                string.Concat(typeof(TFrom).FullName, typeof(TTo).FullName),
                k => CreateConverter<TFrom, TTo>(formatProvider));
        }

        public static object GetConverter(Type fromType, Type toType, IFormatProvider formatProvider)
        {
            return TypeConvertInvokers.GetOrAdd(
                string.Concat(fromType.FullName, toType.FullName),
                k =>
                typeof(TypeMapperEx).DelegateForCallMethod(
                    new[] { fromType, toType },
                    "CreateConverter",
                    new[] { typeof(IFormatProvider) })(null, formatProvider));
        }

        private static Converter<TFrom, TTo> CreateConverter<TFrom, TTo>(IFormatProvider formatProvider)
        {
            var fromType = typeof(TFrom);
            var toType = typeof(TTo);

            

            return value => (TTo)System.Convert.ChangeType(value, typeof(TTo), formatProvider);
        }
   }
}
