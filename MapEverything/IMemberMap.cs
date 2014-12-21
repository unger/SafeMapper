namespace MapEverything
{
    using System;

    public interface IMemberMap
    {
        Type FromPropertyType { get; }

        Type ToPropertyType { get; }

        void Map(object fromObject, object toObject);

        void SetConverter(Func<object, object> conv);

        bool IsValid();

    }
}