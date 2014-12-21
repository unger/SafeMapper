namespace MapEverything
{
    using System;

    using Fasterflect;

    public class DictionaryMemberMap : IMemberMap
    {
        private readonly Type fromType;

        private readonly Type toType;

        private dynamic fromTypeGetDelegate;

        private dynamic toTypeSetDelegate;

        private Func<object, object> converter;

        public DictionaryMemberMap(Type fromType, Type toType, string fromPropertyName, string toPropertyName)
        {
            this.fromType = fromType;
            this.toType = toType;
            this.fromTypeGetDelegate = this.FindGetDelegate(fromType, fromPropertyName);
            this.toTypeSetDelegate = this.FindSetDelegate(toType, toPropertyName);
            this.converter = value => value;
        }

        public Type FromPropertyType { get; private set; }

        public Type ToPropertyType { get; private set; }

        public bool IsValid()
        {
            return this.fromTypeGetDelegate != null && this.toTypeSetDelegate != null && this.converter != null;
        }

        public void SetConverter(Func<object, object> conv)
        {
            this.converter = conv;
        }

        public void Map(object fromObject, object toObject)
        {
            this.toTypeSetDelegate(toObject, this.converter(this.fromTypeGetDelegate(fromObject)));
        }

        private Func<object, object> FindGetDelegate(Type type, string propertyName)
        {
            var getIndexer = type.GetProperty("Item", new[] { typeof(string) });
            if (getIndexer != null)
            {
                this.FromPropertyType = getIndexer.PropertyType;
                return obj => type.DelegateForGetIndexer(new[] { typeof(string) })(obj, propertyName);
            }

            var pi = type.GetProperty(propertyName);
            if (pi != null)
            {
                this.FromPropertyType = pi.PropertyType;
                return obj => pi.DelegateForGetPropertyValue()(obj);
            }

            var fi = type.GetField(propertyName);
            if (fi != null)
            {
                this.FromPropertyType = fi.FieldType;
                return obj => fi.DelegateForGetFieldValue()(obj);
            }

            return null;
        }

        private Action<object, object> FindSetDelegate(Type type, string propertyName)
        {
            var setIndexer = type.GetProperty("Item", new[] { typeof(string) });
            if (setIndexer != null)
            {
                if (setIndexer.IsWritable())
                {
                    this.ToPropertyType = setIndexer.PropertyType;
                    return (obj, value) => type.DelegateForSetIndexer(new[] { typeof(string), setIndexer.PropertyType })(obj, propertyName, value);
                }
            }

            var pi = type.GetProperty(propertyName);
            if (pi != null)
            {
                if (pi.IsWritable())
                {
                    this.ToPropertyType = pi.PropertyType;
                    return (obj, value) => pi.DelegateForSetPropertyValue()(obj, value);
                }
            }

            var fi = type.GetField(propertyName);
            if (fi != null)
            {
                if (fi.IsWritable())
                {
                    this.ToPropertyType = fi.FieldType;
                    return (obj, value) => fi.DelegateForSetFieldValue()(obj, value);
                }
            }

            return null;
        }
    }
}
