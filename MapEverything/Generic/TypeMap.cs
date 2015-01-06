namespace MapEverything.Generic
{
    using System;

    using MapEverything.TypeMaps;

    public class TypeMap<TFrom, TTo>
    {
        public TypeMap(IFormatProvider formatProvider, ITypeMapper typeMapper)
        {
            this.Convert = value => (TTo)TypeMapFactory.Create(typeof(TFrom), typeof(TTo), formatProvider, typeMapper).Convert(value);
        }

        public Converter<TFrom, TTo> Convert { get; private set; }
    }
}
