namespace SafeMapper.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class SafeConvertTests
    {
        #region ToByte

        [TestCase("255", Result = byte.MaxValue)]
        [TestCase("0", Result = byte.MinValue)]
        [TestCase("256", Result = 0)]
        [TestCase("-1", Result = 0)]
        [TestCase(":", Result = 0)]
        [TestCase("/", Result = 0)]
        public byte ToByte_FromString(string input)
        {
            return SafeConvert.ToByte(input);
        }

        [TestCase(sbyte.MaxValue, Result = (byte)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (byte)0)]
        public byte ToByte_FromSByte(sbyte input)
        {
            return SafeConvert.ToByte(input);
        }

        [TestCase(byte.MaxValue, Result = byte.MaxValue)]
        [TestCase(byte.MinValue, Result = byte.MinValue)]
        public byte ToByte_FromByte(byte input)
        {
            return SafeConvert.ToByte(input);
        }

        [TestCase((short)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((short)byte.MinValue, Result = byte.MinValue)]
        [TestCase(short.MaxValue, Result = (byte)0)]
        [TestCase(short.MinValue, Result = (byte)0)]
        [TestCase(byte.MaxValue + 1, Result = (byte)0)]
        [TestCase(byte.MinValue - 1, Result = (byte)0)]
        public byte ToByte_FromInt16(short input)
        {
            return SafeConvert.ToByte(input);
        }

        [TestCase((ushort)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((ushort)byte.MinValue, Result = byte.MinValue)]
        [TestCase(ushort.MaxValue, Result = (byte)0)]
        [TestCase((ushort)(byte.MaxValue + 1), Result = (byte)0)]
        public byte ToByte_FromUInt16(ushort input)
        {
            return SafeConvert.ToByte(input);
        }

        #endregion

        #region ToInt16

        [TestCase(byte.MaxValue, Result = (short)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (short)byte.MinValue)]
        public int ToInt16_FromByte(byte input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase(sbyte.MaxValue, Result = (short)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (short)sbyte.MinValue)]
        public int ToInt16_FromSByte(sbyte input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase(int.MaxValue, Result = (short)0)]
        [TestCase(int.MinValue, Result = (short)0)]
        [TestCase((int)short.MaxValue, Result = short.MaxValue)]
        [TestCase((int)short.MinValue, Result = short.MinValue)]
        public int ToInt16_FromInt32(int input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase(uint.MaxValue, Result = (short)0)]
        [TestCase(uint.MinValue, Result = (short)0)]
        [TestCase((uint)short.MaxValue, Result = short.MaxValue)]
        public int ToInt16_FromUInt32(uint input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase(long.MaxValue, Result = (short)0)]
        [TestCase(long.MinValue, Result = (short)0)]
        [TestCase((long)short.MaxValue, Result = short.MaxValue)]
        public int ToInt16_FromInt64(long input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase(ulong.MaxValue, Result = (short)0)]
        [TestCase(ulong.MinValue, Result = (short)0)]
        [TestCase((ulong)short.MaxValue, Result = short.MaxValue)]
        public int ToInt16_FromUInt64(ulong input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToShortData")]
        public int ToInt16_FromDecimal(decimal input)
        {
            return SafeConvert.ToInt16(input);
        }

        #endregion

        #region ToUInt16

        [TestCase(byte.MaxValue, Result = (ushort)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (ushort)byte.MinValue)]
        public ushort ToUInt16_FromByte(byte input)
        {
            return SafeConvert.ToUInt16(input);
        }

        [TestCase(sbyte.MaxValue, Result = (ushort)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (ushort)0)]
        public ushort ToUInt16_FromSByte(sbyte input)
        {
            return SafeConvert.ToUInt16(input);
        }

        [TestCase(short.MaxValue, Result = (ushort)short.MaxValue)]
        [TestCase(short.MinValue, Result = (ushort)0)]
        [TestCase((short)ushort.MinValue, Result = ushort.MinValue)]
        public ushort ToUInt16_FromInt16(short input)
        {
            return SafeConvert.ToUInt16(input);
        }

        [TestCase(ushort.MaxValue, Result = ushort.MaxValue)]
        [TestCase(ushort.MinValue, Result = ushort.MinValue)]
        public ushort ToUInt16_FromUInt16(ushort input)
        {
            return SafeConvert.ToUInt16(input);
        }

        [TestCase(int.MaxValue, Result = (ushort)0)]
        [TestCase(int.MinValue, Result = (ushort)0)]
        [TestCase((int)ushort.MaxValue, Result = ushort.MaxValue)]
        [TestCase((int)ushort.MinValue, Result = ushort.MinValue)]
        public ushort ToUInt16_FromInt32(int input)
        {
            return SafeConvert.ToUInt16(input);
        }

        [TestCase(uint.MaxValue, Result = (ushort)0)]
        [TestCase(uint.MinValue, Result = (ushort)0)]
        [TestCase((uint)ushort.MaxValue, Result = ushort.MaxValue)]
        public ushort ToUInt16_FromUInt32(uint input)
        {
            return SafeConvert.ToUInt16(input);
        }

        [TestCase(long.MaxValue, Result = (ushort)0)]
        [TestCase(long.MinValue, Result = (ushort)0)]
        [TestCase((long)ushort.MaxValue, Result = ushort.MaxValue)]
        public ushort ToUInt16_FromInt64(long input)
        {
            return SafeConvert.ToUInt16(input);
        }

        [TestCase(ulong.MaxValue, Result = (ushort)0)]
        [TestCase(ulong.MinValue, Result = (ushort)0)]
        [TestCase((ulong)ushort.MaxValue, Result = ushort.MaxValue)]
        public ushort ToUInt16_FromUInt64(ulong input)
        {
            return SafeConvert.ToUInt16(input);
        }

        #endregion

        #region ToInt32

        [TestCaseSource(typeof(TestData), "StringToIntData")]
        public int ToInt32_FromString(string input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCaseSource(typeof(TestData), "UIntToIntData")]
        public int ToInt32_FromUInt(uint input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCaseSource(typeof(TestData), "LongToIntData")]
        public int ToInt32_FromLong(long input)
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

        #endregion
    }
}
