namespace MapEverything
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;

    using Fasterflect;

    using MapEverything.TypeMaps;

    public class TypeMapper : ITypeMapper
    {
        private readonly ConcurrentDictionary<string, ITypeMap> typeMappers;
        private readonly ConcurrentDictionary<Type, TypeDefinition> typeDefinitions;

        public TypeMapper()
        {
            this.typeMappers = new ConcurrentDictionary<string, ITypeMap>();
            this.typeDefinitions = new ConcurrentDictionary<Type, TypeDefinition>();
        }
        
        public TTo Convert<TFrom, TTo>(TFrom value)
        {
            return (TTo)this.Convert(value, typeof(TFrom), typeof(TTo));
        }

        public TTo Convert<TFrom, TTo>(TFrom value, IFormatProvider formatProvider)
        {
            return (TTo)this.Convert(value, typeof(TFrom), typeof(TTo), formatProvider);
        }

        public TTo Convert<TFrom, TTo>(TFrom value, Converter<TFrom, TTo> converter)
        {
            return converter(value);
        }

        public Converter<TFrom, TTo> GetConverter<TFrom, TTo>()
        {
            return this.GetConverter<TFrom, TTo>(CultureInfo.CurrentCulture);
        }

        public Converter<TFrom, TTo> GetConverter<TFrom, TTo>(IFormatProvider formatProvider)
        {
            var converter = this.GetConverter(typeof(TFrom), typeof(TTo), formatProvider);
            return value => (TTo)converter(value);
        }

        public object Convert(object value, Type fromType, Type toType)
        {
            return this.GetTypeMap(fromType, toType, CultureInfo.CurrentCulture).Convert(value);
        }

        public object Convert(object value, Func<object, object> converter)
        {
            return converter(value);
        }

        public virtual object Convert(object value, Type fromType, Type toType, IFormatProvider formatProvider)
        {
            return this.GetTypeMap(fromType, toType, formatProvider).Convert(value);
        }

        public Func<object, object> GetConverter(Type fromType, Type toType)
        {
            return this.GetConverter(fromType, toType, CultureInfo.CurrentCulture);
        }

        public virtual Func<object, object> GetConverter(Type fromType, Type toType, IFormatProvider formatProvider)
        {
            return this.GetTypeMap(fromType, toType, formatProvider).Convert;
        }

        public TypeDefinition GetTypeDefinition(Type type)
        {
            return this.typeDefinitions.AddOrUpdate(type, t => new TypeDefinition(t), (t, definition) => definition);
        }

        private ITypeMap GetTypeMap(Type fromType, Type toType, IFormatProvider formatProvider)
        {
            return this.typeMappers.GetOrAdd(string.Concat(fromType.FullName, toType.FullName), t => TypeMapFactory.Create(fromType, toType, formatProvider, this));
        }
    }
}
