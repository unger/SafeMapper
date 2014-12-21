namespace MapEverything
{
    using System;

    using Fasterflect;

    public class MemberMap : IMemberMap
    {
        private readonly Type fromType;

        private readonly Type toType;

        private MemberGetter fromTypeGetDelegate;

        private MemberSetter toTypeSetDelegate;

        private Func<object, object> converter;

        public MemberMap(Type fromType, Type toType, string fromPropertyName, string toPropertyName)
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

        private MemberGetter FindGetDelegate(Type type, string propertyName)
        {
            var pi = type.GetProperty(propertyName);
            if (pi != null)
            {
                this.FromPropertyType = pi.PropertyType;
                return pi.DelegateForGetPropertyValue();
            }

            var fi = type.GetField(propertyName);
            if (fi != null)
            {
                this.FromPropertyType = fi.FieldType;
                return fi.DelegateForGetFieldValue();
            }

            return null;
        }

        private MemberSetter FindSetDelegate(Type type, string propertyName)
        {
            var pi = type.GetProperty(propertyName);
            if (pi != null)
            {
                if (pi.IsWritable())
                {
                    this.ToPropertyType = pi.PropertyType;
                    return pi.DelegateForSetPropertyValue();
                }
            }

            var fi = type.GetField(propertyName);
            if (fi != null)
            {
                if (fi.IsWritable())
                {
                    this.ToPropertyType = fi.FieldType;
                    return fi.DelegateForSetFieldValue();
                }
            }

            return null;
        }
    }
}
