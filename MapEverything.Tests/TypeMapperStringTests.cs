namespace MapEverything.Tests
{
    using System;

    using MapEverything.Converters;

    using NUnit.Framework;

    public class TypeMapperStringTests : TypeMapperTestsBase
    {
        [Test]
        public void ConvertEmptyStringToDateTime()
        {
            string value = string.Empty;
            var expected = DateTime.MinValue;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<string, DateTime>(value);
                Assert.AreEqual(expected, converted);
            }
        }

        [Test]
        public void ConvertNullStringToDateTime()
        {
            string value = null;
            var expected = DateTime.MinValue;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<string, DateTime>(value);
                Assert.AreEqual(expected, converted);
            }
        }

    }
}
