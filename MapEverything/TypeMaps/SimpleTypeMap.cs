namespace MapEverything.TypeMaps
{
    using System;

    public class SimpleTypeMap : ITypeMap
    {
        public SimpleTypeMap(Func<object, object> converter)
        {
            this.Convert = converter;
        }

        public Func<object, object> Convert { get; private set; }
    }
}
