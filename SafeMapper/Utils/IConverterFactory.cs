namespace SafeMapper.Utils
{
    using System;

    using SafeMapper.Configuration;

    public interface IConverterFactory
    {
        IMapConfiguration Configuration { get; }

        Func<object, object> CreateDelegate(Type fromType, Type toType);

        Func<object, object> CreateDelegate(Type fromType, Type toType, IFormatProvider provider);

        Converter<TFrom, TTo> CreateDelegate<TFrom, TTo>();

        Converter<TFrom, TTo> CreateDelegate<TFrom, TTo>(IFormatProvider provider);
    }
}