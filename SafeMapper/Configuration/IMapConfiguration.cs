using System.Reflection;

namespace SafeMapper.Configuration
{
    using System;

    public interface IMapConfiguration
    {
        ITypeMapping GetTypeMapping(Type fromType, Type toType);

        void SetTypeMapping(ITypeMapping typeMapping);

        MethodWrapper GetConvertMethod(Type fromType, Type toType);

        void SetConvertMethod<TFrom, TTo>(Func<TFrom, TTo> converter);

        void AddConvertMethods<TConvertClass>();

        void AddConvertMethods(Type convertClass);
    }
}