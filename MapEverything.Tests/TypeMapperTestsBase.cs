namespace MapEverything.Tests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    public class TypeMapperTestsBase
    {
        public List<ITypeMapper> Mappers { get; private set; }

        [SetUp]
        public void SetUp()
        {
            this.Mappers = new List<ITypeMapper> { new TypeMapper(), new ReflectionTypeMapper() };
        }
    }
}
