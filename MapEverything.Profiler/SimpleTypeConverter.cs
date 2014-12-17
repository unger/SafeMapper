namespace LuceneNetExtensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;

    public class SimpleTypeConverter
    {
        private static Converter<object, object> guidToString = input => input.ToString();
        private static Converter<object, object> stringToGuid = input =>
            {
                Guid guid;
                return Guid.TryParse((string)input, out guid) ? guid : Guid.Empty;
            };

        private static Dictionary<string, Converter<object, object>> converters = new Dictionary<string, Converter<object, object>>();

        static SimpleTypeConverter()
        {
            var guidToStringKey = string.Format("{0},{1}", typeof(Guid).FullName, typeof(string).FullName);
            var stringToGuidKey = string.Format("{0},{1}", typeof(string).FullName, typeof(Guid).FullName);

            converters.Add(guidToStringKey, guidToString);
            converters.Add(stringToGuidKey, stringToGuid);
        }

        public static TDestination ConvertTo<TDestination>(object value)
        {
            return ConvertTo<TDestination>(value, Thread.CurrentThread.CurrentCulture);
        }

        public static TDestination ConvertTo<TDestination>(object value, IFormatProvider formatProvider)
        {
            return (TDestination)ConvertTo(value, typeof(TDestination), formatProvider);
        }

        public static TDestination ConvertTo<TSource, TDestination>(TSource value)
        {
            return ConvertTo<TSource, TDestination>(value, Thread.CurrentThread.CurrentCulture);
        }

        public static TDestination ConvertTo<TSource, TDestination>(TSource value, IFormatProvider formatProvider)
        {
            return (TDestination)ConvertTo(value, typeof(TDestination), formatProvider);
        }

        public static object ConvertTo(object value, Type toType)
        {
            return ConvertTo(value, toType, Thread.CurrentThread.CurrentCulture);
        }

        public static object ConvertTo(object value, Type toType, IFormatProvider formatProvider)
        {
            if (IsCollectionType(toType))
            {
                return ConvertMultipleValues(value, toType, formatProvider);
            }

            return ConvertSingleValue(value, toType, formatProvider);
        }

        public static object ConvertMultipleValues(object value, Type toType, IFormatProvider formatProvider)
        {
            // Handle Arrays
            if (toType.IsArray)
            {
                var values = GetValuesArray(value);
                var elementType = toType.GetElementType();
                var elements = Array.CreateInstance(elementType ?? typeof(object), values.Length);

                var method = toType.GetMethod("SetValue", new[] { elementType, typeof(int) });

                if (method != null)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        var typedVal = ConvertTo(values.GetValue(i), elementType, formatProvider);
                        if (typedVal != null)
                        {
                            method.Invoke(elements, new[] { typedVal, i });
                        }
                    }
                }

                return elements;
            }

            // Handle Generic types such as List<T>, Collection<T>, IEnumerable<T>
            if (toType.IsGenericType)
            {
                var generictype = toType.GetGenericTypeDefinition();
                var elementType = toType.GetGenericArguments()[0];

                // Try to create collection/list
                var elements = CreateGenericInstance(generictype, elementType);

                if (elements != null)
                {
                    var method = elements.GetType().GetMethod("Add");
                    if (method != null)
                    {
                        var values = GetValuesArray(value);
                        foreach (var val in values)
                        {
                            var typedVal = ConvertTo(val, elementType, formatProvider);
                            if (typedVal != null)
                            {
                                method.Invoke(elements, new[] { typedVal });
                            }
                        }
                    }
                }

                return elements;
            }

            return GetDefaultValue(toType);
        }

        private static Array GetValuesArray(object value)
        {
            if (value.GetType().IsArray)
            {
                return (Array)value;
            }
            
            if (IsCollectionType(value.GetType()))
            {
                var array = new ArrayList();
                foreach (var val in (IEnumerable)value)
                {
                    array.Add(val);
                }

                return array.ToArray();
            }

            return new[] { value };
        }

        private static bool IsCollectionType(Type type)
        {
            return type.IsArray
                || (!type.IsAssignableFrom(typeof(string)) && type.GetInterfaces().Any(t => t == typeof(IEnumerable)));
        }

        private static object ConvertSingleValue(object value, Type toType, IFormatProvider formatProvider)
        {
            var converter = value == null ? null : GetConverter(value.GetType(), toType);

            try
            {
                if (converter != null)
                {
                    return converter(value);
                }

                if (toType.IsGenericType)
                {
                    var underlyingType = Nullable.GetUnderlyingType(toType);
                    if (underlyingType != null)
                    {
                        return Convert.ChangeType(value, underlyingType, formatProvider);
                    }
                }

                return Convert.ChangeType(value, toType, formatProvider);
            }
            catch (Exception)
            {
                return GetDefaultValue(toType);
            }
        }

        private static Converter<object, object> GetConverter(Type fromType, Type toType)
        {
            var key = string.Format("{0},{1}", fromType.FullName, toType.FullName);
            if (converters.ContainsKey(key))
            {
                return converters[key];
            }

            // Return null converter
            return null;
        }

        private static object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        private static object CreateGenericInstance(Type generictype, Type elementType)
        {
            if (!generictype.IsInterface)
            {
                return Activator.CreateInstance(generictype.MakeGenericType(elementType));
            }

            if (generictype == typeof(IList<>))
            {
                return Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
            }

            if (generictype == typeof(IEnumerable<>))
            {
                return Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
            }

            if (generictype == typeof(ICollection<>))
            {
                return Activator.CreateInstance(typeof(Collection<>).MakeGenericType(elementType));
            }

            return null;
        }
    }
}
