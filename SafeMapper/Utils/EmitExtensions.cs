namespace SafeMapper.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using SafeMapper.Configuration;
    using SafeMapper.Reflection;

    public static class EmitExtensions
    {
        public static void EmitConvertArray(this ILGeneratorAdapter il, Type fromType, Type toType, HashSet<Type> convertedTypes)
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
            il.EmitConvertValue(fromElementType, toElementType, new HashSet<Type>(convertedTypes));

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
            MemberMap memberMap,
            HashSet<Type> convertedTypes)
        {
            var skipMemberMap = il.DefineLabel();
            if (memberMap.FromMember.NeedsContainsCheck)
            {
                var containsKey = fromLocal.LocalType.GetMethod("ContainsKey", new[] { typeof(string) });
                il.EmitLocal(fromLocal.LocalType.IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, fromLocal);
                il.EmitString(memberMap.FromMember.Name);
                il.EmitCall(OpCodes.Call, containsKey, null);
                il.EmitBreak(OpCodes.Brfalse, skipMemberMap);
            }

            // Load toLocal as parameter for the setter
            il.EmitLocal(toLocal.LocalType.IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, toLocal);
            if (memberMap.ToMember.NeedsStringIndex)
            {
                il.EmitString(memberMap.ToMember.Name);
            }

            // Load fromLocal as parameter for the getter
            il.EmitLocal(fromLocal.LocalType.IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, fromLocal);

            if (memberMap.FromMember.MemberInfo is PropertyInfo)
            {
                var getter = (memberMap.FromMember.MemberInfo as PropertyInfo).GetGetMethod();
                if (memberMap.FromMember.MemberType == MemberType.StringIndexer)
                {
                    il.EmitString(memberMap.FromMember.Name);
                }

                il.EmitCall(OpCodes.Callvirt, getter, null);
            }
            else if (memberMap.FromMember.MemberInfo is FieldInfo)
            {
                il.EmitField(OpCodes.Ldfld, memberMap.FromMember.MemberInfo as FieldInfo);
            }
            else if (memberMap.FromMember.MemberInfo is MethodInfo)
            {
                var method = memberMap.FromMember.MemberInfo as MethodInfo;
                il.EmitString(memberMap.FromMember.Name);
                il.EmitCall(OpCodes.Callvirt, method, null);
            }

            // Convert the value on top of the stack to the correct toType
            il.EmitConvertValue(memberMap.FromMember.Type, memberMap.ToMember.Type, new HashSet<Type>(convertedTypes));

            if (memberMap.ToMember.MemberInfo is PropertyInfo)
            {
                var setter = (memberMap.ToMember.MemberInfo as PropertyInfo).GetSetMethod();
                il.EmitCall(OpCodes.Callvirt, setter, null);
            }
            else if (memberMap.ToMember.MemberInfo is FieldInfo)
            {
                il.EmitField(OpCodes.Stfld, memberMap.ToMember.MemberInfo as FieldInfo);
            }
            else if (memberMap.ToMember.MemberInfo is MethodInfo)
            {
                il.EmitCall(OpCodes.Call, memberMap.ToMember.MemberInfo as MethodInfo, null);
            }

            if (memberMap.FromMember.NeedsContainsCheck)
            {
                il.MarkLabel(skipMemberMap);
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

        public static void EmitConvertValue(this ILGeneratorAdapter il, Type fromType, Type toType, HashSet<Type> convertedTypes)
        {
            // Circular reference check
            if (!fromType.IsValueType && fromType != typeof(string))
            {
                if (convertedTypes.Contains(fromType))
                {
                    var toLocal = il.DeclareLocal(toType);
                    il.Emit(OpCodes.Pop);
                    il.EmitLocal(OpCodes.Ldloc, toLocal);
                    return;
                }

                convertedTypes.Add(fromType);
            }

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
            else if (ReflectionUtils.IsCollection(fromType) && ReflectionUtils.IsCollection(toType))
            {
                il.EmitConvertCollection(fromType, toType, convertedTypes);
            }
            else if (ReflectionUtils.IsCollection(fromType))
            {
                var concreteFromType = ReflectionUtils.GetConcreteType(fromType);
                var fromElementType = ReflectionUtils.GetElementType(concreteFromType);

                il.EmitFirstCollectionValue(concreteFromType, fromElementType);

                // Convert the element at the top of the stack to toType
                il.EmitConvertValue(fromElementType, toType, new HashSet<Type>(convertedTypes));
            }
            else if (ReflectionUtils.IsCollection(toType))
            {
                var concreteToType = ReflectionUtils.GetConcreteType(toType);
                var toElementType = ReflectionUtils.GetElementType(concreteToType);

                // Convert the element at the top of the stack to toElementType
                il.EmitConvertValue(fromType, toElementType, new HashSet<Type>(convertedTypes));

                il.AddValueToNewCollection(concreteToType, toElementType);
            }
            else if (toType == typeof(string) && fromType != typeof(string))
            {
                il.EmitCallToString(fromType);
            }
            else if (ReflectionUtils.IsDictionary(fromType) && ReflectionUtils.IsDictionary(toType))
            {
                il.EmitMapDictionary(fromType, toType, convertedTypes);
            }
            else
            {
                il.EmitConvertClass(fromType, toType, convertedTypes);
            }

            il.MarkLabel(skipConversion);
        }

        public static void EmitMapDictionary(this ILGeneratorAdapter il, Type fromType, Type toType, HashSet<Type> convertedTypes)
        {
            var fromLocal = il.DeclareLocal(fromType);
            var toLocal = il.DeclareLocal(toType);
            var keysArray = il.DeclareLocal(typeof(string[]));
            var key = il.DeclareLocal(typeof(string));

            // Store value on top of stack into fromLocal
            il.EmitLocal(OpCodes.Stloc, fromLocal);

            var ctor = toLocal.LocalType.GetConstructor(Type.EmptyTypes);
            if (ctor != null)
            {
                il.EmitNewobj(ctor);
                il.EmitLocal(OpCodes.Stloc, toLocal);
            }

            var keysFound = false;
            var allKeys = fromType.GetProperty("AllKeys");
            if (allKeys != null)
            {
                var getter = allKeys.GetGetMethod();
                il.EmitLocal(OpCodes.Ldloc, fromLocal);
                il.EmitCall(OpCodes.Call, getter, null);
                keysFound = true;
            }

            if (!keysFound)
            {
                var keys = fromType.GetProperty("Keys");
                if (keys != null)
                {
                    var getter = keys.GetGetMethod();
                    il.EmitLocal(OpCodes.Ldloc, fromLocal);
                    il.EmitCall(OpCodes.Call, getter, null);
                    var toArrayMethod = typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(new[] { typeof(string) });
                    il.EmitCall(OpCodes.Call, toArrayMethod, null);
                    keysFound = true;
                }
            }

            if (!keysFound)
            {
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Newarr, typeof(string[]));
            }

            il.EmitLocal(OpCodes.Stloc, keysArray);

            // Loop over all keys
            Label startLoop = il.DefineLabel();
            Label afterLoop = il.DefineLabel();
            var keysIndex = il.DeclareLocal(typeof(int));

            il.EmitLocal(OpCodes.Ldloc, keysArray);
            il.Emit(OpCodes.Ldlen);
            il.EmitLocal(OpCodes.Stloc, keysIndex);

            il.MarkLabel(startLoop);
            il.EmitLocal(OpCodes.Ldloc, keysIndex);
            il.EmitBreak(OpCodes.Brfalse, afterLoop);

            il.EmitLocal(OpCodes.Ldloc, keysIndex);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Sub);
            il.EmitLocal(OpCodes.Stloc, keysIndex);

            // loop body
            il.EmitLocal(OpCodes.Ldloc, keysArray);
            il.EmitLocal(OpCodes.Ldloc, keysIndex);
            il.Emit(OpCodes.Ldelem, typeof(string));
            il.EmitLocal(OpCodes.Stloc, key);

            var toMember = ReflectionUtils.GetMemberSetter(toType, "dummy");
            var fromMember = ReflectionUtils.GetMemberGetter(fromType, "dummy", toMember.Type);

            var skipMemberMap = il.DefineLabel();

            // Load toLocal as parameter for the setter
            il.EmitLocal(toLocal.LocalType.IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, toLocal);
            if (toMember.NeedsStringIndex)
            {
                il.EmitLocal(OpCodes.Ldloc, key);
            }

            // Load fromLocal as parameter for the getter
            il.EmitLocal(fromLocal.LocalType.IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, fromLocal);

            if (fromMember.MemberInfo is PropertyInfo)
            {
                var getter = (fromMember.MemberInfo as PropertyInfo).GetGetMethod();
                if (fromMember.MemberType == MemberType.StringIndexer)
                {
                    il.EmitLocal(OpCodes.Ldloc, key);
                }

                il.EmitCall(OpCodes.Callvirt, getter, null);
            }
            else if (fromMember.MemberInfo is MethodInfo)
            {
                var method = fromMember.MemberInfo as MethodInfo;
                il.EmitLocal(OpCodes.Ldloc, key);
                il.EmitCall(OpCodes.Callvirt, method, null);
            }

            // Convert the value on top of the stack to the correct toType
            il.EmitConvertValue(fromMember.Type, toMember.Type, new HashSet<Type>(convertedTypes));

            if (toMember.MemberInfo is PropertyInfo)
            {
                var setter = (toMember.MemberInfo as PropertyInfo).GetSetMethod();
                il.EmitCall(OpCodes.Callvirt, setter, null);
            }
            else if (toMember.MemberInfo is MethodInfo)
            {
                il.EmitCall(OpCodes.Call, toMember.MemberInfo as MethodInfo, null);
            }

            if (fromMember.NeedsContainsCheck)
            {
                il.MarkLabel(skipMemberMap);
            }

            // End loop
            il.EmitBreak(OpCodes.Br, startLoop);
            il.MarkLabel(afterLoop);

            il.EmitLocal(OpCodes.Ldloc, toLocal);
        }

        public static void AddValueToNewCollection(this ILGeneratorAdapter il, Type collectionType, Type elementType)
        {
            var elementLocal = il.DeclareLocal(elementType);
            
            // Store value on top of stack into elementLocal
            il.EmitLocal(OpCodes.Stloc, elementLocal);

            if (collectionType.IsArray)
            {
                var collectionLocal = il.DeclareLocal(collectionType);

                // length 1
                il.Emit(OpCodes.Ldc_I4_1);

                // Create new array and store it in collectionLocal
                il.Emit(OpCodes.Newarr, elementType);
                il.EmitLocal(OpCodes.Stloc, collectionLocal);

                il.EmitLocal(OpCodes.Ldloc, collectionLocal);
                il.Emit(OpCodes.Ldc_I4_0);
                il.EmitLocal(OpCodes.Ldloc, elementLocal);
                il.Emit(OpCodes.Stelem, elementType);

                il.EmitLocal(OpCodes.Ldloc, collectionLocal);
            }
            else if (collectionType.IsGenericType)
            {
                var concreteCollectionType = ReflectionUtils.GetConcreteType(collectionType);
                var collectionLocal = il.DeclareLocal(concreteCollectionType);

                var con = concreteCollectionType.GetConstructor(Type.EmptyTypes);
                if (con != null)
                {
                    il.EmitNewobj(con);
                    il.EmitLocal(OpCodes.Stloc, collectionLocal);

                    var addMethod = concreteCollectionType.GetMethod("Add", new[] { elementType });
                    if (addMethod != null)
                    {
                        il.EmitLocal(OpCodes.Ldloc, collectionLocal);
                        il.EmitLocal(OpCodes.Ldloc, elementLocal);
                        il.EmitCall(OpCodes.Call, addMethod, null);
                    }
                }

                il.EmitLocal(OpCodes.Ldloc, collectionLocal);
                il.Emit(OpCodes.Castclass, collectionType);
            }
        }

        public static void EmitFirstCollectionValue(this ILGeneratorAdapter il, Type collectionType, Type elementType)
        {
            var fromLocal = il.DeclareLocal(collectionType);

            // Store value on top of stack into fromLocal
            il.EmitLocal(OpCodes.Stloc, fromLocal);
            
            if (collectionType.IsArray)
            {
                var defaultLabel = il.DefineLabel();
                var fromElementLocal = il.DeclareLocal(elementType);

                // Load length from fromLocal
                il.EmitLocal(OpCodes.Ldloc, fromLocal);
                il.Emit(OpCodes.Ldlen);
                il.EmitBreak(OpCodes.Brfalse, defaultLabel);

                il.EmitLocal(OpCodes.Ldloc, fromLocal);
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Ldelem, elementType);

                il.EmitLocal(OpCodes.Stloc, fromElementLocal);

                il.MarkLabel(defaultLabel);
                il.EmitLocal(OpCodes.Ldloc, fromElementLocal);
            }
            else if (collectionType.IsGenericType)
            {
                var firstMethod = typeof(Enumerable).GetMethods().Single(method => method.Name == "FirstOrDefault" && method.IsStatic && method.GetParameters().Length == 1).MakeGenericMethod(elementType);

                if (firstMethod != null)
                {
                    il.EmitLocal(OpCodes.Ldloc, fromLocal);
                    il.EmitCall(OpCodes.Call, firstMethod, null);
                }
            }
        }

        public static void EmitConvertCollection(this ILGeneratorAdapter il, Type fromType, Type toType, HashSet<Type> convertedTypes)
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
                    toArrayMethod = typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(fromElementType);
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

            il.EmitConvertArray(fromArrayType, toArrayType, convertedTypes);

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

        public static void EmitConvertClass(this ILGeneratorAdapter il, Type fromType, Type toType, HashSet<Type> convertedTypes)
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

            var typeMapping = TypeMapping.GetTypeMapping(fromType, toType);
            foreach (var memberMap in typeMapping.MemberMaps)
            {
                il.EmitMemberMap(fromLocal, toLocal, memberMap, convertedTypes);
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
