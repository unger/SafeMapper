namespace SafeMapper.Tests
{
    using NUnit.Framework;

    using SafeMapper.Utils;

    [TestFixture]
    public class SafeConvertTests
    {
        [TestCaseSource(typeof(TestData), "LongToIntData")]
        public int ToInt32_FromLong(long input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCaseSource(typeof(TestData), "UIntToIntData")]
        public int ToInt32_FromUInt(uint input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToIntData")]
        public int ToInt32_FromDecimal(decimal input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCaseSource(typeof(TestData), "DoubleToIntData")]
        public int ToInt32_FromDouble(double input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCaseSource(typeof(TestData), "StringToIntData")]
        public int ToInt32_FromString(string input)
        {
            return SafeConvert.ToInt32(input);
        }
    }
}
