namespace MapEverything.Tests
{
    using System.Globalization;

    using NUnit.Framework;

    [TestFixture]
    public class TypeMapperDecimalTests : TypeMapperTestsBase
    {
        [Test]
        public void CanConvertDecimalToInt()
        {
            const decimal DecimalValue = 123m;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<decimal, int>(DecimalValue);
                Assert.AreEqual(123, converted);
            }
        }

        [Test]
        public void CanConvertStringToDecimalWithInvariantCulture()
        {
            const decimal DecimalValue = 123.45m;
            var cultureInfo = CultureInfo.InvariantCulture;
            var value = DecimalValue.ToString(cultureInfo);

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<string, decimal>(value, cultureInfo);
                Assert.AreEqual(DecimalValue, converted);
            }
        }

        [Test]
        public void CanConvertDecimalToStringWithInvariantCulture()
        {
            const decimal DecimalValue = 123.45m;
            var cultureInfo = CultureInfo.InvariantCulture;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<decimal, string>(DecimalValue, cultureInfo);
                Assert.AreEqual(DecimalValue.ToString(cultureInfo), converted);
            }
        }

        [Test]
        public void CanConvertStringToDecimalWithCurrentCulture()
        {
            const decimal DecimalValue = 123.45m;
            var cultureInfo = CultureInfo.CurrentCulture;
            var value = DecimalValue.ToString(cultureInfo);

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<string, decimal>(value, cultureInfo);
                Assert.AreEqual(DecimalValue, converted);
            }
        }

        [Test]
        public void CanConvertDecimalToStringWithCurrentCulture()
        {
            const decimal DecimalValue = 123.45m;
            var cultureInfo = CultureInfo.CurrentCulture;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<decimal, string>(DecimalValue, cultureInfo);
                Assert.AreEqual(DecimalValue.ToString(cultureInfo), converted);
            }
        }

        [Test]
        public void CanConvertStringToDecimalWithoutCulture()
        {
            const decimal DecimalValue = 123.45m;
            var value = DecimalValue.ToString();

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<string, decimal>(value);
                Assert.AreEqual(DecimalValue, converted);
            }
        }

        [Test]
        public void CanConvertDecimalToStringWithoutCulture()
        {
            const decimal DecimalValue = 123.45m;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<decimal, string>(DecimalValue);
                Assert.AreEqual(DecimalValue.ToString(), converted);
            }
        }
    }
}
