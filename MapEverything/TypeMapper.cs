namespace MapEverything
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class TypeMapper : TypeMapperBase
    {
        public override object Convert(object value, Type toType, IFormatProvider formatProvider)
        {
            if (value == null)
            {
                return this.GetDefaultValue(toType);
            }

            if (toType == this.ConvertTypes[(int)TypeCode.String])
            {
                return this.ConvertToString(value, formatProvider);
            }

            var toConverter = this.GetConverter(toType);
            if (toConverter.CanConvertFrom(value.GetType()))
            {
                return toConverter.ConvertFrom(null, (CultureInfo)formatProvider, value);
            }

            var fromConverter = this.GetConverter(value.GetType());
            if (fromConverter.CanConvertTo(toType))
            {
                return fromConverter.ConvertTo(null, (CultureInfo)formatProvider, value, toType);
            }

            return System.Convert.ChangeType(value, toType, formatProvider);
        }

        private string ConvertToString(object value, IFormatProvider formatProvider)
        {
            var ic = value as IConvertible;
            if (ic != null)
            {
                return ic.ToString(formatProvider);
            }

            return value.ToString();
        }

        private TypeConverter GetConverter(Type type)
        {
            return this.TypeConverters.GetOrAdd(type, TypeDescriptor.GetConverter);
        }
    }
}
