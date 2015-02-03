namespace MapEverything.Utils
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using MapEverything.Reflection;

    public static class ConverterFactory
    {
        private static readonly ConcurrentDictionary<string, object> ConverterCache = new ConcurrentDictionary<string, object>();

        public delegate object MethodInvoker(object input);

        public static object Convert(object fromObject, Type fromType, Type toType)
        {
            return Create(fromType, toType, CultureInfo.CurrentCulture)(fromObject);
        }

        public static TTo Convert<TFrom, TTo>(TFrom fromObject)
        {
            return Create<TFrom, TTo>(CultureInfo.CurrentCulture)(fromObject);
        }

        public static Func<object, object> Create(Type fromType, Type toType)
        {
            return Create(fromType, toType, CultureInfo.CurrentCulture);
        }

        public static Func<object, object> Create(Type fromType, Type toType, IFormatProvider provider)
        {
            return (Func<object, object>)ConverterCache.GetOrAdd(
                string.Concat(toType.FullName, fromType.FullName, "NonGeneric"),
                k => CreateDelegate(fromType, toType, provider));
        }

        public static Converter<TFrom, TTo> Create<TFrom, TTo>()
        {
            return Create<TFrom, TTo>(CultureInfo.CurrentCulture);
        }

        public static Converter<TFrom, TTo> Create<TFrom, TTo>(IFormatProvider provider)
        {
            return (Converter<TFrom, TTo>)ConverterCache.GetOrAdd(
                string.Concat(typeof(TTo).FullName, typeof(TFrom).FullName),
                k => CreateDelegateGeneric<TFrom, TTo>(provider));
        }

        private static Func<object, object> CreateDelegate(Type fromType, Type toType, IFormatProvider provider)
        {
            var convertDynamicMethod = new DynamicMethod(
                "ConvertFrom" + fromType.Name + "To" + toType.Name + "NonGeneric",
                typeof(object),
                new[] { typeof(IFormatProvider), typeof(object) },
                typeof(ConverterFactory).Module);

            var il = convertDynamicMethod.GetILGenerator();

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(fromType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, fromType); // cast input to correct type
            il.EmitConvertValue(fromType, toType);
            il.Emit(OpCodes.Box, toType);
            il.Emit(OpCodes.Ret);

            return (Func<object, object>)convertDynamicMethod.CreateDelegate(typeof(Func<object, object>), provider);
        }

        private static Converter<TFrom, TTo> CreateDelegateGeneric<TFrom, TTo>(IFormatProvider provider)
        {
            var toType = typeof(TTo);
            var fromType = typeof(TFrom);

            var convertDynamicMethod = new DynamicMethod(
                "ConvertFrom" + fromType.Name + "To" + toType.Name,
                toType,
                new[] { typeof(IFormatProvider), fromType },
                typeof(ConverterFactory).Module);

            var il = convertDynamicMethod.GetILGenerator();

            il.Emit(OpCodes.Ldarg_1);
            il.EmitConvertValue(fromType, toType);
            il.Emit(OpCodes.Ret);

            return (Converter<TFrom, TTo>)convertDynamicMethod.CreateDelegate(typeof(Converter<TFrom, TTo>), provider);
        }
    }
}
