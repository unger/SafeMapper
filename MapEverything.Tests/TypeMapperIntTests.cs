namespace MapEverything.Tests
{
    using NUnit.Framework;

    public class TypeMapperIntTests : TypeMapperTestsBase
    {
        [Test]
        public void CanConvertStringToInt()
        {
            var value = "12345";

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<string, int>(value);
                Assert.AreEqual(12345, converted);
            }
        }

        [Test]
        public void CanConvertIntToString()
        {
            var value = 12345;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<int, string>(value);
                Assert.AreEqual("12345", converted);
            }
        }
    }
}
