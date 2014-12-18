namespace MapEverything
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.Globalization;

    public class TypeMapper : TypeMapperBase
    {
        private ConcurrentDictionary<Type, TypeConverter> typeConverters;

        public TypeMapper()
        {
            this.typeConverters = new ConcurrentDictionary<Type, TypeConverter>();
        }

        public override Func<object, object> GetConverter(Type fromType, Type toType, IFormatProvider formatProvider)
        {
            if (toType == this.ConvertTypes[(int)TypeCode.String])
            {
                return value => this.ConvertToString(value, formatProvider);
            }

            var toConverter = this.GetTypeConverter(toType);
            if (toConverter.CanConvertFrom(fromType))
            {
                return value => toConverter.ConvertFrom(null, (CultureInfo)formatProvider, value);
            }

            var fromConverter = this.GetTypeConverter(fromType);
            if (fromConverter.CanConvertTo(toType))
            {
                return value => fromConverter.ConvertTo(null, (CultureInfo)formatProvider, value, toType);
            }

            return value => System.Convert.ChangeType(value, toType, formatProvider);
        }

        public void AddConverter<T>(TypeConverter typeConverter)
        {
            this.typeConverters[typeof(T)] = typeConverter;
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

        private TypeConverter GetTypeConverter(Type type)
        {
            return this.typeConverters.GetOrAdd(type, TypeDescriptor.GetConverter);
        }
    }
}
