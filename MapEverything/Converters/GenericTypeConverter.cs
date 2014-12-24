namespace MapEverything.Converters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;

    using Fasterflect;

    public class GenericTypeConverter<TFrom, TTo> : TypeConverter
    {
        private readonly ITypeMapper typeMapper;

        private TypeMap fromTypeMap;
        private TypeMap toTypeMap;

        private TypeDefinition fromTypeDef;

        private TypeDefinition toTypeDef;

        public GenericTypeConverter(ITypeMapper typeMapper) 
            : this(typeMapper, CultureInfo.CurrentCulture)
        {
        }

        public GenericTypeConverter(ITypeMapper typeMapper, IFormatProvider formatProvider)
        {
            this.typeMapper = typeMapper;
            var fromType = typeof(TFrom);
            var toType = typeof(TTo);

            this.fromTypeMap = new TypeMap(toType, fromType, typeMapper);
            this.toTypeMap = new TypeMap(fromType, toType, typeMapper);

            this.fromTypeDef = typeMapper.GetTypeDefinition(fromType);
            this.toTypeDef = typeMapper.GetTypeDefinition(toType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(TTo))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(TTo))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is TTo)
            {
                if (this.fromTypeDef.IsCollection && this.toTypeDef.IsCollection)
                {
                    if (this.fromTypeDef.ActualType.IsArray)
                    {
                        return this.CreateArray(this.toTypeDef, this.fromTypeDef, value);
                    }

                    if (this.fromTypeDef.ActualType.IsGenericType)
                    {
                        return this.CreateGenericCollection(this.toTypeDef, this.fromTypeDef, value);
                    }

                    return this.fromTypeDef.CreateInstanceDelegate();
                }
                else
                {
                    var result = Activator.CreateInstance<TFrom>();
                    this.fromTypeMap.Map(value, result);
                    return result;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        private object CreateGenericCollection(TypeDefinition fromTypeDefinition, TypeDefinition toTypeDefinition, object value)
        {
            var fromElementType = fromTypeDefinition.ElementType;
            var toElementType = toTypeDefinition.ElementType;
            var elementConverter = this.typeMapper.GetConverter(fromElementType, toElementType);

            var toAddDelegate = toTypeDefinition.ActualType.DelegateForCallMethod("Add", new[] { toElementType });

            if (value.GetType().IsArray)
            {
                var values = (Array)value;
                var newElements = toTypeDefinition.CreateInstanceDelegate();

                for (int i = 0; i < values.Length; i++)
                {
                    toAddDelegate(
                        newElements,
                        elementConverter(values.GetElement(i)));
                }

                return newElements;
            }

            var collection = value as ICollection;
            if (collection != null)
            {
                var newElements = toTypeDefinition.CreateInstanceDelegate();

                var i = 0;
                foreach (var elementValue in collection)
                {
                    toAddDelegate(
                        newElements,
                        elementConverter(elementValue));
                    i++;
                }

                return newElements;
            }

            return toTypeDefinition.CreateInstanceDelegate();
        }

        private object CreateArray(TypeDefinition fromTypeDefinition, TypeDefinition toTypeDefinition, object value)
        {
            var fromElementType = fromTypeDefinition.ElementType;
            var toElementType = toTypeDefinition.ElementType;
            var elementConverter = this.typeMapper.GetConverter(fromElementType, toElementType);

            if (value.GetType().IsArray)
            {
                var values = (Array)value;
                var newElements = Array.CreateInstance(fromElementType ?? typeof(object), values.Length);

                for (int i = 0; i < values.Length; i++)
                {
                    newElements.SetValue(elementConverter(values.GetElement(i)), i);
                }

                return newElements;
            }

            var collection = value as ICollection;
            if (collection != null)
            {
                var newElements = Array.CreateInstance(fromElementType ?? typeof(object), collection.Count);

                var i = 0;
                foreach (var elementValue in collection)
                {
                    newElements.SetValue(elementConverter(elementValue), i);
                    i++;
                }

                return newElements;
            }

            return Array.CreateInstance(fromElementType ?? typeof(object), 0);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(TTo) && value is TFrom)
            {
                if (this.fromTypeDef.IsCollection && this.toTypeDef.IsCollection)
                {
                    if (this.toTypeDef.ActualType.IsArray)
                    {
                        return this.CreateArray(this.fromTypeDef, this.toTypeDef, value);
                    }

                    if (this.toTypeDef.ActualType.IsGenericType)
                    {
                        return this.CreateGenericCollection(this.fromTypeDef, this.toTypeDef, value);
                    }

                    return this.fromTypeDef.CreateInstanceDelegate();
                }
                else
                {
                    var result = Activator.CreateInstance<TTo>();
                    this.toTypeMap.Map(value, result);
                    return result;
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
