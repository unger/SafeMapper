namespace SafeMapper.Tests
{
    using System;
    using System.Globalization;

    using NUnit.Framework;

    [TestFixture]
    public class SafeConvertTests
    {
        private IFormatProvider numberFormatProvider;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            var numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();

            numberFormat.NumberDecimalSeparator = ".";
            numberFormat.CurrencyDecimalSeparator = ".";
            numberFormat.NumberGroupSeparator = " ";
            numberFormat.CurrencyGroupSeparator = " ";
            this.numberFormatProvider = numberFormat;
        }

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
        [TestCase(byte.MaxValue - 1.5d, Result = byte.MaxValue - 2)]
        [TestCase(byte.MinValue + 1.5d, Result = byte.MinValue + 1)]
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
        [TestCase(sbyte.MaxValue - 1.5d, Result = sbyte.MaxValue - 2)]
        [TestCase(sbyte.MinValue + 1.5d, Result = sbyte.MinValue + 2)]
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
        [TestCase(short.MaxValue - 1.5d, Result = short.MaxValue - 2)]
        [TestCase(short.MinValue + 1.5d, Result = short.MinValue + 2)]
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

        [TestCase((char)ushort.MinValue, Result = ushort.MinValue)]
        [TestCase(char.MaxValue, Result = ushort.MaxValue)]
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

        [TestCase(byte.MaxValue, Result = (short)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (short)byte.MinValue)]
        public int ToInt32_FromByte(byte input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCase(sbyte.MaxValue, Result = (int)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (int)sbyte.MinValue)]
        public int ToInt32_FromSByte(sbyte input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCase(short.MaxValue, Result = (int)short.MaxValue)]
        [TestCase(short.MinValue, Result = (int)short.MinValue)]
        public int ToInt32_FromInt16(short input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCase(ushort.MaxValue, Result = (int)ushort.MaxValue)]
        [TestCase(ushort.MinValue, Result = 0)]
        public int ToInt32_FromUInt16(ushort input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCase(int.MaxValue, Result = int.MaxValue)]
        [TestCase(int.MinValue, Result = int.MinValue)]
        public int ToInt32_FromInt32(int input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCase(uint.MaxValue, Result = 0)]
        [TestCase(uint.MinValue, Result = 0)]
        [TestCase((uint)int.MaxValue, Result = int.MaxValue)]
        public int ToInt32_FromUInt32(uint input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCaseSource(typeof(TestData), "LongToIntData")]
        public int ToInt32_FromInt64(long input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCase(ulong.MaxValue, Result = 0)]
        [TestCase(ulong.MinValue, Result = 0)]
        [TestCase((ulong)int.MaxValue, Result = int.MaxValue)]
        public int ToInt32_FromUInt64(ulong input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCase(float.MaxValue, Result = 0)]
        [TestCase(float.MinValue, Result = 0)]
        [TestCase((float)int.MinValue, Result = int.MinValue)]
        [TestCase(int.MaxValue - 7483647f, Result = int.MaxValue - 7483647)]
        public int ToInt32_FromSingle(float input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCase((double)int.MaxValue, Result = int.MaxValue)]
        [TestCase((double)int.MinValue, Result = int.MinValue)]
        [TestCase(double.MaxValue, Result = 0)]
        [TestCase(double.MinValue, Result = 0)]
        [TestCase(int.MaxValue - 1.5d, Result = int.MaxValue - 2)]
        [TestCase(int.MinValue + 1.5d, Result = int.MinValue + 2)]
        public int ToInt32_FromDouble(double input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToInt32Data")]
        public int ToInt32_FromDecimal(decimal input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public int ToInt32_FromBoolean(bool input)
        {
            return SafeConvert.ToInt32(input);
        }

        [TestCase(char.MaxValue, Result = (int)char.MaxValue)]
        [TestCase(char.MinValue, Result = 0)]
        public int ToInt32_FromChar(char input)
        {
            return SafeConvert.ToInt32(input);
        }

        #endregion

        #region ToUInt32

        [TestCase("4294967295", Result = uint.MaxValue)]
        [TestCase("0", Result = uint.MinValue)]
        [TestCase("4294967296", Result = 0)]
        [TestCase("-1", Result = 0)]
        [TestCase(":", Result = 0)]
        [TestCase("/", Result = 0)]
        public uint ToUInt32_FromString(string input)
        {
            return SafeConvert.ToUInt32(input);
        }

        [TestCase(byte.MaxValue, Result = (uint)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (uint)byte.MinValue)]
        public uint ToUInt32_FromByte(byte input)
        {
            return SafeConvert.ToUInt32(input);
        }

        [TestCase(sbyte.MaxValue, Result = (uint)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (uint)0)]
        public uint ToUInt32_FromSByte(sbyte input)
        {
            return SafeConvert.ToUInt32(input);
        }

        [TestCase(short.MaxValue, Result = (uint)short.MaxValue)]
        [TestCase(short.MinValue, Result = (uint)0)]
        [TestCase((short)uint.MinValue, Result = uint.MinValue)]
        public uint ToUInt32_FromInt16(short input)
        {
            return SafeConvert.ToUInt32(input);
        }

        [TestCase(ushort.MaxValue, Result = (uint)ushort.MaxValue)]
        [TestCase(ushort.MinValue, Result = (uint)ushort.MinValue)]
        public uint ToUInt32_FromUInt16(ushort input)
        {
            return SafeConvert.ToUInt32(input);
        }

        [TestCase(int.MaxValue, Result = (uint)int.MaxValue)]
        [TestCase(int.MinValue, Result = (uint)0)]
        [TestCase((int)uint.MinValue, Result = uint.MinValue)]
        public uint ToUInt32_FromInt32(int input)
        {
            return SafeConvert.ToUInt32(input);
        }

        [TestCase(uint.MaxValue, Result = uint.MaxValue)]
        [TestCase(uint.MinValue, Result = uint.MinValue)]
        public uint ToUInt32_FromUInt32(uint input)
        {
            return SafeConvert.ToUInt32(input);
        }

        [TestCase(long.MaxValue, Result = (uint)0)]
        [TestCase(long.MinValue, Result = (uint)0)]
        [TestCase((long)uint.MaxValue, Result = uint.MaxValue)]
        public uint ToUInt32_FromInt64(long input)
        {
            return SafeConvert.ToUInt32(input);
        }

        [TestCase(ulong.MaxValue, Result = (uint)0)]
        [TestCase(ulong.MinValue, Result = (uint)0)]
        [TestCase((ulong)uint.MaxValue, Result = uint.MaxValue)]
        public uint ToUInt32_FromUInt64(ulong input)
        {
            return SafeConvert.ToUInt32(input);
        }

        [TestCase((float)uint.MinValue, Result = uint.MinValue)]
        [TestCase(float.MaxValue, Result = (uint)0)]
        [TestCase(float.MinValue, Result = (uint)0)]
        [TestCase(uint.MinValue - 1f, Result = (uint)0)]
        [TestCase(uint.MinValue + 1.5f, Result = uint.MinValue + 1)]
        public uint ToUInt32_FromSingle(float input)
        {
            return SafeConvert.ToUInt32(input);
        }

        [TestCase((double)uint.MaxValue, Result = uint.MaxValue)]
        [TestCase((double)uint.MinValue, Result = uint.MinValue)]
        [TestCase(double.MaxValue, Result = (uint)0)]
        [TestCase(double.MinValue, Result = (uint)0)]
        [TestCase(uint.MaxValue + 1d, Result = (uint)0)]
        [TestCase(uint.MinValue - 1d, Result = (uint)0)]
        [TestCase(uint.MaxValue - 1.5d, Result = uint.MaxValue - 2)]
        [TestCase(uint.MinValue + 1.5d, Result = uint.MinValue + 1)]
        public uint ToUInt32_FromDouble(double input)
        {
            return SafeConvert.ToUInt32(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToUInt32Data")]
        public uint ToUInt32_FromDecimal(decimal input)
        {
            return SafeConvert.ToUInt32(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public uint ToUInt32_FromBoolean(bool input)
        {
            return SafeConvert.ToUInt32(input);
        }

        [TestCase((char)uint.MinValue, Result = uint.MinValue)]
        [TestCase(char.MaxValue, Result = (uint)char.MaxValue)]
        public uint ToUInt32_FromChar(char input)
        {
            return SafeConvert.ToUInt32(input);
        }

        #endregion

        #region ToInt64

        [TestCase(null, Result = 0)]
        [TestCase("", Result = 0)]
        [TestCase("9223372036854775807", Result = long.MaxValue)]
        [TestCase("-9223372036854775808", Result = long.MinValue)]
        //[TestCase("9223372036854775808", Result = 0)]
        //[TestCase("-9223372036854775809", Result = 0)]
        [TestCase(":", Result = 0)]
        [TestCase("/", Result = 0)]
        public long ToInt64_FromString(string input)
        {
            return SafeConvert.ToInt64(input);
        }

        [TestCase(byte.MaxValue, Result = (long)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (long)byte.MinValue)]
        public long ToInt64_FromByte(byte input)
        {
            return SafeConvert.ToInt64(input);
        }

        [TestCase(sbyte.MaxValue, Result = (long)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (long)sbyte.MinValue)]
        public long ToInt64_FromSByte(sbyte input)
        {
            return SafeConvert.ToInt64(input);
        }

        [TestCase(short.MaxValue, Result = (long)short.MaxValue)]
        [TestCase(short.MinValue, Result = (long)short.MinValue)]
        public long ToInt64_FromInt16(short input)
        {
            return SafeConvert.ToInt64(input);
        }

        [TestCase(ushort.MaxValue, Result = (long)ushort.MaxValue)]
        [TestCase(ushort.MinValue, Result = 0)]
        public long ToInt64_FromUInt16(ushort input)
        {
            return SafeConvert.ToInt64(input);
        }

        [TestCase(int.MaxValue, Result = (long)int.MaxValue)]
        [TestCase(int.MinValue, Result = (long)int.MinValue)]
        public long ToInt64_FromInt32(int input)
        {
            return SafeConvert.ToInt64(input);
        }

        [TestCase(uint.MaxValue, Result = (long)uint.MaxValue)]
        [TestCase(uint.MinValue, Result = (long)uint.MinValue)]
        [TestCase((uint)int.MaxValue, Result = int.MaxValue)]
        public long ToInt64_FromUInt32(uint input)
        {
            return SafeConvert.ToInt64(input);
        }

        [TestCase(long.MaxValue, Result = long.MaxValue)]
        [TestCase(long.MinValue, Result = long.MinValue)]
        public long ToInt64_FromInt64(long input)
        {
            return SafeConvert.ToInt64(input);
        }

        [TestCase(ulong.MaxValue, Result = 0)]
        [TestCase(ulong.MinValue, Result = 0)]
        [TestCase((ulong)long.MaxValue, Result = long.MaxValue)]
        public long ToInt64_FromUInt64(ulong input)
        {
            return SafeConvert.ToInt64(input);
        }

        [TestCase(float.MaxValue, Result = 0)]
        [TestCase(float.MinValue, Result = 0)]
        [TestCase((float)int.MinValue, Result = int.MinValue)]
        public long ToInt64_FromSingle(float input)
        {
            return SafeConvert.ToInt64(input);
        }

        [TestCase((double)int.MaxValue, Result = int.MaxValue)]
        [TestCase((double)int.MinValue, Result = int.MinValue)]
        [TestCase(double.MaxValue, Result = 0)]
        [TestCase(double.MinValue, Result = 0)]
        [TestCase(int.MaxValue - 1.5d, Result = int.MaxValue - 2)]
        [TestCase(int.MinValue + 1.5d, Result = int.MinValue + 2)]
        public long ToInt64_FromDouble(double input)
        {
            return SafeConvert.ToInt64(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToInt64Data")]
        public long ToInt64_FromDecimal(decimal input)
        {
            return SafeConvert.ToInt64(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public long ToInt64_FromBoolean(bool input)
        {
            return SafeConvert.ToInt64(input);
        }

        [TestCase(char.MaxValue, Result = (long)char.MaxValue)]
        [TestCase(char.MinValue, Result = 0)]
        public long ToInt64_FromChar(char input)
        {
            return SafeConvert.ToInt64(input);
        }

        #endregion

        #region ToUInt64

        [TestCase("18446744073709551615", Result = ulong.MaxValue)]
        [TestCase("0", Result = ulong.MinValue)]
        [TestCase("18446744073709551616", Result = 0)]
        //[TestCase("18446744073709551617", Result = 0)]
        [TestCase("-1", Result = 0)]
        [TestCase(":", Result = 0)]
        [TestCase("/", Result = 0)]
        public ulong ToUInt64_FromString(string input)
        {
            return SafeConvert.ToUInt64(input);
        }

        [TestCase(byte.MaxValue, Result = (ulong)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (ulong)byte.MinValue)]
        public ulong ToUInt64_FromByte(byte input)
        {
            return SafeConvert.ToUInt64(input);
        }

        [TestCase(sbyte.MaxValue, Result = (ulong)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (ulong)0)]
        public ulong ToUInt64_FromSByte(sbyte input)
        {
            return SafeConvert.ToUInt64(input);
        }

        [TestCase(short.MaxValue, Result = (ulong)short.MaxValue)]
        [TestCase(short.MinValue, Result = (ulong)0)]
        [TestCase((short)uint.MinValue, Result = ulong.MinValue)]
        public ulong ToUInt64_FromInt16(short input)
        {
            return SafeConvert.ToUInt64(input);
        }

        [TestCase(ushort.MaxValue, Result = (ulong)ushort.MaxValue)]
        [TestCase(ushort.MinValue, Result = (ulong)ushort.MinValue)]
        public ulong ToUInt64_FromUInt16(ushort input)
        {
            return SafeConvert.ToUInt64(input);
        }

        [TestCase(int.MaxValue, Result = (ulong)int.MaxValue)]
        [TestCase(int.MinValue, Result = (ulong)0)]
        [TestCase((int)uint.MinValue, Result = ulong.MinValue)]
        public ulong ToUInt64_FromInt32(int input)
        {
            return SafeConvert.ToUInt64(input);
        }

        [TestCase(uint.MaxValue, Result = (ulong)uint.MaxValue)]
        [TestCase(uint.MinValue, Result = (ulong)uint.MinValue)]
        public ulong ToUInt64_FromUInt32(uint input)
        {
            return SafeConvert.ToUInt64(input);
        }

        [TestCase(long.MaxValue, Result = (ulong)long.MaxValue)]
        [TestCase(long.MinValue, Result = (ulong)0)]
        public ulong ToUInt64_FromInt64(long input)
        {
            return SafeConvert.ToUInt64(input);
        }

        [TestCase(ulong.MaxValue, Result = ulong.MaxValue)]
        [TestCase(ulong.MinValue, Result = ulong.MinValue)]
        public ulong ToUInt64_FromUInt64(ulong input)
        {
            return SafeConvert.ToUInt64(input);
        }

        [TestCase((float)ulong.MinValue, Result = ulong.MinValue)]
        [TestCase(float.MaxValue, Result = (ulong)0)]
        [TestCase(float.MinValue, Result = (ulong)0)]
        [TestCase(ulong.MinValue - 1f, Result = (ulong)0)]
        [TestCase(ulong.MinValue + 1.5f, Result = ulong.MinValue + 1)]
        public ulong ToUInt64_FromSingle(float input)
        {
            return SafeConvert.ToUInt64(input);
        }

        //[TestCase((double)ulong.MaxValue, Result = ulong.MaxValue)]
        [TestCase((double)ulong.MinValue, Result = ulong.MinValue)]
        [TestCase(double.MaxValue, Result = (ulong)0)]
        [TestCase(double.MinValue, Result = (ulong)0)]
        [TestCase(ulong.MaxValue + 1d, Result = (ulong)0)]
        [TestCase(ulong.MinValue - 1d, Result = (ulong)0)]
        //[TestCase(ulong.MaxValue - 1.5d, Result = ulong.MaxValue - 2)]
        [TestCase(ulong.MinValue + 1.5d, Result = ulong.MinValue + 1)]
        public ulong ToUInt64_FromDouble(double input)
        {
            return SafeConvert.ToUInt64(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToUInt64Data")]
        public ulong ToUInt64_FromDecimal(decimal input)
        {
            return SafeConvert.ToUInt64(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public ulong ToUInt64_FromBoolean(bool input)
        {
            return SafeConvert.ToUInt64(input);
        }

        [TestCase((char)ulong.MinValue, Result = ulong.MinValue)]
        [TestCase(char.MaxValue, Result = (ulong)char.MaxValue)]
        public ulong ToUInt64_FromChar(char input)
        {
            return SafeConvert.ToUInt64(input);
        }

        #endregion

        #region ToSingle

        [TestCase(null, Result = 0f)]
        [TestCase("", Result = 0f)]
        [TestCase("1", Result = 1f)]
        public float ToSingle_FromString(string input)
        {
            return SafeConvert.ToSingle(input);
        }

        [TestCase(null, Result = 0)]
        [TestCase("", Result = 0)]
        [TestCase("1.7976931348623157", Result = 1.7976931348623157f)]
        [TestCase("-1.7976931348623157", Result = -1.7976931348623157f)]
        [TestCase(":", Result = 0)]
        [TestCase("/", Result = 0)]
        public float ToSingle_FromStringWithFormat(string input)
        {
            return SafeConvert.ToSingle(input, this.numberFormatProvider);
        }

        [TestCase(byte.MaxValue, Result = (float)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (float)byte.MinValue)]
        public float ToSingle_FromByte(byte input)
        {
            return SafeConvert.ToSingle(input);
        }

        [TestCase(sbyte.MaxValue, Result = (float)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (float)sbyte.MinValue)]
        public float ToSingle_FromSByte(sbyte input)
        {
            return SafeConvert.ToSingle(input);
        }

        [TestCase(short.MaxValue, Result = (float)short.MaxValue)]
        [TestCase(short.MinValue, Result = (float)short.MinValue)]
        public float ToSingle_FromInt16(short input)
        {
            return SafeConvert.ToSingle(input);
        }

        [TestCase(ushort.MaxValue, Result = (float)ushort.MaxValue)]
        [TestCase(ushort.MinValue, Result = 0)]
        public float ToSingle_FromUInt16(ushort input)
        {
            return SafeConvert.ToSingle(input);
        }

        [TestCase(int.MaxValue, Result = (float)int.MaxValue)]
        [TestCase(int.MinValue, Result = (float)int.MinValue)]
        public float ToSingle_FromInt32(int input)
        {
            return SafeConvert.ToSingle(input);
        }

        [TestCase(uint.MaxValue, Result = (float)uint.MaxValue)]
        [TestCase(uint.MinValue, Result = (float)uint.MinValue)]
        public float ToSingle_FromUInt32(uint input)
        {
            return SafeConvert.ToSingle(input);
        }

        [TestCase(long.MaxValue, Result = (float)long.MaxValue)]
        [TestCase(long.MinValue, Result = (float)long.MinValue)]
        public float ToSingle_FromInt64(long input)
        {
            return SafeConvert.ToSingle(input);
        }

        [TestCase(ulong.MaxValue, Result = (float)ulong.MaxValue)]
        [TestCase(ulong.MinValue, Result = 0d)]
        public float ToSingle_FromUInt64(ulong input)
        {
            return SafeConvert.ToSingle(input);
        }

        [TestCase(float.MaxValue, Result = float.MaxValue)]
        [TestCase(float.MinValue, Result = float.MinValue)]
        public float ToSingle_FromSingle(float input)
        {
            return SafeConvert.ToSingle(input);
        }

        [TestCase((double)float.MaxValue, Result = float.MaxValue)]
        [TestCase((double)float.MinValue, Result = float.MinValue)]
        //[TestCase(((double)float.MaxValue) + 1d, Result = 0f)]
        //[TestCase(((double)float.MinValue) - 1d, Result = 0f)]
        [TestCase(double.MaxValue, Result = 0f)]
        [TestCase(double.MinValue, Result = 0f)]
        public float ToSingle_FromDouble(double input)
        {
            return SafeConvert.ToSingle(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToSingleData")]
        public float ToSingle_FromDecimal(decimal input)
        {
            return SafeConvert.ToSingle(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public float ToSingle_FromBoolean(bool input)
        {
            return SafeConvert.ToSingle(input);
        }

        [TestCase(char.MaxValue, Result = (float)char.MaxValue)]
        [TestCase(char.MinValue, Result = 0)]
        public float ToSingle_FromChar(char input)
        {
            return SafeConvert.ToSingle(input);
        }

        #endregion

        #region ToDouble

        [TestCase(null, Result = 0d)]
        [TestCase("", Result = 0d)]
        [TestCase("1", Result = 1d)]
        public double ToDouble_FromString(string input)
        {
            return SafeConvert.ToDouble(input);
        }

        [TestCase(null, Result = 0)]
        [TestCase("", Result = 0)]
        [TestCase("1.7976931348623157", Result = 1.7976931348623157)]
        [TestCase("-1.7976931348623157", Result = -1.7976931348623157)]
        [TestCase(":", Result = 0)]
        [TestCase("/", Result = 0)]
        public double ToDouble_FromStringWithFormat(string input)
        {
            return SafeConvert.ToDouble(input, CultureInfo.InvariantCulture);
        }

        [TestCase(byte.MaxValue, Result = (double)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (double)byte.MinValue)]
        public double ToDouble_FromByte(byte input)
        {
            return SafeConvert.ToDouble(input);
        }

        [TestCase(sbyte.MaxValue, Result = (double)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (double)sbyte.MinValue)]
        public double ToDouble_FromSByte(sbyte input)
        {
            return SafeConvert.ToDouble(input);
        }

        [TestCase(short.MaxValue, Result = (double)short.MaxValue)]
        [TestCase(short.MinValue, Result = (double)short.MinValue)]
        public double ToDouble_FromInt16(short input)
        {
            return SafeConvert.ToDouble(input);
        }

        [TestCase(ushort.MaxValue, Result = (double)ushort.MaxValue)]
        [TestCase(ushort.MinValue, Result = 0)]
        public double ToDouble_FromUInt16(ushort input)
        {
            return SafeConvert.ToDouble(input);
        }

        [TestCase(int.MaxValue, Result = (double)int.MaxValue)]
        [TestCase(int.MinValue, Result = (double)int.MinValue)]
        public double ToDouble_FromInt32(int input)
        {
            return SafeConvert.ToDouble(input);
        }

        [TestCase(uint.MaxValue, Result = (double)uint.MaxValue)]
        [TestCase(uint.MinValue, Result = (double)uint.MinValue)]
        public double ToDouble_FromUInt32(uint input)
        {
            return SafeConvert.ToDouble(input);
        }

        [TestCase(long.MaxValue, Result = (double)long.MaxValue)]
        [TestCase(long.MinValue, Result = (double)long.MinValue)]
        public double ToDouble_FromInt64(long input)
        {
            return SafeConvert.ToDouble(input);
        }

        [TestCase(ulong.MaxValue, Result = (double)ulong.MaxValue)]
        [TestCase(ulong.MinValue, Result = 0d)]
        public double ToDouble_FromUInt64(ulong input)
        {
            return SafeConvert.ToDouble(input);
        }

        [TestCase(float.MaxValue, Result = (double)float.MaxValue)]
        [TestCase(float.MinValue, Result = (double)float.MinValue)]
        public double ToDouble_FromSingle(float input)
        {
            return SafeConvert.ToDouble(input);
        }

        [TestCase(double.MaxValue, Result = double.MaxValue)]
        [TestCase(double.MinValue, Result = double.MinValue)]
        public double ToDouble_FromDouble(double input)
        {
            return SafeConvert.ToDouble(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToDoubleData")]
        public double ToDouble_FromDecimal(decimal input)
        {
            return SafeConvert.ToDouble(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public double ToDouble_FromBoolean(bool input)
        {
            return SafeConvert.ToDouble(input);
        }

        [TestCase(char.MaxValue, Result = (double)char.MaxValue)]
        [TestCase(char.MinValue, Result = 0)]
        public double ToDouble_FromChar(char input)
        {
            return SafeConvert.ToDouble(input);
        }

        #endregion

        #region ToDecimal

        [TestCaseSource(typeof(TestData), "StringToDecimalData")]
        public decimal ToDecimal_FromString(string input)
        {
            return SafeConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "StringToDecimalWithFormatData")]
        public decimal ToDecimal_FromStringWithFormat(string input)
        {
            return SafeConvert.ToDecimal(input, this.numberFormatProvider);
        }

        [TestCaseSource(typeof(TestData), "ByteToDecimalData")]
        public decimal ToDecimal_FromByte(byte input)
        {
            return SafeConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "SByteToDecimalData")]
        public decimal ToDecimal_FromSByte(sbyte input)
        {
            return SafeConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "Int16ToDecimalData")]
        public decimal ToDecimal_FromInt16(short input)
        {
            return SafeConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "UInt16ToDecimalData")]
        public decimal ToDecimal_FromUInt16(ushort input)
        {
            return SafeConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "Int32ToDecimalData")]
        public decimal ToDecimal_FromInt32(int input)
        {
            return SafeConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "UInt32ToDecimalData")]
        public decimal ToDecimal_FromUInt32(uint input)
        {
            return SafeConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "Int64ToDecimalData")]
        public decimal ToDecimal_FromInt64(long input)
        {
            return SafeConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "UInt64ToDecimalData")]
        public decimal ToDecimal_FromUInt64(ulong input)
        {
            return SafeConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "SingleToDecimalData")]
        public decimal ToDecimal_FromSingle(float input)
        {
            return SafeConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "DoubleToDecimalData")]
        public decimal ToDecimal_FromDouble(double input)
        {
            return SafeConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToDecimalData")]
        public decimal ToDecimal_FromDecimal(decimal input)
        {
            return SafeConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "BooleanToDecimalData")]
        public decimal ToDecimal_FromBoolean(bool input)
        {
            return SafeConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "CharToDecimalData")]
        public decimal ToDecimal_FromChar(char input)
        {
            return SafeConvert.ToDecimal(input);
        }

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
