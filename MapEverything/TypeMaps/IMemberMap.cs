namespace MapEverything
{
    using System;

    public interface IMemberMap
    {
        Type FromMemberType { get; }

        Type ToMemberType { get; }

        void Map(object fromObject, object toObject);

        void SetConverter(Func<object, object> conv);

        bool IsValid();

    }
}