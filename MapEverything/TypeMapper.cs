namespace MapEverything
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.Globalization;

    public class TypeMapper : ITypeMapper
    {
        private ConcurrentDictionary<Type, TypeConverter> typeConverters;

        public TypeMapper()
        {
            this.typeConverters = new ConcurrentDictionary<Type, TypeConverter>();
        }

        protected readonly Type[] ConvertTypes =
            {
                null, 
                typeof(Object), 
                typeof(DBNull), 
                typeof(Boolean),
                typeof(Char), 
                typeof(SByte), 
                typeof(Byte), 
                typeof(Int16),
                typeof(UInt16), 
                typeof(Int32), 
                typeof(UInt32), 
                typeof(Int64),
                typeof(UInt64), 
                typeof(Single), 
                typeof(Double), 
                typeof(Decimal),
                typeof(DateTime), 

                // TypeCode is discontinuous so we need a placeholder.
                typeof(Object),
                typeof(String)
            };

        public TTo Convert<TFrom, TTo>(TFrom value)
        {
            return (TTo)this.Convert(value, typeof(TTo));
        }

        public TTo Convert<TFrom, TTo>(TFrom value, IFormatProvider formatProvider)
        {
            return (TTo)this.Convert(value, typeof(TTo), formatProvider);
        }

        public TTo Convert<TFrom, TTo>(TFrom value, Converter<TFrom, TTo> converter)
        {
            return converter(value);
        }

        public Converter<TFrom, TTo> GetConverter<TFrom, TTo>()
        {
            return this.GetConverter<TFrom, TTo>(CultureInfo.CurrentCulture);
        }

        public Converter<TFrom, TTo> GetConverter<TFrom, TTo>(IFormatProvider formatProvider)
        {
            var converter = this.GetConverter(typeof(TFrom), typeof(TTo), formatProvider);
            return value => (TTo)converter(value);
        }

        public object Convert(object value, Type toType)
        {
            return this.Convert(value, toType, CultureInfo.CurrentCulture);
        }

        public object Convert(object value, Func<object, object> converter)
        {
            return converter(value);
        }

        public virtual object Convert(object value, Type toType, IFormatProvider formatProvider)
        {
            if (value == null)
            {
                return this.GetDefaultValue(toType);
            }

            var converter = this.GetConverter(value.GetType(), toType, formatProvider);

            return converter(value);
        }

        public Func<object, object> GetConverter(Type fromType, Type toType)
        {
            return this.GetConverter(fromType, toType, CultureInfo.CurrentCulture);
        }

        public virtual Func<object, object> GetConverter(Type fromType, Type toType, IFormatProvider formatProvider)
        {
            if (toType == this.ConvertTypes[(int)TypeCode.String])
            {
                return value => this.ConvertToString(value, formatProvider);
            }

            var toConverter = this.GetTypeConverter(toType);
            if (toConverter.CanConvertFrom(fromType))
            {
                this.AddTypeConverter(toType, toConverter);
                return value => toConverter.ConvertFrom(null, (CultureInfo)formatProvider, value);
            }

            var fromConverter = this.GetTypeConverter(fromType);
            if (fromConverter.CanConvertTo(toType))
            {
                this.AddTypeConverter(fromType, fromConverter);
                return value => fromConverter.ConvertTo(null, (CultureInfo)formatProvider, value, toType);
            }

            return value => System.Convert.ChangeType(value, toType, formatProvider);
        }

        public void AddTypeConverter<T>(TypeConverter typeConverter)
        {
            this.AddTypeConverter(typeof(T), typeConverter);
        }

        protected TypeConverter GetTypeConverter(Type type)
        {
            TypeConverter typeConverter;
            if (this.typeConverters.TryGetValue(type, out typeConverter))
            {
                return typeConverter;
            }

            return TypeDescriptor.GetConverter(type);
        }

        protected virtual object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }

            return null;
        }

        protected virtual string ConvertToString(object value, IFormatProvider formatProvider)
        {
            var ic = value as IConvertible;
            if (ic != null)
            {
                return ic.ToString(formatProvider);
            }

            return value.ToString();
        }

        private void AddTypeConverter(Type keyType, TypeConverter typeConverter)
        {
            this.typeConverters[keyType] = typeConverter;
        }
    }
}
