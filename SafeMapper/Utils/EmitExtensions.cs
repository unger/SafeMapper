namespace SafeMapper.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using SafeMapper.Reflection;

    public static class EmitExtensions
    {
        public static void EmitConvertArray(this ILGenerator il, Type fromType, Type toType)
        {
            var fromLocal = il.DeclareLocal(fromType);
            var toLocal = il.DeclareLocal(toType);

            // Store value on top of stack into fromLocal
            il.Emit(OpCodes.Stloc, fromLocal);

            // Load length from fromLocal
            il.Emit(OpCodes.Ldloc, fromLocal);
            il.Emit(OpCodes.Ldlen);

            // Create new array and store it in toLocal
            il.Emit(OpCodes.Newarr, toLocal.LocalType.GetElementType());
            il.Emit(OpCodes.Stloc, toLocal);

            var fromElementType = fromType.GetElementType();
            var toElementType = toType.GetElementType();

            Label startLoop = il.DefineLabel();
            Label afterLoop = il.DefineLabel();
            LocalBuilder arrayIndex = il.DeclareLocal(typeof(int));

            il.Emit(OpCodes.Ldloc, fromLocal);
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
            il.Emit(OpCodes.Ldloc, toLocal);
            il.Emit(OpCodes.Ldloc, arrayIndex);

            // Ladda in fromarray på stacken
            il.Emit(OpCodes.Ldloc, fromLocal);
            il.Emit(OpCodes.Ldloc, arrayIndex);
            il.Emit(OpCodes.Ldelem, fromElementType);

            // Convert the element at the top of the stack to toElementType
            il.EmitConvertValue(fromElementType, toElementType);

            // Store the converted value
            il.Emit(OpCodes.Stelem, toElementType);

            // End loop
            il.Emit(OpCodes.Br, startLoop);
            il.MarkLabel(afterLoop);

            il.Emit(OpCodes.Ldloc, toLocal);
        }

        public static void EmitMemberMap(this ILGenerator il, LocalBuilder fromLocal, LocalBuilder toLocal, MemberWrapper fromMember, MemberWrapper toMember)
        {
            // Load toLocal as parameter for the setter
            il.Emit(toLocal.LocalType.IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, toLocal);

            // Load fromLocal as parameter for the getter
            il.Emit(fromLocal.LocalType.IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, fromLocal);

            if (fromMember.Member is PropertyInfo)
            {
                var getter = (fromMember.Member as PropertyInfo).GetGetMethod();
                il.Emit(OpCodes.Callvirt, getter);
            }
            else if (fromMember.Member is FieldInfo)
            {
                il.Emit(OpCodes.Ldfld, fromMember.Member as FieldInfo);
            }

            // Convert the value on top of the stack to the correct toType
            il.EmitConvertValue(fromMember.Type, toMember.Type);

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

        public static void EmitCallToString(this ILGenerator il, Type fromType)
        {
            if (fromType.IsValueType)
            {
                // Put property/field value in a local variable to be able to call instance method on it
                LocalBuilder localReturnType = il.DeclareLocal(fromType);
                il.Emit(OpCodes.Stloc, localReturnType);
                il.Emit(OpCodes.Ldloca, localReturnType);
            }

            var toStringMethod = fromType.GetMethod("ToString", new[] { typeof(IFormatProvider) });
            if (toStringMethod != null)
            {
                il.Emit(OpCodes.Ldarg_0); // IFormatProvider
                il.EmitCall(OpCodes.Callvirt, toStringMethod, null);
            }
            else
            {
                toStringMethod = fromType.GetMethod("ToString", Type.EmptyTypes);
                if (toStringMethod != null)
                {
                    il.EmitCall(OpCodes.Callvirt, toStringMethod, null);
                }
            }
        }

        public static void EmitConvertValue(this ILGenerator il, Type fromType, Type toType)
        {
            if (toType.IsAssignableFrom(fromType))
            {
                return;
            }

            if (toType == typeof(string) && fromType != typeof(string))
            {
                il.EmitCallToString(fromType);
            }
            else
            {
                var converter = ReflectionUtils.GetConvertMethod(fromType, toType, new[] { typeof(SafeConvert), typeof(Convert) });
                if (converter != null)
                {
                    // Load IFormatProvider as second argument
                    if (converter.GetParameters().Length == 2)
                    {
                        il.Emit(OpCodes.Ldarg_0);
                    }

                    il.EmitCall(OpCodes.Call, converter, null);
                }
                else
                {
                    var fromArrayType = fromType;
                    var toArrayType = toType;
                    var concreteFromType = ReflectionUtils.GetConcreteType(fromType);
                    var concreteToType = ReflectionUtils.GetConcreteType(toType);
                    var fromElementType = ReflectionUtils.GetElementType(concreteFromType);
                    var toElementType = ReflectionUtils.GetElementType(concreteToType);

                    if (ReflectionUtils.IsCollection(fromType) && ReflectionUtils.IsCollection(toType))
                    {
                        if (!fromType.IsArray)
                        {
                            var toArrayMethod = concreteFromType.GetMethod("ToArray", Type.EmptyTypes);

                            if (toArrayMethod == null)
                            {
                                toArrayMethod = typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(new[] { fromElementType });
                            }

                            if (toArrayMethod != null)
                            {
                                il.EmitCall(OpCodes.Call, toArrayMethod, null);
                                fromArrayType = fromElementType.MakeArrayType();
                            }
                        }

                        if (!toType.IsArray)
                        {
                            toArrayType = toElementType.MakeArrayType();
                        }

                        il.EmitConvertArray(fromArrayType, toArrayType);

                        if (!toType.IsArray)
                        {
                            var toEnumerableType = typeof(IEnumerable<>).MakeGenericType(toElementType);
                            var toConstructor = toType.GetConstructor(new[] { toEnumerableType });

                            if (toConstructor != null)
                            {
                                il.Emit(OpCodes.Newobj, toConstructor);
                            }
                        }
                    }
                    else
                    {
                        il.EmitConvertClass(fromType, toType);
                    }
                }
            }
        }

        public static void EmitConvertClass(this ILGenerator il, Type fromType, Type toType)
        {
            var fromLocal = il.DeclareLocal(fromType);
            var toLocal = il.DeclareLocal(toType);

            // Store value on top of stack into fromLocal
            il.Emit(OpCodes.Stloc, fromLocal);

            var ctor = toLocal.LocalType.GetConstructor(Type.EmptyTypes);
            if (ctor != null)
            {
                il.Emit(OpCodes.Newobj, ctor);
                il.Emit(OpCodes.Stloc, toLocal);
            }

            var memberMaps = ReflectionUtils.GetMemberMaps(fromType, toType);
            for (int i = 0; i < memberMaps.Count; i++)
            {
                il.EmitMemberMap(fromLocal, toLocal, memberMaps[i].Item1, memberMaps[i].Item2);
            }

            il.Emit(OpCodes.Ldloc, toLocal);
        }
    }
}
