using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEverything.Tests
{
    using System.Globalization;

    using NUnit.Framework;

    [TestFixture]
    public class SafeConvertTests
    {
        [Test]
        public void ToInt32_FromLongIntMaxValue_ShouldReturnIntMaxValue()
        {
            long longValue = int.MaxValue;

            var result = SafeConvert.ToInt32(longValue);

            Assert.AreEqual(int.MaxValue, result);
        }

        [Test]
        public void ToInt32_FromLongIntMinValue_ShouldReturnIntMinValue()
        {
            long longValue = int.MinValue;

            var result = SafeConvert.ToInt32(longValue);

            Assert.AreEqual(int.MinValue, result);
        }

        [Test]
        public void ToInt32_FromLongMaxValue_ShouldReturnZero()
        {
            long longValue = long.MaxValue;

            var result = SafeConvert.ToInt32(longValue);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void ToInt32_FromLongMinValue_ShouldReturnZero()
        {
            long longValue = long.MinValue;

            var result = SafeConvert.ToInt32(longValue);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void ToInt32_FromUIntMaxValue_ShouldReturnZero()
        {
            uint uintValue = uint.MaxValue;

            var result = SafeConvert.ToInt32(uintValue);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void ToInt32_FromUIntMinValue_ShouldReturnZero()
        {
            uint uintValue = uint.MinValue;

            var result = SafeConvert.ToInt32(uintValue);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void ToInt32_FromDecimalMaxValue_ShouldReturnZero()
        {
            decimal value = decimal.MaxValue;

            var result = SafeConvert.ToInt32(value);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void ToInt32_FromDecimalMinValue_ShouldReturnZero()
        {
            decimal value = decimal.MinValue;

            var result = SafeConvert.ToInt32(value);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void ToInt32_FromDecimalWithDecimalsBelowZeroPointFive_ShouldReturnTruncatedValue()
        {
            decimal value = 123.49m;

            var result = SafeConvert.ToInt32(value);

            Assert.AreEqual(123, result);
        }

        [Test]
        public void ToInt32_FromDecimalWithDecimalsZeroPointFive_ShouldReturnCeiledValue()
        {
            decimal value = 123.5m;

            var result = SafeConvert.ToInt32(value);

            Assert.AreEqual(124, result);
        }

        [Test]
        public void ToInt32_FromStringWithIntMaxValue_ShouldReturnIntMaxValue()
        {
            string value = int.MaxValue.ToString();

            var result = SafeConvert.ToInt32(value);

            Assert.AreEqual(int.MaxValue, result);
        }

        [Test]
        public void ToInt32_FromStringWithIntMinValue_ShouldReturnIntMinValue()
        {
            string value = int.MinValue.ToString();

            var result = SafeConvert.ToInt32(value);

            Assert.AreEqual(int.MinValue, result);
        }

        [Test]
        public void ToInt32_FromStringWithLongMaxValue_ShouldReturnZero()
        {
            string value = long.MaxValue.ToString();

            var result = SafeConvert.ToInt32(value);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void ToInt32_FromStringWithoutNumeric_ShouldReturnZero()
        {
            string value = "abc";

            var result = SafeConvert.ToInt32(value);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void ToInt32_FromStringWithAlphaNumeric_ShouldReturnZero()
        {
            string value = "123a";

            var result = SafeConvert.ToInt32(value);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void ToInt32_FromStringNumericWithThousandSeperator_ShouldReturnNumeric()
        {
            string value = "1 234";

            var result = SafeConvert.ToInt32(value);

            Assert.AreEqual(1234, result);
        }

        [Test]
        public void ToInt32_FromStringNegativeNumeric_ShouldReturnNegativeValue()
        {
            string value = "-1234";

            var result = SafeConvert.ToInt32(value);

            Assert.AreEqual(-1234, result);
        }

        [Test]
        public void ToInt32_FromStringWithDecimalsLessThanZeroPointFive_ShouldReturnTruncatedValue()
        {
            string value = "1234.000";

            var result = SafeConvert.ToInt32(value);

            Assert.AreEqual(1234, result);
        }
    }
}
