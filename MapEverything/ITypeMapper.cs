namespace MapEverything
{
    using System;
    using System.ComponentModel;

    public interface ITypeMapper
    {
        TTo Convert<TFrom, TTo>(TFrom value);

        TTo Convert<TFrom, TTo>(TFrom value, IFormatProvider formatProvider);

        TTo Convert<TFrom, TTo>(TFrom value, Converter<TFrom, TTo> converter);

        Converter<TFrom, TTo> GetConverter<TFrom, TTo>();

        Converter<TFrom, TTo> GetConverter<TFrom, TTo>(IFormatProvider formatProvider);
        
        object Convert(object value, Type fromType, Type toType);

        object Convert(object value, Type fromType, Type toType, IFormatProvider formatProvider);

        object Convert(object value, Func<object, object> converter);

        Func<object, object> GetConverter(Type fromType, Type toType);

        Func<object, object> GetConverter(Type fromType, Type toType, IFormatProvider formatProvider);

        TypeDefinition GetTypeDefinition(Type type);
    }
}