namespace MapEverything
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;

    using Fasterflect;

    using MapEverything.Generic;
    using MapEverything.TypeMaps;

    public class TypeMapperEx : TypeMapper
    {
        private readonly ConcurrentDictionary<Type, dynamic> typeConvertInvokers;

        private Type genericTypeMap = typeof(TypeMap<,>);

        public TypeMapperEx()
        {
            this.typeConvertInvokers = new ConcurrentDictionary<Type, dynamic>();
        }

        public new TTo Convert<TFrom, TTo>(TFrom value)
        {
            return this.typeConvertInvokers.GetOrAdd(typeof(TypeMap<TFrom, TTo>), type => new TypeMap<TFrom, TTo>(CultureInfo.CurrentCulture, this).Convert)(value);
        }

        public new TTo Convert<TFrom, TTo>(TFrom value, IFormatProvider formatProvider)
        {
            return this.typeConvertInvokers.GetOrAdd(typeof(TypeMap<TFrom, TTo>), type => new TypeMap<TFrom, TTo>(formatProvider, this).Convert)(value);
        }

        public new TTo Convert<TFrom, TTo>(TFrom value, Converter<TFrom, TTo> converter)
        {
            return converter(value);
        }

        public new Converter<TFrom, TTo> GetConverter<TFrom, TTo>()
        {
            return this.typeConvertInvokers.GetOrAdd(typeof(TypeMap<TFrom, TTo>), type => new TypeMap<TFrom, TTo>(CultureInfo.CurrentCulture, this).Convert);
        }

        public new Converter<TFrom, TTo> GetConverter<TFrom, TTo>(IFormatProvider formatProvider)
        {
            return this.typeConvertInvokers.GetOrAdd(typeof(TypeMap<TFrom, TTo>), type => new TypeMap<TFrom, TTo>(formatProvider, this).Convert);
        }

    }
}
