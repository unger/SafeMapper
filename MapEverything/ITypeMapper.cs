namespace MapEverything
{
    using System;
    using System.ComponentModel;

    public interface ITypeMapper
    {
        TTo Convert<TFrom, TTo>(TFrom value);

        TTo Convert<TFrom, TTo>(TFrom value, IFormatProvider formatProvider);

        object Convert(object value, Type toType);

        object Convert(object value, Type toType, IFormatProvider formatProvider);

        Func<object, object> GetConverter(Type fromType, Type toType);

        Func<object, object> GetConverter(Type fromType, Type toType, IFormatProvider formatProvider);
    }
}