namespace MapEverything.TypeMaps
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Fasterflect;

    public class TypeMap : ITypeMap
    {
        private readonly TypeDefinition fromTypeDef;

        private readonly TypeDefinition toTypeDef;

        private readonly ITypeMapper typeMapper;

        private Func<object, object> elementConverter;

        private List<IMemberMap> memberMaps = new List<IMemberMap>();

        public TypeMap(Type fromType, Type toType, IFormatProvider formatProvider, ITypeMapper typeMapper)
        {
            this.fromTypeDef = typeMapper.GetTypeDefinition(fromType);
            this.toTypeDef = typeMapper.GetTypeDefinition(toType);
            this.typeMapper = typeMapper;

            this.Convert = this.GenerateConvertDelegate(formatProvider);

            foreach (var key in this.fromTypeDef.MemberGetters.Keys)
            {
                if (this.toTypeDef.MemberSetters.ContainsKey(key))
                {
                    var fromMember = this.fromTypeDef.Members[key];
                    var toMember = this.toTypeDef.Members[key];

                    var converter = typeMapper.GetConverter(fromMember.Type(), toMember.Type(), formatProvider);

                    var memberMap = new MemberMap(
                        this.fromTypeDef.MemberGetters[key],
                        this.toTypeDef.MemberSetters[key],
                        converter);

                    this.memberMaps.Add(memberMap);
                }
            }
        }

        public Func<object, object> Convert { get; private set; }

        private Func<object, object> GenerateConvertDelegate(IFormatProvider formatProvider)
        {
            if (this.fromTypeDef.IsCollection && this.toTypeDef.IsCollection)
            {
                var fromElementType = this.fromTypeDef.ElementType;
                var toElementType = this.toTypeDef.ElementType;

                // From Array to Array
                if (this.fromTypeDef.ActualType.IsArray && this.toTypeDef.ActualType.IsArray)
                {
                    return this.ArrayConvertAllDelegate(this.fromTypeDef.ActualType, fromElementType, toElementType, formatProvider);
                }

                // From Array to Collection with IEnumerable<T> constructor
                var toEnumerableType = typeof(IEnumerable<>).MakeGenericType(toElementType);
                var toConstructor = this.toTypeDef.ActualType.GetConstructor(new Type[] { toEnumerableType });
                if (toConstructor != null && this.fromTypeDef.ActualType.IsArray)
                {
                    var fastToConstructor = toConstructor.DelegateForCreateInstance();
                    var arrayConverter = this.ArrayConvertAllDelegate(this.fromTypeDef.ActualType, fromElementType, toElementType, formatProvider);
                    return value => fastToConstructor(arrayConverter(value));
                }

                // From Generic collection to Array
                this.elementConverter = this.typeMapper.GetConverter(fromElementType, toElementType, formatProvider);
                
                if (this.toTypeDef.ActualType.IsArray)
                {
                    return this.ConvertFromCollectionToArray;
                }

                // From Generic collection to Generic collection
                if (this.toTypeDef.ActualType.IsGenericType)
                {
                    return this.ConvertToGenericCollection;
                }
            }

            return this.ConvertClass;
         }

        private object ConvertClass(object fromObject)
        {
            var toObject = this.toTypeDef.CreateInstanceDelegate();
            foreach (var member in this.memberMaps)
            {
                member.Map(fromObject, toObject);
            }

            return toObject;
        }

        private dynamic ConvertToGenericCollection(dynamic fromObject)
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

            if (this.fromTypeDef.ActualType.IsArray)
            {
                var values = (Array)fromObject;
                var newElements = this.toTypeDef.CreateInstanceDelegate();

                for (int i = 0; i < values.Length; i++)
                {
                    toAddDelegate(
                        newElements,
                        this.elementConverter(values.GetElement(i)));
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
