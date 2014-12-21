namespace MapEverything.Converters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class GenericTypeConverter<TFrom, TTo> : TypeConverter
    {
        private TypeMap fromTypeMap;
        private TypeMap toTypeMap;

        public GenericTypeConverter(ITypeMapper typeMapper) : this(typeMapper, CultureInfo.CurrentCulture)
        {
        }

        public GenericTypeConverter(ITypeMapper typeMapper, IFormatProvider formatProvider)
        {
            var fromType = typeof(TFrom);
            var toType = typeof(TTo);

            this.fromTypeMap = new TypeMap(toType, fromType, typeMapper);
            this.toTypeMap = new TypeMap(fromType, toType, typeMapper);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(TTo))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(TTo))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is TTo)
            {
                var result = Activator.CreateInstance<TFrom>();

                this.fromTypeMap.Map(value, result);

                return result;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(TTo) && value is TFrom)
            {
                var result = Activator.CreateInstance<TTo>();

                this.toTypeMap.Map(value, result);

                return result;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
