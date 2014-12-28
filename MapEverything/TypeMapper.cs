namespace MapEverything
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.Data.SqlTypes;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters;

    using Fasterflect;

    using MapEverything.Converters;
    using MapEverything.Utils;

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

        private ConcurrentDictionary<string, TypeMap> typeMappers;
        private ConcurrentDictionary<string, TypeConverter> typeConverters;
        private ConcurrentDictionary<Type, TypeDefinition> typeDefinitions;

        public TypeMapper()
        {
            this.typeMappers = new ConcurrentDictionary<string, TypeMap>();
            var sqlDateTimeConverter = new SqlDateTimeTypeConverter();
            this.typeConverters = new ConcurrentDictionary<string, TypeConverter>();
            this.typeDefinitions = new ConcurrentDictionary<Type, TypeDefinition>();
            this.AddTypeConverter(typeof(string), typeof(Guid), new GuidTypeConverter());
            this.AddTypeConverter(typeof(SqlDateTime), typeof(DateTime), sqlDateTimeConverter);
            this.AddTypeConverter(typeof(DateTime), typeof(SqlDateTime), sqlDateTimeConverter);
        }

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
            var fromTypeDef = this.GetTypeDefinition(fromType);
            var toTypeDef = this.GetTypeDefinition(toType);

            if (toType.IsAssignableFrom(fromType))
            {
                return value => System.Convert.ChangeType(value, toType, formatProvider);
            }

            // Specialhandling when converting from string
            if (fromType == this.ConvertTypes[(int)TypeCode.String])
            {
                var stringConverter = this.GetStringConverter(toType, formatProvider);
                if (stringConverter != null)
                {
                    return stringConverter;
                }
            }

            // Specialhandling when converting to string
            if (toType == this.ConvertTypes[(int)TypeCode.String])
            {
                return value => this.ConvertToString(value, formatProvider);
            }

            //return this.GetTypeMap(fromType, toType).Convert;

            
            var toConverter = this.GetTypeConverter(toType, fromType);
            if (toConverter.CanConvertFrom(fromType))
            {
                this.AddTypeConverter(toType, fromType, toConverter);
                return value => toConverter.ConvertFrom(null, (CultureInfo)formatProvider, value);
            }

            var fromConverter = this.GetTypeConverter(fromType, toType);
            if (fromConverter.CanConvertTo(toType))
            {
                this.AddTypeConverter(fromType, toType, fromConverter);
                return value => fromConverter.ConvertTo(null, (CultureInfo)formatProvider, value, toType);
            }

            if (!fromType.IsClass && !toType.IsClass)
            {
                return value => System.Convert.ChangeType(value, toType, formatProvider);
            }

            // Create instance of GenericTypeConverter<TFrom, TTo> and add to list
            Type genericType = typeof(GenericTypeConverter<,>);
            Type[] typeArgs = { fromTypeDef.ActualType, toTypeDef.ActualType };
            Type typedGenericType = genericType.MakeGenericType(typeArgs);
            var genericTypeConverter = (TypeConverter)Activator.CreateInstance(typedGenericType, this, formatProvider);
            this.AddTypeConverter(fromType, toType, genericTypeConverter);
            return value => genericTypeConverter.ConvertTo(null, (CultureInfo)formatProvider, value, toType);
        }

        public void AddTypeConverter<TFrom, TTo>(TypeConverter typeConverter)
        {
            this.AddTypeConverter(typeof(TFrom), typeof(TTo), typeConverter);
        }

        public TypeDefinition GetTypeDefinition(Type type)
        {
            return this.typeDefinitions.AddOrUpdate(type, t => new TypeDefinition(t), (t, definition) => definition);
        }

        public TypeMap GetTypeMap(Type fromType, Type toType)
        {
            var key = string.Format("{0}{1}", fromType.FullName, toType.FullName);
            return this.typeMappers.GetOrAdd(key, t => new TypeMap(this.GetTypeDefinition(fromType), this.GetTypeDefinition(toType), this));
        }

        protected TypeConverter GetTypeConverter(Type fromType, Type toType)
        {
            var key = string.Format("{0}{1}", fromType.FullName, toType.FullName);
            TypeConverter typeConverter;
            if (this.typeConverters.TryGetValue(key, out typeConverter))
            {
                return typeConverter;
            }

            return TypeDescriptor.GetConverter(fromType);
        }

        protected virtual object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }

            return null;
        }

        protected virtual Func<object, object> GetStringConverter(Type toType, IFormatProvider formatProvider)
        {
            if (toType == this.ConvertTypes[(int)TypeCode.UInt16])
            {
                return value => StringParser.TryParseUInt16((string)value, formatProvider);
            }
            
            if (toType == this.ConvertTypes[(int)TypeCode.Int16])
            {
                return value => StringParser.TryParseInt16((string)value, formatProvider);
            }

            if (toType == this.ConvertTypes[(int)TypeCode.Int32])
            {
                return value => StringParser.TryParseInt32((string)value, formatProvider);
            }

            if (toType == this.ConvertTypes[(int)TypeCode.Int64])
            {
                return value => StringParser.TryParseInt64((string)value, formatProvider);
            }

            if (toType == this.ConvertTypes[(int)TypeCode.Decimal])
            {
                return value => StringParser.TryParseDecimal((string)value, formatProvider);
            }

            if (toType == typeof(Guid))
            {
                return value => StringParser.TryParseGuid((string)value, formatProvider);
            }

            if (toType == typeof(DateTime))
            {
                return value => StringParser.TryParseDateTime((string)value, formatProvider);
            }

            // Temporary find missing with reflection
            var typeCode = Type.GetTypeCode(toType);
            var mi = typeof(StringParser).GetMethod("TryParse" + typeCode);
            if (mi != null)
            {
                return value => mi.DelegateForCallMethod()(null, value, formatProvider);
            }

            return null;
        }

        protected virtual string ConvertToString(object value, IFormatProvider formatProvider)
        {
            var formatable = value as IFormattable;
            if (formatable != null)
            {
                return formatable.ToString(null, formatProvider);
            }

            var ic = value as IConvertible;
            if (ic != null)
            {
                return ic.ToString(formatProvider);
            }

            return value.ToString();
        }

        private void AddTypeConverter(Type fromType, Type toType, TypeConverter typeConverter)
        {
            var key = string.Format("{0}{1}", fromType.FullName, toType.FullName);
            this.typeConverters[key] = typeConverter;
        }
    }
}
