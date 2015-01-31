namespace MapEverything.Utils
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using Fasterflect;

    using MapEverything.Reflection;
    using MapEverything.TypeMaps;

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
                il.NewObject(fromType, toType, local);

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
                    if (fromType.IsArray && toType.IsArray)
                    {
                        var fromElementType = fromType.GetElementType();
                        var toElementType = toType.GetElementType();

                        Label startLoop = il.DefineLabel();
                        Label afterLoop = il.DefineLabel();
                        LocalBuilder arrayIndex = il.DeclareLocal(typeof(int));

                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Ldlen);
                        il.Emit(OpCodes.Stloc, arrayIndex);

                        il.MarkLabel(startLoop);
                        il.Emit(OpCodes.Ldloc, arrayIndex);
                        il.Emit(OpCodes.Brfalse, afterLoop);
                        
                        il.Emit(OpCodes.Ldloc, arrayIndex);
                        il.Emit(OpCodes.Ldc_I4_1);
                        il.Emit(OpCodes.Sub);
                        il.Emit(OpCodes.Stloc, arrayIndex);

                        // Ladda in toarray på stacken
                        il.Emit(OpCodes.Ldloc, local);
                        il.Emit(OpCodes.Ldloc, arrayIndex);

                        // Ladda in fromarray på stacken
                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Ldloc, arrayIndex);
                        il.Emit(OpCodes.Ldelem, fromElementType);

                        if (toElementType == typeof(string) && fromElementType != typeof(string))
                        {
                            if (fromElementType.IsValueType)
                            {
                                // Put property/field value in a local variable to be able to call instance method on it
                                LocalBuilder localReturnType = il.DeclareLocal(fromElementType);
                                il.Emit(OpCodes.Stloc, localReturnType);
                                il.Emit(OpCodes.Ldloca, localReturnType);
                            }

                            il.CallToString(fromElementType);
                        }
                        else
                        {
                            var elementConverter = GetConvertMethod(fromElementType, toElementType);
                            if (elementConverter != null)
                            {
                                // Load IFormatProvider as second argument
                                if (elementConverter.Parameters().Count == 2)
                                {
                                    il.Emit(OpCodes.Ldarg_0);
                                }

                                il.EmitCall(OpCodes.Call, elementConverter, null);
                            }
                        }

                        // Store the converted value
                        il.Emit(OpCodes.Stelem, toElementType);

                        // End loop
                        il.Emit(OpCodes.Br, startLoop);
                        il.MarkLabel(afterLoop);
                    }
                    else
                    {
                        var memberMaps = GetMemberMaps(fromType, toType);
                        foreach (var memberMap in memberMaps)
                        {
                            il.MemberMap(local, fromType, memberMap.Item1, memberMap.Item2);
                        }
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

        private static IEnumerable<Tuple<MemberWrapper, MemberWrapper>> GetMemberMaps(Type fromType, Type toType)
        {
            var result = new List<Tuple<MemberWrapper, MemberWrapper>>();

            var fromMembers = ReflectionUtils.GetMembers(fromType);
            var toMembers = ReflectionUtils.GetMembers(toType).ToDictionary(m => m.Name);

            foreach (var fromMember in fromMembers)
            {
                if (fromMember.CanRead)
                {
                    if (toMembers.ContainsKey(fromMember.Name))
                    {
                        var toMember = toMembers[fromMember.Name];
                        if (toMember.CanWrite)
                        {
                            result.Add(new Tuple<MemberWrapper, MemberWrapper>(fromMember, toMember));
                        }
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
                                if (parameters.Count == 2 && parameters[1].ParameterType == typeof(IFormatProvider))
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

        private static void NewObject(this ILGenerator il, Type fromType, Type toType, LocalBuilder local)
        {
            if (toType.IsArray)
            {
                if (fromType.IsArray)
                {
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldlen);
                }
                else
                {
                    // Fallback to create zero length array
                    il.Emit(OpCodes.Ldc_I4_0);
                }

                il.Emit(OpCodes.Newarr, toType.GetElementType());
                il.Emit(OpCodes.Stloc, local);
            } 
            else
            {
                var ctor = toType.GetConstructor(Type.EmptyTypes);
                if (ctor != null)
                {
                    il.Emit(OpCodes.Newobj, ctor);
                    il.Emit(OpCodes.Stloc, local);
                }
            }
        }

        private static void MemberMap(this ILGenerator il, LocalBuilder local, Type fromType, MemberWrapper fromMember, MemberWrapper toMember)
        {
            // Load local as parameter for the setter
            il.Emit(local.LocalType.IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, local);

            // Load arg1 as parameter for the getter
            il.Emit(fromType.IsValueType ? OpCodes.Ldarga : OpCodes.Ldarg, 1);

            if (fromMember.Member is PropertyInfo)
            {
                var getter = (fromMember.Member as PropertyInfo).GetGetMethod();
                il.Emit(OpCodes.Callvirt, getter);
            }
            else if (fromMember.Member is FieldInfo)
            {
                il.Emit(OpCodes.Ldfld, fromMember.Member as FieldInfo);
            }

            if (toMember.Type == typeof(string) && fromMember.Type != typeof(string))
            {
                if (fromMember.Type.IsValueType)
                {
                    // Put property/field value in a local variable to be able to call instance method on it
                    LocalBuilder localReturnType = il.DeclareLocal(fromMember.Type);
                    il.Emit(OpCodes.Stloc, localReturnType);
                    il.Emit(OpCodes.Ldloca, localReturnType);
                }

                il.CallToString(fromMember.Type);
            }
            else
            {
                var memberConverter = GetConvertMethod(fromMember.Type, toMember.Type);
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

            if (toMember.Member is PropertyInfo)
            {
                var setter = (toMember.Member as PropertyInfo).GetSetMethod();
                il.Emit(OpCodes.Callvirt, setter);
            }
            else if (toMember.Member is FieldInfo)
            {
                il.Emit(OpCodes.Stfld, toMember.Member as FieldInfo);
            }
        }
    }
}
