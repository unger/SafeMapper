namespace MapEverything
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;

    using Fasterflect;

    public class ReflectionTypeMapper : TypeMapper
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
        
        private ConcurrentDictionary<string, ConverterInvoker> convertMethods = new ConcurrentDictionary<string, ConverterInvoker>();

        private enum ConverterType
        {
            NoParams,
            OnlyValue,
            OnlyFormatProvider,
            ValueAndFormatProvider
        }

        public override Func<object, object> GetConverter(Type fromType, Type toType, IFormatProvider formatProvider)
        {
            var converter = this.GetConverterInvoker(fromType, toType);

            if (converter != null)
            {
                switch (converter.ConverterType)
                {
                    case ConverterType.NoParams:
                        return value => converter.Method(value.WrapIfValueType(), null);
                    case ConverterType.OnlyFormatProvider:
                        return value => converter.Method(value.WrapIfValueType(), formatProvider);
                    case ConverterType.OnlyValue:
                        return value => converter.Method(null, new[] { value });
                    case ConverterType.ValueAndFormatProvider:
                        return value => converter.Method(null, new[] { value, formatProvider });
                }
            }

            return base.GetConverter(fromType, toType, formatProvider);
        }

        private ConverterInvoker GetConverterInvoker(Type fromType, Type toType)
        {
            var key = string.Format("{0};{1}", fromType.FullName, toType.FullName);

            return this.convertMethods.GetOrAdd(key, k => this.FindConverterInvoker(fromType, toType));
        }

        private ConverterInvoker FindConverterInvoker(Type fromType, Type toType)
        {
            MethodInfo methodInfo;

            if (toType == this.ConvertTypes[(int)TypeCode.String])
            {
                methodInfo = fromType.GetMethod("ToString", new[] { typeof(IFormatProvider) });
                if (methodInfo != null)
                {
                    return new ConverterInvoker { Method = methodInfo.DelegateForCallMethod(), ConverterType = ConverterType.OnlyFormatProvider };
                }

                methodInfo = fromType.GetMethod("ToString", new Type[0]);
                if (methodInfo != null)
                {
                    return new ConverterInvoker { Method = methodInfo.DelegateForCallMethod(), ConverterType = ConverterType.NoParams };
                }
            }

            methodInfo = toType.GetMethod("Parse", new[] { fromType, typeof(IFormatProvider) });
            if (methodInfo != null)
            {
                return new ConverterInvoker { Method = methodInfo.DelegateForCallMethod(), ConverterType = ConverterType.ValueAndFormatProvider };
            }

            methodInfo = toType.GetMethod("Parse", new[] { fromType });
            if (methodInfo != null)
            {
                return new ConverterInvoker { Method = methodInfo.DelegateForCallMethod(), ConverterType = ConverterType.OnlyValue };
            }

            return null;
        }

        private class ConverterInvoker
        {
            public MethodInvoker Method { get; set; }

            public ConverterType ConverterType { get; set; }
        }
    }
}
