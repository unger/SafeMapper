namespace SafeMapper.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection.Emit;

    using SafeMapper.Configuration;

    public class ConverterFactory
    {
        private readonly MapConfiguration mapCfg;

        public ConverterFactory(MapConfiguration configuration)
        {
            this.mapCfg = configuration;
        }

        public Func<object, object> CreateDelegate(Type fromType, Type toType)
        {
            return this.CreateDelegate(fromType, toType, CultureInfo.CurrentCulture);
        }

        public Func<object, object> CreateDelegate(Type fromType, Type toType, IFormatProvider provider)
        {
            var convertDynamicMethod = new DynamicMethod(
                "ConvertFrom" + fromType.Name + "To" + toType.Name + "NonGeneric",
                typeof(object),
                new[] { typeof(IFormatProvider), typeof(object) },
                typeof(ConverterFactory).Module);

            var il = new ILGeneratorAdapter(convertDynamicMethod.GetILGenerator(), this.mapCfg);

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(fromType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, fromType); // cast input to correct type
            il.EmitConvertValue(fromType, toType, new HashSet<Type>());
            il.Emit(OpCodes.Box, toType);
            il.Emit(OpCodes.Ret);

            return (Func<object, object>)convertDynamicMethod.CreateDelegate(typeof(Func<object, object>), provider);
        }

        public Converter<TFrom, TTo> CreateDelegate<TFrom, TTo>()
        {
            return this.CreateDelegate<TFrom, TTo>(CultureInfo.CurrentCulture);
        }

        public Converter<TFrom, TTo> CreateDelegate<TFrom, TTo>(IFormatProvider provider)
        {
            var toType = typeof(TTo);
            var fromType = typeof(TFrom);

            var convertDynamicMethod = new DynamicMethod(
                "ConvertFrom" + fromType.Name + "To" + toType.Name,
                toType,
                new[] { typeof(IFormatProvider), fromType },
                typeof(ConverterFactory).Module);

            var il = new ILGeneratorAdapter(convertDynamicMethod.GetILGenerator(), this.mapCfg);

            il.Emit(OpCodes.Ldarg_1);
            il.EmitConvertValue(fromType, toType, new HashSet<Type>());
            il.Emit(OpCodes.Ret);

            return (Converter<TFrom, TTo>)convertDynamicMethod.CreateDelegate(typeof(Converter<TFrom, TTo>), provider);
        }
    }
}
