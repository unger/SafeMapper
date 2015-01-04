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

        [Test]
        public void ConvertNullToInt_ShouldReturnDefault()
        {
            string value = null;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<string, int>(value);
                Assert.AreEqual(default(int), converted);
            }
        }

        [Test]
        public void Convert_IntToDecimal()
        {
            var value = 12345;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<int, decimal>(value);
                Assert.AreEqual(12345m, converted);
            }
        }

        [Test]
        public void Convert_IntToDouble()
        {
            var value = 12345;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<int, double>(value);
                Assert.IsInstanceOf(typeof(double), converted);
                Assert.AreEqual(12345d, converted);
            }
        }

        [Test]
        public void Convert_IntToShort()
        {
            var value = 123;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<int, short>(value);
                Assert.IsInstanceOf(typeof(short), converted);
                Assert.AreEqual(123, converted);
            }
        }

        [Test]
        public void Convert_LargeIntToShort()
        {
            var value = int.MaxValue;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<int, short>(value);
                Assert.IsInstanceOf(typeof(short), converted);
                Assert.AreEqual(int.MaxValue, converted);
            }
        }


    }
}
