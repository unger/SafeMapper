namespace MapEverything.Generic
{
    using System;

    public class TypeMap<TFrom, TTo>
    {
        private readonly Type toType = typeof(TTo);

        public TypeMap(IFormatProvider formatProvider)
        {
            this.Convert = value => (TTo)System.Convert.ChangeType(value, this.toType, formatProvider);
        }

        public Converter<TFrom, TTo> Convert { get; private set; }
    }
}
