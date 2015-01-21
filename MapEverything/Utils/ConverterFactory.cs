namespace MapEverything.Utils
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Reflection.Emit;

    using Fasterflect;

    public static class ConverterFactory
    {
        public static Converter<TFrom, TTo> Create<TFrom, TTo>()
        {
            return Create<TFrom, TTo>(CultureInfo.CurrentCulture);
        }

        public static Converter<TFrom, TTo> Create<TFrom, TTo>(IFormatProvider provider)
        {
            var toType = typeof(TTo);
            var fromType = typeof(TFrom);
            var convertDynamicMethod = new DynamicMethod(
                "ConvertFrom" + fromType.Name + "To" + toType.Name,
                toType,
                new[] { fromType },
                typeof(ConverterFactory).Module);

            var il = convertDynamicMethod.GetILGenerator();

            var local = il.DeclareLocal(toType);
            il.NewObject(toType, local);

            var methodInfo = GetConvertMethod(fromType, toType);
            if (methodInfo != null)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.EmitCall(OpCodes.Call, methodInfo, null);
                il.Emit(OpCodes.Stloc, local);
            }

            il.Emit(OpCodes.Ldloc, local);
            il.Emit(OpCodes.Ret);

            return (Converter<TFrom, TTo>)convertDynamicMethod.CreateDelegate(typeof(Converter<TFrom, TTo>));
        }

        private static MethodInfo GetConvertMethod(Type fromType, Type toType)
        {
            var convertTypes = new[] { typeof(SafeConvert), typeof(Convert) };

            foreach (var convertType in convertTypes)
            {
                var methods = convertType.GetMethods();
                foreach (var method in methods)
                {
                    if (method.ReturnType == toType)
                    {
                        var parameters = method.Parameters();
                        if (parameters.Count == 1)
                        {
                            if (parameters[0].ParameterType == fromType)
                            {
                                return method;
                            }
                        }
                    }
                }
            }

            return null;
        }

        private static void NewObject(this ILGenerator il, Type type, LocalBuilder local)
        {
            var ctor = type.GetConstructor(new Type[0]);
            if (ctor != null)
            {
                il.Emit(OpCodes.Newobj, ctor);
                il.Emit(OpCodes.Stloc, local);
            }
        }
    }
}
