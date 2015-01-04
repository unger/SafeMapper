namespace MapEverything.TypeMaps
{
    using System;

    public interface IMemberMap
    {
        Action<object, object> Map { get; }
    }
}