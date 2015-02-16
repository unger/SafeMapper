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

        [TestCase(byte.MaxValue, Result = byte.MaxValue)]
        [TestCase(byte.MinValue, Result = byte.MinValue)]
        public byte ToByte_FromByte(byte input)
        {
            return SafeConvert.ToByte(input);
        }

        [TestCase(sbyte.MaxValue, Result = (byte)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (byte)0)]
        public byte ToByte_FromSByte(sbyte input)
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

        [TestCase((int)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((int)byte.MinValue, Result = byte.MinValue)]
        [TestCase(int.MaxValue, Result = (byte)0)]
        [TestCase(int.MinValue, Result = (byte)0)]
        [TestCase(byte.MaxValue + 1, Result = (byte)0)]
        [TestCase(byte.MinValue - 1, Result = (byte)0)]
        public byte ToByte_FromInt32(int input)
        {
            return SafeConvert.ToByte(input);
        }

        [TestCase((uint)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((uint)byte.MinValue, Result = byte.MinValue)]
        [TestCase(uint.MaxValue, Result = (byte)0)]
        [TestCase((uint)(byte.MaxValue + 1), Result = (byte)0)]
        public byte ToByte_FromUInt32(uint input)
        {
            return SafeConvert.ToByte(input);
        }

        [TestCase((long)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((long)byte.MinValue, Result = byte.MinValue)]
        [TestCase(long.MaxValue, Result = (byte)0)]
        [TestCase(long.MinValue, Result = (byte)0)]
        [TestCase(byte.MaxValue + 1, Result = (byte)0)]
        [TestCase(byte.MinValue - 1, Result = (byte)0)]
        public byte ToByte_FromInt64(long input)
        {
            return SafeConvert.ToByte(input);
        }

        [TestCase((ulong)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((ulong)byte.MinValue, Result = byte.MinValue)]
        [TestCase(ulong.MaxValue, Result = (byte)0)]
        [TestCase((ulong)(byte.MaxValue + 1), Result = (byte)0)]
        public byte ToByte_FromUInt64(ulong input)
        {
            return SafeConvert.ToByte(input);
        }

        [TestCase((float)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((float)byte.MinValue, Result = byte.MinValue)]
        [TestCase(float.MaxValue, Result = (byte)0)]
        [TestCase(float.MinValue, Result = (byte)0)]
        [TestCase(byte.MaxValue + 1, Result = (byte)0)]
        [TestCase(byte.MinValue - 1, Result = (byte)0)]
        [TestCase(byte.MaxValue - 1.5f, Result = byte.MaxValue - 2)]
        [TestCase(byte.MinValue + 1.5f, Result = byte.MinValue + 1)]
        public byte ToByte_FromSingle(float input)
        {
            return SafeConvert.ToByte(input);
        }

        [TestCase((double)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((double)byte.MinValue, Result = byte.MinValue)]
        [TestCase(double.MaxValue, Result = (byte)0)]
        [TestCase(double.MinValue, Result = (byte)0)]
        [TestCase(byte.MaxValue + 1, Result = (byte)0)]
        [TestCase(byte.MinValue - 1, Result = (byte)0)]
        [TestCase(byte.MaxValue - 1.5f, Result = byte.MaxValue - 2)]
        [TestCase(byte.MinValue + 1.5f, Result = byte.MinValue + 1)]
        public byte ToByte_FromDouble(double input)
        {
            return SafeConvert.ToByte(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public byte ToByte_FromBoolean(bool input)
        {
            return SafeConvert.ToByte(input);
        }

        [TestCase((char)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((char)byte.MinValue, Result = byte.MinValue)]
        [TestCase(char.MaxValue, Result = (byte)0)]
        [TestCase((char)(byte.MaxValue + 1), Result = (byte)0)]
        public byte ToByte_FromChar(char input)
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
