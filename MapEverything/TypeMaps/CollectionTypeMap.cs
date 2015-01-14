namespace MapEverything.TypeMaps
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Fasterflect;

    public class CollectionTypeMap : ITypeMap
    {
        private readonly TypeDefinition fromTypeDef;

        private readonly TypeDefinition toTypeDef;

        private readonly ITypeMapper typeMapper;

        private Func<object, object> elementConverter;

        public CollectionTypeMap(Type fromType, Type toType, IFormatProvider formatProvider, ITypeMapper typeMapper)
        {
            this.fromTypeDef = typeMapper.GetTypeDefinition(fromType);
            this.toTypeDef = typeMapper.GetTypeDefinition(toType);
            this.typeMapper = typeMapper;

            this.Convert = this.GenerateConvertDelegate(fromType, toType, formatProvider);
        }

        public Func<object, object> Convert { get; private set; }

        private Func<object, object> GenerateConvertDelegate(Type fromType, Type toType, IFormatProvider formatProvider)
        {
            var fromElementType = this.fromTypeDef.ElementType;
            var toElementType = this.toTypeDef.ElementType;

            // From Array to Array
            if (fromType.IsArray && toType.IsArray)
            {
                return this.ArrayConvertAllDelegate(fromType, fromElementType, toElementType, formatProvider);
            }

            // From Array to Collection with IEnumerable<T> constructor
            var toEnumerableType = typeof(IEnumerable<>).MakeGenericType(toElementType);
            var toConstructor = toType.GetConstructor(new Type[] { toEnumerableType });
            var toArrayMethod = fromType.GetMethod("ToArray", new Type[0]);

            if (toConstructor != null && fromType.IsArray)
            {
                var fastToConstructor = toConstructor.DelegateForCreateInstance();
                var arrayConverter = this.ArrayConvertAllDelegate(fromType, fromElementType, toElementType, formatProvider);
                return value => fastToConstructor(arrayConverter(value));
            }

            // Generic collection with toArray method to generic collection with IEnumerable<T> constructor
            if (toConstructor != null && toArrayMethod != null)
            {
                var fastToConstructor = toConstructor.DelegateForCreateInstance();
                var fastToArrayMethod = toArrayMethod.DelegateForCallMethod();
                var arrayConverter = this.ArrayConvertAllDelegate(fromType, fromElementType, toElementType, formatProvider);
                return value => fastToConstructor(arrayConverter(fastToArrayMethod(value)));
            }

            // From Generic collection to Array
            this.elementConverter = this.typeMapper.GetConverter(fromElementType, toElementType, formatProvider);

            if (toType.IsArray)
            {
                // Try to execute ToArray and ConvertAll
                if (toArrayMethod != null)
                {
                    var fastToArrayMethod = toArrayMethod.DelegateForCallMethod();

                    if (fromElementType == toElementType)
                    {
                        return value => fastToArrayMethod(value);
                    }

                    var arrayConverter = this.ArrayConvertAllDelegate(fromType, fromElementType, toElementType, formatProvider);
                    return value => arrayConverter(fastToArrayMethod(value));
                }

                return this.ConvertFromCollectionToArray;
            }

            // From Generic collection to Generic collection
            return this.ConvertToGenericCollection;
        }

        private object ConvertToGenericCollection(object fromObject)
        {
            var toAddDelegate = this.toTypeDef.AddElementDelegate;

            var collection = fromObject as ICollection;
            if (collection != null)
            {
                var newElements = this.toTypeDef.CreateInstanceDelegate();

                foreach (var elementValue in collection)
                {
                    toAddDelegate(
                        newElements,
                        this.elementConverter(elementValue));
                }

                return newElements;
            }

            return this.toTypeDef.CreateInstanceDelegate();
        }

        private Func<object, object> ArrayConvertAllDelegate(Type fromType, Type fromElementType, Type toElementType, IFormatProvider formatProvider)
        {
            var genericConvertType = typeof(Converter<,>).MakeGenericType(fromElementType, toElementType);
            var fasterflectConvertAll = typeof(Array).DelegateForCallMethod(
                new Type[] { fromElementType, toElementType },
                "ConvertAll",
                new Type[] { fromType, genericConvertType });

            var getConverterDelegate = this.typeMapper.GetType().DelegateForCallMethod(
                new Type[] { fromElementType, toElementType },
                "GetConverter",
                new Type[] { typeof(IFormatProvider) });

            var genericConverter = getConverterDelegate(this.typeMapper, formatProvider);

            return value => fasterflectConvertAll(null, value, genericConverter);
        }

        private object ConvertFromCollectionToArray(object fromObject)
        {
            var collection = fromObject as ICollection;
            if (collection != null)
            {
                var newElements = Array.CreateInstance(this.toTypeDef.ElementType, collection.Count);

                var i = 0;
                foreach (var elementValue in collection)
                {
                    newElements.SetValue(this.elementConverter(elementValue), i);
                    i++;
                }

                return newElements;
            }

            return Array.CreateInstance(this.toTypeDef.ElementType, 0);
        }
    }
}
