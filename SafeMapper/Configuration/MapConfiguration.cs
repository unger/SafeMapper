using System.Reflection;
using System.Runtime.InteropServices;

namespace SafeMapper.Configuration
{
    using System;
    using System.Collections.Concurrent;

    using SafeMapper.Reflection;

    public class MapConfiguration : IMapConfiguration
    {
        private readonly ConcurrentDictionary<string, ITypeMapping> typeMappings = new ConcurrentDictionary<string, ITypeMapping>();

        private readonly ConcurrentDictionary<string, MethodWrapper> convertMethods = new ConcurrentDictionary<string, MethodWrapper>();

        public MapConfiguration()
        {
            AddConvertMethods<SafeConvert>();
            AddConvertMethods<SafeNullableConvert>();
            //AddConvertMethods<SafeConvertNonStatic>();
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
            this.SetConvertMethod(typeof(TFrom), typeof(TTo), converter.Method);
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
                    if (method.IsStatic)
                    {
                        SetConvertMethod(pars[0].ParameterType, method.ReturnType, new MethodWrapper(method));
                    }
                    else
                    {
                        var staticInstanceMember = ReflectionUtils.GetStaticMemberInfo(convertClass);
                        if (staticInstanceMember != null)
                        {
                            SetConvertMethod(pars[0].ParameterType, method.ReturnType, new MethodWrapper(method, staticInstanceMember));
                        }
                    }
                }
            }
        }

        private void SetConvertMethod(Type fromType, Type toType, MethodInfo method)
        {
            if (method.IsStatic)
            {
                this.SetConvertMethod(fromType, toType, new MethodWrapper(method));
            }
            else
            {
                var staticInstanceMember = ReflectionUtils.GetStaticMemberInfo(method.DeclaringType);
                if (staticInstanceMember == null)
                {
                    throw new ArgumentException("Only static Func-lamdas are supported, or non static methods defined in a class with a static instance member");
                }

                this.SetConvertMethod(fromType, toType, new MethodWrapper(method, staticInstanceMember));
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
