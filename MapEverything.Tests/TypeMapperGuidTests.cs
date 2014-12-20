namespace MapEverything.Tests
{
    using System;

    using MapEverything.Converters;

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

        [Test]
        public void ConvertNonGuidStringToGuid_ShouldReturnEmptyGuid()
        {
            var value = "test";

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<string, Guid>(value);
                Assert.AreEqual(Guid.Empty, converted);
            }
        }
    }
}
