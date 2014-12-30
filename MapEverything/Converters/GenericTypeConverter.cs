namespace MapEverything.Converters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    using MapEverything.TypeMaps;

    public class GenericTypeConverter<TFrom, TTo> : TypeConverter
    {
        private readonly ITypeMap toFromTypeMap;
        private readonly ITypeMap fromToTypeMap;

        private readonly Type toType;

        public GenericTypeConverter(ITypeMapper typeMapper) 
            : this(typeMapper, CultureInfo.CurrentCulture)
        {
        }

        public GenericTypeConverter(ITypeMapper typeMapper, IFormatProvider formatProvider)
        {
            Type fromType = typeof(TFrom);
            this.toType = typeof(TTo);

            this.fromToTypeMap = TypeMapFactory.Create(fromType, this.toType, formatProvider, typeMapper);
            this.toFromTypeMap = TypeMapFactory.Create(this.toType, fromType, formatProvider, typeMapper);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == this.toType)
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == this.toType)
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is TTo)
            {
                return this.toFromTypeMap.Convert(value);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == this.toType)
            {
                return this.fromToTypeMap.Convert(value);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
