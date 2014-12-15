namespace MapEverything
{
    using System;
    using System.Globalization;

    public static class TypeMapperExtensions
    {
        public static TDestination ConvertTo<TSource, TDestination>(this TSource value, Type type)
        {
            return ConvertTo<TSource, TDestination>(value, type, CultureInfo.CurrentCulture);
        }

        public static TDestination ConvertTo<TSource, TDestination>(this TSource value, Type type, IFormatProvider formatProvider)
        {
            return (TDestination)ConvertTo(value, type, formatProvider);
        }

        public static object ConvertTo(this object value, Type type)
        {
            return ConvertTo(value, type, CultureInfo.CurrentCulture);
        }

        public static object ConvertTo(this object value, Type type, IFormatProvider formatProvider)
        {
            return Convert.ChangeType(value, type, formatProvider);
        }
    }
}
