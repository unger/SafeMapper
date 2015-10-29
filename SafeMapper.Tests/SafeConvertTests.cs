//namespace SafeMapper.Tests
//{
//    using System;
//    using System.Data.SqlTypes;
//    using System.Globalization;

//    using Xunit;

    
//    public class SafeConvertTests
//    {
//        private IFormatProvider numberFormatProvider;

//        public SafeConvertTests()
//        {
//            var numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();

//            numberFormat.NumberDecimalSeparator = ".";
//            numberFormat.CurrencyDecimalSeparator = ".";
//            numberFormat.NumberGroupSeparator = " ";
//            numberFormat.CurrencyGroupSeparator = " ";
//            this.numberFormatProvider = numberFormat;
//        }

//        #region ToByte

//        [TestCase("255", ExpectedResult = byte.MaxValue)]
//        [TestCase("0", ExpectedResult = byte.MinValue)]
//        [TestCase("256", ExpectedResult = 0)]
//        [TestCase("-1", ExpectedResult = 0)]
//        [TestCase(":", ExpectedResult = 0)]
//        [TestCase("/", ExpectedResult = 0)]
//        public byte ToByte_FromString(string input)
//        {
//            return SafeConvert.ToByte(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = byte.MinValue)]
//        public byte ToByte_FromByte(byte input)
//        {
//            return SafeConvert.ToByte(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (byte)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = (byte)0)]
//        public byte ToByte_FromSByte(sbyte input)
//        {
//            return SafeConvert.ToByte(input);
//        }

//        [TestCase((short)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((short)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(short.MaxValue, ExpectedResult = (byte)0)]
//        [TestCase(short.MinValue, ExpectedResult = (byte)0)]
//        [TestCase(byte.MaxValue + 1, ExpectedResult = (byte)0)]
//        [TestCase(byte.MinValue - 1, ExpectedResult = (byte)0)]
//        public byte ToByte_FromInt16(short input)
//        {
//            return SafeConvert.ToByte(input);
//        }

//        [TestCase((ushort)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((ushort)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(ushort.MaxValue, ExpectedResult = (byte)0)]
//        [TestCase((ushort)(byte.MaxValue + 1), ExpectedResult = (byte)0)]
//        public byte ToByte_FromUInt16(ushort input)
//        {
//            return SafeConvert.ToByte(input);
//        }

//        [TestCase((int)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((int)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(int.MaxValue, ExpectedResult = (byte)0)]
//        [TestCase(int.MinValue, ExpectedResult = (byte)0)]
//        [TestCase(byte.MaxValue + 1, ExpectedResult = (byte)0)]
//        [TestCase(byte.MinValue - 1, ExpectedResult = (byte)0)]
//        public byte ToByte_FromInt32(int input)
//        {
//            return SafeConvert.ToByte(input);
//        }

//        [TestCase((uint)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((uint)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(uint.MaxValue, ExpectedResult = (byte)0)]
//        [TestCase((uint)(byte.MaxValue + 1), ExpectedResult = (byte)0)]
//        public byte ToByte_FromUInt32(uint input)
//        {
//            return SafeConvert.ToByte(input);
//        }

//        [TestCase((long)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((long)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(long.MaxValue, ExpectedResult = (byte)0)]
//        [TestCase(long.MinValue, ExpectedResult = (byte)0)]
//        [TestCase(byte.MaxValue + 1, ExpectedResult = (byte)0)]
//        [TestCase(byte.MinValue - 1, ExpectedResult = (byte)0)]
//        public byte ToByte_FromInt64(long input)
//        {
//            return SafeConvert.ToByte(input);
//        }

//        [TestCase((ulong)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((ulong)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(ulong.MaxValue, ExpectedResult = (byte)0)]
//        [TestCase((ulong)(byte.MaxValue + 1), ExpectedResult = (byte)0)]
//        public byte ToByte_FromUInt64(ulong input)
//        {
//            return SafeConvert.ToByte(input);
//        }

//        [TestCase((float)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((float)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(float.MaxValue, ExpectedResult = (byte)0)]
//        [TestCase(float.MinValue, ExpectedResult = (byte)0)]
//        [TestCase(byte.MaxValue + 1, ExpectedResult = (byte)0)]
//        [TestCase(byte.MinValue - 1, ExpectedResult = (byte)0)]
//        [TestCase(byte.MaxValue - 1.5f, ExpectedResult = byte.MaxValue - 2)]
//        [TestCase(byte.MinValue + 1.5f, ExpectedResult = byte.MinValue + 1)]
//        public byte ToByte_FromSingle(float input)
//        {
//            return SafeConvert.ToByte(input);
//        }

//        [TestCase((double)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((double)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = (byte)0)]
//        [TestCase(double.MinValue, ExpectedResult = (byte)0)]
//        [TestCase(byte.MaxValue + 1, ExpectedResult = (byte)0)]
//        [TestCase(byte.MinValue - 1, ExpectedResult = (byte)0)]
//        [TestCase(byte.MaxValue - 1.5d, ExpectedResult = byte.MaxValue - 2)]
//        [TestCase(byte.MinValue + 1.5d, ExpectedResult = byte.MinValue + 1)]
//        public byte ToByte_FromDouble(double input)
//        {
//            return SafeConvert.ToByte(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToByteData")]
//        public byte ToByte_FromDecimal(decimal input)
//        {
//            return SafeConvert.ToByte(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public byte ToByte_FromBoolean(bool input)
//        {
//            return SafeConvert.ToByte(input);
//        }

//        [TestCase((char)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((char)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(char.MaxValue, ExpectedResult = (byte)0)]
//        [TestCase((char)(byte.MaxValue + 1), ExpectedResult = (byte)0)]
//        public byte ToByte_FromChar(char input)
//        {
//            return SafeConvert.ToByte(input);
//        }

//        #endregion

//        #region ToSByte

//        [TestCase("127", ExpectedResult = sbyte.MaxValue)]
//        [TestCase("-128", ExpectedResult = sbyte.MinValue)]
//        [TestCase("128", ExpectedResult = 0)]
//        [TestCase("-129", ExpectedResult = 0)]
//        [TestCase(":", ExpectedResult = 0)]
//        [TestCase("/", ExpectedResult = 0)]
//        public sbyte ToSByte_FromString(string input)
//        {
//            return SafeConvert.ToSByte(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = 0)]
//        [TestCase(byte.MinValue, ExpectedResult = 0)]
//        [TestCase((byte)127, ExpectedResult = sbyte.MaxValue)]
//        public sbyte ToSByte_FromByte(byte input)
//        {
//            return SafeConvert.ToSByte(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = sbyte.MinValue)]
//        public sbyte ToSByte_FromSByte(sbyte input)
//        {
//            return SafeConvert.ToSByte(input);
//        }

//        [TestCase((short)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase((short)sbyte.MinValue, ExpectedResult = sbyte.MinValue)]
//        [TestCase(short.MaxValue, ExpectedResult = (sbyte)0)]
//        [TestCase(short.MinValue, ExpectedResult = (sbyte)0)]
//        [TestCase(sbyte.MaxValue + 1, ExpectedResult = (sbyte)0)]
//        [TestCase(sbyte.MinValue - 1, ExpectedResult = (sbyte)0)]
//        public sbyte ToSByte_FromInt16(short input)
//        {
//            return SafeConvert.ToSByte(input);
//        }

//        [TestCase((ushort)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase(ushort.MaxValue, ExpectedResult = (sbyte)0)]
//        [TestCase((ushort)(sbyte.MaxValue + 1), ExpectedResult = (sbyte)0)]
//        public sbyte ToSByte_FromUInt16(ushort input)
//        {
//            return SafeConvert.ToSByte(input);
//        }

//        [TestCase((int)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase((int)sbyte.MinValue, ExpectedResult = sbyte.MinValue)]
//        [TestCase(int.MaxValue, ExpectedResult = (sbyte)0)]
//        [TestCase(int.MinValue, ExpectedResult = (sbyte)0)]
//        [TestCase(sbyte.MaxValue + 1, ExpectedResult = (sbyte)0)]
//        [TestCase(sbyte.MinValue - 1, ExpectedResult = (sbyte)0)]
//        public sbyte ToSByte_FromInt32(int input)
//        {
//            return SafeConvert.ToSByte(input);
//        }

//        [TestCase((uint)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase(uint.MaxValue, ExpectedResult = (sbyte)0)]
//        [TestCase((uint)(sbyte.MaxValue + 1), ExpectedResult = (sbyte)0)]
//        public sbyte ToSByte_FromUInt32(uint input)
//        {
//            return SafeConvert.ToSByte(input);
//        }

//        [TestCase((long)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase((long)sbyte.MinValue, ExpectedResult = sbyte.MinValue)]
//        [TestCase(long.MaxValue, ExpectedResult = (sbyte)0)]
//        [TestCase(long.MinValue, ExpectedResult = (sbyte)0)]
//        [TestCase(sbyte.MaxValue + 1, ExpectedResult = (sbyte)0)]
//        [TestCase(sbyte.MinValue - 1, ExpectedResult = (sbyte)0)]
//        public sbyte ToSByte_FromInt64(long input)
//        {
//            return SafeConvert.ToSByte(input);
//        }

//        [TestCase((ulong)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase(ulong.MaxValue, ExpectedResult = (sbyte)0)]
//        [TestCase((ulong)(sbyte.MaxValue + 1), ExpectedResult = (sbyte)0)]
//        public sbyte ToSByte_FromUInt64(ulong input)
//        {
//            return SafeConvert.ToSByte(input);
//        }

//        [TestCase((float)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase((float)sbyte.MinValue, ExpectedResult = sbyte.MinValue)]
//        [TestCase(float.MaxValue, ExpectedResult = (sbyte)0)]
//        [TestCase(float.MinValue, ExpectedResult = (sbyte)0)]
//        [TestCase(sbyte.MaxValue + 1, ExpectedResult = (sbyte)0)]
//        [TestCase(sbyte.MinValue - 1, ExpectedResult = (sbyte)0)]
//        [TestCase(sbyte.MaxValue - 1.5f, ExpectedResult = sbyte.MaxValue - 2)]
//        [TestCase(sbyte.MinValue + 1.5f, ExpectedResult = sbyte.MinValue + 2)]
//        public sbyte ToSByte_FromSingle(float input)
//        {
//            return SafeConvert.ToSByte(input);
//        }

//        [TestCase((double)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase((double)sbyte.MinValue, ExpectedResult = sbyte.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = (sbyte)0)]
//        [TestCase(double.MinValue, ExpectedResult = (sbyte)0)]
//        [TestCase(sbyte.MaxValue + 1, ExpectedResult = (sbyte)0)]
//        [TestCase(sbyte.MinValue - 1, ExpectedResult = (sbyte)0)]
//        [TestCase(sbyte.MaxValue - 1.5d, ExpectedResult = sbyte.MaxValue - 2)]
//        [TestCase(sbyte.MinValue + 1.5d, ExpectedResult = sbyte.MinValue + 2)]
//        public sbyte ToSByte_FromDouble(double input)
//        {
//            return SafeConvert.ToSByte(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToSByteData")]
//        public sbyte ToSByte_FromDecimal(decimal input)
//        {
//            return SafeConvert.ToSByte(input);
//        }

//        [TestCase(false, ExpectedResult = (sbyte)0)]
//        [TestCase(true, ExpectedResult = (sbyte)1)]
//        public sbyte ToSByte_FromBoolean(bool input)
//        {
//            return SafeConvert.ToSByte(input);
//        }

//        [TestCase((char)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase(char.MaxValue, ExpectedResult = (sbyte)0)]
//        [TestCase((char)(sbyte.MaxValue + 1), ExpectedResult = (sbyte)0)]
//        public sbyte ToSByte_FromChar(char input)
//        {
//            return SafeConvert.ToSByte(input);
//        }

//        #endregion

//        #region ToInt16

//        [TestCase("32767", ExpectedResult = short.MaxValue)]
//        [TestCase("-32768", ExpectedResult = short.MinValue)]
//        [TestCase("32768", ExpectedResult = 0)]
//        [TestCase("-32769", ExpectedResult = 0)]
//        [TestCase(":", ExpectedResult = 0)]
//        [TestCase("/", ExpectedResult = 0)]
//        public short ToInt16_FromString(string input)
//        {
//            return SafeConvert.ToInt16(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (short)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (short)byte.MinValue)]
//        public short ToInt16_FromByte(byte input)
//        {
//            return SafeConvert.ToInt16(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (short)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = (short)sbyte.MinValue)]
//        public short ToInt16_FromSByte(sbyte input)
//        {
//            return SafeConvert.ToInt16(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = short.MinValue)]
//        public short ToInt16_FromInt16(short input)
//        {
//            return SafeConvert.ToInt16(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = (short)0)]
//        [TestCase(ushort.MinValue, ExpectedResult = (short)0)]
//        [TestCase((ushort)short.MaxValue, ExpectedResult = short.MaxValue)]
//        public short ToInt16_FromUInt16(ushort input)
//        {
//            return SafeConvert.ToInt16(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = (short)0)]
//        [TestCase(int.MinValue, ExpectedResult = (short)0)]
//        [TestCase((int)short.MaxValue, ExpectedResult = short.MaxValue)]
//        [TestCase((int)short.MinValue, ExpectedResult = short.MinValue)]
//        public short ToInt16_FromInt32(int input)
//        {
//            return SafeConvert.ToInt16(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = (short)0)]
//        [TestCase(uint.MinValue, ExpectedResult = (short)0)]
//        [TestCase((uint)short.MaxValue, ExpectedResult = short.MaxValue)]
//        public short ToInt16_FromUInt32(uint input)
//        {
//            return SafeConvert.ToInt16(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = (short)0)]
//        [TestCase(long.MinValue, ExpectedResult = (short)0)]
//        [TestCase((long)short.MaxValue, ExpectedResult = short.MaxValue)]
//        public short ToInt16_FromInt64(long input)
//        {
//            return SafeConvert.ToInt16(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = (short)0)]
//        [TestCase(ulong.MinValue, ExpectedResult = (short)0)]
//        [TestCase((ulong)short.MaxValue, ExpectedResult = short.MaxValue)]
//        public short ToInt16_FromUInt64(ulong input)
//        {
//            return SafeConvert.ToInt16(input);
//        }

//        [TestCase((float)short.MaxValue, ExpectedResult = short.MaxValue)]
//        [TestCase((float)short.MinValue, ExpectedResult = short.MinValue)]
//        [TestCase(float.MaxValue, ExpectedResult = (short)0)]
//        [TestCase(float.MinValue, ExpectedResult = (short)0)]
//        [TestCase(short.MaxValue + 1, ExpectedResult = (short)0)]
//        [TestCase(short.MinValue - 1, ExpectedResult = (short)0)]
//        [TestCase(short.MaxValue - 1.5f, ExpectedResult = short.MaxValue - 2)]
//        [TestCase(short.MinValue + 1.5f, ExpectedResult = short.MinValue + 2)]
//        public short ToInt16_FromSingle(float input)
//        {
//            return SafeConvert.ToInt16(input);
//        }

//        [TestCase((double)short.MaxValue, ExpectedResult = short.MaxValue)]
//        [TestCase((double)short.MinValue, ExpectedResult = short.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = (short)0)]
//        [TestCase(double.MinValue, ExpectedResult = (short)0)]
//        [TestCase(short.MaxValue + 1, ExpectedResult = (short)0)]
//        [TestCase(short.MinValue - 1, ExpectedResult = (short)0)]
//        [TestCase(short.MaxValue - 1.5d, ExpectedResult = short.MaxValue - 2)]
//        [TestCase(short.MinValue + 1.5d, ExpectedResult = short.MinValue + 2)]
//        public short ToInt16_FromDouble(double input)
//        {
//            return SafeConvert.ToInt16(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToInt16Data")]
//        public short ToInt16_FromDecimal(decimal input)
//        {
//            return SafeConvert.ToInt16(input);
//        }

//        [TestCase(false, ExpectedResult = (short)0)]
//        [TestCase(true, ExpectedResult = (short)1)]
//        public short ToInt16_FromBoolean(bool input)
//        {
//            return SafeConvert.ToInt16(input);
//        }

//        [TestCase((char)short.MaxValue, ExpectedResult = short.MaxValue)]
//        [TestCase(char.MaxValue, ExpectedResult = (short)0)]
//        [TestCase((char)(short.MaxValue + 1), ExpectedResult = (short)0)]
//        public short ToInt16_FromChar(char input)
//        {
//            return SafeConvert.ToInt16(input);
//        }

//        #endregion

//        #region ToUInt16

//        [TestCase("65535", ExpectedResult = ushort.MaxValue)]
//        [TestCase("0", ExpectedResult = ushort.MinValue)]
//        [TestCase("65536", ExpectedResult = 0)]
//        [TestCase("-1", ExpectedResult = 0)]
//        [TestCase(":", ExpectedResult = 0)]
//        [TestCase("/", ExpectedResult = 0)]
//        public ushort ToUInt16_FromString(string input)
//        {
//            return SafeConvert.ToUInt16(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (ushort)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (ushort)byte.MinValue)]
//        public ushort ToUInt16_FromByte(byte input)
//        {
//            return SafeConvert.ToUInt16(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (ushort)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = (ushort)0)]
//        public ushort ToUInt16_FromSByte(sbyte input)
//        {
//            return SafeConvert.ToUInt16(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (ushort)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = (ushort)0)]
//        [TestCase((short)ushort.MinValue, ExpectedResult = ushort.MinValue)]
//        public ushort ToUInt16_FromInt16(short input)
//        {
//            return SafeConvert.ToUInt16(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = ushort.MinValue)]
//        public ushort ToUInt16_FromUInt16(ushort input)
//        {
//            return SafeConvert.ToUInt16(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = (ushort)0)]
//        [TestCase(int.MinValue, ExpectedResult = (ushort)0)]
//        [TestCase((int)ushort.MaxValue, ExpectedResult = ushort.MaxValue)]
//        [TestCase((int)ushort.MinValue, ExpectedResult = ushort.MinValue)]
//        public ushort ToUInt16_FromInt32(int input)
//        {
//            return SafeConvert.ToUInt16(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = (ushort)0)]
//        [TestCase(uint.MinValue, ExpectedResult = (ushort)0)]
//        [TestCase((uint)ushort.MaxValue, ExpectedResult = ushort.MaxValue)]
//        public ushort ToUInt16_FromUInt32(uint input)
//        {
//            return SafeConvert.ToUInt16(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = (ushort)0)]
//        [TestCase(long.MinValue, ExpectedResult = (ushort)0)]
//        [TestCase((long)ushort.MaxValue, ExpectedResult = ushort.MaxValue)]
//        public ushort ToUInt16_FromInt64(long input)
//        {
//            return SafeConvert.ToUInt16(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = (ushort)0)]
//        [TestCase(ulong.MinValue, ExpectedResult = (ushort)0)]
//        [TestCase((ulong)ushort.MaxValue, ExpectedResult = ushort.MaxValue)]
//        public ushort ToUInt16_FromUInt64(ulong input)
//        {
//            return SafeConvert.ToUInt16(input);
//        }

//        [TestCase((float)ushort.MaxValue, ExpectedResult = ushort.MaxValue)]
//        [TestCase((float)ushort.MinValue, ExpectedResult = ushort.MinValue)]
//        [TestCase(float.MaxValue, ExpectedResult = (ushort)0)]
//        [TestCase(float.MinValue, ExpectedResult = (ushort)0)]
//        [TestCase(ushort.MaxValue + 1, ExpectedResult = (ushort)0)]
//        [TestCase(ushort.MinValue - 1, ExpectedResult = (ushort)0)]
//        [TestCase(ushort.MaxValue - 1.5f, ExpectedResult = ushort.MaxValue - 2)]
//        [TestCase(ushort.MinValue + 1.5f, ExpectedResult = ushort.MinValue + 1)]
//        public ushort ToUInt16_FromSingle(float input)
//        {
//            return SafeConvert.ToUInt16(input);
//        }

//        [TestCase((double)ushort.MaxValue, ExpectedResult = ushort.MaxValue)]
//        [TestCase((double)ushort.MinValue, ExpectedResult = ushort.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = (ushort)0)]
//        [TestCase(double.MinValue, ExpectedResult = (ushort)0)]
//        [TestCase(ushort.MaxValue + 1, ExpectedResult = (ushort)0)]
//        [TestCase(ushort.MinValue - 1, ExpectedResult = (ushort)0)]
//        [TestCase(ushort.MaxValue - 1.5d, ExpectedResult = ushort.MaxValue - 2)]
//        [TestCase(ushort.MinValue + 1.5d, ExpectedResult = ushort.MinValue + 1)]
//        public ushort ToUInt16_FromDouble(double input)
//        {
//            return SafeConvert.ToUInt16(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToUInt16Data")]
//        public ushort ToUInt16_FromDecimal(decimal input)
//        {
//            return SafeConvert.ToUInt16(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public ushort ToUInt16_FromBoolean(bool input)
//        {
//            return SafeConvert.ToUInt16(input);
//        }

//        [TestCase((char)ushort.MinValue, ExpectedResult = ushort.MinValue)]
//        [TestCase(char.MaxValue, ExpectedResult = ushort.MaxValue)]
//        public ushort ToUInt16_FromChar(char input)
//        {
//            return SafeConvert.ToUInt16(input);
//        }

//        #endregion

//        #region ToInt32

//        [TestCaseSource(typeof(TestData), "StringToIntData")]
//        public int ToInt32_FromString(string input)
//        {
//            return SafeConvert.ToInt32(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (short)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (short)byte.MinValue)]
//        public int ToInt32_FromByte(byte input)
//        {
//            return SafeConvert.ToInt32(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (int)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = (int)sbyte.MinValue)]
//        public int ToInt32_FromSByte(sbyte input)
//        {
//            return SafeConvert.ToInt32(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (int)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = (int)short.MinValue)]
//        public int ToInt32_FromInt16(short input)
//        {
//            return SafeConvert.ToInt32(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = (int)ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = 0)]
//        public int ToInt32_FromUInt16(ushort input)
//        {
//            return SafeConvert.ToInt32(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = int.MaxValue)]
//        [TestCase(int.MinValue, ExpectedResult = int.MinValue)]
//        public int ToInt32_FromInt32(int input)
//        {
//            return SafeConvert.ToInt32(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = 0)]
//        [TestCase(uint.MinValue, ExpectedResult = 0)]
//        [TestCase((uint)int.MaxValue, ExpectedResult = int.MaxValue)]
//        public int ToInt32_FromUInt32(uint input)
//        {
//            return SafeConvert.ToInt32(input);
//        }

//        [TestCaseSource(typeof(TestData), "LongToIntData")]
//        public int ToInt32_FromInt64(long input)
//        {
//            return SafeConvert.ToInt32(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = 0)]
//        [TestCase(ulong.MinValue, ExpectedResult = 0)]
//        [TestCase((ulong)int.MaxValue, ExpectedResult = int.MaxValue)]
//        public int ToInt32_FromUInt64(ulong input)
//        {
//            return SafeConvert.ToInt32(input);
//        }

//        [TestCase(float.MaxValue, ExpectedResult = 0)]
//        [TestCase(float.MinValue, ExpectedResult = 0)]
//        [TestCase((float)int.MinValue, ExpectedResult = int.MinValue)]
//        [TestCase(int.MaxValue - 7483647f, ExpectedResult = int.MaxValue - 7483647)]
//        public int ToInt32_FromSingle(float input)
//        {
//            return SafeConvert.ToInt32(input);
//        }

//        [TestCase((double)int.MaxValue, ExpectedResult = int.MaxValue)]
//        [TestCase((double)int.MinValue, ExpectedResult = int.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = 0)]
//        [TestCase(double.MinValue, ExpectedResult = 0)]
//        [TestCase(int.MaxValue - 1.5d, ExpectedResult = int.MaxValue - 2)]
//        [TestCase(int.MinValue + 1.5d, ExpectedResult = int.MinValue + 2)]
//        public int ToInt32_FromDouble(double input)
//        {
//            return SafeConvert.ToInt32(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToInt32Data")]
//        public int ToInt32_FromDecimal(decimal input)
//        {
//            return SafeConvert.ToInt32(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public int ToInt32_FromBoolean(bool input)
//        {
//            return SafeConvert.ToInt32(input);
//        }

//        [TestCase(char.MaxValue, ExpectedResult = (int)char.MaxValue)]
//        [TestCase(char.MinValue, ExpectedResult = 0)]
//        public int ToInt32_FromChar(char input)
//        {
//            return SafeConvert.ToInt32(input);
//        }

//        #endregion

//        #region ToUInt32

//        [TestCase("4294967295", ExpectedResult = uint.MaxValue)]
//        [TestCase("0", ExpectedResult = uint.MinValue)]
//        [TestCase("4294967296", ExpectedResult = 0)]
//        [TestCase("-1", ExpectedResult = 0)]
//        [TestCase(":", ExpectedResult = 0)]
//        [TestCase("/", ExpectedResult = 0)]
//        public uint ToUInt32_FromString(string input)
//        {
//            return SafeConvert.ToUInt32(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (uint)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (uint)byte.MinValue)]
//        public uint ToUInt32_FromByte(byte input)
//        {
//            return SafeConvert.ToUInt32(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (uint)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = (uint)0)]
//        public uint ToUInt32_FromSByte(sbyte input)
//        {
//            return SafeConvert.ToUInt32(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (uint)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = (uint)0)]
//        [TestCase((short)uint.MinValue, ExpectedResult = uint.MinValue)]
//        public uint ToUInt32_FromInt16(short input)
//        {
//            return SafeConvert.ToUInt32(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = (uint)ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = (uint)ushort.MinValue)]
//        public uint ToUInt32_FromUInt16(ushort input)
//        {
//            return SafeConvert.ToUInt32(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = (uint)int.MaxValue)]
//        [TestCase(int.MinValue, ExpectedResult = (uint)0)]
//        [TestCase((int)uint.MinValue, ExpectedResult = uint.MinValue)]
//        public uint ToUInt32_FromInt32(int input)
//        {
//            return SafeConvert.ToUInt32(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = uint.MaxValue)]
//        [TestCase(uint.MinValue, ExpectedResult = uint.MinValue)]
//        public uint ToUInt32_FromUInt32(uint input)
//        {
//            return SafeConvert.ToUInt32(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = (uint)0)]
//        [TestCase(long.MinValue, ExpectedResult = (uint)0)]
//        [TestCase((long)uint.MaxValue, ExpectedResult = uint.MaxValue)]
//        public uint ToUInt32_FromInt64(long input)
//        {
//            return SafeConvert.ToUInt32(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = (uint)0)]
//        [TestCase(ulong.MinValue, ExpectedResult = (uint)0)]
//        [TestCase((ulong)uint.MaxValue, ExpectedResult = uint.MaxValue)]
//        public uint ToUInt32_FromUInt64(ulong input)
//        {
//            return SafeConvert.ToUInt32(input);
//        }

//        [TestCase((float)uint.MinValue, ExpectedResult = uint.MinValue)]
//        [TestCase(float.MaxValue, ExpectedResult = (uint)0)]
//        [TestCase(float.MinValue, ExpectedResult = (uint)0)]
//        [TestCase(uint.MinValue - 1f, ExpectedResult = (uint)0)]
//        [TestCase(uint.MinValue + 1.5f, ExpectedResult = uint.MinValue + 1)]
//        public uint ToUInt32_FromSingle(float input)
//        {
//            return SafeConvert.ToUInt32(input);
//        }

//        [TestCase((double)uint.MaxValue, ExpectedResult = uint.MaxValue)]
//        [TestCase((double)uint.MinValue, ExpectedResult = uint.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = (uint)0)]
//        [TestCase(double.MinValue, ExpectedResult = (uint)0)]
//        [TestCase(uint.MaxValue + 1d, ExpectedResult = (uint)0)]
//        [TestCase(uint.MinValue - 1d, ExpectedResult = (uint)0)]
//        [TestCase(uint.MaxValue - 1.5d, ExpectedResult = uint.MaxValue - 2)]
//        [TestCase(uint.MinValue + 1.5d, ExpectedResult = uint.MinValue + 1)]
//        public uint ToUInt32_FromDouble(double input)
//        {
//            return SafeConvert.ToUInt32(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToUInt32Data")]
//        public uint ToUInt32_FromDecimal(decimal input)
//        {
//            return SafeConvert.ToUInt32(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public uint ToUInt32_FromBoolean(bool input)
//        {
//            return SafeConvert.ToUInt32(input);
//        }

//        [TestCase((char)uint.MinValue, ExpectedResult = uint.MinValue)]
//        [TestCase(char.MaxValue, ExpectedResult = (uint)char.MaxValue)]
//        public uint ToUInt32_FromChar(char input)
//        {
//            return SafeConvert.ToUInt32(input);
//        }

//        #endregion

//        #region ToInt64

//        [TestCase(null, ExpectedResult = 0)]
//        [TestCase("", ExpectedResult = 0)]
//        [TestCase("9223372036854775807", ExpectedResult = long.MaxValue)]
//        [TestCase("-9223372036854775808", ExpectedResult = long.MinValue)]
//        //[TestCase("9223372036854775808", ExpectedResult = 0)]
//        //[TestCase("-9223372036854775809", ExpectedResult = 0)]
//        [TestCase(":", ExpectedResult = 0)]
//        [TestCase("/", ExpectedResult = 0)]
//        public long ToInt64_FromString(string input)
//        {
//            return SafeConvert.ToInt64(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (long)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (long)byte.MinValue)]
//        public long ToInt64_FromByte(byte input)
//        {
//            return SafeConvert.ToInt64(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (long)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = (long)sbyte.MinValue)]
//        public long ToInt64_FromSByte(sbyte input)
//        {
//            return SafeConvert.ToInt64(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (long)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = (long)short.MinValue)]
//        public long ToInt64_FromInt16(short input)
//        {
//            return SafeConvert.ToInt64(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = (long)ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = 0)]
//        public long ToInt64_FromUInt16(ushort input)
//        {
//            return SafeConvert.ToInt64(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = (long)int.MaxValue)]
//        [TestCase(int.MinValue, ExpectedResult = (long)int.MinValue)]
//        public long ToInt64_FromInt32(int input)
//        {
//            return SafeConvert.ToInt64(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = (long)uint.MaxValue)]
//        [TestCase(uint.MinValue, ExpectedResult = (long)uint.MinValue)]
//        [TestCase((uint)int.MaxValue, ExpectedResult = int.MaxValue)]
//        public long ToInt64_FromUInt32(uint input)
//        {
//            return SafeConvert.ToInt64(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = long.MaxValue)]
//        [TestCase(long.MinValue, ExpectedResult = long.MinValue)]
//        public long ToInt64_FromInt64(long input)
//        {
//            return SafeConvert.ToInt64(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = 0)]
//        [TestCase(ulong.MinValue, ExpectedResult = 0)]
//        [TestCase((ulong)long.MaxValue, ExpectedResult = long.MaxValue)]
//        public long ToInt64_FromUInt64(ulong input)
//        {
//            return SafeConvert.ToInt64(input);
//        }

//        [TestCase(float.MaxValue, ExpectedResult = 0)]
//        [TestCase(float.MinValue, ExpectedResult = 0)]
//        [TestCase((float)int.MinValue, ExpectedResult = int.MinValue)]
//        public long ToInt64_FromSingle(float input)
//        {
//            return SafeConvert.ToInt64(input);
//        }

//        [TestCase((double)int.MaxValue, ExpectedResult = int.MaxValue)]
//        [TestCase((double)int.MinValue, ExpectedResult = int.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = 0)]
//        [TestCase(double.MinValue, ExpectedResult = 0)]
//        [TestCase(int.MaxValue - 1.5d, ExpectedResult = int.MaxValue - 2)]
//        [TestCase(int.MinValue + 1.5d, ExpectedResult = int.MinValue + 2)]
//        public long ToInt64_FromDouble(double input)
//        {
//            return SafeConvert.ToInt64(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToInt64Data")]
//        public long ToInt64_FromDecimal(decimal input)
//        {
//            return SafeConvert.ToInt64(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public long ToInt64_FromBoolean(bool input)
//        {
//            return SafeConvert.ToInt64(input);
//        }

//        [TestCase(char.MaxValue, ExpectedResult = (long)char.MaxValue)]
//        [TestCase(char.MinValue, ExpectedResult = 0)]
//        public long ToInt64_FromChar(char input)
//        {
//            return SafeConvert.ToInt64(input);
//        }

//        #endregion

//        #region ToUInt64

//        [TestCase("18446744073709551615", ExpectedResult = ulong.MaxValue)]
//        [TestCase("0", ExpectedResult = ulong.MinValue)]
//        [TestCase("18446744073709551616", ExpectedResult = 0)]
//        //[TestCase("18446744073709551617", ExpectedResult = 0)]
//        [TestCase("-1", ExpectedResult = 0)]
//        [TestCase(":", ExpectedResult = 0)]
//        [TestCase("/", ExpectedResult = 0)]
//        public ulong ToUInt64_FromString(string input)
//        {
//            return SafeConvert.ToUInt64(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (ulong)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (ulong)byte.MinValue)]
//        public ulong ToUInt64_FromByte(byte input)
//        {
//            return SafeConvert.ToUInt64(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (ulong)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = (ulong)0)]
//        public ulong ToUInt64_FromSByte(sbyte input)
//        {
//            return SafeConvert.ToUInt64(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (ulong)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = (ulong)0)]
//        [TestCase((short)uint.MinValue, ExpectedResult = ulong.MinValue)]
//        public ulong ToUInt64_FromInt16(short input)
//        {
//            return SafeConvert.ToUInt64(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = (ulong)ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = (ulong)ushort.MinValue)]
//        public ulong ToUInt64_FromUInt16(ushort input)
//        {
//            return SafeConvert.ToUInt64(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = (ulong)int.MaxValue)]
//        [TestCase(int.MinValue, ExpectedResult = (ulong)0)]
//        [TestCase((int)uint.MinValue, ExpectedResult = ulong.MinValue)]
//        public ulong ToUInt64_FromInt32(int input)
//        {
//            return SafeConvert.ToUInt64(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = (ulong)uint.MaxValue)]
//        [TestCase(uint.MinValue, ExpectedResult = (ulong)uint.MinValue)]
//        public ulong ToUInt64_FromUInt32(uint input)
//        {
//            return SafeConvert.ToUInt64(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = (ulong)long.MaxValue)]
//        [TestCase(long.MinValue, ExpectedResult = (ulong)0)]
//        public ulong ToUInt64_FromInt64(long input)
//        {
//            return SafeConvert.ToUInt64(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = ulong.MaxValue)]
//        [TestCase(ulong.MinValue, ExpectedResult = ulong.MinValue)]
//        public ulong ToUInt64_FromUInt64(ulong input)
//        {
//            return SafeConvert.ToUInt64(input);
//        }

//        [TestCase((float)ulong.MinValue, ExpectedResult = ulong.MinValue)]
//        [TestCase(float.MaxValue, ExpectedResult = (ulong)0)]
//        [TestCase(float.MinValue, ExpectedResult = (ulong)0)]
//        [TestCase(ulong.MinValue - 1f, ExpectedResult = (ulong)0)]
//        [TestCase(ulong.MinValue + 1.5f, ExpectedResult = ulong.MinValue + 1)]
//        public ulong ToUInt64_FromSingle(float input)
//        {
//            return SafeConvert.ToUInt64(input);
//        }

//        //[TestCase((double)ulong.MaxValue, ExpectedResult = ulong.MaxValue)]
//        [TestCase((double)ulong.MinValue, ExpectedResult = ulong.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = (ulong)0)]
//        [TestCase(double.MinValue, ExpectedResult = (ulong)0)]
//        [TestCase(ulong.MaxValue + 1d, ExpectedResult = (ulong)0)]
//        [TestCase(ulong.MinValue - 1d, ExpectedResult = (ulong)0)]
//        //[TestCase(ulong.MaxValue - 1.5d, ExpectedResult = ulong.MaxValue - 2)]
//        [TestCase(ulong.MinValue + 1.5d, ExpectedResult = ulong.MinValue + 1)]
//        public ulong ToUInt64_FromDouble(double input)
//        {
//            return SafeConvert.ToUInt64(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToUInt64Data")]
//        public ulong ToUInt64_FromDecimal(decimal input)
//        {
//            return SafeConvert.ToUInt64(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public ulong ToUInt64_FromBoolean(bool input)
//        {
//            return SafeConvert.ToUInt64(input);
//        }

//        [TestCase((char)ulong.MinValue, ExpectedResult = ulong.MinValue)]
//        [TestCase(char.MaxValue, ExpectedResult = (ulong)char.MaxValue)]
//        public ulong ToUInt64_FromChar(char input)
//        {
//            return SafeConvert.ToUInt64(input);
//        }

//        #endregion

//        #region ToSingle

//        [TestCase(null, ExpectedResult = 0f)]
//        [TestCase("", ExpectedResult = 0f)]
//        [TestCase("1", ExpectedResult = 1f)]
//        public float ToSingle_FromString(string input)
//        {
//            return SafeConvert.ToSingle(input);
//        }

//        [TestCase(null, ExpectedResult = 0)]
//        [TestCase("", ExpectedResult = 0)]
//        [TestCase("1.7976931348623157", ExpectedResult = 1.7976931348623157f)]
//        [TestCase("-1.7976931348623157", ExpectedResult = -1.7976931348623157f)]
//        [TestCase(":", ExpectedResult = 0)]
//        [TestCase("/", ExpectedResult = 0)]
//        public float ToSingle_FromStringWithFormat(string input)
//        {
//            return SafeConvert.ToSingle(input, this.numberFormatProvider);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (float)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (float)byte.MinValue)]
//        public float ToSingle_FromByte(byte input)
//        {
//            return SafeConvert.ToSingle(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (float)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = (float)sbyte.MinValue)]
//        public float ToSingle_FromSByte(sbyte input)
//        {
//            return SafeConvert.ToSingle(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (float)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = (float)short.MinValue)]
//        public float ToSingle_FromInt16(short input)
//        {
//            return SafeConvert.ToSingle(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = (float)ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = 0)]
//        public float ToSingle_FromUInt16(ushort input)
//        {
//            return SafeConvert.ToSingle(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = (float)int.MaxValue)]
//        [TestCase(int.MinValue, ExpectedResult = (float)int.MinValue)]
//        public float ToSingle_FromInt32(int input)
//        {
//            return SafeConvert.ToSingle(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = (float)uint.MaxValue)]
//        [TestCase(uint.MinValue, ExpectedResult = (float)uint.MinValue)]
//        public float ToSingle_FromUInt32(uint input)
//        {
//            return SafeConvert.ToSingle(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = (float)long.MaxValue)]
//        [TestCase(long.MinValue, ExpectedResult = (float)long.MinValue)]
//        public float ToSingle_FromInt64(long input)
//        {
//            return SafeConvert.ToSingle(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = (float)ulong.MaxValue)]
//        [TestCase(ulong.MinValue, ExpectedResult = 0d)]
//        public float ToSingle_FromUInt64(ulong input)
//        {
//            return SafeConvert.ToSingle(input);
//        }

//        [TestCase(float.MaxValue, ExpectedResult = float.MaxValue)]
//        [TestCase(float.MinValue, ExpectedResult = float.MinValue)]
//        public float ToSingle_FromSingle(float input)
//        {
//            return SafeConvert.ToSingle(input);
//        }

//        [TestCase((double)float.MaxValue, ExpectedResult = float.MaxValue)]
//        [TestCase((double)float.MinValue, ExpectedResult = float.MinValue)]
//        //[TestCase(((double)float.MaxValue) + 1d, ExpectedResult = 0f)]
//        //[TestCase(((double)float.MinValue) - 1d, ExpectedResult = 0f)]
//        [TestCase(double.MaxValue, ExpectedResult = 0f)]
//        [TestCase(double.MinValue, ExpectedResult = 0f)]
//        public float ToSingle_FromDouble(double input)
//        {
//            return SafeConvert.ToSingle(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToSingleData")]
//        public float ToSingle_FromDecimal(decimal input)
//        {
//            return SafeConvert.ToSingle(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public float ToSingle_FromBoolean(bool input)
//        {
//            return SafeConvert.ToSingle(input);
//        }

//        [TestCase(char.MaxValue, ExpectedResult = (float)char.MaxValue)]
//        [TestCase(char.MinValue, ExpectedResult = 0)]
//        public float ToSingle_FromChar(char input)
//        {
//            return SafeConvert.ToSingle(input);
//        }

//        #endregion

//        #region ToDouble

//        [TestCase(null, ExpectedResult = 0d)]
//        [TestCase("", ExpectedResult = 0d)]
//        [TestCase("1", ExpectedResult = 1d)]
//        public double ToDouble_FromString(string input)
//        {
//            return SafeConvert.ToDouble(input);
//        }

//        [TestCase(null, ExpectedResult = 0)]
//        [TestCase("", ExpectedResult = 0)]
//        [TestCase("1.7976931348623157", ExpectedResult = 1.7976931348623157)]
//        [TestCase("-1.7976931348623157", ExpectedResult = -1.7976931348623157)]
//        [TestCase(":", ExpectedResult = 0)]
//        [TestCase("/", ExpectedResult = 0)]
//        public double ToDouble_FromStringWithFormat(string input)
//        {
//            return SafeConvert.ToDouble(input, CultureInfo.InvariantCulture);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (double)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (double)byte.MinValue)]
//        public double ToDouble_FromByte(byte input)
//        {
//            return SafeConvert.ToDouble(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (double)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = (double)sbyte.MinValue)]
//        public double ToDouble_FromSByte(sbyte input)
//        {
//            return SafeConvert.ToDouble(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (double)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = (double)short.MinValue)]
//        public double ToDouble_FromInt16(short input)
//        {
//            return SafeConvert.ToDouble(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = (double)ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = 0)]
//        public double ToDouble_FromUInt16(ushort input)
//        {
//            return SafeConvert.ToDouble(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = (double)int.MaxValue)]
//        [TestCase(int.MinValue, ExpectedResult = (double)int.MinValue)]
//        public double ToDouble_FromInt32(int input)
//        {
//            return SafeConvert.ToDouble(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = (double)uint.MaxValue)]
//        [TestCase(uint.MinValue, ExpectedResult = (double)uint.MinValue)]
//        public double ToDouble_FromUInt32(uint input)
//        {
//            return SafeConvert.ToDouble(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = (double)long.MaxValue)]
//        [TestCase(long.MinValue, ExpectedResult = (double)long.MinValue)]
//        public double ToDouble_FromInt64(long input)
//        {
//            return SafeConvert.ToDouble(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = (double)ulong.MaxValue)]
//        [TestCase(ulong.MinValue, ExpectedResult = 0d)]
//        public double ToDouble_FromUInt64(ulong input)
//        {
//            return SafeConvert.ToDouble(input);
//        }

//        [TestCase(float.MaxValue, ExpectedResult = (double)float.MaxValue)]
//        [TestCase(float.MinValue, ExpectedResult = (double)float.MinValue)]
//        public double ToDouble_FromSingle(float input)
//        {
//            return SafeConvert.ToDouble(input);
//        }

//        [TestCase(double.MaxValue, ExpectedResult = double.MaxValue)]
//        [TestCase(double.MinValue, ExpectedResult = double.MinValue)]
//        public double ToDouble_FromDouble(double input)
//        {
//            return SafeConvert.ToDouble(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToDoubleData")]
//        public double ToDouble_FromDecimal(decimal input)
//        {
//            return SafeConvert.ToDouble(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public double ToDouble_FromBoolean(bool input)
//        {
//            return SafeConvert.ToDouble(input);
//        }

//        [TestCase(char.MaxValue, ExpectedResult = (double)char.MaxValue)]
//        [TestCase(char.MinValue, ExpectedResult = 0)]
//        public double ToDouble_FromChar(char input)
//        {
//            return SafeConvert.ToDouble(input);
//        }

//        #endregion

//        #region ToDecimal

//        [TestCaseSource(typeof(TestData), "StringToDecimalData")]
//        public decimal ToDecimal_FromString(string input)
//        {
//            return SafeConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "StringToDecimalWithFormatData")]
//        public decimal ToDecimal_FromStringWithFormat(string input)
//        {
//            return SafeConvert.ToDecimal(input, this.numberFormatProvider);
//        }

//        [TestCaseSource(typeof(TestData), "ByteToDecimalData")]
//        public decimal ToDecimal_FromByte(byte input)
//        {
//            return SafeConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "SByteToDecimalData")]
//        public decimal ToDecimal_FromSByte(sbyte input)
//        {
//            return SafeConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "Int16ToDecimalData")]
//        public decimal ToDecimal_FromInt16(short input)
//        {
//            return SafeConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "UInt16ToDecimalData")]
//        public decimal ToDecimal_FromUInt16(ushort input)
//        {
//            return SafeConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "Int32ToDecimalData")]
//        public decimal ToDecimal_FromInt32(int input)
//        {
//            return SafeConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "UInt32ToDecimalData")]
//        public decimal ToDecimal_FromUInt32(uint input)
//        {
//            return SafeConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "Int64ToDecimalData")]
//        public decimal ToDecimal_FromInt64(long input)
//        {
//            return SafeConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "UInt64ToDecimalData")]
//        public decimal ToDecimal_FromUInt64(ulong input)
//        {
//            return SafeConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "SingleToDecimalData")]
//        public decimal ToDecimal_FromSingle(float input)
//        {
//            return SafeConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "DoubleToDecimalData")]
//        public decimal ToDecimal_FromDouble(double input)
//        {
//            return SafeConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToDecimalData")]
//        public decimal ToDecimal_FromDecimal(decimal input)
//        {
//            return SafeConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "BooleanToDecimalData")]
//        public decimal ToDecimal_FromBoolean(bool input)
//        {
//            return SafeConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "CharToDecimalData")]
//        public decimal ToDecimal_FromChar(char input)
//        {
//            return SafeConvert.ToDecimal(input);
//        }

//        #endregion

//        #region ToBoolean

//        [TestCase("True", ExpectedResult = true)]
//        [TestCase("False", ExpectedResult = false)]
//        [TestCase("true", ExpectedResult = true)]
//        [TestCase("false", ExpectedResult = false)]
//        [TestCase("Foo", ExpectedResult = false)]
//        public bool ToBoolean_FromString(string input)
//        {
//            return SafeConvert.ToBoolean(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = true)]
//        [TestCase(byte.MinValue, ExpectedResult = false)]
//        public bool ToBoolean_FromByte(byte input)
//        {
//            return SafeConvert.ToBoolean(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = true)]
//        [TestCase(sbyte.MinValue, ExpectedResult = true)]
//        [TestCase((sbyte)0, ExpectedResult = false)]
//        public bool ToBoolean_FromSByte(sbyte input)
//        {
//            return SafeConvert.ToBoolean(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = true)]
//        [TestCase(short.MinValue, ExpectedResult = true)]
//        [TestCase((short)0, ExpectedResult = false)]
//        public bool ToBoolean_FromInt16(short input)
//        {
//            return SafeConvert.ToBoolean(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = true)]
//        [TestCase(ushort.MinValue, ExpectedResult = false)]
//        public bool ToBoolean_FromUInt16(ushort input)
//        {
//            return SafeConvert.ToBoolean(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = true)]
//        [TestCase(int.MinValue, ExpectedResult = true)]
//        [TestCase(0, ExpectedResult = false)]
//        public bool ToBoolean_FromInt32(int input)
//        {
//            return SafeConvert.ToBoolean(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = true)]
//        [TestCase(uint.MinValue, ExpectedResult = false)]
//        public bool ToBoolean_FromUInt32(uint input)
//        {
//            return SafeConvert.ToBoolean(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = true)]
//        [TestCase(long.MinValue, ExpectedResult = true)]
//        [TestCase(0L, ExpectedResult = false)]
//        public bool ToBoolean_FromInt64(long input)
//        {
//            return SafeConvert.ToBoolean(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = true)]
//        [TestCase(ulong.MinValue, ExpectedResult = false)]
//        public bool ToBoolean_FromUInt64(ulong input)
//        {
//            return SafeConvert.ToBoolean(input);
//        }

//        [TestCase(float.MaxValue, ExpectedResult = true)]
//        [TestCase(float.MinValue, ExpectedResult = true)]
//        [TestCase(0f, ExpectedResult = false)]
//        public bool ToBoolean_FromSingle(float input)
//        {
//            return SafeConvert.ToBoolean(input);
//        }

//        [TestCase(double.MaxValue, ExpectedResult = true)]
//        [TestCase(double.MinValue, ExpectedResult = true)]
//        [TestCase(0d, ExpectedResult = false)]
//        public bool ToBoolean_FromDouble(double input)
//        {
//            return SafeConvert.ToBoolean(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToBooleanData")]
//        public bool ToBoolean_FromDecimal(decimal input)
//        {
//            return SafeConvert.ToBoolean(input);
//        }

//        [TestCase(false, ExpectedResult = false)]
//        [TestCase(true, ExpectedResult = true)]
//        public bool ToBoolean_FromBoolean(bool input)
//        {
//            return SafeConvert.ToBoolean(input);
//        }

//        [TestCase(char.MaxValue, ExpectedResult = true)]
//        [TestCase(char.MinValue, ExpectedResult = false)]
//        public bool ToBoolean_FromChar(char input)
//        {
//            return SafeConvert.ToBoolean(input);
//        }

//        #endregion

//        #region ToChar

//        [TestCase("", ExpectedResult = (char)0)]
//        [TestCase(null, ExpectedResult = (char)0)]
//        [TestCase("A", ExpectedResult = 'A')]
//        [TestCase("0", ExpectedResult = '0')]
//        [TestCase("ab", ExpectedResult = (char)0)]
//        public char ToChar_FromString(string input)
//        {
//            return SafeConvert.ToChar(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (char)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (char)byte.MinValue)]
//        public char ToChar_FromByte(byte input)
//        {
//            return SafeConvert.ToChar(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (char)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = (char)0)]
//        public char ToChar_FromSByte(sbyte input)
//        {
//            return SafeConvert.ToChar(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (char)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = (char)0)]
//        [TestCase((short)char.MinValue, ExpectedResult = char.MinValue)]
//        public char ToChar_FromInt16(short input)
//        {
//            return SafeConvert.ToChar(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = (char)ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = (char)ushort.MinValue)]
//        public char ToChar_FromUInt16(ushort input)
//        {
//            return SafeConvert.ToChar(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = (char)0)]
//        [TestCase(int.MinValue, ExpectedResult = (char)0)]
//        [TestCase((int)char.MaxValue, ExpectedResult = char.MaxValue)]
//        [TestCase((int)char.MinValue, ExpectedResult = char.MinValue)]
//        public char ToChar_FromInt32(int input)
//        {
//            return SafeConvert.ToChar(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = (char)0)]
//        [TestCase(uint.MinValue, ExpectedResult = (char)0)]
//        [TestCase((uint)char.MaxValue, ExpectedResult = char.MaxValue)]
//        public char ToChar_FromUInt32(uint input)
//        {
//            return SafeConvert.ToChar(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = (char)0)]
//        [TestCase(long.MinValue, ExpectedResult = (char)0)]
//        [TestCase((long)char.MaxValue, ExpectedResult = char.MaxValue)]
//        public char ToChar_FromInt64(long input)
//        {
//            return SafeConvert.ToChar(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = (char)0)]
//        [TestCase(ulong.MinValue, ExpectedResult = (char)0)]
//        [TestCase((ulong)char.MaxValue, ExpectedResult = char.MaxValue)]
//        public char ToChar_FromUInt64(ulong input)
//        {
//            return SafeConvert.ToChar(input);
//        }

//        [TestCase((float)char.MinValue, ExpectedResult = char.MinValue)]
//        [TestCase(float.MaxValue, ExpectedResult = (char)0)]
//        [TestCase(float.MinValue, ExpectedResult = (char)0)]
//        [TestCase((float)char.MinValue - 1f, ExpectedResult = (char)0)]
//        [TestCase(65f, ExpectedResult = 'A')]
//        [TestCase(65.5f, ExpectedResult = 'A')]
//        public char ToChar_FromSingle(float input)
//        {
//            return SafeConvert.ToChar(input);
//        }

//        [TestCase((double)char.MaxValue, ExpectedResult = char.MaxValue)]
//        [TestCase((double)char.MinValue, ExpectedResult = char.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = (char)0)]
//        [TestCase(double.MinValue, ExpectedResult = (char)0)]
//        [TestCase((double)char.MaxValue + 1d, ExpectedResult = (char)0)]
//        [TestCase((double)char.MinValue - 1d, ExpectedResult = (char)0)]
//        [TestCase((double)char.MaxValue - 1.5d, ExpectedResult = (char)(char.MaxValue - 2))]
//        [TestCase(65d, ExpectedResult = 'A')]
//        [TestCase(65.5d, ExpectedResult = 'A')]
//        public char ToChar_FromDouble(double input)
//        {
//            return SafeConvert.ToChar(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToCharData")]
//        public char ToChar_FromDecimal(decimal input)
//        {
//            return SafeConvert.ToChar(input);
//        }

//        [TestCase(false, ExpectedResult = '0')]
//        [TestCase(true, ExpectedResult = '1')]
//        public char ToChar_FromBoolean(bool input)
//        {
//            return SafeConvert.ToChar(input);
//        }

//        [TestCase(char.MaxValue, ExpectedResult = char.MaxValue)]
//        [TestCase(char.MinValue, ExpectedResult = char.MinValue)]
//        public char ToChar_FromChar(char input)
//        {
//            return SafeConvert.ToChar(input);
//        }

//        #endregion

//        #region ToGuid

//        [TestCaseSource(typeof(TestData), "StringToGuidData")]
//        public Guid ToGuid_FromString(string input)
//        {
//            return SafeConvert.ToGuid(input);
//        }

//        #endregion

//        #region ToDateTime

//        [TestCaseSource(typeof(TestData), "StringToDateTimeData")]
//        public DateTime ToDateTime_FromString(string input)
//        {
//            return SafeConvert.ToDateTime(input);
//        }

//        [TestCaseSource(typeof(TestData), "StringToDateTimeData")]
//        public DateTime ToDateTime_FromStringWithFormat(string input)
//        {
//            return SafeConvert.ToDateTime(input, CultureInfo.InvariantCulture);
//        }

//        [TestCaseSource(typeof(TestData), "SqlDateTimeToDateTimeData")]
//        public DateTime ToDateTime_FromSqlDateTime(SqlDateTime input)
//        {
//            return SafeConvert.ToDateTime(input);
//        }

//        #endregion

//        #region ToSqlDateTime

//        [TestCaseSource(typeof(TestData), "DateTimeToSqlDateTimeData")]
//        public SqlDateTime ToSqlDateTime_FromSqlDateTime(DateTime input)
//        {
//            return SafeConvert.ToSqlDateTime(input);
//        }

//        #endregion

//        #region ToString

//        [TestCase(null, ExpectedResult = null)]
//        [TestCase(new char[0], ExpectedResult = "")]
//        [TestCase(new[] { 'F', 'o', 'o' }, ExpectedResult = "Foo")]
//        public string ToString_FromCharArray(char[] input)
//        {
//            return SafeConvert.ToString(input);
//        }

//        #endregion

//        #region Other

//        [TestCase(null, ExpectedResult = null)]
//        [TestCase("", ExpectedResult = new char[0])]
//        [TestCase("Foo", ExpectedResult = new[] { 'F', 'o', 'o' })]
//        public char[] ToCharArray_FromString(string input)
//        {
//            return SafeConvert.ToCharArray(input);
//        }

//        #endregion
//    }
//}
