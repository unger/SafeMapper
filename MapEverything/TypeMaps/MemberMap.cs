namespace MapEverything.TypeMaps
{
    using System;

    using Fasterflect;

    public class MemberMap : IMemberMap
    {
        public MemberMap(MemberGetter fromMemberGetter, MemberSetter toMemberSetter, Func<object, object> converter)
        {
            this.Map = this.CreateMapAction(fromMemberGetter, toMemberSetter, converter);
        }

        public Action<object, object> Map { get; private set; }

        private Action<object, object> CreateMapAction(MemberGetter fromMemberGetter, MemberSetter toMemberSetter, Func<object, object> converter)
        {
            if (converter != null)
            {
                return
                    (fromObject, toObject) =>
                    toMemberSetter(
                        toObject,
                        converter(fromMemberGetter(fromObject)));
            }

            return (fromObject, toObject) =>
                    toMemberSetter(
                        toObject,
                        fromMemberGetter(fromObject));
        }
    }
}
