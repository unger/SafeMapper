namespace MapEverything
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;

    public abstract class TypeMapperBase : ITypeMapper
    {
        protected readonly Type[] ConvertTypes =
            {
                null, 
                typeof(Object), 
                typeof(System.DBNull), 
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

        private ConcurrentDictionary<Type, TypeConverter> typeConverters;

        protected TypeMapperBase()
        {
            this.typeConverters = new ConcurrentDictionary<Type, TypeConverter>();
        }

        protected ConcurrentDictionary<Type, TypeConverter> TypeConverters
        {
            get
            {
                return this.typeConverters;
            }
        }

        public TTo Convert<TFrom, TTo>(TFrom value)
        {
            return (TTo)this.Convert(value, typeof(TTo));
        }

        public TTo Convert<TFrom, TTo>(TFrom value, IFormatProvider formatProvider)
        {
            return (TTo)this.Convert(value, typeof(TTo), formatProvider);
        }

        public object Convert(object value, Type toType)
        {
            return this.Convert(value, toType, CultureInfo.CurrentCulture);
        }

        public abstract object Convert(object value, Type toType, IFormatProvider formatProvider);

        public void AddConverter<T>(TypeConverter typeConverter)
        {
            this.typeConverters[typeof(T)] = typeConverter;
        }

        protected object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }

            return null;
        }
    }
}
