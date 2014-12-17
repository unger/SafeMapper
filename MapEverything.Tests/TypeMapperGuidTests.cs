namespace MapEverything.Tests
{
    using System;

    using NUnit.Framework;

    public class TypeMapperGuidTests : TypeMapperTestsBase
    {
        [Test]
        public void CanConvertStringToGuid()
        {
            var value = Guid.NewGuid();

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<string, Guid>(value.ToString());
                Assert.AreEqual(value, converted);
            }
        }

        [Test]
        public void CanConvertGuidToString()
        {
            var value = Guid.NewGuid();

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<Guid, string>(value);
                Assert.AreEqual(value.ToString(), converted);
            }
        }
    }
}
