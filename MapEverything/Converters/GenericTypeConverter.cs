namespace MapEverything.Converters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    using Fasterflect;

    public class GenericTypeConverter<TFrom, TTo> : TypeConverter
    {
        private readonly ITypeMapper typeMapper;

        private readonly TypeMap toFromTypeMap;
        private readonly TypeMap fromToTypeMap;

        private Type toType;
        private Type fromType;

        public GenericTypeConverter(ITypeMapper typeMapper) 
            : this(typeMapper, CultureInfo.CurrentCulture)
        {
        }

        public GenericTypeConverter(ITypeMapper typeMapper, IFormatProvider formatProvider)
        {
            this.fromType = typeof(TFrom);
            this.toType = typeof(TTo);

            TypeDefinition toTypeDef = typeMapper.GetTypeDefinition(this.toType);
            TypeDefinition fromTypeDef = typeMapper.GetTypeDefinition(this.fromType);
            this.typeMapper = typeMapper;

            this.fromToTypeMap = new TypeMap(fromTypeDef, toTypeDef, typeMapper);
            this.toFromTypeMap = new TypeMap(toTypeDef, fromTypeDef, typeMapper);
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
