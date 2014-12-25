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

        public MemberMap(Type fromType, Type toType, Type fromMemberType, Type toMemberType, MemberGetter fromMemberGetter, MemberSetter toMemberSetter)
        {
            this.fromType = fromType;
            this.toType = toType;
            this.fromTypeGetDelegate = fromMemberGetter;
            this.toTypeSetDelegate = toMemberSetter;

            this.converter = value => value;

            this.FromMemberType = fromMemberType;
            this.ToMemberType = toMemberType;
        }

        public Type FromMemberType { get; private set; }

        public Type ToMemberType { get; private set; }

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
            this.toTypeSetDelegate(
                toObject, 
                this.converter(this.fromTypeGetDelegate(fromObject)));
        }
        /*
        protected MemberGetter FindMemberGetter(Type type, string propertyName)
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

        protected MemberSetter FindMemberSetter(Type type, string propertyName)
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
        }*/
    }
}
