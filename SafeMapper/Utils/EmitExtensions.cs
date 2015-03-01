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

        public static void EmitMemberMap(
            this ILGenerator il,
            LocalBuilder fromLocal,
            LocalBuilder toLocal,
            MemberWrapper fromMember,
            MemberWrapper toMember)
        {
            // Load toLocal as parameter for the setter
            il.Emit(toLocal.LocalType.IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, toLocal);
            if (toMember.MemberSetterType == MemberType.StringIndexer)
            {
                il.Emit(OpCodes.Ldstr, toMember.Name);
            }

            // Load fromLocal as parameter for the getter
            il.Emit(fromLocal.LocalType.IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, fromLocal);

            if (fromMember.MemberGetter is PropertyInfo)
            {
                var getter = (fromMember.MemberGetter as PropertyInfo).GetGetMethod();
                if (fromMember.MemberGetterType == MemberType.StringIndexer)
                {
                    il.Emit(OpCodes.Ldstr, fromMember.Name);
                }

                il.Emit(OpCodes.Callvirt, getter);
            }
            else if (fromMember.MemberGetter is FieldInfo)
            {
                il.Emit(OpCodes.Ldfld, fromMember.MemberGetter as FieldInfo);
            }

            // Convert the value on top of the stack to the correct toType
            il.EmitConvertValue(fromMember.Type, toMember.Type);

            if (toMember.MemberSetter is PropertyInfo)
            {
                var setter = (toMember.MemberSetter as PropertyInfo).GetSetMethod();
                il.Emit(OpCodes.Callvirt, setter);
            }
            else if (toMember.MemberSetter is FieldInfo)
            {
                il.Emit(OpCodes.Stfld, toMember.MemberSetter as FieldInfo);
            }
        }

        public static void EmitCallToString(this ILGenerator il, Type fromType)
        {
            if (fromType.IsValueType)
            {
                // Put property/field value in a local variable to be able to call instance method on it
                LocalBuilder localReturnType = il.DeclareLocal(fromType);
                il.Emit(OpCodes.Stloc, localReturnType);
                if (fromType.IsEnum)
                {
                    il.Emit(OpCodes.Ldloc, localReturnType);
                    il.Emit(OpCodes.Box, fromType);
                }
                else
                {
                    il.Emit(OpCodes.Ldloca, localReturnType);
                }
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
                il.EmitCall(OpCodes.Callvirt, toStringMethod, null);
            }
        }

        public static void EmitConvertValue(this ILGenerator il, Type fromType, Type toType)
        {
            Label skipConversion = il.DefineLabel();

            if (toType.IsAssignableFrom(fromType))
            {
                return;
            }

            // Check if fromValue is null
            if (!fromType.IsValueType || Nullable.GetUnderlyingType(fromType) != null)
            {
                var fromLocal = il.DeclareLocal(fromType);
                var toLocal = il.DeclareLocal(toType);
                Label nonNull = il.DefineLabel();

                // Store value on top of stack into fromLocal
                il.Emit(OpCodes.Stloc, fromLocal);

                if (Nullable.GetUnderlyingType(fromType) != null)
                {
                    il.Emit(OpCodes.Ldloca, fromLocal);
                    MethodInfo mi = fromType.GetMethod("get_HasValue", BindingFlags.Instance | BindingFlags.Public);
                    il.Emit(OpCodes.Call, mi);
                }
                else
                {
                    il.Emit(OpCodes.Ldloc, fromLocal);
                }

                il.Emit(OpCodes.Brtrue_S, nonNull);

                // Load toLocal with default value on stack and skip rest of conversion logic
                il.Emit(OpCodes.Ldloc, toLocal);
                il.Emit(OpCodes.Br, skipConversion);

                // Not null, put the fromValue back on stack
                il.MarkLabel(nonNull);
                il.Emit(OpCodes.Ldloc, fromLocal);
            }

            var converter = ReflectionUtils.GetConvertMethod(
                fromType,
                toType,
                new[] { typeof(SafeConvert) });

            if (converter != null)
            {
                // Load IFormatProvider as second argument
                if (converter.GetParameters().Length == 2)
                {
                    il.Emit(OpCodes.Ldarg_0);
                }

                il.EmitCall(OpCodes.Call, converter, null);
            }
            else if (toType.IsEnum)
            {
                il.EmitConvertToEnum(fromType, toType);
            }
            else if (fromType.IsEnum)
            {
                il.EmitConvertFromEnum(fromType, toType);
            }
            else if (toType == typeof(string) && fromType != typeof(string))
            {
                il.EmitCallToString(fromType);
            }
            else
            {
                if (ReflectionUtils.IsCollection(fromType) && ReflectionUtils.IsCollection(toType))
                {
                    var fromArrayType = fromType;
                    var toArrayType = toType;
                    var concreteFromType = ReflectionUtils.GetConcreteType(fromType);
                    var concreteToType = ReflectionUtils.GetConcreteType(toType);
                    var fromElementType = ReflectionUtils.GetElementType(concreteFromType);
                    var toElementType = ReflectionUtils.GetElementType(concreteToType);

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

            il.MarkLabel(skipConversion);
        }

        public static void EmitConvertFromEnum(this ILGenerator il, Type fromType, Type toType)
        {
            if (!fromType.IsEnum)
            {
                throw new ArgumentException("fromType needs to be an enum", "fromType");
            }

            // TODO: Add handling for all numeric values
            if (toType == typeof(string))
            {
                il.EmitCallToString(fromType);
            }
            else
            {
                var underlayingFromType = fromType.GetEnumUnderlyingType();

                if (toType != underlayingFromType)
                {
                    var converter = ReflectionUtils.GetConvertMethod(
                        underlayingFromType,
                        toType,
                        new[] { typeof(SafeConvert) });

                    if (converter != null && converter.GetParameters().Length == 1)
                    {
                        il.EmitCall(OpCodes.Call, converter, null);
                    }
                }
            }
        }

        public static void EmitConvertToEnum(this ILGenerator il, Type fromType, Type toType)
        {
            if (!toType.IsEnum)
            {
                throw new ArgumentException("toType needs to be an enum", "toType");
            }

            var enumValues = Enum.GetValues(toType);
            var underlayingToType = toType.GetEnumUnderlyingType();
            var underlayingFromType = fromType.IsEnum ? fromType.GetEnumUnderlyingType() : fromType;
            var switchType = fromType == typeof(string) ? typeof(string) : underlayingToType;
            var switchReturnValues = new List<Tuple<object, object>>();

            foreach (var enumValue in enumValues)
            {
                if (fromType == typeof(string))
                {
                    var enumText = enumValue.ToString();
                    var enumDisplay = AttributeHelper.GetDisplayValue((Enum)enumValue);
                    var enumDescription = AttributeHelper.GetDescriptionValue((Enum)enumValue);

                    if (!string.IsNullOrEmpty(enumDisplay))
                    {
                        switchReturnValues.Add(new Tuple<object, object>(enumDisplay, enumValue));
                    }

                    if (!string.IsNullOrEmpty(enumDescription))
                    {
                        switchReturnValues.Add(new Tuple<object, object>(enumDescription, enumValue));
                    }

                    switchReturnValues.Add(new Tuple<object, object>(enumText, enumValue));
                }
                else
                {
                    switchReturnValues.Add(new Tuple<object, object>(enumValue, enumValue));
                }
            }

            if (fromType != typeof(string) && underlayingToType != underlayingFromType)
            {
                var converter = ReflectionUtils.GetConvertMethod(
                    underlayingFromType,
                    underlayingToType,
                    new[] { typeof(SafeConvert) });

                if (converter != null && converter.GetParameters().Length == 1)
                {
                    il.EmitCall(OpCodes.Call, converter, null);
                }
                else
                {
                    // if it is not possible to convert load enum default value
                    il.Emit(OpCodes.Pop);
                    il.EmitLoadEnumValue(underlayingToType, enumValues.GetValue(0));
                    return;
                }
            }

            Label defaultCase = il.DefineLabel();
            Label endOfMethod = il.DefineLabel();
            LocalBuilder switchValue = il.DeclareLocal(switchType);

            il.Emit(OpCodes.Stloc, switchValue);

            var jumpTable = new Label[switchReturnValues.Count];
            for (int i = 0; i < switchReturnValues.Count; i++)
            {
                jumpTable[i] = il.DefineLabel();
                il.Emit(OpCodes.Ldloc, switchValue);
                il.EmitLoadEnumValue(switchType, switchReturnValues[i].Item1);
                if (fromType == typeof(string))
                {
                    var stringEquals = typeof(string).GetMethod("op_Equality", new[] { typeof(string), typeof(string) });
                    il.EmitCall(OpCodes.Call, stringEquals, null);
                    il.Emit(OpCodes.Ldc_I4_1);
                }

                il.Emit(OpCodes.Beq, jumpTable[i]);
            }

            // Branch on default case
            il.Emit(OpCodes.Br_S, defaultCase);

            for (int i = 0; i < switchReturnValues.Count; i++)
            {
                il.MarkLabel(jumpTable[i]);
                il.EmitLoadEnumValue(underlayingToType, switchReturnValues[i].Item2);
                il.Emit(OpCodes.Br_S, endOfMethod);
            }

            // Default case
            il.MarkLabel(defaultCase);
            il.EmitLoadEnumValue(underlayingToType, enumValues.GetValue(0));

            il.MarkLabel(endOfMethod);
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

        private static void EmitLoadEnumValue(this ILGenerator il, Type type, object enumValue)
        {
            if (type == typeof(string))
            {
                il.Emit(OpCodes.Ldstr, (string)enumValue);
            }
            else if (type == typeof(byte))
            {
                il.Emit(OpCodes.Ldc_I4, (int)(byte)enumValue);
                il.Emit(OpCodes.Conv_U1);
            }
            else if (type == typeof(sbyte))
            {
                il.Emit(OpCodes.Ldc_I4, (int)(sbyte)enumValue);
                il.Emit(OpCodes.Conv_I1);
            }
            else if (type == typeof(short))
            {
                il.Emit(OpCodes.Ldc_I4, (int)(short)enumValue);
                il.Emit(OpCodes.Conv_I2);
            }
            else if (type == typeof(ushort))
            {
                il.Emit(OpCodes.Ldc_I4, (ushort)enumValue);
                il.Emit(OpCodes.Conv_U2);
            }
            else if (type == typeof(int))
            {
                il.Emit(OpCodes.Ldc_I4, (int)enumValue);
            }
            else if (type == typeof(uint))
            {
                il.Emit(OpCodes.Ldc_I4, (int)(uint)enumValue);
                il.Emit(OpCodes.Conv_U4);
            }
            else if (type == typeof(long))
            {
                il.Emit(OpCodes.Ldc_I8, (long)enumValue);
            }
            else if (type == typeof(ulong))
            {
                il.Emit(OpCodes.Ldc_I8, (long)(ulong)enumValue);
                il.Emit(OpCodes.Conv_U8);
            }
        }
    }
}
