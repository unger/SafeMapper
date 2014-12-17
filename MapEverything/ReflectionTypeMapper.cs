namespace MapEverything
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;

    using Fasterflect;

    public class ReflectionTypeMapper : TypeMapperBase
    {
        private ConcurrentDictionary<string, ConverterInvoker> convertMethods = new ConcurrentDictionary<string, ConverterInvoker>();

        public override object Convert(object value, Type toType, IFormatProvider formatProvider)
        {
            if (value == null)
            {
                return this.GetDefaultValue(toType);
            }

            var converter = this.GetConverter(value.GetType(), toType, formatProvider);

            if (converter == null)
            {
                return this.GetDefaultValue(toType);
            }

            switch (converter.ConverterType)
            {
                case ConverterType.NoParams:
                    return converter.Method.Invoke(value, null);
                case ConverterType.OnlyFormatProvider:
                    return converter.Method.Invoke(value, formatProvider);
                case ConverterType.OnlyValue:
                    return converter.Method.Invoke(null, new[] { value });
                case ConverterType.ValueAndFormatProvider:
                    return converter.Method.Invoke(null, new[] { value, formatProvider });
            }

            return this.GetDefaultValue(toType);
        }

        private ConverterInvoker GetConverter(Type fromType, Type toType, IFormatProvider formatProvider)
        {
            var key = string.Format("{0};{1}", fromType.FullName, toType.FullName);

            return this.convertMethods.GetOrAdd(key, k => this.FindConverter(fromType, toType, formatProvider));
        }

        private ConverterInvoker FindConverter(Type fromType, Type toType, IFormatProvider formatProvider)
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

        private enum ConverterType
        {
            NoParams,
            OnlyValue,
            OnlyFormatProvider,
            ValueAndFormatProvider
        }

        private class ConverterInvoker
        {
            public MethodInvoker Method { get; set; }

            public ConverterType ConverterType { get; set; }
        }
    }
}
