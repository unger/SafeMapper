namespace MapEverything.Tests
{
    using System;
    using System.Data.SqlTypes;
    using System.Globalization;

    using NUnit.Framework;

    public class TypeMapperDateTimeTests : TypeMapperTestsBase
    {
        [Test]
        public void ConvertStringToDateTime()
        {
            var value = DateTime.Today;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<string, DateTime>(value.ToString());
                Assert.AreEqual(value, converted);
            }
        }

        [Test]
        public void CanConvertDateTimeToString()
        {
            var value = DateTime.Today;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<DateTime, string>(value);
                Assert.AreEqual(value.ToString(), converted);
            }
        }

        [Test]
        public void ConvertStringToDateTimeWithInvariantCulture()
        {
            var value = DateTime.Today;
            var cultureInfo = CultureInfo.InvariantCulture;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<string, DateTime>(value.ToString(cultureInfo), cultureInfo);
                Assert.AreEqual(value, converted);
            }
        }

        [Test]
        public void CanConvertDateTimeToStringWithInvariantCulture()
        {
            var value = DateTime.Today;
            var cultureInfo = CultureInfo.InvariantCulture;

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<DateTime, string>(value, cultureInfo);
                Assert.AreEqual(value.ToString(cultureInfo), converted);
            }
        }

    }
}
