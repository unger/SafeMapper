//namespace SafeMapper.Tests
//{
//    using System;
//    using System.Data.SqlTypes;
//    using System.Globalization;

//    using Xunit;

    
//    public class SafeNullableConvertTests
//    {
//        private IFormatProvider numberFormatProvider;

//        public SafeNullableConvertTests()
//        {
//            var numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();

//            numberFormat.NumberDecimalSeparator = ".";
//            numberFormat.CurrencyDecimalSeparator = ".";
//            numberFormat.NumberGroupSeparator = " ";
//            numberFormat.CurrencyGroupSeparator = " ";
//            this.numberFormatProvider = numberFormat;
//        }

//        #region? ToNullableByte

//        [TestCase("255", ExpectedResult = byte.MaxValue)]
//        [TestCase("0", ExpectedResult = byte.MinValue)]
//        [TestCase("256", ExpectedResult = null)]
//        [TestCase("-1", ExpectedResult = null)]
//        [TestCase(":", ExpectedResult = null)]
//        [TestCase("/", ExpectedResult = null)]
//        public byte? ToNullableByte_FromString(string input)
//        {
//            return SafeNullableConvert.ToByte(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = byte.MinValue)]
//        public byte? ToNullableByte_FromByte(byte input)
//        {
//            return SafeNullableConvert.ToByte(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (byte)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = null)]
//        public byte? ToNullableByte_FromSByte(sbyte input)
//        {
//            return SafeNullableConvert.ToByte(input);
//        }

//        [TestCase((short)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((short)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(short.MaxValue, ExpectedResult = null)]
//        [TestCase(short.MinValue, ExpectedResult = null)]
//        [TestCase(byte.MaxValue + 1, ExpectedResult = null)]
//        [TestCase(byte.MinValue - 1, ExpectedResult = null)]
//        public byte? ToNullableByte_FromInt16(short input)
//        {
//            return SafeNullableConvert.ToByte(input);
//        }

//        [TestCase((ushort)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((ushort)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(ushort.MaxValue, ExpectedResult = null)]
//        [TestCase((ushort)(byte.MaxValue + 1), ExpectedResult = null)]
//        public byte? ToNullableByte_FromUInt16(ushort input)
//        {
//            return SafeNullableConvert.ToByte(input);
//        }

//        [TestCase((int)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((int)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(int.MaxValue, ExpectedResult = null)]
//        [TestCase(int.MinValue, ExpectedResult = null)]
//        [TestCase(byte.MaxValue + 1, ExpectedResult = null)]
//        [TestCase(byte.MinValue - 1, ExpectedResult = null)]
//        public byte? ToNullableByte_FromInt32(int input)
//        {
//            return SafeNullableConvert.ToByte(input);
//        }

//        [TestCase((uint)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((uint)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(uint.MaxValue, ExpectedResult = null)]
//        [TestCase((uint)(byte.MaxValue + 1), ExpectedResult = null)]
//        public byte? ToNullableByte_FromUInt32(uint input)
//        {
//            return SafeNullableConvert.ToByte(input);
//        }

//        [TestCase((long)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((long)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(long.MaxValue, ExpectedResult = null)]
//        [TestCase(long.MinValue, ExpectedResult = null)]
//        [TestCase(byte.MaxValue + 1, ExpectedResult = null)]
//        [TestCase(byte.MinValue - 1, ExpectedResult = null)]
//        public byte? ToNullableByte_FromInt64(long input)
//        {
//            return SafeNullableConvert.ToByte(input);
//        }

//        [TestCase((ulong)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((ulong)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(ulong.MaxValue, ExpectedResult = null)]
//        [TestCase((ulong)(byte.MaxValue + 1), ExpectedResult = null)]
//        public byte? ToNullableByte_FromUInt64(ulong input)
//        {
//            return SafeNullableConvert.ToByte(input);
//        }

//        [TestCase((float)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((float)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(float.MaxValue, ExpectedResult = null)]
//        [TestCase(float.MinValue, ExpectedResult = null)]
//        [TestCase(byte.MaxValue + 1, ExpectedResult = null)]
//        [TestCase(byte.MinValue - 1, ExpectedResult = null)]
//        [TestCase(byte.MaxValue - 1.5f, ExpectedResult = byte.MaxValue - 2)]
//        [TestCase(byte.MinValue + 1.5f, ExpectedResult = byte.MinValue + 1)]
//        public byte? ToNullableByte_FromSingle(float input)
//        {
//            return SafeNullableConvert.ToByte(input);
//        }

//        [TestCase((double)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((double)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = null)]
//        [TestCase(double.MinValue, ExpectedResult = null)]
//        [TestCase(byte.MaxValue + 1, ExpectedResult = null)]
//        [TestCase(byte.MinValue - 1, ExpectedResult = null)]
//        [TestCase(byte.MaxValue - 1.5d, ExpectedResult = byte.MaxValue - 2)]
//        [TestCase(byte.MinValue + 1.5d, ExpectedResult = byte.MinValue + 1)]
//        public byte? ToNullableByte_FromDouble(double input)
//        {
//            return SafeNullableConvert.ToByte(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToNullableByteData")]
//        public byte? ToNullableByte_FromDecimal(decimal input)
//        {
//            return SafeNullableConvert.ToByte(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public byte? ToNullableByte_FromBoolean(bool input)
//        {
//            return SafeNullableConvert.ToByte(input);
//        }

//        [TestCase((char)byte.MaxValue, ExpectedResult = byte.MaxValue)]
//        [TestCase((char)byte.MinValue, ExpectedResult = byte.MinValue)]
//        [TestCase(char.MaxValue, ExpectedResult = null)]
//        [TestCase((char)(byte.MaxValue + 1), ExpectedResult = null)]
//        public byte? ToNullableByte_FromChar(char input)
//        {
//            return SafeNullableConvert.ToByte(input);
//        }

//        #endregion

//        #region? ToNullableSByte

//        [TestCase("127", ExpectedResult = sbyte.MaxValue)]
//        [TestCase("-128", ExpectedResult = sbyte.MinValue)]
//        [TestCase("128", ExpectedResult = null)]
//        [TestCase("-129", ExpectedResult = null)]
//        [TestCase(":", ExpectedResult = null)]
//        [TestCase("/", ExpectedResult = null)]
//        public sbyte? ToNullableSByte_FromString(string input)
//        {
//            return SafeNullableConvert.ToSByte(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = null)]
//        [TestCase(byte.MinValue, ExpectedResult = 0)]
//        [TestCase((byte)127, ExpectedResult = sbyte.MaxValue)]
//        public sbyte? ToNullableSByte_FromByte(byte input)
//        {
//            return SafeNullableConvert.ToSByte(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = sbyte.MinValue)]
//        public sbyte? ToNullableSByte_FromSByte(sbyte input)
//        {
//            return SafeNullableConvert.ToSByte(input);
//        }

//        [TestCase((short)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase((short)sbyte.MinValue, ExpectedResult = sbyte.MinValue)]
//        [TestCase(short.MaxValue, ExpectedResult = null)]
//        [TestCase(short.MinValue, ExpectedResult = null)]
//        [TestCase(sbyte.MaxValue + 1, ExpectedResult = null)]
//        [TestCase(sbyte.MinValue - 1, ExpectedResult = null)]
//        public sbyte? ToNullableSByte_FromInt16(short input)
//        {
//            return SafeNullableConvert.ToSByte(input);
//        }

//        [TestCase((ushort)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase(ushort.MaxValue, ExpectedResult = null)]
//        [TestCase((ushort)(sbyte.MaxValue + 1), ExpectedResult = null)]
//        public sbyte? ToNullableSByte_FromUInt16(ushort input)
//        {
//            return SafeNullableConvert.ToSByte(input);
//        }

//        [TestCase((int)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase((int)sbyte.MinValue, ExpectedResult = sbyte.MinValue)]
//        [TestCase(int.MaxValue, ExpectedResult = null)]
//        [TestCase(int.MinValue, ExpectedResult = null)]
//        [TestCase(sbyte.MaxValue + 1, ExpectedResult = null)]
//        [TestCase(sbyte.MinValue - 1, ExpectedResult = null)]
//        public sbyte? ToNullableSByte_FromInt32(int input)
//        {
//            return SafeNullableConvert.ToSByte(input);
//        }

//        [TestCase((uint)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase(uint.MaxValue, ExpectedResult = null)]
//        [TestCase((uint)(sbyte.MaxValue + 1), ExpectedResult = null)]
//        public sbyte? ToNullableSByte_FromUInt32(uint input)
//        {
//            return SafeNullableConvert.ToSByte(input);
//        }

//        [TestCase((long)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase((long)sbyte.MinValue, ExpectedResult = sbyte.MinValue)]
//        [TestCase(long.MaxValue, ExpectedResult = null)]
//        [TestCase(long.MinValue, ExpectedResult = null)]
//        [TestCase(sbyte.MaxValue + 1, ExpectedResult = null)]
//        [TestCase(sbyte.MinValue - 1, ExpectedResult = null)]
//        public sbyte? ToNullableSByte_FromInt64(long input)
//        {
//            return SafeNullableConvert.ToSByte(input);
//        }

//        [TestCase((ulong)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase(ulong.MaxValue, ExpectedResult = null)]
//        [TestCase((ulong)(sbyte.MaxValue + 1), ExpectedResult = null)]
//        public sbyte? ToNullableSByte_FromUInt64(ulong input)
//        {
//            return SafeNullableConvert.ToSByte(input);
//        }

//        [TestCase((float)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase((float)sbyte.MinValue, ExpectedResult = sbyte.MinValue)]
//        [TestCase(float.MaxValue, ExpectedResult = null)]
//        [TestCase(float.MinValue, ExpectedResult = null)]
//        [TestCase(sbyte.MaxValue + 1, ExpectedResult = null)]
//        [TestCase(sbyte.MinValue - 1, ExpectedResult = null)]
//        [TestCase(sbyte.MaxValue - 1.5f, ExpectedResult = sbyte.MaxValue - 2)]
//        [TestCase(sbyte.MinValue + 1.5f, ExpectedResult = sbyte.MinValue + 2)]
//        public sbyte? ToNullableSByte_FromSingle(float input)
//        {
//            return SafeNullableConvert.ToSByte(input);
//        }

//        [TestCase((double)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase((double)sbyte.MinValue, ExpectedResult = sbyte.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = null)]
//        [TestCase(double.MinValue, ExpectedResult = null)]
//        [TestCase(sbyte.MaxValue + 1, ExpectedResult = null)]
//        [TestCase(sbyte.MinValue - 1, ExpectedResult = null)]
//        [TestCase(sbyte.MaxValue - 1.5d, ExpectedResult = sbyte.MaxValue - 2)]
//        [TestCase(sbyte.MinValue + 1.5d, ExpectedResult = sbyte.MinValue + 2)]
//        public sbyte? ToNullableSByte_FromDouble(double input)
//        {
//            return SafeNullableConvert.ToSByte(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToNullableSByteData")]
//        public sbyte? ToNullableSByte_FromDecimal(decimal input)
//        {
//            return SafeNullableConvert.ToSByte(input);
//        }

//        [TestCase(false, ExpectedResult = (sbyte)0)]
//        [TestCase(true, ExpectedResult = (sbyte)1)]
//        public sbyte? ToNullableSByte_FromBoolean(bool input)
//        {
//            return SafeNullableConvert.ToSByte(input);
//        }

//        [TestCase((char)sbyte.MaxValue, ExpectedResult = sbyte.MaxValue)]
//        [TestCase(char.MaxValue, ExpectedResult = null)]
//        [TestCase((char)(sbyte.MaxValue + 1), ExpectedResult = null)]
//        public sbyte? ToNullableSByte_FromChar(char input)
//        {
//            return SafeNullableConvert.ToSByte(input);
//        }

//        #endregion

//        #region? ToNullableInt16

//        [TestCase("32767", ExpectedResult = short.MaxValue)]
//        [TestCase("-32768", ExpectedResult = short.MinValue)]
//        [TestCase("32768", ExpectedResult = null)]
//        [TestCase("-32769", ExpectedResult = null)]
//        [TestCase(":", ExpectedResult = null)]
//        [TestCase("/", ExpectedResult = null)]
//        public short? ToNullableInt16_FromString(string input)
//        {
//            return SafeNullableConvert.ToInt16(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (short)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (short)byte.MinValue)]
//        public short? ToNullableInt16_FromByte(byte input)
//        {
//            return SafeNullableConvert.ToInt16(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (short)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = (short)sbyte.MinValue)]
//        public short? ToNullableInt16_FromSByte(sbyte input)
//        {
//            return SafeNullableConvert.ToInt16(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = short.MinValue)]
//        public short? ToNullableInt16_FromInt16(short input)
//        {
//            return SafeNullableConvert.ToInt16(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = null)]
//        [TestCase(ushort.MinValue, ExpectedResult = 0)]
//        [TestCase((ushort)short.MaxValue, ExpectedResult = short.MaxValue)]
//        public short? ToNullableInt16_FromUInt16(ushort input)
//        {
//            return SafeNullableConvert.ToInt16(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = null)]
//        [TestCase(int.MinValue, ExpectedResult = null)]
//        [TestCase((int)short.MaxValue, ExpectedResult = short.MaxValue)]
//        [TestCase((int)short.MinValue, ExpectedResult = short.MinValue)]
//        public short? ToNullableInt16_FromInt32(int input)
//        {
//            return SafeNullableConvert.ToInt16(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = null)]
//        [TestCase(uint.MinValue, ExpectedResult = 0)]
//        [TestCase((uint)short.MaxValue, ExpectedResult = short.MaxValue)]
//        public short? ToNullableInt16_FromUInt32(uint input)
//        {
//            return SafeNullableConvert.ToInt16(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = null)]
//        [TestCase(long.MinValue, ExpectedResult = null)]
//        [TestCase((long)short.MaxValue, ExpectedResult = short.MaxValue)]
//        public short? ToNullableInt16_FromInt64(long input)
//        {
//            return SafeNullableConvert.ToInt16(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = null)]
//        [TestCase(ulong.MinValue, ExpectedResult = 0)]
//        [TestCase((ulong)short.MaxValue, ExpectedResult = short.MaxValue)]
//        public short? ToNullableInt16_FromUInt64(ulong input)
//        {
//            return SafeNullableConvert.ToInt16(input);
//        }

//        [TestCase((float)short.MaxValue, ExpectedResult = short.MaxValue)]
//        [TestCase((float)short.MinValue, ExpectedResult = short.MinValue)]
//        [TestCase(float.MaxValue, ExpectedResult = null)]
//        [TestCase(float.MinValue, ExpectedResult = null)]
//        [TestCase(short.MaxValue + 1, ExpectedResult = null)]
//        [TestCase(short.MinValue - 1, ExpectedResult = null)]
//        [TestCase(short.MaxValue - 1.5f, ExpectedResult = short.MaxValue - 2)]
//        [TestCase(short.MinValue + 1.5f, ExpectedResult = short.MinValue + 2)]
//        public short? ToNullableInt16_FromSingle(float input)
//        {
//            return SafeNullableConvert.ToInt16(input);
//        }

//        [TestCase((double)short.MaxValue, ExpectedResult = short.MaxValue)]
//        [TestCase((double)short.MinValue, ExpectedResult = short.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = null)]
//        [TestCase(double.MinValue, ExpectedResult = null)]
//        [TestCase(short.MaxValue + 1, ExpectedResult = null)]
//        [TestCase(short.MinValue - 1, ExpectedResult = null)]
//        [TestCase(short.MaxValue - 1.5d, ExpectedResult = short.MaxValue - 2)]
//        [TestCase(short.MinValue + 1.5d, ExpectedResult = short.MinValue + 2)]
//        public short? ToNullableInt16_FromDouble(double input)
//        {
//            return SafeNullableConvert.ToInt16(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToNullableInt16Data")]
//        public short? ToNullableInt16_FromDecimal(decimal input)
//        {
//            return SafeNullableConvert.ToInt16(input);
//        }

//        [TestCase(false, ExpectedResult = (short)0)]
//        [TestCase(true, ExpectedResult = (short)1)]
//        public short? ToNullableInt16_FromBoolean(bool input)
//        {
//            return SafeNullableConvert.ToInt16(input);
//        }

//        [TestCase((char)short.MaxValue, ExpectedResult = short.MaxValue)]
//        [TestCase(char.MaxValue, ExpectedResult = null)]
//        [TestCase((char)(short.MaxValue + 1), ExpectedResult = null)]
//        public short? ToNullableInt16_FromChar(char input)
//        {
//            return SafeNullableConvert.ToInt16(input);
//        }

//        #endregion

//        #region? ToNullableUInt16

//        [TestCase("65535", ExpectedResult = ushort.MaxValue)]
//        [TestCase("0", ExpectedResult = ushort.MinValue)]
//        [TestCase("65536", ExpectedResult = null)]
//        [TestCase("-1", ExpectedResult = null)]
//        [TestCase(":", ExpectedResult = null)]
//        [TestCase("/", ExpectedResult = null)]
//        public ushort? ToNullableUInt16_FromString(string input)
//        {
//            return SafeNullableConvert.ToUInt16(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (ushort)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (ushort)byte.MinValue)]
//        public ushort? ToNullableUInt16_FromByte(byte input)
//        {
//            return SafeNullableConvert.ToUInt16(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (ushort)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = null)]
//        public ushort? ToNullableUInt16_FromSByte(sbyte input)
//        {
//            return SafeNullableConvert.ToUInt16(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (ushort)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = null)]
//        [TestCase((short)ushort.MinValue, ExpectedResult = ushort.MinValue)]
//        public ushort? ToNullableUInt16_FromInt16(short input)
//        {
//            return SafeNullableConvert.ToUInt16(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = ushort.MinValue)]
//        public ushort? ToNullableUInt16_FromUInt16(ushort input)
//        {
//            return SafeNullableConvert.ToUInt16(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = null)]
//        [TestCase(int.MinValue, ExpectedResult = null)]
//        [TestCase((int)ushort.MaxValue, ExpectedResult = ushort.MaxValue)]
//        [TestCase((int)ushort.MinValue, ExpectedResult = ushort.MinValue)]
//        public ushort? ToNullableUInt16_FromInt32(int input)
//        {
//            return SafeNullableConvert.ToUInt16(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = null)]
//        [TestCase(uint.MinValue, ExpectedResult = 0)]
//        [TestCase((uint)ushort.MaxValue, ExpectedResult = ushort.MaxValue)]
//        public ushort? ToNullableUInt16_FromUInt32(uint input)
//        {
//            return SafeNullableConvert.ToUInt16(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = null)]
//        [TestCase(long.MinValue, ExpectedResult = null)]
//        [TestCase((long)ushort.MaxValue, ExpectedResult = ushort.MaxValue)]
//        public ushort? ToNullableUInt16_FromInt64(long input)
//        {
//            return SafeNullableConvert.ToUInt16(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = null)]
//        [TestCase(ulong.MinValue, ExpectedResult = 0)]
//        [TestCase((ulong)ushort.MaxValue, ExpectedResult = ushort.MaxValue)]
//        public ushort? ToNullableUInt16_FromUInt64(ulong input)
//        {
//            return SafeNullableConvert.ToUInt16(input);
//        }

//        [TestCase((float)ushort.MaxValue, ExpectedResult = ushort.MaxValue)]
//        [TestCase((float)ushort.MinValue, ExpectedResult = ushort.MinValue)]
//        [TestCase(float.MaxValue, ExpectedResult = null)]
//        [TestCase(float.MinValue, ExpectedResult = null)]
//        [TestCase(ushort.MaxValue + 1, ExpectedResult = null)]
//        [TestCase(ushort.MinValue - 1, ExpectedResult = null)]
//        [TestCase(ushort.MaxValue - 1.5f, ExpectedResult = ushort.MaxValue - 2)]
//        [TestCase(ushort.MinValue + 1.5f, ExpectedResult = ushort.MinValue + 1)]
//        public ushort? ToNullableUInt16_FromSingle(float input)
//        {
//            return SafeNullableConvert.ToUInt16(input);
//        }

//        [TestCase((double)ushort.MaxValue, ExpectedResult = ushort.MaxValue)]
//        [TestCase((double)ushort.MinValue, ExpectedResult = ushort.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = null)]
//        [TestCase(double.MinValue, ExpectedResult = null)]
//        [TestCase(ushort.MaxValue + 1, ExpectedResult = null)]
//        [TestCase(ushort.MinValue - 1, ExpectedResult = null)]
//        [TestCase(ushort.MaxValue - 1.5d, ExpectedResult = ushort.MaxValue - 2)]
//        [TestCase(ushort.MinValue + 1.5d, ExpectedResult = ushort.MinValue + 1)]
//        public ushort? ToNullableUInt16_FromDouble(double input)
//        {
//            return SafeNullableConvert.ToUInt16(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToNullableUInt16Data")]
//        public ushort? ToNullableUInt16_FromDecimal(decimal input)
//        {
//            return SafeNullableConvert.ToUInt16(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public ushort? ToNullableUInt16_FromBoolean(bool input)
//        {
//            return SafeNullableConvert.ToUInt16(input);
//        }

//        [TestCase((char)ushort.MinValue, ExpectedResult = ushort.MinValue)]
//        [TestCase(char.MaxValue, ExpectedResult = ushort.MaxValue)]
//        public ushort? ToNullableUInt16_FromChar(char input)
//        {
//            return SafeNullableConvert.ToUInt16(input);
//        }

//        #endregion

//        #region? ToNullableInt32

//        [TestCaseSource(typeof(TestData), "StringToNullableIntData")]
//        public int? ToNullableInt32_FromString(string input)
//        {
//            return SafeNullableConvert.ToInt32(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (short)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (short)byte.MinValue)]
//        public int? ToNullableInt32_FromByte(byte input)
//        {
//            return SafeNullableConvert.ToInt32(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (int)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = (int)sbyte.MinValue)]
//        public int? ToNullableInt32_FromSByte(sbyte input)
//        {
//            return SafeNullableConvert.ToInt32(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (int)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = (int)short.MinValue)]
//        public int? ToNullableInt32_FromInt16(short input)
//        {
//            return SafeNullableConvert.ToInt32(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = (int)ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = 0)]
//        public int? ToNullableInt32_FromUInt16(ushort input)
//        {
//            return SafeNullableConvert.ToInt32(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = int.MaxValue)]
//        [TestCase(int.MinValue, ExpectedResult = int.MinValue)]
//        public int? ToNullableInt32_FromInt32(int input)
//        {
//            return SafeNullableConvert.ToInt32(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = null)]
//        [TestCase(uint.MinValue, ExpectedResult = 0)]
//        [TestCase((uint)int.MaxValue, ExpectedResult = int.MaxValue)]
//        public int? ToNullableInt32_FromUInt32(uint input)
//        {
//            return SafeNullableConvert.ToInt32(input);
//        }

//        [TestCase(0, ExpectedResult = 0)]
//        [TestCase((long)int.MinValue, ExpectedResult = int.MinValue)]
//        [TestCase((long)int.MaxValue, ExpectedResult = int.MaxValue)]
//        [TestCase(long.MinValue, ExpectedResult = null)]
//        [TestCase(long.MaxValue, ExpectedResult = null)]
//        public int? ToNullableInt32_FromInt64(long input)
//        {
//            return SafeNullableConvert.ToInt32(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = null)]
//        [TestCase(ulong.MinValue, ExpectedResult = 0)]
//        [TestCase((ulong)int.MaxValue, ExpectedResult = int.MaxValue)]
//        public int? ToNullableInt32_FromUInt64(ulong input)
//        {
//            return SafeNullableConvert.ToInt32(input);
//        }

//        [TestCase(float.MaxValue, ExpectedResult = null)]
//        [TestCase(float.MinValue, ExpectedResult = null)]
//        [TestCase((float)int.MinValue, ExpectedResult = int.MinValue)]
//        [TestCase(int.MaxValue - 7483647f, ExpectedResult = int.MaxValue - 7483647)]
//        public int? ToNullableInt32_FromSingle(float input)
//        {
//            return SafeNullableConvert.ToInt32(input);
//        }

//        [TestCase((double)int.MaxValue, ExpectedResult = int.MaxValue)]
//        [TestCase((double)int.MinValue, ExpectedResult = int.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = null)]
//        [TestCase(double.MinValue, ExpectedResult = null)]
//        [TestCase(int.MaxValue - 1.5d, ExpectedResult = int.MaxValue - 2)]
//        [TestCase(int.MinValue + 1.5d, ExpectedResult = int.MinValue + 2)]
//        public int? ToNullableInt32_FromDouble(double input)
//        {
//            return SafeNullableConvert.ToInt32(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToNullableInt32Data")]
//        public int? ToNullableInt32_FromDecimal(decimal input)
//        {
//            return SafeNullableConvert.ToInt32(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public int? ToNullableInt32_FromBoolean(bool input)
//        {
//            return SafeNullableConvert.ToInt32(input);
//        }

//        [TestCase(char.MaxValue, ExpectedResult = (int)char.MaxValue)]
//        [TestCase(char.MinValue, ExpectedResult = 0)]
//        public int? ToNullableInt32_FromChar(char input)
//        {
//            return SafeNullableConvert.ToInt32(input);
//        }

//        #endregion

//        #region? ToNullableUInt32

//        [TestCase("4294967295", ExpectedResult = uint.MaxValue)]
//        [TestCase("0", ExpectedResult = uint.MinValue)]
//        [TestCase("4294967296", ExpectedResult = null)]
//        [TestCase("-1", ExpectedResult = null)]
//        [TestCase(":", ExpectedResult = null)]
//        [TestCase("/", ExpectedResult = null)]
//        public uint? ToNullableUInt32_FromString(string input)
//        {
//            return SafeNullableConvert.ToUInt32(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (uint)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (uint)byte.MinValue)]
//        public uint? ToNullableUInt32_FromByte(byte input)
//        {
//            return SafeNullableConvert.ToUInt32(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (uint)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = null)]
//        public uint? ToNullableUInt32_FromSByte(sbyte input)
//        {
//            return SafeNullableConvert.ToUInt32(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (uint)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = null)]
//        [TestCase((short)uint.MinValue, ExpectedResult = uint.MinValue)]
//        public uint? ToNullableUInt32_FromInt16(short input)
//        {
//            return SafeNullableConvert.ToUInt32(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = (uint)ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = (uint)ushort.MinValue)]
//        public uint? ToNullableUInt32_FromUInt16(ushort input)
//        {
//            return SafeNullableConvert.ToUInt32(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = (uint)int.MaxValue)]
//        [TestCase(int.MinValue, ExpectedResult = null)]
//        [TestCase((int)uint.MinValue, ExpectedResult = uint.MinValue)]
//        public uint? ToNullableUInt32_FromInt32(int input)
//        {
//            return SafeNullableConvert.ToUInt32(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = uint.MaxValue)]
//        [TestCase(uint.MinValue, ExpectedResult = uint.MinValue)]
//        public uint? ToNullableUInt32_FromUInt32(uint input)
//        {
//            return SafeNullableConvert.ToUInt32(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = null)]
//        [TestCase(long.MinValue, ExpectedResult = null)]
//        [TestCase((long)uint.MaxValue, ExpectedResult = uint.MaxValue)]
//        public uint? ToNullableUInt32_FromInt64(long input)
//        {
//            return SafeNullableConvert.ToUInt32(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = null)]
//        [TestCase(ulong.MinValue, ExpectedResult = 0)]
//        [TestCase((ulong)uint.MaxValue, ExpectedResult = uint.MaxValue)]
//        public uint? ToNullableUInt32_FromUInt64(ulong input)
//        {
//            return SafeNullableConvert.ToUInt32(input);
//        }

//        [TestCase((float)uint.MinValue, ExpectedResult = uint.MinValue)]
//        [TestCase(float.MaxValue, ExpectedResult = null)]
//        [TestCase(float.MinValue, ExpectedResult = null)]
//        [TestCase(uint.MinValue - 1f, ExpectedResult = null)]
//        [TestCase(uint.MinValue + 1.5f, ExpectedResult = uint.MinValue + 1)]
//        public uint? ToNullableUInt32_FromSingle(float input)
//        {
//            return SafeNullableConvert.ToUInt32(input);
//        }

//        [TestCase((double)uint.MaxValue, ExpectedResult = uint.MaxValue)]
//        [TestCase((double)uint.MinValue, ExpectedResult = uint.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = null)]
//        [TestCase(double.MinValue, ExpectedResult = null)]
//        [TestCase(uint.MaxValue + 1d, ExpectedResult = null)]
//        [TestCase(uint.MinValue - 1d, ExpectedResult = null)]
//        [TestCase(uint.MaxValue - 1.5d, ExpectedResult = uint.MaxValue - 2)]
//        [TestCase(uint.MinValue + 1.5d, ExpectedResult = uint.MinValue + 1)]
//        public uint? ToNullableUInt32_FromDouble(double input)
//        {
//            return SafeNullableConvert.ToUInt32(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToNullableUInt32Data")]
//        public uint? ToNullableUInt32_FromDecimal(decimal input)
//        {
//            return SafeNullableConvert.ToUInt32(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public uint? ToNullableUInt32_FromBoolean(bool input)
//        {
//            return SafeNullableConvert.ToUInt32(input);
//        }

//        [TestCase((char)uint.MinValue, ExpectedResult = uint.MinValue)]
//        [TestCase(char.MaxValue, ExpectedResult = (uint)char.MaxValue)]
//        public uint? ToNullableUInt32_FromChar(char input)
//        {
//            return SafeNullableConvert.ToUInt32(input);
//        }

//        #endregion

//        #region? ToNullableInt64

//        [TestCase(null, ExpectedResult = null)]
//        [TestCase("", ExpectedResult = null)]
//        [TestCase("9223372036854775807", ExpectedResult = long.MaxValue)]
//        [TestCase("-9223372036854775808", ExpectedResult = long.MinValue)]
//        //[TestCase("9223372036854775808", ExpectedResult = null)]
//        //[TestCase("-9223372036854775809", ExpectedResult = null)]
//        [TestCase(":", ExpectedResult = null)]
//        [TestCase("/", ExpectedResult = null)]
//        public long? ToNullableInt64_FromString(string input)
//        {
//            return SafeNullableConvert.ToInt64(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (long)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (long)byte.MinValue)]
//        public long? ToNullableInt64_FromByte(byte input)
//        {
//            return SafeNullableConvert.ToInt64(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (long)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = (long)sbyte.MinValue)]
//        public long? ToNullableInt64_FromSByte(sbyte input)
//        {
//            return SafeNullableConvert.ToInt64(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (long)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = (long)short.MinValue)]
//        public long? ToNullableInt64_FromInt16(short input)
//        {
//            return SafeNullableConvert.ToInt64(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = (long)ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = 0)]
//        public long? ToNullableInt64_FromUInt16(ushort input)
//        {
//            return SafeNullableConvert.ToInt64(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = (long)int.MaxValue)]
//        [TestCase(int.MinValue, ExpectedResult = (long)int.MinValue)]
//        public long? ToNullableInt64_FromInt32(int input)
//        {
//            return SafeNullableConvert.ToInt64(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = (long)uint.MaxValue)]
//        [TestCase(uint.MinValue, ExpectedResult = (long)uint.MinValue)]
//        [TestCase((uint)int.MaxValue, ExpectedResult = int.MaxValue)]
//        public long? ToNullableInt64_FromUInt32(uint input)
//        {
//            return SafeNullableConvert.ToInt64(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = long.MaxValue)]
//        [TestCase(long.MinValue, ExpectedResult = long.MinValue)]
//        public long? ToNullableInt64_FromInt64(long input)
//        {
//            return SafeNullableConvert.ToInt64(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = null)]
//        [TestCase(ulong.MinValue, ExpectedResult = 0)]
//        [TestCase((ulong)long.MaxValue, ExpectedResult = long.MaxValue)]
//        public long? ToNullableInt64_FromUInt64(ulong input)
//        {
//            return SafeNullableConvert.ToInt64(input);
//        }

//        [TestCase(float.MaxValue, ExpectedResult = null)]
//        [TestCase(float.MinValue, ExpectedResult = null)]
//        [TestCase((float)int.MinValue, ExpectedResult = int.MinValue)]
//        public long? ToNullableInt64_FromSingle(float input)
//        {
//            return SafeNullableConvert.ToInt64(input);
//        }

//        [TestCase((double)int.MaxValue, ExpectedResult = int.MaxValue)]
//        [TestCase((double)int.MinValue, ExpectedResult = int.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = null)]
//        [TestCase(double.MinValue, ExpectedResult = null)]
//        [TestCase(int.MaxValue - 1.5d, ExpectedResult = int.MaxValue - 2)]
//        [TestCase(int.MinValue + 1.5d, ExpectedResult = int.MinValue + 2)]
//        public long? ToNullableInt64_FromDouble(double input)
//        {
//            return SafeNullableConvert.ToInt64(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToNullableInt64Data")]
//        public long? ToNullableInt64_FromDecimal(decimal input)
//        {
//            return SafeNullableConvert.ToInt64(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public long? ToNullableInt64_FromBoolean(bool input)
//        {
//            return SafeNullableConvert.ToInt64(input);
//        }

//        [TestCase(char.MaxValue, ExpectedResult = (long)char.MaxValue)]
//        [TestCase(char.MinValue, ExpectedResult = 0)]
//        public long? ToNullableInt64_FromChar(char input)
//        {
//            return SafeNullableConvert.ToInt64(input);
//        }

//        #endregion

//        #region? ToNullableUInt64

//        [TestCase("18446744073709551615", ExpectedResult = ulong.MaxValue)]
//        [TestCase("0", ExpectedResult = ulong.MinValue)]
//        [TestCase("18446744073709551616", ExpectedResult = null)]
//        //[TestCase("18446744073709551617", ExpectedResult = null)]
//        [TestCase("-1", ExpectedResult = null)]
//        [TestCase(":", ExpectedResult = null)]
//        [TestCase("/", ExpectedResult = null)]
//        public ulong? ToNullableUInt64_FromString(string input)
//        {
//            return SafeNullableConvert.ToUInt64(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (ulong)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (ulong)byte.MinValue)]
//        public ulong? ToNullableUInt64_FromByte(byte input)
//        {
//            return SafeNullableConvert.ToUInt64(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (ulong)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = null)]
//        public ulong? ToNullableUInt64_FromSByte(sbyte input)
//        {
//            return SafeNullableConvert.ToUInt64(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (ulong)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = null)]
//        [TestCase((short)uint.MinValue, ExpectedResult = ulong.MinValue)]
//        public ulong? ToNullableUInt64_FromInt16(short input)
//        {
//            return SafeNullableConvert.ToUInt64(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = (ulong)ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = (ulong)ushort.MinValue)]
//        public ulong? ToNullableUInt64_FromUInt16(ushort input)
//        {
//            return SafeNullableConvert.ToUInt64(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = (ulong)int.MaxValue)]
//        [TestCase(int.MinValue, ExpectedResult = null)]
//        [TestCase((int)uint.MinValue, ExpectedResult = ulong.MinValue)]
//        public ulong? ToNullableUInt64_FromInt32(int input)
//        {
//            return SafeNullableConvert.ToUInt64(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = (ulong)uint.MaxValue)]
//        [TestCase(uint.MinValue, ExpectedResult = (ulong)uint.MinValue)]
//        public ulong? ToNullableUInt64_FromUInt32(uint input)
//        {
//            return SafeNullableConvert.ToUInt64(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = (ulong)long.MaxValue)]
//        [TestCase(long.MinValue, ExpectedResult = null)]
//        public ulong? ToNullableUInt64_FromInt64(long input)
//        {
//            return SafeNullableConvert.ToUInt64(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = ulong.MaxValue)]
//        [TestCase(ulong.MinValue, ExpectedResult = ulong.MinValue)]
//        public ulong? ToNullableUInt64_FromUInt64(ulong input)
//        {
//            return SafeNullableConvert.ToUInt64(input);
//        }

//        [TestCase((float)ulong.MinValue, ExpectedResult = ulong.MinValue)]
//        [TestCase(float.MaxValue, ExpectedResult = null)]
//        [TestCase(float.MinValue, ExpectedResult = null)]
//        [TestCase(ulong.MinValue - 1f, ExpectedResult = null)]
//        [TestCase(ulong.MinValue + 1.5f, ExpectedResult = ulong.MinValue + 1)]
//        public ulong? ToNullableUInt64_FromSingle(float input)
//        {
//            return SafeNullableConvert.ToUInt64(input);
//        }

//        //[TestCase((double)ulong.MaxValue, ExpectedResult = ulong.MaxValue)]
//        [TestCase((double)ulong.MinValue, ExpectedResult = ulong.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = null)]
//        [TestCase(double.MinValue, ExpectedResult = null)]
//        //[TestCase(ulong.MaxValue + 1d, ExpectedResult = null)]
//        [TestCase(ulong.MinValue - 1d, ExpectedResult = null)]
//        //[TestCase(ulong.MaxValue - 1.5d, ExpectedResult = ulong.MaxValue - 2)]
//        [TestCase(ulong.MinValue + 1.5d, ExpectedResult = ulong.MinValue + 1)]
//        public ulong? ToNullableUInt64_FromDouble(double input)
//        {
//            return SafeNullableConvert.ToUInt64(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToNullableUInt64Data")]
//        public ulong? ToNullableUInt64_FromDecimal(decimal input)
//        {
//            return SafeNullableConvert.ToUInt64(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public ulong? ToNullableUInt64_FromBoolean(bool input)
//        {
//            return SafeNullableConvert.ToUInt64(input);
//        }

//        [TestCase((char)ulong.MinValue, ExpectedResult = ulong.MinValue)]
//        [TestCase(char.MaxValue, ExpectedResult = (ulong)char.MaxValue)]
//        public ulong? ToNullableUInt64_FromChar(char input)
//        {
//            return SafeNullableConvert.ToUInt64(input);
//        }

//        #endregion

//        #region? ToNullableSingle

//        [TestCase(null, ExpectedResult = null)]
//        [TestCase("", ExpectedResult = null)]
//        [TestCase("1", ExpectedResult = 1f)]
//        public float? ToNullableSingle_FromString(string input)
//        {
//            return SafeNullableConvert.ToSingle(input);
//        }

//        [TestCase(null, ExpectedResult = null)]
//        [TestCase("", ExpectedResult = null)]
//        [TestCase("1.7976931348623157", ExpectedResult = 1.7976931348623157f)]
//        [TestCase("-1.7976931348623157", ExpectedResult = -1.7976931348623157f)]
//        [TestCase(":", ExpectedResult = null)]
//        [TestCase("/", ExpectedResult = null)]
//        public float? ToNullableSingle_FromStringWithFormat(string input)
//        {
//            return SafeNullableConvert.ToSingle(input, this.numberFormatProvider);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (float)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (float)byte.MinValue)]
//        public float? ToNullableSingle_FromByte(byte input)
//        {
//            return SafeNullableConvert.ToSingle(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (float)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = (float)sbyte.MinValue)]
//        public float? ToNullableSingle_FromSByte(sbyte input)
//        {
//            return SafeNullableConvert.ToSingle(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (float)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = (float)short.MinValue)]
//        public float? ToNullableSingle_FromInt16(short input)
//        {
//            return SafeNullableConvert.ToSingle(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = (float)ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = 0)]
//        public float? ToNullableSingle_FromUInt16(ushort input)
//        {
//            return SafeNullableConvert.ToSingle(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = (float)int.MaxValue)]
//        [TestCase(int.MinValue, ExpectedResult = (float)int.MinValue)]
//        public float? ToNullableSingle_FromInt32(int input)
//        {
//            return SafeNullableConvert.ToSingle(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = (float)uint.MaxValue)]
//        [TestCase(uint.MinValue, ExpectedResult = (float)uint.MinValue)]
//        public float? ToNullableSingle_FromUInt32(uint input)
//        {
//            return SafeNullableConvert.ToSingle(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = (float)long.MaxValue)]
//        [TestCase(long.MinValue, ExpectedResult = (float)long.MinValue)]
//        public float? ToNullableSingle_FromInt64(long input)
//        {
//            return SafeNullableConvert.ToSingle(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = (float)ulong.MaxValue)]
//        [TestCase(ulong.MinValue, ExpectedResult = 0f)]
//        public float? ToNullableSingle_FromUInt64(ulong input)
//        {
//            return SafeNullableConvert.ToSingle(input);
//        }

//        [TestCase(float.MaxValue, ExpectedResult = float.MaxValue)]
//        [TestCase(float.MinValue, ExpectedResult = float.MinValue)]
//        public float? ToNullableSingle_FromSingle(float input)
//        {
//            return SafeNullableConvert.ToSingle(input);
//        }

//        [TestCase((double)float.MaxValue, ExpectedResult = float.MaxValue)]
//        [TestCase((double)float.MinValue, ExpectedResult = float.MinValue)]
//        //[TestCase(((double)float.MaxValue) + 1d, ExpectedResult = nullf)]
//        //[TestCase(((double)float.MinValue) - 1d, ExpectedResult = nullf)]
//        [TestCase(double.MaxValue, ExpectedResult = null)]
//        [TestCase(double.MinValue, ExpectedResult = null)]
//        public float? ToNullableSingle_FromDouble(double input)
//        {
//            return SafeNullableConvert.ToSingle(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToNullableSingleData")]
//        public float? ToNullableSingle_FromDecimal(decimal input)
//        {
//            return SafeNullableConvert.ToSingle(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public float? ToNullableSingle_FromBoolean(bool input)
//        {
//            return SafeNullableConvert.ToSingle(input);
//        }

//        [TestCase(char.MaxValue, ExpectedResult = (float)char.MaxValue)]
//        [TestCase(char.MinValue, ExpectedResult = 0f)]
//        public float? ToNullableSingle_FromChar(char input)
//        {
//            return SafeNullableConvert.ToSingle(input);
//        }

//        #endregion

//        #region? ToNullableDouble

//        [TestCase(null, ExpectedResult = null)]
//        [TestCase("", ExpectedResult = null)]
//        [TestCase("1", ExpectedResult = 1d)]
//        public double? ToNullableDouble_FromString(string input)
//        {
//            return SafeNullableConvert.ToDouble(input);
//        }

//        [TestCase(null, ExpectedResult = null)]
//        [TestCase("", ExpectedResult = null)]
//        [TestCase("1.7976931348623157", ExpectedResult = 1.7976931348623157)]
//        [TestCase("-1.7976931348623157", ExpectedResult = -1.7976931348623157)]
//        [TestCase(":", ExpectedResult = null)]
//        [TestCase("/", ExpectedResult = null)]
//        public double? ToNullableDouble_FromStringWithFormat(string input)
//        {
//            return SafeNullableConvert.ToDouble(input, CultureInfo.InvariantCulture);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (double)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (double)byte.MinValue)]
//        public double? ToNullableDouble_FromByte(byte input)
//        {
//            return SafeNullableConvert.ToDouble(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (double)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = (double)sbyte.MinValue)]
//        public double? ToNullableDouble_FromSByte(sbyte input)
//        {
//            return SafeNullableConvert.ToDouble(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (double)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = (double)short.MinValue)]
//        public double? ToNullableDouble_FromInt16(short input)
//        {
//            return SafeNullableConvert.ToDouble(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = (double)ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = 0d)]
//        public double? ToNullableDouble_FromUInt16(ushort input)
//        {
//            return SafeNullableConvert.ToDouble(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = (double)int.MaxValue)]
//        [TestCase(int.MinValue, ExpectedResult = (double)int.MinValue)]
//        public double? ToNullableDouble_FromInt32(int input)
//        {
//            return SafeNullableConvert.ToDouble(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = (double)uint.MaxValue)]
//        [TestCase(uint.MinValue, ExpectedResult = (double)uint.MinValue)]
//        public double? ToNullableDouble_FromUInt32(uint input)
//        {
//            return SafeNullableConvert.ToDouble(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = (double)long.MaxValue)]
//        [TestCase(long.MinValue, ExpectedResult = (double)long.MinValue)]
//        public double? ToNullableDouble_FromInt64(long input)
//        {
//            return SafeNullableConvert.ToDouble(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = (double)ulong.MaxValue)]
//        [TestCase(ulong.MinValue, ExpectedResult = 0d)]
//        public double? ToNullableDouble_FromUInt64(ulong input)
//        {
//            return SafeNullableConvert.ToDouble(input);
//        }

//        [TestCase(float.MaxValue, ExpectedResult = (double)float.MaxValue)]
//        [TestCase(float.MinValue, ExpectedResult = (double)float.MinValue)]
//        public double? ToNullableDouble_FromSingle(float input)
//        {
//            return SafeNullableConvert.ToDouble(input);
//        }

//        [TestCase(double.MaxValue, ExpectedResult = double.MaxValue)]
//        [TestCase(double.MinValue, ExpectedResult = double.MinValue)]
//        public double? ToNullableDouble_FromDouble(double input)
//        {
//            return SafeNullableConvert.ToDouble(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToNullableDoubleData")]
//        public double? ToNullableDouble_FromDecimal(decimal input)
//        {
//            return SafeNullableConvert.ToDouble(input);
//        }

//        [TestCase(false, ExpectedResult = 0)]
//        [TestCase(true, ExpectedResult = 1)]
//        public double? ToNullableDouble_FromBoolean(bool input)
//        {
//            return SafeNullableConvert.ToDouble(input);
//        }

//        [TestCase(char.MaxValue, ExpectedResult = (double)char.MaxValue)]
//        [TestCase(char.MinValue, ExpectedResult = (double)0)]
//        public double? ToNullableDouble_FromChar(char input)
//        {
//            return SafeNullableConvert.ToDouble(input);
//        }

//        #endregion

//        #region? ToNullableDecimal

//        [TestCaseSource(typeof(TestData), "StringToNullableDecimalData")]
//        public decimal? ToNullableDecimal_FromString(string input)
//        {
//            return SafeNullableConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "StringToNullableDecimalWithFormatData")]
//        public decimal? ToNullableDecimal_FromStringWithFormat(string input)
//        {
//            return SafeNullableConvert.ToDecimal(input, this.numberFormatProvider);
//        }

//        [TestCaseSource(typeof(TestData), "ByteToDecimalData")]
//        public decimal? ToNullableDecimal_FromByte(byte input)
//        {
//            return SafeNullableConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "SByteToDecimalData")]
//        public decimal? ToNullableDecimal_FromSByte(sbyte input)
//        {
//            return SafeNullableConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "Int16ToDecimalData")]
//        public decimal? ToNullableDecimal_FromInt16(short input)
//        {
//            return SafeNullableConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "UInt16ToDecimalData")]
//        public decimal? ToNullableDecimal_FromUInt16(ushort input)
//        {
//            return SafeNullableConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "Int32ToDecimalData")]
//        public decimal? ToNullableDecimal_FromInt32(int input)
//        {
//            return SafeNullableConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "UInt32ToDecimalData")]
//        public decimal? ToNullableDecimal_FromUInt32(uint input)
//        {
//            return SafeNullableConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "Int64ToDecimalData")]
//        public decimal? ToNullableDecimal_FromInt64(long input)
//        {
//            return SafeNullableConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "UInt64ToDecimalData")]
//        public decimal? ToNullableDecimal_FromUInt64(ulong input)
//        {
//            return SafeNullableConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "SingleToNullableDecimalData")]
//        public decimal? ToNullableDecimal_FromSingle(float input)
//        {
//            return SafeNullableConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "DoubleToNullableDecimalData")]
//        public decimal? ToNullableDecimal_FromDouble(double input)
//        {
//            return SafeNullableConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToNullableDecimalData")]
//        public decimal? ToNullableDecimal_FromDecimal(decimal input)
//        {
//            return SafeNullableConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "BooleanToDecimalData")]
//        public decimal? ToNullableDecimal_FromBoolean(bool input)
//        {
//            return SafeNullableConvert.ToDecimal(input);
//        }

//        [TestCaseSource(typeof(TestData), "CharToDecimalData")]
//        public decimal? ToNullableDecimal_FromChar(char input)
//        {
//            return SafeNullableConvert.ToDecimal(input);
//        }

//        #endregion

//        #region? ToNullableBoolean

//        [TestCase("True", ExpectedResult = true)]
//        [TestCase("False", ExpectedResult = false)]
//        [TestCase("true", ExpectedResult = true)]
//        [TestCase("false", ExpectedResult = false)]
//        [TestCase("Foo", ExpectedResult = false)]
//        public bool? ToNullableBoolean_FromString(string input)
//        {
//            return SafeNullableConvert.ToBoolean(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = true)]
//        [TestCase(byte.MinValue, ExpectedResult = false)]
//        public bool? ToNullableBoolean_FromByte(byte input)
//        {
//            return SafeNullableConvert.ToBoolean(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = true)]
//        [TestCase(sbyte.MinValue, ExpectedResult = true)]
//        [TestCase((sbyte)0, ExpectedResult = false)]
//        public bool? ToNullableBoolean_FromSByte(sbyte input)
//        {
//            return SafeNullableConvert.ToBoolean(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = true)]
//        [TestCase(short.MinValue, ExpectedResult = true)]
//        [TestCase((short)0, ExpectedResult = false)]
//        public bool? ToNullableBoolean_FromInt16(short input)
//        {
//            return SafeNullableConvert.ToBoolean(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = true)]
//        [TestCase(ushort.MinValue, ExpectedResult = false)]
//        public bool? ToNullableBoolean_FromUInt16(ushort input)
//        {
//            return SafeNullableConvert.ToBoolean(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = true)]
//        [TestCase(int.MinValue, ExpectedResult = true)]
//        [TestCase(0, ExpectedResult = false)]
//        public bool? ToNullableBoolean_FromInt32(int input)
//        {
//            return SafeNullableConvert.ToBoolean(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = true)]
//        [TestCase(uint.MinValue, ExpectedResult = false)]
//        public bool? ToNullableBoolean_FromUInt32(uint input)
//        {
//            return SafeNullableConvert.ToBoolean(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = true)]
//        [TestCase(long.MinValue, ExpectedResult = true)]
//        [TestCase(0L, ExpectedResult = false)]
//        public bool? ToNullableBoolean_FromInt64(long input)
//        {
//            return SafeNullableConvert.ToBoolean(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = true)]
//        [TestCase(ulong.MinValue, ExpectedResult = false)]
//        public bool? ToNullableBoolean_FromUInt64(ulong input)
//        {
//            return SafeNullableConvert.ToBoolean(input);
//        }

//        [TestCase(float.MaxValue, ExpectedResult = true)]
//        [TestCase(float.MinValue, ExpectedResult = true)]
//        [TestCase(0f, ExpectedResult = false)]
//        public bool? ToNullableBoolean_FromSingle(float input)
//        {
//            return SafeNullableConvert.ToBoolean(input);
//        }

//        [TestCase(double.MaxValue, ExpectedResult = true)]
//        [TestCase(double.MinValue, ExpectedResult = true)]
//        [TestCase(0d, ExpectedResult = false)]
//        public bool? ToNullableBoolean_FromDouble(double input)
//        {
//            return SafeNullableConvert.ToBoolean(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToNullableBooleanData")]
//        public bool? ToNullableBoolean_FromDecimal(decimal input)
//        {
//            return SafeNullableConvert.ToBoolean(input);
//        }

//        [TestCase(false, ExpectedResult = false)]
//        [TestCase(true, ExpectedResult = true)]
//        public bool? ToNullableBoolean_FromBoolean(bool input)
//        {
//            return SafeNullableConvert.ToBoolean(input);
//        }

//        [TestCase(char.MaxValue, ExpectedResult = true)]
//        [TestCase(char.MinValue, ExpectedResult = false)]
//        public bool? ToNullableBoolean_FromChar(char input)
//        {
//            return SafeNullableConvert.ToBoolean(input);
//        }

//        #endregion

//        #region? ToNullableChar

//        [TestCase("", ExpectedResult = null)]
//        [TestCase(null, ExpectedResult = null)]
//        [TestCase("A", ExpectedResult = 'A')]
//        [TestCase("0", ExpectedResult = '0')]
//        [TestCase("ab", ExpectedResult = null)]
//        public char? ToNullableChar_FromString(string input)
//        {
//            return SafeNullableConvert.ToChar(input);
//        }

//        [TestCase(byte.MaxValue, ExpectedResult = (char)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (char)byte.MinValue)]
//        public char? ToNullableChar_FromByte(byte input)
//        {
//            return SafeNullableConvert.ToChar(input);
//        }

//        [TestCase(sbyte.MaxValue, ExpectedResult = (char)sbyte.MaxValue)]
//        [TestCase(sbyte.MinValue, ExpectedResult = null)]
//        public char? ToNullableChar_FromSByte(sbyte input)
//        {
//            return SafeNullableConvert.ToChar(input);
//        }

//        [TestCase(short.MaxValue, ExpectedResult = (char)short.MaxValue)]
//        [TestCase(short.MinValue, ExpectedResult = null)]
//        [TestCase((short)char.MinValue, ExpectedResult = char.MinValue)]
//        public char? ToNullableChar_FromInt16(short input)
//        {
//            return SafeNullableConvert.ToChar(input);
//        }

//        [TestCase(ushort.MaxValue, ExpectedResult = (char)ushort.MaxValue)]
//        [TestCase(ushort.MinValue, ExpectedResult = (char)ushort.MinValue)]
//        public char? ToNullableChar_FromUInt16(ushort input)
//        {
//            return SafeNullableConvert.ToChar(input);
//        }

//        [TestCase(int.MaxValue, ExpectedResult = null)]
//        [TestCase(int.MinValue, ExpectedResult = null)]
//        [TestCase((int)char.MaxValue, ExpectedResult = char.MaxValue)]
//        [TestCase((int)char.MinValue, ExpectedResult = char.MinValue)]
//        public char? ToNullableChar_FromInt32(int input)
//        {
//            return SafeNullableConvert.ToChar(input);
//        }

//        [TestCase(uint.MaxValue, ExpectedResult = null)]
//        [TestCase(uint.MinValue, ExpectedResult = (char)0)]
//        [TestCase((uint)char.MaxValue, ExpectedResult = char.MaxValue)]
//        public char? ToNullableChar_FromUInt32(uint input)
//        {
//            return SafeNullableConvert.ToChar(input);
//        }

//        [TestCase(long.MaxValue, ExpectedResult = null)]
//        [TestCase(long.MinValue, ExpectedResult = null)]
//        [TestCase((long)char.MaxValue, ExpectedResult = char.MaxValue)]
//        public char? ToNullableChar_FromInt64(long input)
//        {
//            return SafeNullableConvert.ToChar(input);
//        }

//        [TestCase(ulong.MaxValue, ExpectedResult = null)]
//        [TestCase(ulong.MinValue, ExpectedResult = (char)0)]
//        [TestCase((ulong)char.MaxValue, ExpectedResult = char.MaxValue)]
//        public char? ToNullableChar_FromUInt64(ulong input)
//        {
//            return SafeNullableConvert.ToChar(input);
//        }

//        [TestCase((float)char.MinValue, ExpectedResult = char.MinValue)]
//        [TestCase(float.MaxValue, ExpectedResult = null)]
//        [TestCase(float.MinValue, ExpectedResult = null)]
//        [TestCase((float)char.MinValue - 1f, ExpectedResult = null)]
//        [TestCase(65f, ExpectedResult = 'A')]
//        [TestCase(65.5f, ExpectedResult = 'A')]
//        public char? ToNullableChar_FromSingle(float input)
//        {
//            return SafeNullableConvert.ToChar(input);
//        }

//        [TestCase((double)char.MaxValue, ExpectedResult = char.MaxValue)]
//        [TestCase((double)char.MinValue, ExpectedResult = char.MinValue)]
//        [TestCase(double.MaxValue, ExpectedResult = null)]
//        [TestCase(double.MinValue, ExpectedResult = null)]
//        [TestCase((double)char.MaxValue + 1d, ExpectedResult = null)]
//        [TestCase((double)char.MinValue - 1d, ExpectedResult = null)]
//        [TestCase((double)char.MaxValue - 1.5d, ExpectedResult = (char)(char.MaxValue - 2))]
//        [TestCase(65d, ExpectedResult = 'A')]
//        [TestCase(65.5d, ExpectedResult = 'A')]
//        public char? ToNullableChar_FromDouble(double input)
//        {
//            return SafeNullableConvert.ToChar(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToNullableCharData")]
//        public char? ToNullableChar_FromDecimal(decimal input)
//        {
//            return SafeNullableConvert.ToChar(input);
//        }

//        [TestCase(false, ExpectedResult = '0')]
//        [TestCase(true, ExpectedResult = '1')]
//        public char? ToNullableChar_FromBoolean(bool input)
//        {
//            return SafeNullableConvert.ToChar(input);
//        }

//        [TestCase(char.MaxValue, ExpectedResult = char.MaxValue)]
//        [TestCase(char.MinValue, ExpectedResult = char.MinValue)]
//        public char? ToNullableChar_FromChar(char input)
//        {
//            return SafeNullableConvert.ToChar(input);
//        }

//        #endregion

//        #region? ToNullableGuid

//        [TestCaseSource(typeof(TestData), "StringToGuidData")]
//        public Guid? ToNullableGuid_FromString(string input)
//        {
//            return SafeNullableConvert.ToGuid(input);
//        }

//        #endregion

//        #region? ToNullableDateTime

//        [TestCaseSource(typeof(TestData), "StringToDateTimeData")]
//        public DateTime? ToNullableDateTime_FromString(string input)
//        {
//            return SafeNullableConvert.ToDateTime(input);
//        }

//        [TestCaseSource(typeof(TestData), "StringToDateTimeData")]
//        public DateTime? ToNullableDateTime_FromStringWithFormat(string input)
//        {
//            return SafeNullableConvert.ToDateTime(input, CultureInfo.InvariantCulture);
//        }

//        [TestCaseSource(typeof(TestData), "SqlDateTimeToDateTimeData")]
//        public DateTime? ToNullableDateTime_FromSqlDateTime(SqlDateTime input)
//        {
//            return SafeNullableConvert.ToDateTime(input);
//        }

//        #endregion

//        #region? ToNullableSqlDateTime

//        [TestCaseSource(typeof(TestData), "DateTimeToSqlDateTimeData")]
//        public SqlDateTime? ToNullableSqlDateTime_FromSqlDateTime(DateTime input)
//        {
//            return SafeNullableConvert.ToSqlDateTime(input);
//        }

//        #endregion
//    }
//}
