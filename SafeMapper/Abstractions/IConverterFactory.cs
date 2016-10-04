namespace SafeMapper.Abstractions
{
    using System;

    public interface IConverterFactory
    {
        IMapConfiguration Configuration { get; }

        Func<object, object> CreateDelegate(Type fromType, Type toType);

        Func<object, object> CreateDelegate(Type fromType, Type toType, IFormatProvider provider);

        Func<TFrom, TTo> CreateDelegate<TFrom, TTo>();

        Func<TFrom, TTo> CreateDelegate<TFrom, TTo>(IFormatProvider provider);
    }
}