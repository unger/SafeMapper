namespace MapEverything.TypeMaps
{
    using System;
    using System.Reflection;

    using Fasterflect;

    public class MemberMap : IMemberMap
    {
        private MemberGetter fromTypeGetDelegate;

        private MemberSetter toTypeSetDelegate;

        private Func<object, object> converter;

        public MemberMap(
            MemberInfo fromMember,
            MemberInfo toMember,
            MemberGetter fromMemberGetter,
            MemberSetter toMemberSetter)
            : this(
                fromMember.Type(),
                toMember.Type(),
                fromMemberGetter,
                toMemberSetter)
        {
        }

        public MemberMap(Type fromMemberType, Type toMemberType, MemberGetter fromMemberGetter, MemberSetter toMemberSetter)
        {
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
                this.converter(this.fromTypeGetDelegate(fromObject.WrapIfValueType())));
        }
    }
}
