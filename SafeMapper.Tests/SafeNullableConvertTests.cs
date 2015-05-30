namespace SafeMapper.Tests
{
    using System;
    using System.Data.SqlTypes;
    using System.Globalization;

    using NUnit.Framework;

    [TestFixture]
    public class SafeNullableConvertTests
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

        #region? ToNullableByte

        [TestCase("255", Result = byte.MaxValue)]
        [TestCase("0", Result = byte.MinValue)]
        [TestCase("256", Result = null)]
        [TestCase("-1", Result = null)]
        [TestCase(":", Result = null)]
        [TestCase("/", Result = null)]
        public byte? ToNullableByte_FromString(string input)
        {
            return SafeNullableConvert.ToByte(input);
        }

        [TestCase(byte.MaxValue, Result = byte.MaxValue)]
        [TestCase(byte.MinValue, Result = byte.MinValue)]
        public byte? ToNullableByte_FromByte(byte input)
        {
            return SafeNullableConvert.ToByte(input);
        }

        [TestCase(sbyte.MaxValue, Result = (byte)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = null)]
        public byte? ToNullableByte_FromSByte(sbyte input)
        {
            return SafeNullableConvert.ToByte(input);
        }

        [TestCase((short)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((short)byte.MinValue, Result = byte.MinValue)]
        [TestCase(short.MaxValue, Result = null)]
        [TestCase(short.MinValue, Result = null)]
        [TestCase(byte.MaxValue + 1, Result = null)]
        [TestCase(byte.MinValue - 1, Result = null)]
        public byte? ToNullableByte_FromInt16(short input)
        {
            return SafeNullableConvert.ToByte(input);
        }

        [TestCase((ushort)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((ushort)byte.MinValue, Result = byte.MinValue)]
        [TestCase(ushort.MaxValue, Result = null)]
        [TestCase((ushort)(byte.MaxValue + 1), Result = null)]
        public byte? ToNullableByte_FromUInt16(ushort input)
        {
            return SafeNullableConvert.ToByte(input);
        }

        [TestCase((int)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((int)byte.MinValue, Result = byte.MinValue)]
        [TestCase(int.MaxValue, Result = null)]
        [TestCase(int.MinValue, Result = null)]
        [TestCase(byte.MaxValue + 1, Result = null)]
        [TestCase(byte.MinValue - 1, Result = null)]
        public byte? ToNullableByte_FromInt32(int input)
        {
            return SafeNullableConvert.ToByte(input);
        }

        [TestCase((uint)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((uint)byte.MinValue, Result = byte.MinValue)]
        [TestCase(uint.MaxValue, Result = null)]
        [TestCase((uint)(byte.MaxValue + 1), Result = null)]
        public byte? ToNullableByte_FromUInt32(uint input)
        {
            return SafeNullableConvert.ToByte(input);
        }

        [TestCase((long)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((long)byte.MinValue, Result = byte.MinValue)]
        [TestCase(long.MaxValue, Result = null)]
        [TestCase(long.MinValue, Result = null)]
        [TestCase(byte.MaxValue + 1, Result = null)]
        [TestCase(byte.MinValue - 1, Result = null)]
        public byte? ToNullableByte_FromInt64(long input)
        {
            return SafeNullableConvert.ToByte(input);
        }

        [TestCase((ulong)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((ulong)byte.MinValue, Result = byte.MinValue)]
        [TestCase(ulong.MaxValue, Result = null)]
        [TestCase((ulong)(byte.MaxValue + 1), Result = null)]
        public byte? ToNullableByte_FromUInt64(ulong input)
        {
            return SafeNullableConvert.ToByte(input);
        }

        [TestCase((float)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((float)byte.MinValue, Result = byte.MinValue)]
        [TestCase(float.MaxValue, Result = null)]
        [TestCase(float.MinValue, Result = null)]
        [TestCase(byte.MaxValue + 1, Result = null)]
        [TestCase(byte.MinValue - 1, Result = null)]
        [TestCase(byte.MaxValue - 1.5f, Result = byte.MaxValue - 2)]
        [TestCase(byte.MinValue + 1.5f, Result = byte.MinValue + 1)]
        public byte? ToNullableByte_FromSingle(float input)
        {
            return SafeNullableConvert.ToByte(input);
        }

        [TestCase((double)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((double)byte.MinValue, Result = byte.MinValue)]
        [TestCase(double.MaxValue, Result = null)]
        [TestCase(double.MinValue, Result = null)]
        [TestCase(byte.MaxValue + 1, Result = null)]
        [TestCase(byte.MinValue - 1, Result = null)]
        [TestCase(byte.MaxValue - 1.5d, Result = byte.MaxValue - 2)]
        [TestCase(byte.MinValue + 1.5d, Result = byte.MinValue + 1)]
        public byte? ToNullableByte_FromDouble(double input)
        {
            return SafeNullableConvert.ToByte(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToNullableByteData")]
        public byte? ToNullableByte_FromDecimal(decimal input)
        {
            return SafeNullableConvert.ToByte(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public byte? ToNullableByte_FromBoolean(bool input)
        {
            return SafeNullableConvert.ToByte(input);
        }

        [TestCase((char)byte.MaxValue, Result = byte.MaxValue)]
        [TestCase((char)byte.MinValue, Result = byte.MinValue)]
        [TestCase(char.MaxValue, Result = null)]
        [TestCase((char)(byte.MaxValue + 1), Result = null)]
        public byte? ToNullableByte_FromChar(char input)
        {
            return SafeNullableConvert.ToByte(input);
        }

        #endregion

        #region? ToNullableSByte

        [TestCase("127", Result = sbyte.MaxValue)]
        [TestCase("-128", Result = sbyte.MinValue)]
        [TestCase("128", Result = null)]
        [TestCase("-129", Result = null)]
        [TestCase(":", Result = null)]
        [TestCase("/", Result = null)]
        public sbyte? ToNullableSByte_FromString(string input)
        {
            return SafeNullableConvert.ToSByte(input);
        }

        [TestCase(byte.MaxValue, Result = null)]
        [TestCase(byte.MinValue, Result = 0)]
        [TestCase((byte)127, Result = sbyte.MaxValue)]
        public sbyte? ToNullableSByte_FromByte(byte input)
        {
            return SafeNullableConvert.ToSByte(input);
        }

        [TestCase(sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = sbyte.MinValue)]
        public sbyte? ToNullableSByte_FromSByte(sbyte input)
        {
            return SafeNullableConvert.ToSByte(input);
        }

        [TestCase((short)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase((short)sbyte.MinValue, Result = sbyte.MinValue)]
        [TestCase(short.MaxValue, Result = null)]
        [TestCase(short.MinValue, Result = null)]
        [TestCase(sbyte.MaxValue + 1, Result = null)]
        [TestCase(sbyte.MinValue - 1, Result = null)]
        public sbyte? ToNullableSByte_FromInt16(short input)
        {
            return SafeNullableConvert.ToSByte(input);
        }

        [TestCase((ushort)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase(ushort.MaxValue, Result = null)]
        [TestCase((ushort)(sbyte.MaxValue + 1), Result = null)]
        public sbyte? ToNullableSByte_FromUInt16(ushort input)
        {
            return SafeNullableConvert.ToSByte(input);
        }

        [TestCase((int)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase((int)sbyte.MinValue, Result = sbyte.MinValue)]
        [TestCase(int.MaxValue, Result = null)]
        [TestCase(int.MinValue, Result = null)]
        [TestCase(sbyte.MaxValue + 1, Result = null)]
        [TestCase(sbyte.MinValue - 1, Result = null)]
        public sbyte? ToNullableSByte_FromInt32(int input)
        {
            return SafeNullableConvert.ToSByte(input);
        }

        [TestCase((uint)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase(uint.MaxValue, Result = null)]
        [TestCase((uint)(sbyte.MaxValue + 1), Result = null)]
        public sbyte? ToNullableSByte_FromUInt32(uint input)
        {
            return SafeNullableConvert.ToSByte(input);
        }

        [TestCase((long)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase((long)sbyte.MinValue, Result = sbyte.MinValue)]
        [TestCase(long.MaxValue, Result = null)]
        [TestCase(long.MinValue, Result = null)]
        [TestCase(sbyte.MaxValue + 1, Result = null)]
        [TestCase(sbyte.MinValue - 1, Result = null)]
        public sbyte? ToNullableSByte_FromInt64(long input)
        {
            return SafeNullableConvert.ToSByte(input);
        }

        [TestCase((ulong)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase(ulong.MaxValue, Result = null)]
        [TestCase((ulong)(sbyte.MaxValue + 1), Result = null)]
        public sbyte? ToNullableSByte_FromUInt64(ulong input)
        {
            return SafeNullableConvert.ToSByte(input);
        }

        [TestCase((float)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase((float)sbyte.MinValue, Result = sbyte.MinValue)]
        [TestCase(float.MaxValue, Result = null)]
        [TestCase(float.MinValue, Result = null)]
        [TestCase(sbyte.MaxValue + 1, Result = null)]
        [TestCase(sbyte.MinValue - 1, Result = null)]
        [TestCase(sbyte.MaxValue - 1.5f, Result = sbyte.MaxValue - 2)]
        [TestCase(sbyte.MinValue + 1.5f, Result = sbyte.MinValue + 2)]
        public sbyte? ToNullableSByte_FromSingle(float input)
        {
            return SafeNullableConvert.ToSByte(input);
        }

        [TestCase((double)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase((double)sbyte.MinValue, Result = sbyte.MinValue)]
        [TestCase(double.MaxValue, Result = null)]
        [TestCase(double.MinValue, Result = null)]
        [TestCase(sbyte.MaxValue + 1, Result = null)]
        [TestCase(sbyte.MinValue - 1, Result = null)]
        [TestCase(sbyte.MaxValue - 1.5d, Result = sbyte.MaxValue - 2)]
        [TestCase(sbyte.MinValue + 1.5d, Result = sbyte.MinValue + 2)]
        public sbyte? ToNullableSByte_FromDouble(double input)
        {
            return SafeNullableConvert.ToSByte(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToNullableSByteData")]
        public sbyte? ToNullableSByte_FromDecimal(decimal input)
        {
            return SafeNullableConvert.ToSByte(input);
        }

        [TestCase(false, Result = (sbyte)0)]
        [TestCase(true, Result = (sbyte)1)]
        public sbyte? ToNullableSByte_FromBoolean(bool input)
        {
            return SafeNullableConvert.ToSByte(input);
        }

        [TestCase((char)sbyte.MaxValue, Result = sbyte.MaxValue)]
        [TestCase(char.MaxValue, Result = null)]
        [TestCase((char)(sbyte.MaxValue + 1), Result = null)]
        public sbyte? ToNullableSByte_FromChar(char input)
        {
            return SafeNullableConvert.ToSByte(input);
        }

        #endregion

        #region? ToNullableInt16

        [TestCase("32767", Result = short.MaxValue)]
        [TestCase("-32768", Result = short.MinValue)]
        [TestCase("32768", Result = null)]
        [TestCase("-32769", Result = null)]
        [TestCase(":", Result = null)]
        [TestCase("/", Result = null)]
        public short? ToNullableInt16_FromString(string input)
        {
            return SafeNullableConvert.ToInt16(input);
        }

        [TestCase(byte.MaxValue, Result = (short)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (short)byte.MinValue)]
        public short? ToNullableInt16_FromByte(byte input)
        {
            return SafeNullableConvert.ToInt16(input);
        }

        [TestCase(sbyte.MaxValue, Result = (short)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (short)sbyte.MinValue)]
        public short? ToNullableInt16_FromSByte(sbyte input)
        {
            return SafeNullableConvert.ToInt16(input);
        }

        [TestCase(short.MaxValue, Result = short.MaxValue)]
        [TestCase(short.MinValue, Result = short.MinValue)]
        public short? ToNullableInt16_FromInt16(short input)
        {
            return SafeNullableConvert.ToInt16(input);
        }

        [TestCase(ushort.MaxValue, Result = null)]
        [TestCase(ushort.MinValue, Result = 0)]
        [TestCase((ushort)short.MaxValue, Result = short.MaxValue)]
        public short? ToNullableInt16_FromUInt16(ushort input)
        {
            return SafeNullableConvert.ToInt16(input);
        }

        [TestCase(int.MaxValue, Result = null)]
        [TestCase(int.MinValue, Result = null)]
        [TestCase((int)short.MaxValue, Result = short.MaxValue)]
        [TestCase((int)short.MinValue, Result = short.MinValue)]
        public short? ToNullableInt16_FromInt32(int input)
        {
            return SafeNullableConvert.ToInt16(input);
        }

        [TestCase(uint.MaxValue, Result = null)]
        [TestCase(uint.MinValue, Result = 0)]
        [TestCase((uint)short.MaxValue, Result = short.MaxValue)]
        public short? ToNullableInt16_FromUInt32(uint input)
        {
            return SafeNullableConvert.ToInt16(input);
        }

        [TestCase(long.MaxValue, Result = null)]
        [TestCase(long.MinValue, Result = null)]
        [TestCase((long)short.MaxValue, Result = short.MaxValue)]
        public short? ToNullableInt16_FromInt64(long input)
        {
            return SafeNullableConvert.ToInt16(input);
        }

        [TestCase(ulong.MaxValue, Result = null)]
        [TestCase(ulong.MinValue, Result = 0)]
        [TestCase((ulong)short.MaxValue, Result = short.MaxValue)]
        public short? ToNullableInt16_FromUInt64(ulong input)
        {
            return SafeNullableConvert.ToInt16(input);
        }

        [TestCase((float)short.MaxValue, Result = short.MaxValue)]
        [TestCase((float)short.MinValue, Result = short.MinValue)]
        [TestCase(float.MaxValue, Result = null)]
        [TestCase(float.MinValue, Result = null)]
        [TestCase(short.MaxValue + 1, Result = null)]
        [TestCase(short.MinValue - 1, Result = null)]
        [TestCase(short.MaxValue - 1.5f, Result = short.MaxValue - 2)]
        [TestCase(short.MinValue + 1.5f, Result = short.MinValue + 2)]
        public short? ToNullableInt16_FromSingle(float input)
        {
            return SafeNullableConvert.ToInt16(input);
        }

        [TestCase((double)short.MaxValue, Result = short.MaxValue)]
        [TestCase((double)short.MinValue, Result = short.MinValue)]
        [TestCase(double.MaxValue, Result = null)]
        [TestCase(double.MinValue, Result = null)]
        [TestCase(short.MaxValue + 1, Result = null)]
        [TestCase(short.MinValue - 1, Result = null)]
        [TestCase(short.MaxValue - 1.5d, Result = short.MaxValue - 2)]
        [TestCase(short.MinValue + 1.5d, Result = short.MinValue + 2)]
        public short? ToNullableInt16_FromDouble(double input)
        {
            return SafeNullableConvert.ToInt16(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToNullableInt16Data")]
        public short? ToNullableInt16_FromDecimal(decimal input)
        {
            return SafeNullableConvert.ToInt16(input);
        }

        [TestCase(false, Result = (short)0)]
        [TestCase(true, Result = (short)1)]
        public short? ToNullableInt16_FromBoolean(bool input)
        {
            return SafeNullableConvert.ToInt16(input);
        }

        [TestCase((char)short.MaxValue, Result = short.MaxValue)]
        [TestCase(char.MaxValue, Result = null)]
        [TestCase((char)(short.MaxValue + 1), Result = null)]
        public short? ToNullableInt16_FromChar(char input)
        {
            return SafeNullableConvert.ToInt16(input);
        }

        #endregion

        #region? ToNullableUInt16

        [TestCase("65535", Result = ushort.MaxValue)]
        [TestCase("0", Result = ushort.MinValue)]
        [TestCase("65536", Result = null)]
        [TestCase("-1", Result = null)]
        [TestCase(":", Result = null)]
        [TestCase("/", Result = null)]
        public ushort? ToNullableUInt16_FromString(string input)
        {
            return SafeNullableConvert.ToUInt16(input);
        }

        [TestCase(byte.MaxValue, Result = (ushort)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (ushort)byte.MinValue)]
        public ushort? ToNullableUInt16_FromByte(byte input)
        {
            return SafeNullableConvert.ToUInt16(input);
        }

        [TestCase(sbyte.MaxValue, Result = (ushort)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = null)]
        public ushort? ToNullableUInt16_FromSByte(sbyte input)
        {
            return SafeNullableConvert.ToUInt16(input);
        }

        [TestCase(short.MaxValue, Result = (ushort)short.MaxValue)]
        [TestCase(short.MinValue, Result = null)]
        [TestCase((short)ushort.MinValue, Result = ushort.MinValue)]
        public ushort? ToNullableUInt16_FromInt16(short input)
        {
            return SafeNullableConvert.ToUInt16(input);
        }

        [TestCase(ushort.MaxValue, Result = ushort.MaxValue)]
        [TestCase(ushort.MinValue, Result = ushort.MinValue)]
        public ushort? ToNullableUInt16_FromUInt16(ushort input)
        {
            return SafeNullableConvert.ToUInt16(input);
        }

        [TestCase(int.MaxValue, Result = null)]
        [TestCase(int.MinValue, Result = null)]
        [TestCase((int)ushort.MaxValue, Result = ushort.MaxValue)]
        [TestCase((int)ushort.MinValue, Result = ushort.MinValue)]
        public ushort? ToNullableUInt16_FromInt32(int input)
        {
            return SafeNullableConvert.ToUInt16(input);
        }

        [TestCase(uint.MaxValue, Result = null)]
        [TestCase(uint.MinValue, Result = 0)]
        [TestCase((uint)ushort.MaxValue, Result = ushort.MaxValue)]
        public ushort? ToNullableUInt16_FromUInt32(uint input)
        {
            return SafeNullableConvert.ToUInt16(input);
        }

        [TestCase(long.MaxValue, Result = null)]
        [TestCase(long.MinValue, Result = null)]
        [TestCase((long)ushort.MaxValue, Result = ushort.MaxValue)]
        public ushort? ToNullableUInt16_FromInt64(long input)
        {
            return SafeNullableConvert.ToUInt16(input);
        }

        [TestCase(ulong.MaxValue, Result = null)]
        [TestCase(ulong.MinValue, Result = 0)]
        [TestCase((ulong)ushort.MaxValue, Result = ushort.MaxValue)]
        public ushort? ToNullableUInt16_FromUInt64(ulong input)
        {
            return SafeNullableConvert.ToUInt16(input);
        }

        [TestCase((float)ushort.MaxValue, Result = ushort.MaxValue)]
        [TestCase((float)ushort.MinValue, Result = ushort.MinValue)]
        [TestCase(float.MaxValue, Result = null)]
        [TestCase(float.MinValue, Result = null)]
        [TestCase(ushort.MaxValue + 1, Result = null)]
        [TestCase(ushort.MinValue - 1, Result = null)]
        [TestCase(ushort.MaxValue - 1.5f, Result = ushort.MaxValue - 2)]
        [TestCase(ushort.MinValue + 1.5f, Result = ushort.MinValue + 1)]
        public ushort? ToNullableUInt16_FromSingle(float input)
        {
            return SafeNullableConvert.ToUInt16(input);
        }

        [TestCase((double)ushort.MaxValue, Result = ushort.MaxValue)]
        [TestCase((double)ushort.MinValue, Result = ushort.MinValue)]
        [TestCase(double.MaxValue, Result = null)]
        [TestCase(double.MinValue, Result = null)]
        [TestCase(ushort.MaxValue + 1, Result = null)]
        [TestCase(ushort.MinValue - 1, Result = null)]
        [TestCase(ushort.MaxValue - 1.5d, Result = ushort.MaxValue - 2)]
        [TestCase(ushort.MinValue + 1.5d, Result = ushort.MinValue + 1)]
        public ushort? ToNullableUInt16_FromDouble(double input)
        {
            return SafeNullableConvert.ToUInt16(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToNullableUInt16Data")]
        public ushort? ToNullableUInt16_FromDecimal(decimal input)
        {
            return SafeNullableConvert.ToUInt16(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public ushort? ToNullableUInt16_FromBoolean(bool input)
        {
            return SafeNullableConvert.ToUInt16(input);
        }

        [TestCase((char)ushort.MinValue, Result = ushort.MinValue)]
        [TestCase(char.MaxValue, Result = ushort.MaxValue)]
        public ushort? ToNullableUInt16_FromChar(char input)
        {
            return SafeNullableConvert.ToUInt16(input);
        }

        #endregion

        #region? ToNullableInt32

        [TestCaseSource(typeof(TestData), "StringToNullableIntData")]
        public int? ToNullableInt32_FromString(string input)
        {
            return SafeNullableConvert.ToInt32(input);
        }

        [TestCase(byte.MaxValue, Result = (short)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (short)byte.MinValue)]
        public int? ToNullableInt32_FromByte(byte input)
        {
            return SafeNullableConvert.ToInt32(input);
        }

        [TestCase(sbyte.MaxValue, Result = (int)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (int)sbyte.MinValue)]
        public int? ToNullableInt32_FromSByte(sbyte input)
        {
            return SafeNullableConvert.ToInt32(input);
        }

        [TestCase(short.MaxValue, Result = (int)short.MaxValue)]
        [TestCase(short.MinValue, Result = (int)short.MinValue)]
        public int? ToNullableInt32_FromInt16(short input)
        {
            return SafeNullableConvert.ToInt32(input);
        }

        [TestCase(ushort.MaxValue, Result = (int)ushort.MaxValue)]
        [TestCase(ushort.MinValue, Result = 0)]
        public int? ToNullableInt32_FromUInt16(ushort input)
        {
            return SafeNullableConvert.ToInt32(input);
        }

        [TestCase(int.MaxValue, Result = int.MaxValue)]
        [TestCase(int.MinValue, Result = int.MinValue)]
        public int? ToNullableInt32_FromInt32(int input)
        {
            return SafeNullableConvert.ToInt32(input);
        }

        [TestCase(uint.MaxValue, Result = null)]
        [TestCase(uint.MinValue, Result = 0)]
        [TestCase((uint)int.MaxValue, Result = int.MaxValue)]
        public int? ToNullableInt32_FromUInt32(uint input)
        {
            return SafeNullableConvert.ToInt32(input);
        }

        [TestCase(0, Result = 0)]
        [TestCase((long)int.MinValue, Result = int.MinValue)]
        [TestCase((long)int.MaxValue, Result = int.MaxValue)]
        [TestCase(long.MinValue, Result = null)]
        [TestCase(long.MaxValue, Result = null)]
        public int? ToNullableInt32_FromInt64(long input)
        {
            return SafeNullableConvert.ToInt32(input);
        }

        [TestCase(ulong.MaxValue, Result = null)]
        [TestCase(ulong.MinValue, Result = 0)]
        [TestCase((ulong)int.MaxValue, Result = int.MaxValue)]
        public int? ToNullableInt32_FromUInt64(ulong input)
        {
            return SafeNullableConvert.ToInt32(input);
        }

        [TestCase(float.MaxValue, Result = null)]
        [TestCase(float.MinValue, Result = null)]
        [TestCase((float)int.MinValue, Result = int.MinValue)]
        [TestCase(int.MaxValue - 7483647f, Result = int.MaxValue - 7483647)]
        public int? ToNullableInt32_FromSingle(float input)
        {
            return SafeNullableConvert.ToInt32(input);
        }

        [TestCase((double)int.MaxValue, Result = int.MaxValue)]
        [TestCase((double)int.MinValue, Result = int.MinValue)]
        [TestCase(double.MaxValue, Result = null)]
        [TestCase(double.MinValue, Result = null)]
        [TestCase(int.MaxValue - 1.5d, Result = int.MaxValue - 2)]
        [TestCase(int.MinValue + 1.5d, Result = int.MinValue + 2)]
        public int? ToNullableInt32_FromDouble(double input)
        {
            return SafeNullableConvert.ToInt32(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToNullableInt32Data")]
        public int? ToNullableInt32_FromDecimal(decimal input)
        {
            return SafeNullableConvert.ToInt32(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public int? ToNullableInt32_FromBoolean(bool input)
        {
            return SafeNullableConvert.ToInt32(input);
        }

        [TestCase(char.MaxValue, Result = (int)char.MaxValue)]
        [TestCase(char.MinValue, Result = 0)]
        public int? ToNullableInt32_FromChar(char input)
        {
            return SafeNullableConvert.ToInt32(input);
        }

        #endregion

        #region? ToNullableUInt32

        [TestCase("4294967295", Result = uint.MaxValue)]
        [TestCase("0", Result = uint.MinValue)]
        [TestCase("4294967296", Result = null)]
        [TestCase("-1", Result = null)]
        [TestCase(":", Result = null)]
        [TestCase("/", Result = null)]
        public uint? ToNullableUInt32_FromString(string input)
        {
            return SafeNullableConvert.ToUInt32(input);
        }

        [TestCase(byte.MaxValue, Result = (uint)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (uint)byte.MinValue)]
        public uint? ToNullableUInt32_FromByte(byte input)
        {
            return SafeNullableConvert.ToUInt32(input);
        }

        [TestCase(sbyte.MaxValue, Result = (uint)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = null)]
        public uint? ToNullableUInt32_FromSByte(sbyte input)
        {
            return SafeNullableConvert.ToUInt32(input);
        }

        [TestCase(short.MaxValue, Result = (uint)short.MaxValue)]
        [TestCase(short.MinValue, Result = null)]
        [TestCase((short)uint.MinValue, Result = uint.MinValue)]
        public uint? ToNullableUInt32_FromInt16(short input)
        {
            return SafeNullableConvert.ToUInt32(input);
        }

        [TestCase(ushort.MaxValue, Result = (uint)ushort.MaxValue)]
        [TestCase(ushort.MinValue, Result = (uint)ushort.MinValue)]
        public uint? ToNullableUInt32_FromUInt16(ushort input)
        {
            return SafeNullableConvert.ToUInt32(input);
        }

        [TestCase(int.MaxValue, Result = (uint)int.MaxValue)]
        [TestCase(int.MinValue, Result = null)]
        [TestCase((int)uint.MinValue, Result = uint.MinValue)]
        public uint? ToNullableUInt32_FromInt32(int input)
        {
            return SafeNullableConvert.ToUInt32(input);
        }

        [TestCase(uint.MaxValue, Result = uint.MaxValue)]
        [TestCase(uint.MinValue, Result = uint.MinValue)]
        public uint? ToNullableUInt32_FromUInt32(uint input)
        {
            return SafeNullableConvert.ToUInt32(input);
        }

        [TestCase(long.MaxValue, Result = null)]
        [TestCase(long.MinValue, Result = null)]
        [TestCase((long)uint.MaxValue, Result = uint.MaxValue)]
        public uint? ToNullableUInt32_FromInt64(long input)
        {
            return SafeNullableConvert.ToUInt32(input);
        }

        [TestCase(ulong.MaxValue, Result = null)]
        [TestCase(ulong.MinValue, Result = 0)]
        [TestCase((ulong)uint.MaxValue, Result = uint.MaxValue)]
        public uint? ToNullableUInt32_FromUInt64(ulong input)
        {
            return SafeNullableConvert.ToUInt32(input);
        }

        [TestCase((float)uint.MinValue, Result = uint.MinValue)]
        [TestCase(float.MaxValue, Result = null)]
        [TestCase(float.MinValue, Result = null)]
        [TestCase(uint.MinValue - 1f, Result = null)]
        [TestCase(uint.MinValue + 1.5f, Result = uint.MinValue + 1)]
        public uint? ToNullableUInt32_FromSingle(float input)
        {
            return SafeNullableConvert.ToUInt32(input);
        }

        [TestCase((double)uint.MaxValue, Result = uint.MaxValue)]
        [TestCase((double)uint.MinValue, Result = uint.MinValue)]
        [TestCase(double.MaxValue, Result = null)]
        [TestCase(double.MinValue, Result = null)]
        [TestCase(uint.MaxValue + 1d, Result = null)]
        [TestCase(uint.MinValue - 1d, Result = null)]
        [TestCase(uint.MaxValue - 1.5d, Result = uint.MaxValue - 2)]
        [TestCase(uint.MinValue + 1.5d, Result = uint.MinValue + 1)]
        public uint? ToNullableUInt32_FromDouble(double input)
        {
            return SafeNullableConvert.ToUInt32(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToNullableUInt32Data")]
        public uint? ToNullableUInt32_FromDecimal(decimal input)
        {
            return SafeNullableConvert.ToUInt32(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public uint? ToNullableUInt32_FromBoolean(bool input)
        {
            return SafeNullableConvert.ToUInt32(input);
        }

        [TestCase((char)uint.MinValue, Result = uint.MinValue)]
        [TestCase(char.MaxValue, Result = (uint)char.MaxValue)]
        public uint? ToNullableUInt32_FromChar(char input)
        {
            return SafeNullableConvert.ToUInt32(input);
        }

        #endregion

        #region? ToNullableInt64

        [TestCase(null, Result = null)]
        [TestCase("", Result = null)]
        [TestCase("9223372036854775807", Result = long.MaxValue)]
        [TestCase("-9223372036854775808", Result = long.MinValue)]
        //[TestCase("9223372036854775808", Result = null)]
        //[TestCase("-9223372036854775809", Result = null)]
        [TestCase(":", Result = null)]
        [TestCase("/", Result = null)]
        public long? ToNullableInt64_FromString(string input)
        {
            return SafeNullableConvert.ToInt64(input);
        }

        [TestCase(byte.MaxValue, Result = (long)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (long)byte.MinValue)]
        public long? ToNullableInt64_FromByte(byte input)
        {
            return SafeNullableConvert.ToInt64(input);
        }

        [TestCase(sbyte.MaxValue, Result = (long)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (long)sbyte.MinValue)]
        public long? ToNullableInt64_FromSByte(sbyte input)
        {
            return SafeNullableConvert.ToInt64(input);
        }

        [TestCase(short.MaxValue, Result = (long)short.MaxValue)]
        [TestCase(short.MinValue, Result = (long)short.MinValue)]
        public long? ToNullableInt64_FromInt16(short input)
        {
            return SafeNullableConvert.ToInt64(input);
        }

        [TestCase(ushort.MaxValue, Result = (long)ushort.MaxValue)]
        [TestCase(ushort.MinValue, Result = 0)]
        public long? ToNullableInt64_FromUInt16(ushort input)
        {
            return SafeNullableConvert.ToInt64(input);
        }

        [TestCase(int.MaxValue, Result = (long)int.MaxValue)]
        [TestCase(int.MinValue, Result = (long)int.MinValue)]
        public long? ToNullableInt64_FromInt32(int input)
        {
            return SafeNullableConvert.ToInt64(input);
        }

        [TestCase(uint.MaxValue, Result = (long)uint.MaxValue)]
        [TestCase(uint.MinValue, Result = (long)uint.MinValue)]
        [TestCase((uint)int.MaxValue, Result = int.MaxValue)]
        public long? ToNullableInt64_FromUInt32(uint input)
        {
            return SafeNullableConvert.ToInt64(input);
        }

        [TestCase(long.MaxValue, Result = long.MaxValue)]
        [TestCase(long.MinValue, Result = long.MinValue)]
        public long? ToNullableInt64_FromInt64(long input)
        {
            return SafeNullableConvert.ToInt64(input);
        }

        [TestCase(ulong.MaxValue, Result = null)]
        [TestCase(ulong.MinValue, Result = 0)]
        [TestCase((ulong)long.MaxValue, Result = long.MaxValue)]
        public long? ToNullableInt64_FromUInt64(ulong input)
        {
            return SafeNullableConvert.ToInt64(input);
        }

        [TestCase(float.MaxValue, Result = null)]
        [TestCase(float.MinValue, Result = null)]
        [TestCase((float)int.MinValue, Result = int.MinValue)]
        public long? ToNullableInt64_FromSingle(float input)
        {
            return SafeNullableConvert.ToInt64(input);
        }

        [TestCase((double)int.MaxValue, Result = int.MaxValue)]
        [TestCase((double)int.MinValue, Result = int.MinValue)]
        [TestCase(double.MaxValue, Result = null)]
        [TestCase(double.MinValue, Result = null)]
        [TestCase(int.MaxValue - 1.5d, Result = int.MaxValue - 2)]
        [TestCase(int.MinValue + 1.5d, Result = int.MinValue + 2)]
        public long? ToNullableInt64_FromDouble(double input)
        {
            return SafeNullableConvert.ToInt64(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToNullableInt64Data")]
        public long? ToNullableInt64_FromDecimal(decimal input)
        {
            return SafeNullableConvert.ToInt64(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public long? ToNullableInt64_FromBoolean(bool input)
        {
            return SafeNullableConvert.ToInt64(input);
        }

        [TestCase(char.MaxValue, Result = (long)char.MaxValue)]
        [TestCase(char.MinValue, Result = 0)]
        public long? ToNullableInt64_FromChar(char input)
        {
            return SafeNullableConvert.ToInt64(input);
        }

        #endregion

        #region? ToNullableUInt64

        [TestCase("18446744073709551615", Result = ulong.MaxValue)]
        [TestCase("0", Result = ulong.MinValue)]
        [TestCase("18446744073709551616", Result = null)]
        //[TestCase("18446744073709551617", Result = null)]
        [TestCase("-1", Result = null)]
        [TestCase(":", Result = null)]
        [TestCase("/", Result = null)]
        public ulong? ToNullableUInt64_FromString(string input)
        {
            return SafeNullableConvert.ToUInt64(input);
        }

        [TestCase(byte.MaxValue, Result = (ulong)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (ulong)byte.MinValue)]
        public ulong? ToNullableUInt64_FromByte(byte input)
        {
            return SafeNullableConvert.ToUInt64(input);
        }

        [TestCase(sbyte.MaxValue, Result = (ulong)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = null)]
        public ulong? ToNullableUInt64_FromSByte(sbyte input)
        {
            return SafeNullableConvert.ToUInt64(input);
        }

        [TestCase(short.MaxValue, Result = (ulong)short.MaxValue)]
        [TestCase(short.MinValue, Result = null)]
        [TestCase((short)uint.MinValue, Result = ulong.MinValue)]
        public ulong? ToNullableUInt64_FromInt16(short input)
        {
            return SafeNullableConvert.ToUInt64(input);
        }

        [TestCase(ushort.MaxValue, Result = (ulong)ushort.MaxValue)]
        [TestCase(ushort.MinValue, Result = (ulong)ushort.MinValue)]
        public ulong? ToNullableUInt64_FromUInt16(ushort input)
        {
            return SafeNullableConvert.ToUInt64(input);
        }

        [TestCase(int.MaxValue, Result = (ulong)int.MaxValue)]
        [TestCase(int.MinValue, Result = null)]
        [TestCase((int)uint.MinValue, Result = ulong.MinValue)]
        public ulong? ToNullableUInt64_FromInt32(int input)
        {
            return SafeNullableConvert.ToUInt64(input);
        }

        [TestCase(uint.MaxValue, Result = (ulong)uint.MaxValue)]
        [TestCase(uint.MinValue, Result = (ulong)uint.MinValue)]
        public ulong? ToNullableUInt64_FromUInt32(uint input)
        {
            return SafeNullableConvert.ToUInt64(input);
        }

        [TestCase(long.MaxValue, Result = (ulong)long.MaxValue)]
        [TestCase(long.MinValue, Result = null)]
        public ulong? ToNullableUInt64_FromInt64(long input)
        {
            return SafeNullableConvert.ToUInt64(input);
        }

        [TestCase(ulong.MaxValue, Result = ulong.MaxValue)]
        [TestCase(ulong.MinValue, Result = ulong.MinValue)]
        public ulong? ToNullableUInt64_FromUInt64(ulong input)
        {
            return SafeNullableConvert.ToUInt64(input);
        }

        [TestCase((float)ulong.MinValue, Result = ulong.MinValue)]
        [TestCase(float.MaxValue, Result = null)]
        [TestCase(float.MinValue, Result = null)]
        [TestCase(ulong.MinValue - 1f, Result = null)]
        [TestCase(ulong.MinValue + 1.5f, Result = ulong.MinValue + 1)]
        public ulong? ToNullableUInt64_FromSingle(float input)
        {
            return SafeNullableConvert.ToUInt64(input);
        }

        //[TestCase((double)ulong.MaxValue, Result = ulong.MaxValue)]
        [TestCase((double)ulong.MinValue, Result = ulong.MinValue)]
        [TestCase(double.MaxValue, Result = null)]
        [TestCase(double.MinValue, Result = null)]
        //[TestCase(ulong.MaxValue + 1d, Result = null)]
        [TestCase(ulong.MinValue - 1d, Result = null)]
        //[TestCase(ulong.MaxValue - 1.5d, Result = ulong.MaxValue - 2)]
        [TestCase(ulong.MinValue + 1.5d, Result = ulong.MinValue + 1)]
        public ulong? ToNullableUInt64_FromDouble(double input)
        {
            return SafeNullableConvert.ToUInt64(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToNullableUInt64Data")]
        public ulong? ToNullableUInt64_FromDecimal(decimal input)
        {
            return SafeNullableConvert.ToUInt64(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public ulong? ToNullableUInt64_FromBoolean(bool input)
        {
            return SafeNullableConvert.ToUInt64(input);
        }

        [TestCase((char)ulong.MinValue, Result = ulong.MinValue)]
        [TestCase(char.MaxValue, Result = (ulong)char.MaxValue)]
        public ulong? ToNullableUInt64_FromChar(char input)
        {
            return SafeNullableConvert.ToUInt64(input);
        }

        #endregion

        #region? ToNullableSingle

        [TestCase(null, Result = null)]
        [TestCase("", Result = null)]
        [TestCase("1", Result = 1f)]
        public float? ToNullableSingle_FromString(string input)
        {
            return SafeNullableConvert.ToSingle(input);
        }

        [TestCase(null, Result = null)]
        [TestCase("", Result = null)]
        [TestCase("1.7976931348623157", Result = 1.7976931348623157f)]
        [TestCase("-1.7976931348623157", Result = -1.7976931348623157f)]
        [TestCase(":", Result = null)]
        [TestCase("/", Result = null)]
        public float? ToNullableSingle_FromStringWithFormat(string input)
        {
            return SafeNullableConvert.ToSingle(input, this.numberFormatProvider);
        }

        [TestCase(byte.MaxValue, Result = (float)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (float)byte.MinValue)]
        public float? ToNullableSingle_FromByte(byte input)
        {
            return SafeNullableConvert.ToSingle(input);
        }

        [TestCase(sbyte.MaxValue, Result = (float)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (float)sbyte.MinValue)]
        public float? ToNullableSingle_FromSByte(sbyte input)
        {
            return SafeNullableConvert.ToSingle(input);
        }

        [TestCase(short.MaxValue, Result = (float)short.MaxValue)]
        [TestCase(short.MinValue, Result = (float)short.MinValue)]
        public float? ToNullableSingle_FromInt16(short input)
        {
            return SafeNullableConvert.ToSingle(input);
        }

        [TestCase(ushort.MaxValue, Result = (float)ushort.MaxValue)]
        [TestCase(ushort.MinValue, Result = 0)]
        public float? ToNullableSingle_FromUInt16(ushort input)
        {
            return SafeNullableConvert.ToSingle(input);
        }

        [TestCase(int.MaxValue, Result = (float)int.MaxValue)]
        [TestCase(int.MinValue, Result = (float)int.MinValue)]
        public float? ToNullableSingle_FromInt32(int input)
        {
            return SafeNullableConvert.ToSingle(input);
        }

        [TestCase(uint.MaxValue, Result = (float)uint.MaxValue)]
        [TestCase(uint.MinValue, Result = (float)uint.MinValue)]
        public float? ToNullableSingle_FromUInt32(uint input)
        {
            return SafeNullableConvert.ToSingle(input);
        }

        [TestCase(long.MaxValue, Result = (float)long.MaxValue)]
        [TestCase(long.MinValue, Result = (float)long.MinValue)]
        public float? ToNullableSingle_FromInt64(long input)
        {
            return SafeNullableConvert.ToSingle(input);
        }

        [TestCase(ulong.MaxValue, Result = (float)ulong.MaxValue)]
        [TestCase(ulong.MinValue, Result = 0f)]
        public float? ToNullableSingle_FromUInt64(ulong input)
        {
            return SafeNullableConvert.ToSingle(input);
        }

        [TestCase(float.MaxValue, Result = float.MaxValue)]
        [TestCase(float.MinValue, Result = float.MinValue)]
        public float? ToNullableSingle_FromSingle(float input)
        {
            return SafeNullableConvert.ToSingle(input);
        }

        [TestCase((double)float.MaxValue, Result = float.MaxValue)]
        [TestCase((double)float.MinValue, Result = float.MinValue)]
        //[TestCase(((double)float.MaxValue) + 1d, Result = nullf)]
        //[TestCase(((double)float.MinValue) - 1d, Result = nullf)]
        [TestCase(double.MaxValue, Result = null)]
        [TestCase(double.MinValue, Result = null)]
        public float? ToNullableSingle_FromDouble(double input)
        {
            return SafeNullableConvert.ToSingle(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToNullableSingleData")]
        public float? ToNullableSingle_FromDecimal(decimal input)
        {
            return SafeNullableConvert.ToSingle(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public float? ToNullableSingle_FromBoolean(bool input)
        {
            return SafeNullableConvert.ToSingle(input);
        }

        [TestCase(char.MaxValue, Result = (float)char.MaxValue)]
        [TestCase(char.MinValue, Result = 0f)]
        public float? ToNullableSingle_FromChar(char input)
        {
            return SafeNullableConvert.ToSingle(input);
        }

        #endregion

        #region? ToNullableDouble

        [TestCase(null, Result = null)]
        [TestCase("", Result = null)]
        [TestCase("1", Result = 1d)]
        public double? ToNullableDouble_FromString(string input)
        {
            return SafeNullableConvert.ToDouble(input);
        }

        [TestCase(null, Result = null)]
        [TestCase("", Result = null)]
        [TestCase("1.7976931348623157", Result = 1.7976931348623157)]
        [TestCase("-1.7976931348623157", Result = -1.7976931348623157)]
        [TestCase(":", Result = null)]
        [TestCase("/", Result = null)]
        public double? ToNullableDouble_FromStringWithFormat(string input)
        {
            return SafeNullableConvert.ToDouble(input, CultureInfo.InvariantCulture);
        }

        [TestCase(byte.MaxValue, Result = (double)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (double)byte.MinValue)]
        public double? ToNullableDouble_FromByte(byte input)
        {
            return SafeNullableConvert.ToDouble(input);
        }

        [TestCase(sbyte.MaxValue, Result = (double)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = (double)sbyte.MinValue)]
        public double? ToNullableDouble_FromSByte(sbyte input)
        {
            return SafeNullableConvert.ToDouble(input);
        }

        [TestCase(short.MaxValue, Result = (double)short.MaxValue)]
        [TestCase(short.MinValue, Result = (double)short.MinValue)]
        public double? ToNullableDouble_FromInt16(short input)
        {
            return SafeNullableConvert.ToDouble(input);
        }

        [TestCase(ushort.MaxValue, Result = (double)ushort.MaxValue)]
        [TestCase(ushort.MinValue, Result = 0d)]
        public double? ToNullableDouble_FromUInt16(ushort input)
        {
            return SafeNullableConvert.ToDouble(input);
        }

        [TestCase(int.MaxValue, Result = (double)int.MaxValue)]
        [TestCase(int.MinValue, Result = (double)int.MinValue)]
        public double? ToNullableDouble_FromInt32(int input)
        {
            return SafeNullableConvert.ToDouble(input);
        }

        [TestCase(uint.MaxValue, Result = (double)uint.MaxValue)]
        [TestCase(uint.MinValue, Result = (double)uint.MinValue)]
        public double? ToNullableDouble_FromUInt32(uint input)
        {
            return SafeNullableConvert.ToDouble(input);
        }

        [TestCase(long.MaxValue, Result = (double)long.MaxValue)]
        [TestCase(long.MinValue, Result = (double)long.MinValue)]
        public double? ToNullableDouble_FromInt64(long input)
        {
            return SafeNullableConvert.ToDouble(input);
        }

        [TestCase(ulong.MaxValue, Result = (double)ulong.MaxValue)]
        [TestCase(ulong.MinValue, Result = 0d)]
        public double? ToNullableDouble_FromUInt64(ulong input)
        {
            return SafeNullableConvert.ToDouble(input);
        }

        [TestCase(float.MaxValue, Result = (double)float.MaxValue)]
        [TestCase(float.MinValue, Result = (double)float.MinValue)]
        public double? ToNullableDouble_FromSingle(float input)
        {
            return SafeNullableConvert.ToDouble(input);
        }

        [TestCase(double.MaxValue, Result = double.MaxValue)]
        [TestCase(double.MinValue, Result = double.MinValue)]
        public double? ToNullableDouble_FromDouble(double input)
        {
            return SafeNullableConvert.ToDouble(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToNullableDoubleData")]
        public double? ToNullableDouble_FromDecimal(decimal input)
        {
            return SafeNullableConvert.ToDouble(input);
        }

        [TestCase(false, Result = 0)]
        [TestCase(true, Result = 1)]
        public double? ToNullableDouble_FromBoolean(bool input)
        {
            return SafeNullableConvert.ToDouble(input);
        }

        [TestCase(char.MaxValue, Result = (double)char.MaxValue)]
        [TestCase(char.MinValue, Result = (double)0)]
        public double? ToNullableDouble_FromChar(char input)
        {
            return SafeNullableConvert.ToDouble(input);
        }

        #endregion

        #region? ToNullableDecimal

        [TestCaseSource(typeof(TestData), "StringToNullableDecimalData")]
        public decimal? ToNullableDecimal_FromString(string input)
        {
            return SafeNullableConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "StringToNullableDecimalWithFormatData")]
        public decimal? ToNullableDecimal_FromStringWithFormat(string input)
        {
            return SafeNullableConvert.ToDecimal(input, this.numberFormatProvider);
        }

        [TestCaseSource(typeof(TestData), "ByteToDecimalData")]
        public decimal? ToNullableDecimal_FromByte(byte input)
        {
            return SafeNullableConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "SByteToDecimalData")]
        public decimal? ToNullableDecimal_FromSByte(sbyte input)
        {
            return SafeNullableConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "Int16ToDecimalData")]
        public decimal? ToNullableDecimal_FromInt16(short input)
        {
            return SafeNullableConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "UInt16ToDecimalData")]
        public decimal? ToNullableDecimal_FromUInt16(ushort input)
        {
            return SafeNullableConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "Int32ToDecimalData")]
        public decimal? ToNullableDecimal_FromInt32(int input)
        {
            return SafeNullableConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "UInt32ToDecimalData")]
        public decimal? ToNullableDecimal_FromUInt32(uint input)
        {
            return SafeNullableConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "Int64ToDecimalData")]
        public decimal? ToNullableDecimal_FromInt64(long input)
        {
            return SafeNullableConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "UInt64ToDecimalData")]
        public decimal? ToNullableDecimal_FromUInt64(ulong input)
        {
            return SafeNullableConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "SingleToNullableDecimalData")]
        public decimal? ToNullableDecimal_FromSingle(float input)
        {
            return SafeNullableConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "DoubleToNullableDecimalData")]
        public decimal? ToNullableDecimal_FromDouble(double input)
        {
            return SafeNullableConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToNullableDecimalData")]
        public decimal? ToNullableDecimal_FromDecimal(decimal input)
        {
            return SafeNullableConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "BooleanToDecimalData")]
        public decimal? ToNullableDecimal_FromBoolean(bool input)
        {
            return SafeNullableConvert.ToDecimal(input);
        }

        [TestCaseSource(typeof(TestData), "CharToDecimalData")]
        public decimal? ToNullableDecimal_FromChar(char input)
        {
            return SafeNullableConvert.ToDecimal(input);
        }

        #endregion

        #region? ToNullableBoolean

        [TestCase("True", Result = true)]
        [TestCase("False", Result = false)]
        [TestCase("true", Result = true)]
        [TestCase("false", Result = false)]
        [TestCase("Foo", Result = false)]
        public bool? ToNullableBoolean_FromString(string input)
        {
            return SafeNullableConvert.ToBoolean(input);
        }

        [TestCase(byte.MaxValue, Result = true)]
        [TestCase(byte.MinValue, Result = false)]
        public bool? ToNullableBoolean_FromByte(byte input)
        {
            return SafeNullableConvert.ToBoolean(input);
        }

        [TestCase(sbyte.MaxValue, Result = true)]
        [TestCase(sbyte.MinValue, Result = true)]
        [TestCase((sbyte)0, Result = false)]
        public bool? ToNullableBoolean_FromSByte(sbyte input)
        {
            return SafeNullableConvert.ToBoolean(input);
        }

        [TestCase(short.MaxValue, Result = true)]
        [TestCase(short.MinValue, Result = true)]
        [TestCase((short)0, Result = false)]
        public bool? ToNullableBoolean_FromInt16(short input)
        {
            return SafeNullableConvert.ToBoolean(input);
        }

        [TestCase(ushort.MaxValue, Result = true)]
        [TestCase(ushort.MinValue, Result = false)]
        public bool? ToNullableBoolean_FromUInt16(ushort input)
        {
            return SafeNullableConvert.ToBoolean(input);
        }

        [TestCase(int.MaxValue, Result = true)]
        [TestCase(int.MinValue, Result = true)]
        [TestCase(0, Result = false)]
        public bool? ToNullableBoolean_FromInt32(int input)
        {
            return SafeNullableConvert.ToBoolean(input);
        }

        [TestCase(uint.MaxValue, Result = true)]
        [TestCase(uint.MinValue, Result = false)]
        public bool? ToNullableBoolean_FromUInt32(uint input)
        {
            return SafeNullableConvert.ToBoolean(input);
        }

        [TestCase(long.MaxValue, Result = true)]
        [TestCase(long.MinValue, Result = true)]
        [TestCase(0L, Result = false)]
        public bool? ToNullableBoolean_FromInt64(long input)
        {
            return SafeNullableConvert.ToBoolean(input);
        }

        [TestCase(ulong.MaxValue, Result = true)]
        [TestCase(ulong.MinValue, Result = false)]
        public bool? ToNullableBoolean_FromUInt64(ulong input)
        {
            return SafeNullableConvert.ToBoolean(input);
        }

        [TestCase(float.MaxValue, Result = true)]
        [TestCase(float.MinValue, Result = true)]
        [TestCase(0f, Result = false)]
        public bool? ToNullableBoolean_FromSingle(float input)
        {
            return SafeNullableConvert.ToBoolean(input);
        }

        [TestCase(double.MaxValue, Result = true)]
        [TestCase(double.MinValue, Result = true)]
        [TestCase(0d, Result = false)]
        public bool? ToNullableBoolean_FromDouble(double input)
        {
            return SafeNullableConvert.ToBoolean(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToNullableBooleanData")]
        public bool? ToNullableBoolean_FromDecimal(decimal input)
        {
            return SafeNullableConvert.ToBoolean(input);
        }

        [TestCase(false, Result = false)]
        [TestCase(true, Result = true)]
        public bool? ToNullableBoolean_FromBoolean(bool input)
        {
            return SafeNullableConvert.ToBoolean(input);
        }

        [TestCase(char.MaxValue, Result = true)]
        [TestCase(char.MinValue, Result = false)]
        public bool? ToNullableBoolean_FromChar(char input)
        {
            return SafeNullableConvert.ToBoolean(input);
        }

        #endregion

        #region? ToNullableChar

        [TestCase("", Result = null)]
        [TestCase(null, Result = null)]
        [TestCase("A", Result = 'A')]
        [TestCase("0", Result = '0')]
        [TestCase("ab", Result = null)]
        public char? ToNullableChar_FromString(string input)
        {
            return SafeNullableConvert.ToChar(input);
        }

        [TestCase(byte.MaxValue, Result = (char)byte.MaxValue)]
        [TestCase(byte.MinValue, Result = (char)byte.MinValue)]
        public char? ToNullableChar_FromByte(byte input)
        {
            return SafeNullableConvert.ToChar(input);
        }

        [TestCase(sbyte.MaxValue, Result = (char)sbyte.MaxValue)]
        [TestCase(sbyte.MinValue, Result = null)]
        public char? ToNullableChar_FromSByte(sbyte input)
        {
            return SafeNullableConvert.ToChar(input);
        }

        [TestCase(short.MaxValue, Result = (char)short.MaxValue)]
        [TestCase(short.MinValue, Result = null)]
        [TestCase((short)char.MinValue, Result = char.MinValue)]
        public char? ToNullableChar_FromInt16(short input)
        {
            return SafeNullableConvert.ToChar(input);
        }

        [TestCase(ushort.MaxValue, Result = (char)ushort.MaxValue)]
        [TestCase(ushort.MinValue, Result = (char)ushort.MinValue)]
        public char? ToNullableChar_FromUInt16(ushort input)
        {
            return SafeNullableConvert.ToChar(input);
        }

        [TestCase(int.MaxValue, Result = null)]
        [TestCase(int.MinValue, Result = null)]
        [TestCase((int)char.MaxValue, Result = char.MaxValue)]
        [TestCase((int)char.MinValue, Result = char.MinValue)]
        public char? ToNullableChar_FromInt32(int input)
        {
            return SafeNullableConvert.ToChar(input);
        }

        [TestCase(uint.MaxValue, Result = null)]
        [TestCase(uint.MinValue, Result = (char)0)]
        [TestCase((uint)char.MaxValue, Result = char.MaxValue)]
        public char? ToNullableChar_FromUInt32(uint input)
        {
            return SafeNullableConvert.ToChar(input);
        }

        [TestCase(long.MaxValue, Result = null)]
        [TestCase(long.MinValue, Result = null)]
        [TestCase((long)char.MaxValue, Result = char.MaxValue)]
        public char? ToNullableChar_FromInt64(long input)
        {
            return SafeNullableConvert.ToChar(input);
        }

        [TestCase(ulong.MaxValue, Result = null)]
        [TestCase(ulong.MinValue, Result = (char)0)]
        [TestCase((ulong)char.MaxValue, Result = char.MaxValue)]
        public char? ToNullableChar_FromUInt64(ulong input)
        {
            return SafeNullableConvert.ToChar(input);
        }

        [TestCase((float)char.MinValue, Result = char.MinValue)]
        [TestCase(float.MaxValue, Result = null)]
        [TestCase(float.MinValue, Result = null)]
        [TestCase((float)char.MinValue - 1f, Result = null)]
        [TestCase(65f, Result = 'A')]
        [TestCase(65.5f, Result = 'A')]
        public char? ToNullableChar_FromSingle(float input)
        {
            return SafeNullableConvert.ToChar(input);
        }

        [TestCase((double)char.MaxValue, Result = char.MaxValue)]
        [TestCase((double)char.MinValue, Result = char.MinValue)]
        [TestCase(double.MaxValue, Result = null)]
        [TestCase(double.MinValue, Result = null)]
        [TestCase((double)char.MaxValue + 1d, Result = null)]
        [TestCase((double)char.MinValue - 1d, Result = null)]
        [TestCase((double)char.MaxValue - 1.5d, Result = (char)(char.MaxValue - 2))]
        [TestCase(65d, Result = 'A')]
        [TestCase(65.5d, Result = 'A')]
        public char? ToNullableChar_FromDouble(double input)
        {
            return SafeNullableConvert.ToChar(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToNullableCharData")]
        public char? ToNullableChar_FromDecimal(decimal input)
        {
            return SafeNullableConvert.ToChar(input);
        }

        [TestCase(false, Result = '0')]
        [TestCase(true, Result = '1')]
        public char? ToNullableChar_FromBoolean(bool input)
        {
            return SafeNullableConvert.ToChar(input);
        }

        [TestCase(char.MaxValue, Result = char.MaxValue)]
        [TestCase(char.MinValue, Result = char.MinValue)]
        public char? ToNullableChar_FromChar(char input)
        {
            return SafeNullableConvert.ToChar(input);
        }

        #endregion

        #region? ToNullableGuid

        [TestCaseSource(typeof(TestData), "StringToGuidData")]
        public Guid? ToNullableGuid_FromString(string input)
        {
            return SafeNullableConvert.ToGuid(input);
        }

        #endregion

        #region? ToNullableDateTime

        [TestCaseSource(typeof(TestData), "StringToDateTimeData")]
        public DateTime? ToNullableDateTime_FromString(string input)
        {
            return SafeNullableConvert.ToDateTime(input);
        }

        [TestCaseSource(typeof(TestData), "StringToDateTimeData")]
        public DateTime? ToNullableDateTime_FromStringWithFormat(string input)
        {
            return SafeNullableConvert.ToDateTime(input, CultureInfo.InvariantCulture);
        }

        [TestCaseSource(typeof(TestData), "SqlDateTimeToDateTimeData")]
        public DateTime? ToNullableDateTime_FromSqlDateTime(SqlDateTime input)
        {
            return SafeNullableConvert.ToDateTime(input);
        }

        #endregion

        #region? ToNullableSqlDateTime

        [TestCaseSource(typeof(TestData), "DateTimeToSqlDateTimeData")]
        public SqlDateTime? ToNullableSqlDateTime_FromSqlDateTime(DateTime input)
        {
            return SafeNullableConvert.ToSqlDateTime(input);
        }

        #endregion
    }
}
