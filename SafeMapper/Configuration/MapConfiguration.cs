using System.Reflection;

using SafeMapper.Utils;

namespace SafeMapper.Configuration
{
    using System;
    using System.Collections.Concurrent;

    using SafeMapper.Abstractions;
    using SafeMapper.Reflection;

    public class MapConfiguration : IMapConfiguration
    {
        private readonly ConcurrentDictionary<string, ITypeMapping> typeMappings = new ConcurrentDictionary<string, ITypeMapping>();

        private readonly ConcurrentDictionary<string, MethodWrapper> convertMethods = new ConcurrentDictionary<string, MethodWrapper>();

        private readonly ConcurrentDictionary<string, ILInstruction[]> convertInstructions = new ConcurrentDictionary<string, ILInstruction[]>();

        public MapConfiguration()
        {
            this.AddConvertMethods<SafeConvert>();
            this.AddConvertMethods<SafeNullableConvert>();
            this.AddConvertMethods<SafeSqlConvert>();
        }

        public ITypeMapping GetTypeMapping(Type fromType, Type toType)
        {
            return this.typeMappings.GetOrAdd(
                string.Concat(fromType.FullName, toType.FullName),
                k => new TypeMapping(fromType, toType));
        }

        public void SetTypeMapping(ITypeMapping typeMapping)
        {
            this.typeMappings.AddOrUpdate(
                string.Concat(typeMapping.FromType.FullName, typeMapping.ToType.FullName),
                typeMapping,
                (key, oldValue) => typeMapping);
        }

        public ILInstruction[] GetConvertInstructions(Type fromType, Type toType)
        {
            ILInstruction[] instructions;
            if (this.convertInstructions.TryGetValue(string.Concat(fromType.FullName, toType.FullName), out instructions))
            {
                return instructions;
            }

            return null;
        }

        public void SetConvertInstructions<TFrom, TTo>(ILInstruction[] instructions)
        {
            this.SetConvertInstructions(typeof(TFrom), typeof(TTo), instructions);
        }

        public void SetConvertInstructions(Type fromType, Type toType, ILInstruction[] instructions)
        {
            this.convertInstructions.AddOrUpdate(
                string.Concat(fromType.FullName, toType.FullName),
                instructions,
                (key, oldValue) => instructions);
        }
        
        public MethodWrapper GetConvertMethod(Type fromType, Type toType)
        {
            MethodWrapper mv;
            if (this.convertMethods.TryGetValue(string.Concat(fromType.FullName, toType.FullName), out mv))
            {
                return mv;
            }

            return null;
        }

        public void SetConvertMethod<TFrom, TTo>(Func<TFrom, TTo> converter)
        {
            this.SetConvertMethod(typeof(TFrom), typeof(TTo), converter.Method, converter.Target);
        }

        public void AddConvertMethods<TConvertClass>()
        {
            this.AddConvertMethods(typeof(TConvertClass));
        }

        public void AddConvertMethods(Type convertClass)
        {
            var methods = convertClass.GetMethods();
            foreach (var method in methods)
            {
                var pars = method.GetParameters();
                if ((pars.Length == 2 && pars[1].ParameterType == typeof (IFormatProvider)) || pars.Length == 1)
                {
                    this.SetConvertMethod(pars[0].ParameterType, method.ReturnType, method, null);
                }
            }
        }

        private void SetConvertMethod(Type fromType, Type toType, MethodInfo method, object target)
        {
            if (method.IsStatic)
            {
                this.SetConvertMethod(fromType, toType, new MethodWrapper(method, target, null));
            }
            else
            {
                var staticInstanceMember = ReflectionUtils.GetStaticMemberInfo(method.DeclaringType);
                this.SetConvertMethod(fromType, toType, new MethodWrapper(method, target, staticInstanceMember));
            }
        }

        private void SetConvertMethod(Type fromType, Type toType, MethodWrapper convertMethod)
        {
            this.convertMethods.AddOrUpdate(
                string.Concat(fromType.FullName, toType.FullName),
                convertMethod,
                (key, oldValue) => convertMethod);
        }
    }
}
