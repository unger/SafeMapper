namespace MapEverything.TypeMaps
{
    using System;
    using System.Linq;

    public class ToStringTypeMap : ITypeMap
    {
        public ToStringTypeMap(Type fromType, IFormatProvider formatProvider)
        {
            this.Convert = this.GetToStringConverter(fromType, formatProvider);
        }

        public Func<object, object> Convert { get; private set; }

        protected Func<object, object> GetToStringConverter(Type fromType, IFormatProvider formatProvider)
        {
            if (fromType.GetInterfaces().Any(i => i == typeof(IFormattable)))
            {
                return f => ((IFormattable)f).ToString(null, formatProvider);
            }

            return v => v.ToString();
        }
    }
}
