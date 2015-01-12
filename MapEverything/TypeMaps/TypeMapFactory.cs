namespace MapEverything.TypeMaps
{
    using System;
    using System.Collections;
    using System.Data.SqlTypes;
    using System.Linq;

    public class TypeMapFactory
    {
        private static readonly Type StringType = typeof(string);
        private static readonly Type ConvertibleType = typeof(IConvertible);

        public static ITypeMap Create(Type fromType, Type toType, IFormatProvider formatProvider, ITypeMapper typeMapper)
        {
            if (toType.IsAssignableFrom(fromType))
            {
                return Create(value => value);
            }
            
            if (fromType == StringType)
            {
                return new FromStringTypeMap(toType, formatProvider);
            }

            if (toType == StringType)
            {
                return new ToStringTypeMap(fromType, formatProvider);
            }

            if (fromType.GetInterfaces().Any(t => t == ConvertibleType) && toType.GetInterfaces().Any(t => t == ConvertibleType))
            {
                return new ConvertibleTypeMap(fromType, toType, formatProvider);
            }

            if (fromType == typeof(SqlDateTime) && toType == typeof(DateTime))
            {
                return Create(
                    v => ((SqlDateTime)v).Value);
            }

            if (fromType == typeof(DateTime) && toType == typeof(SqlDateTime))
            {
                return Create(
                    v => (DateTime)v < (DateTime)SqlDateTime.MinValue ? SqlDateTime.MinValue : (SqlDateTime)(DateTime)v);
            }

            if (IsCollectionType(fromType) && IsCollectionType(toType))
            {
                return new CollectionTypeMap(fromType, toType, formatProvider, typeMapper);
            }

            // Use legacy TypeMap until all types are converted to own classes
            return new ClassTypeMap(fromType, toType, formatProvider, typeMapper);
        }

        public static ITypeMap Create(Func<object, object> converter)
        {
            return new SimpleTypeMap(converter);
        }

        private static bool IsCollectionType(Type type)
        {
            return type.IsArray
                || (!type.IsAssignableFrom(typeof(string)) && type.GetInterfaces().Any(t => t == typeof(IEnumerable)));
        }
    }
}
