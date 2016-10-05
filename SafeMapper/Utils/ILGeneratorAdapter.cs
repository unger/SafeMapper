namespace SafeMapper.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using SafeMapper.Abstractions;
    using SafeMapper.Configuration;
    using SafeMapper.Reflection;

    public class ILGeneratorAdapter : ILGeneratorAdapterBase
    {
        private readonly IMapConfiguration mapCfg;

        public ILGeneratorAdapter(IMapConfiguration configuration) 
        {
            this.mapCfg = configuration;
        }

        public void EmitConvertArray(Type fromType, Type toType, HashSet<Type> convertedTypes)
        {
            var fromLocal = this.DeclareLocal(fromType);
            var toLocal = this.DeclareLocal(toType);

            // Store value on top of stack into fromLocal
            this.EmitLocal(OpCodes.Stloc, fromLocal);

            // Load length from fromLocal
            this.EmitLocal(OpCodes.Ldloc, fromLocal);
            this.Emit(OpCodes.Ldlen);

            // Create new array and store it in toLocal
            this.Emit(OpCodes.Newarr, toLocal.LocalType.GetElementType());
            this.EmitLocal(OpCodes.Stloc, toLocal);

            var fromElementType = fromType.GetElementType();
            var toElementType = toType.GetElementType();

            var startLoop = this.DefineLabel();
            var afterLoop = this.DefineLabel();
            var arrayIndex = this.DeclareLocal(typeof(int));

            this.EmitLocal(OpCodes.Ldloc, fromLocal);
            this.Emit(OpCodes.Ldlen);
            this.EmitLocal(OpCodes.Stloc, arrayIndex);

            this.MarkLabel(startLoop);
            this.EmitLocal(OpCodes.Ldloc, arrayIndex);
            this.EmitBreak(OpCodes.Brfalse, afterLoop);

            this.EmitLocal(OpCodes.Ldloc, arrayIndex);
            this.Emit(OpCodes.Ldc_I4_1);
            this.Emit(OpCodes.Sub);
            this.EmitLocal(OpCodes.Stloc, arrayIndex);

            // Ladda in toarray på stacken
            this.EmitLocal(OpCodes.Ldloc, toLocal);
            this.EmitLocal(OpCodes.Ldloc, arrayIndex);

            // Ladda in fromarray på stacken
            this.EmitLocal(OpCodes.Ldloc, fromLocal);
            this.EmitLocal(OpCodes.Ldloc, arrayIndex);
            this.Emit(OpCodes.Ldelem, fromElementType);

            // Convert the element at the top of the stack to toElementType
            this.EmitConvertValue(fromElementType, toElementType, new HashSet<Type>(convertedTypes));

            // Store the converted value
            this.Emit(OpCodes.Stelem, toElementType);

            // End loop
            this.EmitBreak(OpCodes.Br, startLoop);
            this.MarkLabel(afterLoop);

            this.EmitLocal(OpCodes.Ldloc, toLocal);
        }

        public void EmitMemberMap(
            LocalBuilderWrapper fromLocal,
            LocalBuilderWrapper toLocal,
            MemberMap memberMap,
            HashSet<Type> convertedTypes)
        {
            var skipMemberMap = this.DefineLabel();
            if (memberMap.FromMember.NeedsContainsCheck)
            {
                var containsKey = fromLocal.LocalType.GetMethod("ContainsKey", new[] { typeof(string) });
                this.EmitLocal(fromLocal.LocalType.GetTypeInfo().IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, fromLocal);
                this.EmitString(memberMap.FromMember.Name);
                this.EmitCall(OpCodes.Call, containsKey);
                this.EmitBreak(OpCodes.Brfalse, skipMemberMap);
            }

            // Load toLocal as parameter for the setter
            this.EmitLocal(toLocal.LocalType.GetTypeInfo().IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, toLocal);
            if (memberMap.ToMember.NeedsStringIndex)
            {
                this.EmitString(memberMap.ToMember.Name);
            }

            // Load fromLocal as parameter for the getter
            this.EmitLocal(fromLocal.LocalType.GetTypeInfo().IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, fromLocal);

            if (memberMap.FromMember.MemberInfo is PropertyInfo)
            {
                var getter = (memberMap.FromMember.MemberInfo as PropertyInfo).GetGetMethod();
                if (memberMap.FromMember.MemberType == MemberType.StringIndexer)
                {
                    this.EmitString(memberMap.FromMember.Name);
                }

                this.EmitCall(OpCodes.Callvirt, getter);
            }
            else if (memberMap.FromMember.MemberInfo is FieldInfo)
            {
                this.EmitField(OpCodes.Ldfld, memberMap.FromMember.MemberInfo as FieldInfo);
            }
            else if (memberMap.FromMember.MemberInfo is MethodInfo)
            {
                var method = memberMap.FromMember.MemberInfo as MethodInfo;
                if (memberMap.FromMember.NeedsStringIndex)
                {
                    this.EmitString(memberMap.FromMember.Name);
                }

                this.EmitCall(OpCodes.Callvirt, method);
            }

            // Convert the value on top of the stack to the correct toType
            this.EmitConvertValue(memberMap.FromMember.Type, memberMap.ToMember.Type, new HashSet<Type>(convertedTypes));

            if (memberMap.ToMember.MemberInfo is PropertyInfo)
            {
                var setter = (memberMap.ToMember.MemberInfo as PropertyInfo).GetSetMethod();
                this.EmitCall(OpCodes.Callvirt, setter);
            }
            else if (memberMap.ToMember.MemberInfo is FieldInfo)
            {
                this.EmitField(OpCodes.Stfld, memberMap.ToMember.MemberInfo as FieldInfo);
            }
            else if (memberMap.ToMember.MemberInfo is MethodInfo)
            {
                this.EmitCall(OpCodes.Call, memberMap.ToMember.MemberInfo as MethodInfo);
            }

            if (memberMap.FromMember.NeedsContainsCheck)
            {
                this.MarkLabel(skipMemberMap);
            }
        }

        public void EmitValueTypeBox(Type fromType)
        {
            if (fromType.GetTypeInfo().IsValueType)
            {
                // Put property/field value in a local variable to be able to call instance method on it
                var localReturnType = this.DeclareLocal(fromType);
                this.EmitLocal(OpCodes.Stloc, localReturnType);
                if (fromType.GetTypeInfo().IsEnum)
                {
                    this.EmitLocal(OpCodes.Ldloc, localReturnType);
                    this.Emit(OpCodes.Box, fromType);
                }
                else
                {
                    this.EmitLocal(OpCodes.Ldloca, localReturnType);
                }
            }
        }

        public void EmitCallToString(Type fromType)
        {
            this.EmitValueTypeBox(fromType);

            var toStringMethod = fromType.GetMethod("ToString", new[] { typeof(IFormatProvider) });
            if (toStringMethod != null)
            {
                this.Emit(OpCodes.Ldarg_0); // IFormatProvider
                this.EmitCall(OpCodes.Callvirt, toStringMethod);
            }
            else
            {
                toStringMethod = fromType.GetMethod("ToString", Type.EmptyTypes);
                this.EmitCall(OpCodes.Callvirt, toStringMethod);
            }
        }

        public void EmitConvertValue(Type fromType, Type toType, HashSet<Type> convertedTypes)
        {
            // Circular reference check
            if (!fromType.GetTypeInfo().IsValueType && fromType != typeof(string))
            {
                if (convertedTypes.Contains(fromType))
                {
                    var toLocal = this.DeclareLocal(toType);
                    this.Emit(OpCodes.Pop);
                    this.EmitLocal(OpCodes.Ldloc, toLocal);
                    return;
                }

                convertedTypes.Add(fromType);
            }

            var skipConversion = this.DefineLabel();

            if (toType.IsAssignableFrom(fromType))
            {
                if (toType == fromType)
                {
                    return;
                }

                // if types differ and totype is not nullable
                if (!(toType.GetTypeInfo().IsGenericType && toType.GetGenericTypeDefinition() == typeof (Nullable<>)))
                {
                    return;
                }
            }

            // Check if fromValue is null
            var underlyingFromType = Nullable.GetUnderlyingType(fromType);
            if (!fromType.GetTypeInfo().IsValueType || underlyingFromType != null)
            {
                var fromLocal = this.DeclareLocal(fromType);
                var toLocal = this.DeclareLocal(toType);
                var nonNull = this.DefineLabel();

                // Store value on top of stack into fromLocal
                this.EmitLocal(OpCodes.Stloc, fromLocal);

                if (underlyingFromType != null)
                {
                    this.EmitLocal(OpCodes.Ldloca, fromLocal);
                    MethodInfo mi = fromType.GetMethod("get_HasValue", BindingFlags.Instance | BindingFlags.Public);
                    this.EmitCall(OpCodes.Call, mi);
                }
                else
                {
                    this.EmitLocal(OpCodes.Ldloc, fromLocal);
                }

                this.EmitBreak(OpCodes.Brtrue_S, nonNull);

                // Load toLocal with default value on stack and skip rest of conversion logic
                this.EmitLocal(OpCodes.Ldloc, toLocal);
                this.EmitBreak(OpCodes.Br, skipConversion);

                // Not null, put the fromValue back on stack
                this.MarkLabel(nonNull);
                if (underlyingFromType != null)
                {
                    this.EmitLocal(OpCodes.Ldloca, fromLocal);
                    MethodInfo mi = fromType.GetMethod("get_Value", BindingFlags.Instance | BindingFlags.Public);
                    this.EmitCall(OpCodes.Call, mi);

                    if (toType.IsAssignableFrom(underlyingFromType))
                    {
                        this.EmitBreak(OpCodes.Br, skipConversion);
                    }
                    
                    fromType = underlyingFromType;
                }
                else
                {
                    this.EmitLocal(OpCodes.Ldloc, fromLocal);
                }
            }

            var convertInstructions = this.mapCfg.GetConvertInstructions(fromType, toType);
            var converter = this.mapCfg.GetConvertMethod(fromType, toType);

            if (convertInstructions != null)
            {
                this.EmitInstructions(convertInstructions);
            }
            else if (converter != null)
            {
                this.EmitCallConverter(fromType, toType, converter);
            }
            else if (toType.GetTypeInfo().IsEnum)
            {
                this.EmitConvertToEnum(fromType, toType);
            }
            else if (fromType.GetTypeInfo().IsEnum)
            {
                this.EmitConvertFromEnum(fromType, toType);
            }
            else if (ReflectionUtils.IsCollection(fromType) && ReflectionUtils.IsCollection(toType))
            {
                this.EmitConvertCollection(fromType, toType, convertedTypes);
            }
            else if (ReflectionUtils.IsCollection(fromType))
            {
                var concreteFromType = ReflectionUtils.GetConcreteType(fromType);
                var fromElementType = ReflectionUtils.GetElementType(concreteFromType);

                this.EmitFirstCollectionValue(concreteFromType, fromElementType);

                // Convert the element at the top of the stack to toType
                this.EmitConvertValue(fromElementType, toType, new HashSet<Type>(convertedTypes));
            }
            else if (ReflectionUtils.IsCollection(toType))
            {
                var concreteToType = ReflectionUtils.GetConcreteType(toType);
                var toElementType = ReflectionUtils.GetElementType(concreteToType);

                // Convert the element at the top of the stack to toElementType
                this.EmitConvertValue(fromType, toElementType, new HashSet<Type>(convertedTypes));

                this.AddValueToNewCollection(concreteToType, toElementType);
            }
            else if (toType == typeof(string) && fromType != typeof(string))
            {
                this.EmitCallToString(fromType);
            }
            else if (ReflectionUtils.IsDictionary(fromType) && ReflectionUtils.IsDictionary(toType))
            {
                this.EmitMapDictionary(fromType, toType, convertedTypes);
            }
            else
            {
                this.EmitConvertClass(fromType, toType, convertedTypes);
            }

            this.MarkLabel(skipConversion);
        }

        private void EmitCallConverter(Type fromType, Type toType, MethodWrapper converter)
        {
            if (converter.Method.IsStatic)
            {
                // Load IFormatProvider as second argument
                if (converter.Method.GetParameters().Length == 2)
                {
                    this.Emit(OpCodes.Ldarg_0);
                }

                this.EmitCall(OpCodes.Call, converter.Method);
            }
            else
            {
                var fromLocal = this.DeclareLocal(fromType);
                // Store value on top of stack into fromLocal
                this.EmitLocal(OpCodes.Stloc, fromLocal);

                var classInstanceLoaded = false;
                if (converter.StaticInstanceMember != null)
                {
                    var staticField = converter.StaticInstanceMember as FieldInfo;
                    if (staticField != null)
                    {
                        classInstanceLoaded = true;
                        this.EmitField(OpCodes.Ldsfld, staticField);
                    }

                    var staticProperty = converter.StaticInstanceMember as PropertyInfo;
                    if (staticProperty != null)
                    {
                        classInstanceLoaded = true;
                        var getter = staticProperty.GetGetMethod();
                        this.EmitCall(OpCodes.Call, getter);
                    }
                }
                else
                {
                    // For example anonymous methods will be defined in a seperate automatically generated class
                    // all local variables will be defined as fields on this class
                    var classInstanceLocal = this.DeclareLocal(converter.Method.DeclaringType);
                    var ctor = classInstanceLocal.LocalType.GetConstructor(Type.EmptyTypes);
                    if (ctor != null)
                    {
                        classInstanceLoaded = true;
                        this.EmitNewobj(ctor);
                        this.EmitLocal(OpCodes.Stloc, classInstanceLocal);

                        if (converter.Target != null)
                        {
                            var fields = classInstanceLocal.LocalType.GetFields();
                            foreach (var field in fields)
                            {
                                // TODO Add support för more fieldtypes than int
                                if (field.FieldType == typeof(int))
                                {
                                    this.EmitLocal(OpCodes.Ldloc, classInstanceLocal);
                                    this.EmitInt((int)field.GetValue(converter.Target));
                                    this.EmitField(OpCodes.Stfld, field);
                                }
                            }
                        }

                        this.EmitLocal(OpCodes.Ldloc, classInstanceLocal);
                    }
                }

                if (classInstanceLoaded)
                {
                    this.EmitLocal(OpCodes.Ldloc, fromLocal);
                    if (converter.Method.GetParameters().Length == 2)
                    {
                        this.Emit(OpCodes.Ldarg_0);
                    }

                    this.EmitCall(OpCodes.Callvirt, converter.Method);
                }
                else
                {
                    // Load default of toType when it fails to load class instance on stack
                    var toLocal = this.DeclareLocal(toType);
                    this.EmitLocal(OpCodes.Ldloc, toLocal);                    
                }
            }
        }

        public void EmitMapDictionary(Type fromType, Type toType, HashSet<Type> convertedTypes)
        {
            var fromLocal = this.DeclareLocal(fromType);
            var toLocal = this.DeclareLocal(toType);
            var keysArray = this.DeclareLocal(typeof(string[]));
            var key = this.DeclareLocal(typeof(string));

            // Store value on top of stack into fromLocal
            this.EmitLocal(OpCodes.Stloc, fromLocal);

            var ctor = toLocal.LocalType.GetConstructor(Type.EmptyTypes);
            if (ctor != null)
            {
                this.EmitNewobj(ctor);
                this.EmitLocal(OpCodes.Stloc, toLocal);
            }

            var keysFound = false;
            var allKeys = fromType.GetProperty("AllKeys");
            if (allKeys != null)
            {
                var getter = allKeys.GetGetMethod();
                this.EmitLocal(OpCodes.Ldloc, fromLocal);
                this.EmitCall(OpCodes.Call, getter);
                keysFound = true;
            }

            if (!keysFound)
            {
                var keys = fromType.GetProperty("Keys");
                if (keys != null)
                {
                    var getter = keys.GetGetMethod();
                    this.EmitLocal(OpCodes.Ldloc, fromLocal);
                    this.EmitCall(OpCodes.Call, getter);
                    var toArrayMethod = typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(new[] { typeof(string) });
                    this.EmitCall(OpCodes.Call, toArrayMethod);
                    keysFound = true;
                }
            }

            if (!keysFound)
            {
                this.Emit(OpCodes.Ldc_I4_0);
                this.Emit(OpCodes.Newarr, typeof(string[]));
            }

            this.EmitLocal(OpCodes.Stloc, keysArray);

            // Loop over all keys
            var startLoop = this.DefineLabel();
            var afterLoop = this.DefineLabel();
            var keysIndex = this.DeclareLocal(typeof(int));

            this.EmitLocal(OpCodes.Ldloc, keysArray);
            this.Emit(OpCodes.Ldlen);
            this.EmitLocal(OpCodes.Stloc, keysIndex);

            this.MarkLabel(startLoop);
            this.EmitLocal(OpCodes.Ldloc, keysIndex);
            this.EmitBreak(OpCodes.Brfalse, afterLoop);

            this.EmitLocal(OpCodes.Ldloc, keysIndex);
            this.Emit(OpCodes.Ldc_I4_1);
            this.Emit(OpCodes.Sub);
            this.EmitLocal(OpCodes.Stloc, keysIndex);

            // loop body
            this.EmitLocal(OpCodes.Ldloc, keysArray);
            this.EmitLocal(OpCodes.Ldloc, keysIndex);
            this.Emit(OpCodes.Ldelem, typeof(string));
            this.EmitLocal(OpCodes.Stloc, key);

            var toMember = ReflectionUtils.GetMemberSetter(toType, "dummy");
            var fromMember = ReflectionUtils.GetMemberGetter(fromType, "dummy", toMember.Type);

            var skipMemberMap = this.DefineLabel();

            // Load toLocal as parameter for the setter
            this.EmitLocal(toLocal.LocalType.GetTypeInfo().IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, toLocal);
            if (toMember.NeedsStringIndex)
            {
                this.EmitLocal(OpCodes.Ldloc, key);
            }

            // Load fromLocal as parameter for the getter
            this.EmitLocal(fromLocal.LocalType.GetTypeInfo().IsValueType ? OpCodes.Ldloca : OpCodes.Ldloc, fromLocal);

            if (fromMember.MemberInfo is PropertyInfo)
            {
                var getter = (fromMember.MemberInfo as PropertyInfo).GetGetMethod();
                if (fromMember.MemberType == MemberType.StringIndexer)
                {
                    this.EmitLocal(OpCodes.Ldloc, key);
                }

                this.EmitCall(OpCodes.Callvirt, getter);
            }
            else if (fromMember.MemberInfo is MethodInfo)
            {
                var method = fromMember.MemberInfo as MethodInfo;
                this.EmitLocal(OpCodes.Ldloc, key);
                this.EmitCall(OpCodes.Callvirt, method);
            }

            // Convert the value on top of the stack to the correct toType
            this.EmitConvertValue(fromMember.Type, toMember.Type, new HashSet<Type>(convertedTypes));

            if (toMember.MemberInfo is PropertyInfo)
            {
                var setter = (toMember.MemberInfo as PropertyInfo).GetSetMethod();
                this.EmitCall(OpCodes.Callvirt, setter);
            }
            else if (toMember.MemberInfo is MethodInfo)
            {
                this.EmitCall(OpCodes.Call, toMember.MemberInfo as MethodInfo);
            }

            if (fromMember.NeedsContainsCheck)
            {
                this.MarkLabel(skipMemberMap);
            }

            // End loop
            this.EmitBreak(OpCodes.Br, startLoop);
            this.MarkLabel(afterLoop);

            this.EmitLocal(OpCodes.Ldloc, toLocal);
        }

        public void AddValueToNewCollection(Type collectionType, Type elementType)
        {
            var elementLocal = this.DeclareLocal(elementType);
            
            // Store value on top of stack into elementLocal
            this.EmitLocal(OpCodes.Stloc, elementLocal);

            if (collectionType.IsArray)
            {
                var collectionLocal = this.DeclareLocal(collectionType);

                // length 1
                this.Emit(OpCodes.Ldc_I4_1);

                // Create new array and store it in collectionLocal
                this.Emit(OpCodes.Newarr, elementType);
                this.EmitLocal(OpCodes.Stloc, collectionLocal);

                this.EmitLocal(OpCodes.Ldloc, collectionLocal);
                this.Emit(OpCodes.Ldc_I4_0);
                this.EmitLocal(OpCodes.Ldloc, elementLocal);
                this.Emit(OpCodes.Stelem, elementType);

                this.EmitLocal(OpCodes.Ldloc, collectionLocal);
            }
            else if (collectionType.GetTypeInfo().IsGenericType)
            {
                var concreteCollectionType = ReflectionUtils.GetConcreteType(collectionType);
                var collectionLocal = this.DeclareLocal(concreteCollectionType);

                var con = concreteCollectionType.GetConstructor(Type.EmptyTypes);
                if (con != null)
                {
                    this.EmitNewobj(con);
                    this.EmitLocal(OpCodes.Stloc, collectionLocal);

                    var addMethod = concreteCollectionType.GetMethod("Add", new[] { elementType });
                    if (addMethod != null)
                    {
                        this.EmitLocal(OpCodes.Ldloc, collectionLocal);
                        this.EmitLocal(OpCodes.Ldloc, elementLocal);
                        this.EmitCall(OpCodes.Call, addMethod);
                    }
                }

                this.EmitLocal(OpCodes.Ldloc, collectionLocal);
                this.Emit(OpCodes.Castclass, collectionType);
            }
        }

        public void EmitFirstCollectionValue(Type collectionType, Type elementType)
        {
            var fromLocal = this.DeclareLocal(collectionType);

            // Store value on top of stack into fromLocal
            this.EmitLocal(OpCodes.Stloc, fromLocal);
            
            if (collectionType.IsArray)
            {
                var defaultLabel = this.DefineLabel();
                var fromElementLocal = this.DeclareLocal(elementType);

                // Load length from fromLocal
                this.EmitLocal(OpCodes.Ldloc, fromLocal);
                this.Emit(OpCodes.Ldlen);
                this.EmitBreak(OpCodes.Brfalse, defaultLabel);

                this.EmitLocal(OpCodes.Ldloc, fromLocal);
                this.Emit(OpCodes.Ldc_I4_0);
                this.Emit(OpCodes.Ldelem, elementType);

                this.EmitLocal(OpCodes.Stloc, fromElementLocal);

                this.MarkLabel(defaultLabel);
                this.EmitLocal(OpCodes.Ldloc, fromElementLocal);
            }
            else if (collectionType.GetTypeInfo().IsGenericType)
            {
                var firstMethod = typeof(Enumerable).GetMethods().Single(method => method.Name == "FirstOrDefault" && method.IsStatic && method.GetParameters().Length == 1).MakeGenericMethod(elementType);

                this.EmitLocal(OpCodes.Ldloc, fromLocal);
                this.EmitCall(OpCodes.Call, firstMethod);
            }
        }

        public void EmitConvertCollection(Type fromType, Type toType, HashSet<Type> convertedTypes)
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

                this.EmitCall(OpCodes.Call, toArrayMethod);
                fromArrayType = fromElementType.MakeArrayType();
            }

            if (!toType.IsArray)
            {
                toArrayType = toElementType.MakeArrayType();
            }

            this.EmitConvertArray(fromArrayType, toArrayType, convertedTypes);

            if (!toType.IsArray)
            {
                var toEnumerableType = typeof(IEnumerable<>).MakeGenericType(toElementType);
                var toConstructor = toType.GetConstructor(new[] { toEnumerableType });

                if (toConstructor != null)
                {
                    this.EmitNewobj(toConstructor);
                }
            }
        }

        public void EmitConvertFromEnum(Type fromType, Type toType)
        {
            if (!fromType.GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("fromType needs to be an enum", "fromType");
            }

            if (toType == typeof(string))
            {
                var enumValues = Enum.GetValues(fromType);

                var switchType = Enum.GetUnderlyingType(fromType);
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

                this.EmitSwitchCases(switchType, toType, switchReturnValues);
            }
            else
            {
                var underlayingFromType = Enum.GetUnderlyingType(fromType);

                if (toType != underlayingFromType)
                {
                    var converter = this.mapCfg.GetConvertMethod(underlayingFromType, toType);

                    if (converter != null)
                    {
                        this.EmitCallConverter(underlayingFromType, toType, converter);
                    }
                }
            }
        }

        public void EmitConvertToEnum(Type fromType, Type toType)
        {
            if (!toType.GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("toType needs to be an enum", "toType");
            }

            var enumValues = Enum.GetValues(toType);
            var underlayingToType = Enum.GetUnderlyingType(toType);
            var underlayingFromType = fromType.GetTypeInfo().IsEnum ? Enum.GetUnderlyingType(fromType) : fromType;
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
                var converter = this.mapCfg.GetConvertMethod(underlayingFromType, underlayingToType);

                if (converter != null)
                {
                    this.EmitCallConverter(underlayingFromType, underlayingToType, converter);
                }
                else
                {
                    // if it is not possible to convert load enum default value
                    this.Emit(OpCodes.Pop);
                    this.EmitLoadEnumValue(underlayingToType, enumValues.GetValue(0));
                    return;
                }
            }

            this.EmitSwitchCases(switchType, underlayingToType, switchReturnValues);
        }

        public void EmitSwitchCases(Type switchType, Type returnType, List<Tuple<object, object>> switchReturnValues)
        {
            var defaultCase = this.DefineLabel();
            var endOfMethod = this.DefineLabel();
            var switchValue = this.DeclareLocal(switchType);

            this.EmitLocal(OpCodes.Stloc, switchValue);

            var jumpTable = new LabelWrapper[switchReturnValues.Count];
            for (int i = 0; i < switchReturnValues.Count; i++)
            {
                jumpTable[i] = this.DefineLabel();
                this.EmitLocal(OpCodes.Ldloc, switchValue);
                this.EmitLoadEnumValue(switchType, switchReturnValues[i].Item1);
                if (switchType == typeof(string))
                {
                    var stringEquals = typeof(string).GetMethod("op_Equality", new[] { typeof(string), typeof(string) });
                    this.EmitCall(OpCodes.Call, stringEquals);
                    this.Emit(OpCodes.Ldc_I4_1);
                }

                this.EmitBreak(OpCodes.Beq, jumpTable[i]);
            }

            // Branch on default case
            this.EmitBreak(OpCodes.Br, defaultCase);

            for (int i = 0; i < switchReturnValues.Count; i++)
            {
                this.MarkLabel(jumpTable[i]);
                this.EmitLoadEnumValue(returnType, switchReturnValues[i].Item2);
                this.EmitBreak(OpCodes.Br, endOfMethod);
            }

            // Default case
            this.MarkLabel(defaultCase);
            this.EmitLoadEnumValue(returnType, switchReturnValues[0].Item2);

            this.MarkLabel(endOfMethod);
        }

        public void EmitConvertClass(Type fromType, Type toType, HashSet<Type> convertedTypes)
        {
            var fromLocal = this.DeclareLocal(fromType);
            var toLocal = this.DeclareLocal(toType);

            // Store value on top of stack into fromLocal
            this.EmitLocal(OpCodes.Stloc, fromLocal);

            var ctor = toLocal.LocalType.GetConstructor(Type.EmptyTypes);
            if (ctor != null)
            {
                this.EmitNewobj(ctor);
                this.EmitLocal(OpCodes.Stloc, toLocal);
            }

            var typeMapping = this.mapCfg.GetTypeMapping(fromType, toType);
            foreach (var memberMap in typeMapping.MemberMaps)
            {
                this.EmitMemberMap(fromLocal, toLocal, memberMap, convertedTypes);
            }

            this.EmitLocal(OpCodes.Ldloc, toLocal);
        }

        private void EmitLoadEnumValue(Type type, object enumValue)
        {
            if (type == typeof(string))
            {
                this.EmitString((string)enumValue);
            }
            else if (type == typeof(byte))
            {
                this.EmitByte((byte)enumValue);
            }
            else if (type == typeof(sbyte))
            {
                this.EmitSByte((sbyte)enumValue);
            }
            else if (type == typeof(short))
            {
                this.EmitShort((short)enumValue);
            }
            else if (type == typeof(ushort))
            {
                this.EmitUShort((ushort)enumValue);
            }
            else if (type == typeof(int))
            {
                this.EmitInt((int)enumValue);
            }
            else if (type == typeof(uint))
            {
                this.EmitUInt((uint)enumValue);
            }
            else if (type == typeof(long))
            {
                this.EmitLong((long)enumValue);
            }
            else if (type == typeof(ulong))
            {
                this.EmitULong((ulong)enumValue);
            }
        }
    }
}
