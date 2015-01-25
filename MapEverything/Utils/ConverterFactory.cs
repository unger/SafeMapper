namespace MapEverything.Utils
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Reflection.Emit;

    using Fasterflect;

    public static class ConverterFactory
    {
        private static ConcurrentDictionary<string, object> converterCache = new ConcurrentDictionary<string, object>();

        public static Converter<TFrom, TTo> Create<TFrom, TTo>()
        {
            return Create<TFrom, TTo>(CultureInfo.CurrentCulture);
        }

        public static Converter<TFrom, TTo> Create<TFrom, TTo>(IFormatProvider provider)
        {
            return (Converter<TFrom, TTo>)converterCache.GetOrAdd(
                string.Concat(typeof(TTo).FullName, typeof(TFrom).FullName),
                k => CreateDelegate<TFrom, TTo>(provider));
        }

        private static Converter<TFrom, TTo> CreateDelegate<TFrom, TTo>(IFormatProvider provider)
        {
            var toType = typeof(TTo);
            var fromType = typeof(TFrom);

            var convertDynamicMethod = new DynamicMethod(
                "ConvertFrom" + fromType.Name + "To" + toType.Name,
                toType,
                new[] { typeof(IFormatProvider), fromType },
                typeof(ConverterFactory).Module);

            var il = convertDynamicMethod.GetILGenerator();

            if (toType.IsAssignableFrom(fromType))
            {
                il.Emit(OpCodes.Ldarg_1);
            }
            else if (toType == typeof(string) && fromType != typeof(string))
            {
                il.Emit(OpCodes.Ldarga, 1); // fromObject
                il.CallToString(fromType);
            }
            else
            {
                var local = il.DeclareLocal(toType);
                il.NewObject(toType, local);

                var methodInfo = GetConvertMethod(fromType, toType);
                if (methodInfo != null)
                {
                    il.Emit(OpCodes.Ldarg_1);

                    // Load IFormatProvider as second argument
                    if (methodInfo.Parameters().Count == 2)
                    {
                        il.Emit(OpCodes.Ldarg_0);
                    }

                    il.EmitCall(OpCodes.Call, methodInfo, null);
                    il.Emit(OpCodes.Stloc, local);
                }
                else
                {
                    var memberMaps = GetMemberMaps(fromType, toType);
                    foreach (var memberMap in memberMaps)
                    {
                        il.MemberMap(local, fromType, memberMap.Item1, memberMap.Item2);
                    }
                }

                il.Emit(OpCodes.Ldloc, local);
            }

            il.Emit(OpCodes.Ret);

            return (Converter<TFrom, TTo>)convertDynamicMethod.CreateDelegate(typeof(Converter<TFrom, TTo>), provider);
        }

        private static void CallToString(this ILGenerator il, Type type)
        {
            var toStringMethod = type.GetMethod("ToString", new[] { typeof(IFormatProvider) });
            if (toStringMethod != null)
            {
                il.Emit(OpCodes.Ldarg_0); // IFormatProvider
                il.EmitCall(OpCodes.Callvirt, toStringMethod, null);
            }
            else
            {
                toStringMethod = type.GetMethod("ToString", Type.EmptyTypes);
                if (toStringMethod != null)
                {
                    il.EmitCall(OpCodes.Callvirt, toStringMethod, null);
                }
            }
        }

        private static IEnumerable<Tuple<MemberInfo, MemberInfo>> GetMemberMaps(Type fromType, Type toType)
        {
            var result = new List<Tuple<MemberInfo, MemberInfo>>();
            var fromMembers = fromType.GetMembers();

            foreach (var fromMember in fromMembers)
            {
                MemberInfo validFromMember = null;
                var fromMemberProperty = fromMember as PropertyInfo;
                if (fromMemberProperty != null)
                {
                    if (fromMemberProperty.CanRead)
                    {
                        validFromMember = fromMemberProperty;
                    }
                }
                else
                {
                    var fromMemberField = fromMember as FieldInfo;
                    if (fromMemberField != null)
                    {
                        validFromMember = fromMemberField;
                    }
                }

                if (validFromMember != null)
                {
                    var toMemberProperty = toType.GetProperty(validFromMember.Name);
                    if (toMemberProperty != null)
                    {
                        if (toMemberProperty.CanWrite)
                        {
                            result.Add(new Tuple<MemberInfo, MemberInfo>(validFromMember, toMemberProperty));
                        }
                    }

                    var toMemberField = toType.GetField(validFromMember.Name);
                    if (toMemberField != null)
                    {
                        result.Add(new Tuple<MemberInfo, MemberInfo>(validFromMember, toMemberField));
                    }
                }
            }

            return result;
        }

        private static MethodInfo GetConvertMethod(Type fromType, Type toType)
        {
            if (toType.IsAssignableFrom(fromType))
            {
                return null;
            }

            var convertTypes = new[] { typeof(SafeConvert), typeof(Convert) };

            foreach (var convertType in convertTypes)
            {
                MethodInfo methodInfoWithoutFormatProvider = null;
                var methods = convertType.GetMethods();
                foreach (var method in methods)
                {
                    if (method.ReturnType == toType)
                    {
                        var parameters = method.Parameters();
                        if (parameters.Count >= 1)
                        {
                            if (parameters[0].ParameterType == fromType)
                            {
                                if (parameters.Count == 2 && parameters[0].ParameterType == typeof(IFormatProvider))
                                {
                                    return method;
                                }

                                if (parameters.Count == 1)
                                {
                                    methodInfoWithoutFormatProvider = method;
                                }
                            }
                        }
                    }
                }

                if (methodInfoWithoutFormatProvider != null)
                {
                    return methodInfoWithoutFormatProvider;
                }
            }

            return null;
        }

        private static void NewObject(this ILGenerator il, Type type, LocalBuilder local)
        {
            var ctor = type.GetConstructor(Type.EmptyTypes);
            if (ctor != null)
            {
                il.Emit(OpCodes.Newobj, ctor);
                il.Emit(OpCodes.Stloc, local);
            }
        }

        private static void MemberMap(this ILGenerator il, LocalBuilder local, Type fromType, MemberInfo fromMember, MemberInfo toMember)
        {
            var fromMemberType = fromMember.Type();
            var toMemberType = toMember.Type();

            // Load local as parameter for the setter
            il.Emit(local.LocalType.IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, local);

            // Load arg1 as parameter for the getter
            il.Emit(fromType.IsValueType ? OpCodes.Ldarga : OpCodes.Ldarg, 1);

            if (fromMember is PropertyInfo)
            {
                var getter = (fromMember as PropertyInfo).GetGetMethod();
                il.Emit(OpCodes.Callvirt, getter);
            }
            else if (fromMember is FieldInfo)
            {
                il.Emit(OpCodes.Ldfld, fromMember as FieldInfo);
            }

            if (toMemberType == typeof(string) && fromMemberType != typeof(string))
            {
                if (fromMemberType.IsValueType)
                {
                    // Put property/field value in a local variable to be able to call instance method on it
                    LocalBuilder localReturnType = il.DeclareLocal(fromMemberType);
                    il.Emit(OpCodes.Stloc, localReturnType);
                    il.Emit(OpCodes.Ldloca, localReturnType);
                }

                il.CallToString(fromMemberType);
            }
            else
            {
                var memberConverter = GetConvertMethod(fromMemberType, toMemberType);
                if (memberConverter != null)
                {
                    // Load IFormatProvider as second argument
                    if (memberConverter.Parameters().Count == 2)
                    {
                        il.Emit(OpCodes.Ldarg_0);
                    }

                    il.EmitCall(OpCodes.Call, memberConverter, null);
                }
            }

            if (toMember is PropertyInfo)
            {
                var setter = (toMember as PropertyInfo).GetSetMethod();
                il.Emit(OpCodes.Callvirt, setter);
            }
            else if (toMember is FieldInfo)
            {
                il.Emit(OpCodes.Stfld, toMember as FieldInfo);
            }
        }
    }
}
