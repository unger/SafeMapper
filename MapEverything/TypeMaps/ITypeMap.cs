namespace MapEverything.TypeMaps
{
    using System;

    public interface ITypeMap
    {
        Func<object, object> Convert { get; }
    }
}