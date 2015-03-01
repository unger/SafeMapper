namespace SafeMapper.Utils
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using SafeMapper.Reflection;

    public static class EmitExtensions
    {
        public static void EmitConvertArray(this ILGeneratorAdapter il, Type fromType, Type toType)
        {
            var fromLocal = il.DeclareLocal(fromType);
            var toLocal = il.DeclareLocal(toType);

            // Store value on top of stack into fromLocal
            il.EmitLocal(OpCodes.Stloc, fromLocal);

            // Load length from fromLocal
            il.EmitLocal(OpCodes.Ldloc, fromLocal);
            il.Emit(OpCodes.Ldlen);

            // Create new array and store it in toLocal
            il.Emit(OpCodes.Newarr, toLocal.LocalType.GetElementType());
            il.EmitLocal(OpCodes.Stloc, toLocal);

            var fromElementType = fromType.GetElementType();
            var toElementType = toType.GetElementType();

            Label startLoop = il.DefineLabel();
            Label afterLoop = il.DefineLabel();
            LocalBuilder arrayIndex = il.DeclareLocal(typeof(int));

            il.EmitLocal(OpCodes.Ldloc, fromLocal);
            il.Emit(OpCodes.Ldlen);
            il.EmitLocal(OpCodes.Stloc, arrayIndex);

            il.MarkLabel(startLoop);
            il.EmitLocal(OpCodes.Ldloc, arrayIndex);
            il.EmitBreak(OpCodes.Brfalse, afterLoop);

            il.EmitLocal(OpCodes.Ldloc, arrayIndex);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Sub);
            il.EmitLocal(OpCodes.Stloc, arrayIndex);

            // Ladda in toarray på stacken
            il.EmitLocal(OpCodes.Ldloc, toLocal);
            il.EmitLocal(OpCodes.Ldloc, arrayIndex);

            // Ladda in fromarray på stacken
            il.EmitLocal(OpCodes.Ldloc, fromLocal);
            il.EmitLocal(OpCodes.Ldloc, arrayIndex);
            il.Emit(OpCodes.Ldelem, fromElementType);

            // Convert the element at the top of the stack to toElementType
            il.EmitConvertValue(fromElementType, toElementType);

            // Store the converted value
            il.Emit(OpCodes.Stelem, toElementType);

            // End loop
            il.EmitBreak(OpCodes.Br, startLoop);
            il.MarkLabel(afterLoop);

            il.EmitLocal(OpCodes.Ldloc, toLocal);
        }

        public static void EmitMemberMap(
            this ILGeneratorAdapter il,
            LocalBuilder fromLocal,
            LocalBuilder toLocal,
            MemberWrapper fromMember,
            MemberWrapper toMember)
        {
            // Load toLocal as parameter for the setter
            il.EmitLocal(toLocal.LocalType.IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, toLocal);
            if (toMember.MemberSetterType == MemberType.StringIndexer)
            {
                il.EmitString(toMember.Name);
            }

            // Load fromLocal as parameter for the getter
            il.EmitLocal(fromLocal.LocalType.IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, fromLocal);

            if (fromMember.MemberGetter is PropertyInfo)
            {
                var getter = (fromMember.MemberGetter as PropertyInfo).GetGetMethod();
                if (fromMember.MemberGetterType == MemberType.StringIndexer)
                {
                    il.EmitString(fromMember.Name);
                }

                il.EmitCall(OpCodes.Callvirt, getter, null);
            }
            else if (fromMember.MemberGetter is FieldInfo)
            {
                il.EmitField(OpCodes.Ldfld, fromMember.MemberGetter as FieldInfo);
            }

            // Convert the value on top of the stack to the correct toType
            il.EmitConvertValue(fromMember.Type, toMember.Type);

            if (toMember.MemberSetter is PropertyInfo)
            {
                var setter = (toMember.MemberSetter as PropertyInfo).GetSetMethod();
                il.EmitCall(OpCodes.Callvirt, setter, null);
            }
            else if (toMember.MemberSetter is FieldInfo)
            {
                il.EmitField(OpCodes.Stfld, toMember.MemberSetter as FieldInfo);
            }
        }

        public static void EmitValueTypeBox(this ILGeneratorAdapter il, Type fromType)
        {
            if (fromType.IsValueType)
            {
                // Put property/field value in a local variable to be able to call instance method on it
                LocalBuilder localReturnType = il.DeclareLocal(fromType);
                il.EmitLocal(OpCodes.Stloc, localReturnType);
                if (fromType.IsEnum)
                {
                    il.EmitLocal(OpCodes.Ldloc, localReturnType);
                    il.Emit(OpCodes.Box, fromType);
                }
                else
                {
                    il.EmitLocal(OpCodes.Ldloca, localReturnType);
                }
            }
        }

        public static void EmitCallToString(this ILGeneratorAdapter il, Type fromType)
        {
            il.EmitValueTypeBox(fromType);

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

        public static void EmitConvertValue(this ILGeneratorAdapter il, Type fromType, Type toType)
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
                il.EmitLocal(OpCodes.Stloc, fromLocal);

                if (Nullable.GetUnderlyingType(fromType) != null)
                {
                    il.EmitLocal(OpCodes.Ldloca, fromLocal);
                    MethodInfo mi = fromType.GetMethod("get_HasValue", BindingFlags.Instance | BindingFlags.Public);
                    il.EmitCall(OpCodes.Call, mi, null);
                }
                else
                {
                    il.EmitLocal(OpCodes.Ldloc, fromLocal);
                }

                il.EmitBreak(OpCodes.Brtrue_S, nonNull);

                // Load toLocal with default value on stack and skip rest of conversion logic
                il.EmitLocal(OpCodes.Ldloc, toLocal);
                il.EmitBreak(OpCodes.Br, skipConversion);

                // Not null, put the fromValue back on stack
                il.MarkLabel(nonNull);
                il.EmitLocal(OpCodes.Ldloc, fromLocal);
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
                            il.EmitNewobj(toConstructor);
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

        public static void EmitConvertFromEnum(this ILGeneratorAdapter il, Type fromType, Type toType)
        {
            if (!fromType.IsEnum)
            {
                throw new ArgumentException("fromType needs to be an enum", "fromType");
            }

            if (toType == typeof(string))
            {
                var enumValues = Enum.GetValues(fromType);
                var switchType = fromType.GetEnumUnderlyingType();
                var switchReturnValues = new List<Tuple<object, object>>();

                foreach (var enumValue in enumValues)
                {
                    var enumDisplay = AttributeHelper.GetEnumDisplayValue((Enum)enumValue);
                    var enumDescription = AttributeHelper.GetEnumDescriptionValue((Enum)enumValue);

                    if (!string.IsNullOrEmpty(enumDisplay))
                    {
                        switchReturnValues.Add(new Tuple<object, object>(enumValue, enumDisplay));
                    }
                    else if (!string.IsNullOrEmpty(enumDescription))
                    {
                        switchReturnValues.Add(new Tuple<object, object>(enumValue, enumDescription));
                    }
                    else
                    {
                        switchReturnValues.Add(new Tuple<object, object>(enumValue, enumValue.ToString()));
                    }
                }

                il.EmitSwitchCases(switchType, toType, switchReturnValues);
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

        public static void EmitConvertToEnum(this ILGeneratorAdapter il, Type fromType, Type toType)
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
                    var enumDisplay = AttributeHelper.GetEnumDisplayValue((Enum)enumValue);
                    var enumDescription = AttributeHelper.GetEnumDescriptionValue((Enum)enumValue);

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

            il.EmitSwitchCases(switchType, underlayingToType, switchReturnValues);
        }

        public static void EmitSwitchCases(this ILGeneratorAdapter il, Type switchType, Type returnType, List<Tuple<object, object>> switchReturnValues)
        {
            Label defaultCase = il.DefineLabel();
            Label endOfMethod = il.DefineLabel();
            LocalBuilder switchValue = il.DeclareLocal(switchType);

            il.EmitLocal(OpCodes.Stloc, switchValue);

            var jumpTable = new Label[switchReturnValues.Count];
            for (int i = 0; i < switchReturnValues.Count; i++)
            {
                jumpTable[i] = il.DefineLabel();
                il.EmitLocal(OpCodes.Ldloc, switchValue);
                il.EmitLoadEnumValue(switchType, switchReturnValues[i].Item1);
                if (switchType == typeof(string))
                {
                    var stringEquals = typeof(string).GetMethod("op_Equality", new[] { typeof(string), typeof(string) });
                    il.EmitCall(OpCodes.Call, stringEquals, null);
                    il.Emit(OpCodes.Ldc_I4_1);
                }

                il.EmitBreak(OpCodes.Beq, jumpTable[i]);
            }

            // Branch on default case
            il.EmitBreak(OpCodes.Br_S, defaultCase);

            for (int i = 0; i < switchReturnValues.Count; i++)
            {
                il.MarkLabel(jumpTable[i]);
                il.EmitLoadEnumValue(returnType, switchReturnValues[i].Item2);
                il.EmitBreak(OpCodes.Br_S, endOfMethod);
            }

            // Default case
            il.MarkLabel(defaultCase);
            il.EmitLoadEnumValue(returnType, switchReturnValues[0].Item2);

            il.MarkLabel(endOfMethod);
        }

        public static void EmitConvertClass(this ILGeneratorAdapter il, Type fromType, Type toType)
        {
            var fromLocal = il.DeclareLocal(fromType);
            var toLocal = il.DeclareLocal(toType);

            // Store value on top of stack into fromLocal
            il.EmitLocal(OpCodes.Stloc, fromLocal);

            var ctor = toLocal.LocalType.GetConstructor(Type.EmptyTypes);
            if (ctor != null)
            {
                il.EmitNewobj(ctor);
                il.EmitLocal(OpCodes.Stloc, toLocal);
            }

            var memberMaps = ReflectionUtils.GetMemberMaps(fromType, toType);
            for (int i = 0; i < memberMaps.Count; i++)
            {
                il.EmitMemberMap(fromLocal, toLocal, memberMaps[i].Item1, memberMaps[i].Item2);
            }

            il.EmitLocal(OpCodes.Ldloc, toLocal);
        }

        private static void EmitLoadEnumValue(this ILGeneratorAdapter il, Type type, object enumValue)
        {
            if (type == typeof(string))
            {
                il.EmitString((string)enumValue);
            }
            else if (type == typeof(byte))
            {
                il.EmitByte((byte)enumValue);
            }
            else if (type == typeof(sbyte))
            {
                il.EmitSByte((sbyte)enumValue);
            }
            else if (type == typeof(short))
            {
                il.EmitShort((short)enumValue);
            }
            else if (type == typeof(ushort))
            {
                il.EmitUShort((ushort)enumValue);
            }
            else if (type == typeof(int))
            {
                il.EmitInt((int)enumValue);
            }
            else if (type == typeof(uint))
            {
                il.EmitUInt((uint)enumValue);
            }
            else if (type == typeof(long))
            {
                il.EmitLong((long)enumValue);
            }
            else if (type == typeof(ulong))
            {
                il.EmitULong((ulong)enumValue);
            }
        }
    }
}
