namespace MapEverything.TypeMaps
{
    using System;

    public class ConvertibleTypeMap : ITypeMap
    {
        public ConvertibleTypeMap(Type fromType, Type toType, IFormatProvider formatProvider)
        {
            if (toType == typeof(decimal))
            {
                this.Convert = o => System.Convert.ToDecimal(o, formatProvider);
            }
            else
            {
                this.Convert = o => System.Convert.ChangeType(o, toType, formatProvider);
            }
          }

        public Func<object, object> Convert { get; private set; }
    }
}
