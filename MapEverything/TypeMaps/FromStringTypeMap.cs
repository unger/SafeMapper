namespace MapEverything.TypeMaps
{
    using System;

    using Fasterflect;

    using MapEverything.Utils;

    public class FromStringTypeMap : ITypeMap
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
        
        public FromStringTypeMap(Type toType, IFormatProvider formatProvider)
        {
            this.Convert = this.GetStringConverter(toType, formatProvider);
        }

        public Func<object, object> Convert { get; private set; }

        protected Func<object, object> GetStringConverter(Type toType, IFormatProvider formatProvider)
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
    }
}
