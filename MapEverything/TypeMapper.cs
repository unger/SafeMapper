namespace MapEverything
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;

    public class TypeMapper
    {
        public TypeMapper()
        {
            
        }

        public object ConvertTo(object value, Type type)
        {
            return this.ConvertTo(value, type, CultureInfo.CurrentCulture);
        }

        public object ConvertTo(object value, Type type, IFormatProvider formatProvider)
        {
            if (value == null)
            {
                return this.GetDefaultValue(type);
            }

            var converter = this.FindConverter(value.GetType(), type, formatProvider);


            return converter.Invoke(null, new[] { value });
        }

        private MethodInfo FindConverter(Type sourceType, Type destinationType, IFormatProvider formatProvider)
        {
            var parseMethod = destinationType.GetMethod("Parse", new Type[] { sourceType });
            if (parseMethod != null)
            {
                return parseMethod;
            }

            return null;
        }

        private object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }

            return null;
        }
    }
}
