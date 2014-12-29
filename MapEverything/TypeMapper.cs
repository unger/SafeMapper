namespace MapEverything
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;

    using MapEverything.TypeMaps;

    public class TypeMapper : ITypeMapper
    {
        protected readonly Type[] ConvertTypes =
            {
                null,               // TypeCode.Empty = 0
                typeof(object),     // TypeCode.Object = 1
                typeof(DBNull),     // TypeCode.DBNull = 2
                typeof(bool),       // TypeCode.Boolean = 3
                typeof(char),       // TypeCode.Char = 4
                typeof(sbyte),      // TypeCode.SByte = 5
                typeof(byte),       // TypeCode.Byte = 6
                typeof(short),      // TypeCode.Int16 = 7
                typeof(ushort),     // TypeCode.UInt16 = 8
                typeof(int),        // TypeCode.Int32 = 9
                typeof(uint),       // TypeCode.UInt32 = 10
                typeof(long),       // TypeCode.Int64 = 11
                typeof(ulong),      // TypeCode.UInt64 = 12
                typeof(float),      // TypeCode.Single = 13
                typeof(double),     // TypeCode.Double = 14
                typeof(decimal),    // TypeCode.Decimal = 15
                typeof(DateTime),   // TypeCode.DateTime = 16
                typeof(object),     // 17 is missing
                typeof(string)      // TypeCode.String = 18
            };

        private readonly ConcurrentDictionary<string, ITypeMap> typeMappers;
        private readonly ConcurrentDictionary<Type, TypeDefinition> typeDefinitions;

        public TypeMapper()
        {
            this.typeMappers = new ConcurrentDictionary<string, ITypeMap>();
            this.typeDefinitions = new ConcurrentDictionary<Type, TypeDefinition>();
        }

        public TTo Convert<TFrom, TTo>(TFrom value)
        {
            return (TTo)this.Convert(value, typeof(TFrom), typeof(TTo));
        }

        public TTo Convert<TFrom, TTo>(TFrom value, IFormatProvider formatProvider)
        {
            return (TTo)this.Convert(value, typeof(TFrom), typeof(TTo), formatProvider);
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

        public object Convert(object value, Type fromType, Type toType)
        {
            return this.GetTypeMap(fromType, toType, CultureInfo.CurrentCulture).Convert(value);
        }

        public object Convert(object value, Func<object, object> converter)
        {
            return converter(value);
        }

        public virtual object Convert(object value, Type fromType, Type toType, IFormatProvider formatProvider)
        {
            return this.GetTypeMap(fromType, toType, formatProvider).Convert(value);
        }

        public Func<object, object> GetConverter(Type fromType, Type toType)
        {
            return this.GetConverter(fromType, toType, CultureInfo.CurrentCulture);
        }

        public virtual Func<object, object> GetConverter(Type fromType, Type toType, IFormatProvider formatProvider)
        {
            return this.GetTypeMap(fromType, toType, formatProvider).Convert;
        }

        public TypeDefinition GetTypeDefinition(Type type)
        {
            return this.typeDefinitions.AddOrUpdate(type, t => new TypeDefinition(t), (t, definition) => definition);
        }

        public ITypeMap GetTypeMap(Type fromType, Type toType, IFormatProvider formatProvider)
        {
            //var key = string.Format("{0}{1}", fromType.FullName, toType.FullName);
            //var key = string.Concat(fromType.FullName, toType.FullName);
            return this.typeMappers.GetOrAdd(string.Concat(fromType.FullName, toType.FullName), t => TypeMapFactory.Create(fromType, toType, formatProvider, this));
        }

        public ITypeMap AddTypeMap(Type fromType, Type toType, Func<object, object> converter)
        {
            //var key = string.Format("{0}{1}", fromType.FullName, toType.FullName);
            //var key = string.Concat(fromType.FullName, toType.FullName);
            return this.typeMappers.GetOrAdd(string.Concat(fromType.FullName, toType.FullName), t => TypeMapFactory.Create(converter));
        }

        protected virtual object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }

            return null;
        }
    }
}
