namespace MapEverything
{
    using System;
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

        public Func<object, object> GetConverter(Type fromType, Type toType)
        {
            return this.GetConverter(fromType, toType, CultureInfo.CurrentCulture);
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

        public abstract Func<object, object> GetConverter(Type fromType, Type toType, IFormatProvider formatProvider);

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
