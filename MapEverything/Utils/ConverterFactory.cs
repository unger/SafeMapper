namespace MapEverything.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.CompilerServices;

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
            else
            {
                var memberMaps = GetMemberMaps(fromType, toType);

                foreach (var memberMap in memberMaps)
                {
                    il.MemberMap(local, memberMap.Item1, memberMap.Item2);
                }
            }

            il.Emit(OpCodes.Ldloc, local);
            il.Emit(OpCodes.Ret);

            return (Converter<TFrom, TTo>)convertDynamicMethod.CreateDelegate(typeof(Converter<TFrom, TTo>));
        }

        private static IEnumerable<Tuple<MemberInfo, MemberInfo>> GetMemberMaps(Type fromType, Type toType)
        {
            var result = new List<Tuple<MemberInfo, MemberInfo>>();
            var fromMembers = fromType.GetMembers();

            foreach (var fromMember in fromMembers)
            {
                var fromMemberProperty = fromMember as PropertyInfo;
                if (fromMemberProperty != null)
                {
                    if (fromMemberProperty.CanRead)
                    {
                        var toMemberProperty = toType.GetProperty(fromMember.Name);
                        if (toMemberProperty != null)
                        {
                            if (toMemberProperty.CanWrite)
                            {
                                result.Add(new Tuple<MemberInfo, MemberInfo>(fromMemberProperty, toMemberProperty));
                            }
                        }
                    }    
                }
            }

            return result;
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

        private static void MemberMap(this ILGenerator il, LocalBuilder local, MemberInfo fromMember, MemberInfo toMember)
        {
            var fromMemberType = fromMember.Type();
            var toMemberType = toMember.Type();


            il.Emit(OpCodes.Ldloc, local);

            var fromMemberProperty = fromMember as PropertyInfo;
            if (fromMemberProperty != null)
            {
                var getter = fromMemberProperty.GetGetMethod();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Callvirt, getter);
            }

            var memberConverter = GetConvertMethod(fromMemberType, toMemberType);
            if (memberConverter != null)
            {
                il.EmitCall(OpCodes.Call, memberConverter, null);
            }

            var toMemberProperty = toMember as PropertyInfo;
            if (toMemberProperty != null)
            {
                var setter = toMemberProperty.GetSetMethod();
                il.Emit(OpCodes.Callvirt, setter);
            }
        }
    }
}
