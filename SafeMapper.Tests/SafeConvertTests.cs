namespace SafeMapper.Tests
{
    using System;

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

        [TestCaseSource(typeof(TestData), "DecimalToByteData")]
        public byte ToByte_FromDecimal(decimal input)
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

        #region ToSByte

        [TestCase("127", Result = sbyte.MaxValue)]
        [TestCase("-128", Result = sbyte.MinValue)]
        [TestCase("128", Result = 0)]
        [TestCase("-129", Result = 0)]
        [TestCase(":", Result = 0)]
        [TestCase("/", Result = 0)]
        public sbyte ToSByte_FromString(string input)
        {
            return SafeConvert.ToSByte(input);
        }

        [TestCase(byte.MaxValue, Result = 0)]
        [TestCase(byte.MinValue, Result = 0)]
        [TestCase((byte)127, Result = sbyte.MaxValue)]
        public sbyte ToSByte_FromByte(byte input)
        {
            return SafeConvert.ToSByte(input);
        }

        [TestCase(sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = sbyte.MinValue)]
        public sbyte ToSByte_FromSByte(sbyte input)
        {
            return SafeConvert.ToSByte(input);
        }

        [TestCase((short)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase((short)sbyte.MinValue, Result = sbyte.MinValue)]
        [TestCase(short.MaxValue, Result = (sbyte)0)]
        [TestCase(short.MinValue, Result = (sbyte)0)]
        [TestCase(sbyte.MaxValue + 1, Result = (sbyte)0)]
        [TestCase(sbyte.MinValue - 1, Result = (sbyte)0)]
        public sbyte ToSByte_FromInt16(short input)
        {
            return SafeConvert.ToSByte(input);
        }

        [TestCase((ushort)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase(ushort.MaxValue, Result = (sbyte)0)]
        [TestCase((ushort)(sbyte.MaxValue + 1), Result = (sbyte)0)]
        public sbyte ToSByte_FromUInt16(ushort input)
        {
            return SafeConvert.ToSByte(input);
        }

        [TestCase((int)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase((int)sbyte.MinValue, Result = sbyte.MinValue)]
        [TestCase(int.MaxValue, Result = (sbyte)0)]
        [TestCase(int.MinValue, Result = (sbyte)0)]
        [TestCase(sbyte.MaxValue + 1, Result = (sbyte)0)]
        [TestCase(sbyte.MinValue - 1, Result = (sbyte)0)]
        public sbyte ToSByte_FromInt32(int input)
        {
            return SafeConvert.ToSByte(input);
        }

        [TestCase((uint)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase(uint.MaxValue, Result = (sbyte)0)]
        [TestCase((uint)(sbyte.MaxValue + 1), Result = (sbyte)0)]
        public sbyte ToSByte_FromUInt32(uint input)
        {
            return SafeConvert.ToSByte(input);
        }

        [TestCase((long)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase((long)sbyte.MinValue, Result = sbyte.MinValue)]
        [TestCase(long.MaxValue, Result = (sbyte)0)]
        [TestCase(long.MinValue, Result = (sbyte)0)]
        [TestCase(sbyte.MaxValue + 1, Result = (sbyte)0)]
        [TestCase(sbyte.MinValue - 1, Result = (sbyte)0)]
        public sbyte ToSByte_FromInt64(long input)
        {
            return SafeConvert.ToSByte(input);
        }

        [TestCase((ulong)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase(ulong.MaxValue, Result = (sbyte)0)]
        [TestCase((ulong)(sbyte.MaxValue + 1), Result = (sbyte)0)]
        public sbyte ToSByte_FromUInt64(ulong input)
        {
            return SafeConvert.ToSByte(input);
        }

        [TestCase((float)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase((float)sbyte.MinValue, Result = sbyte.MinValue)]
        [TestCase(float.MaxValue, Result = (sbyte)0)]
        [TestCase(float.MinValue, Result = (sbyte)0)]
        [TestCase(sbyte.MaxValue + 1, Result = (sbyte)0)]
        [TestCase(sbyte.MinValue - 1, Result = (sbyte)0)]
        [TestCase(sbyte.MaxValue - 1.5f, Result = sbyte.MaxValue - 2)]
        [TestCase(sbyte.MinValue + 1.5f, Result = sbyte.MinValue + 2)]
        public sbyte ToSByte_FromSingle(float input)
        {
            return SafeConvert.ToSByte(input);
        }

        [TestCase((double)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase((double)sbyte.MinValue, Result = sbyte.MinValue)]
        [TestCase(double.MaxValue, Result = (sbyte)0)]
        [TestCase(double.MinValue, Result = (sbyte)0)]
        [TestCase(sbyte.MaxValue + 1, Result = (sbyte)0)]
        [TestCase(sbyte.MinValue - 1, Result = (sbyte)0)]
        [TestCase(sbyte.MaxValue - 1.5f, Result = sbyte.MaxValue - 2)]
        [TestCase(sbyte.MinValue + 1.5f, Result = sbyte.MinValue + 2)]
        public sbyte ToSByte_FromDouble(double input)
        {
            return SafeConvert.ToSByte(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToSByteData")]
        public sbyte ToSByte_FromDecimal(decimal input)
        {
            return SafeConvert.ToSByte(input);
        }

        [TestCase(false, Result = (sbyte)0)]
        [TestCase(true, Result = (sbyte)1)]
        public sbyte ToSByte_FromBoolean(bool input)
        {
            return SafeConvert.ToSByte(input);
        }

        [TestCase((char)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase(char.MaxValue, Result = (sbyte)0)]
        [TestCase((char)(sbyte.MaxValue + 1), Result = (sbyte)0)]
        public sbyte ToSByte_FromChar(char input)
        {
            return SafeConvert.ToSByte(input);
        }

        #endregion

        #region ToInt16

        [TestCase("32767", Result = short.MaxValue)]
        [TestCase("-32768", Result = short.MinValue)]
        [TestCase("32768", Result = 0)]
        [TestCase("-32769", Result = 0)]
        [TestCase(":", Result = 0)]
        [TestCase("/", Result = 0)]
        public short ToInt16_FromString(string input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase(byte.MaxValue, Result = (short)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (short)byte.MinValue)]
        public short ToInt16_FromByte(byte input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase(sbyte.MaxValue, Result = (short)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (short)sbyte.MinValue)]
        public short ToInt16_FromSByte(sbyte input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase(short.MaxValue, Result = short.MaxValue)]
        [TestCase(short.MinValue, Result = short.MinValue)]
        public short ToInt16_FromInt16(short input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase(ushort.MaxValue, Result = (short)0)]
        [TestCase(ushort.MinValue, Result = (short)0)]
        [TestCase((ushort)short.MaxValue, Result = short.MaxValue)]
        public short ToInt16_FromUInt16(ushort input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase(int.MaxValue, Result = (short)0)]
        [TestCase(int.MinValue, Result = (short)0)]
        [TestCase((int)short.MaxValue, Result = short.MaxValue)]
        [TestCase((int)short.MinValue, Result = short.MinValue)]
        public short ToInt16_FromInt32(int input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase(uint.MaxValue, Result = (short)0)]
        [TestCase(uint.MinValue, Result = (short)0)]
        [TestCase((uint)short.MaxValue, Result = short.MaxValue)]
        public short ToInt16_FromUInt32(uint input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase(long.MaxValue, Result = (short)0)]
        [TestCase(long.MinValue, Result = (short)0)]
        [TestCase((long)short.MaxValue, Result = short.MaxValue)]
        public short ToInt16_FromInt64(long input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase(ulong.MaxValue, Result = (short)0)]
        [TestCase(ulong.MinValue, Result = (short)0)]
        [TestCase((ulong)short.MaxValue, Result = short.MaxValue)]
        public short ToInt16_FromUInt64(ulong input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase((float)short.MaxValue, Result = short.MaxValue)]
        [TestCase((float)short.MinValue, Result = short.MinValue)]
        [TestCase(float.MaxValue, Result = (short)0)]
        [TestCase(float.MinValue, Result = (short)0)]
        [TestCase(short.MaxValue + 1, Result = (short)0)]
        [TestCase(short.MinValue - 1, Result = (short)0)]
        [TestCase(short.MaxValue - 1.5f, Result = short.MaxValue - 2)]
        [TestCase(short.MinValue + 1.5f, Result = short.MinValue + 2)]
        public short ToInt16_FromSingle(float input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase((double)short.MaxValue, Result = short.MaxValue)]
        [TestCase((double)short.MinValue, Result = short.MinValue)]
        [TestCase(double.MaxValue, Result = (short)0)]
        [TestCase(double.MinValue, Result = (short)0)]
        [TestCase(short.MaxValue + 1, Result = (short)0)]
        [TestCase(short.MinValue - 1, Result = (short)0)]
        [TestCase(short.MaxValue - 1.5f, Result = short.MaxValue - 2)]
        [TestCase(short.MinValue + 1.5f, Result = short.MinValue + 2)]
        public short ToInt16_FromDouble(double input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToInt16Data")]
        public short ToInt16_FromDecimal(decimal input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase(false, Result = (short)0)]
        [TestCase(true, Result = (short)1)]
        public short ToInt16_FromBoolean(bool input)
        {
            return SafeConvert.ToInt16(input);
        }

        [TestCase((char)short.MaxValue, Result = short.MaxValue)]
        [TestCase(char.MaxValue, Result = (short)0)]
        [TestCase((char)(short.MaxValue + 1), Result = (short)0)]
        public short ToInt16_FromChar(char input)
        {
            return SafeConvert.ToInt16(input);
        }

        #endregion

        #region ToUInt16

        [TestCase("65535", Result = ushort.MaxValue)]
        [TestCase("0", Result = ushort.MinValue)]
        [TestCase("65536", Result = 0)]
        [TestCase("-1", Result = 0)]
        [TestCase(":", Result = 0)]
        [TestCase("/", Result = 0)]
        public ushort ToUInt16_FromString(string input)
        {
            return SafeConvert.ToUInt16(input);
        }

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

        [TestCase((float)ushort.MaxValue, Result = ushort.MaxValue)]
        [TestCase((float)ushort.MinValue, Result = ushort.MinValue)]
        [TestCase(float.MaxValue, Result = (ushort)0)]
        [TestCase(float.MinValue, Result = (ushort)0)]
        [TestCase(ushort.MaxValue + 1, Result = (ushort)0)]
        [TestCase(ushort.MinValue - 1, Result = (ushort)0)]
        [TestCase(ushort.MaxValue - 1.5f, Result = ushort.MaxValue - 2)]
        [TestCase(ushort.MinValue + 1.5f, Result = ushort.MinValue + 1)]
        public ushort ToUInt16_FromSingle(float input)
        {
            return SafeConvert.ToUInt16(input);
        }

        [TestCase((double)ushort.MaxValue, Result = ushort.MaxValue)]
        [TestCase((double)ushort.MinValue, Result = ushort.MinValue)]
        [TestCase(double.MaxValue, Result = (ushort)0)]
        [TestCase(double.MinValue, Result = (ushort)0)]
        [TestCase(ushort.MaxValue + 1, Result = (ushort)0)]
        [TestCase(ushort.MinValue - 1, Result = (ushort)0)]
        [TestCase(ushort.MaxValue - 1.5d, Result = ushort.MaxValue - 2)]
        [TestCase(ushort.MinValue + 1.5d, Result = ushort.MinValue + 1)]
        public ushort ToUInt16_FromDouble(double input)
        {
            return SafeConvert.ToUInt16(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToUInt16Data")]
        public ushort ToUInt16_FromDecimal(decimal input)
        {
            return SafeConvert.ToUInt16(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public ushort ToUInt16_FromBoolean(bool input)
        {
            return SafeConvert.ToUInt16(input);
        }

        [TestCase((char)ushort.MaxValue, Result = ushort.MaxValue)]
        [TestCase((char)ushort.MinValue, Result = ushort.MinValue)]
        [TestCase(char.MaxValue, Result = (ushort)0)]
        public ushort ToUInt16_FromChar(char input)
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
        public int ToInt32_FromInt64(long input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToInt32Data")]
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

        #region ToUInt32



        #endregion

        #region ToString

        [TestCase(null, Result = null)]
        [TestCase(new char[0], Result = "")]
        [TestCase(new[] { 'F', 'o', 'o' }, Result = "Foo")]
        public string ToString_FromCharArray(char[] input)
        {
            return SafeConvert.ToString(input);
        }

        #endregion

        #region Other

        [TestCase(null, Result = null)]
        [TestCase("", Result = new char[0])]
        [TestCase("Foo", Result = new[] { 'F', 'o', 'o' })]
        public char[] ToCharArray_FromString(string input)
        {
            return SafeConvert.ToCharArray(input);
        }

        #endregion
    }
}
