namespace SafeMapper.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data.SqlTypes;
    using System.Globalization;
    using System.Threading;

    using NUnit.Framework;

    using SafeMapper.Tests.Model;
    using SafeMapper.Tests.Model.Enums;
    using SafeMapper.Tests.Model.GenericClasses;
    using SafeMapper.Tests.Model.Person;
    using SafeMapper.Utils;

    [TestFixture]
    public class ConverterFactoryTests
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

        /************************************************************************/
        /*                                                                      
        /*   String                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "StringToStringData")]
        public string CreateDelegate_StringToString(string input)
        {
            var converter = ConverterFactory.CreateDelegate<string, string>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToStringData")]
        public string CreateDelegate_StringMemberToStringMember(string input)
        {
            return this.AssertConverterOutput<string, string>(input);
        }

        [TestCaseSource(typeof(TestData), "StringToIntData")]
        public int CreateDelegate_StringToInt(string input)
        {
            var converter = ConverterFactory.CreateDelegate<string, int>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToIntData")]
        public int CreateDelegate_StringMemberToIntMember(string input)
        {
            return this.AssertConverterOutput<string, int>(input);
        }

        [TestCaseSource(typeof(TestData), "StringToGuidData")]
        public Guid CreateDelegate_StringToGuid(string input)
        {
            var converter = ConverterFactory.CreateDelegate<string, Guid>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToGuidData")]
        public Guid CreateDelegate_StringMemberToGuidMember(string input)
        {
            return this.AssertConverterOutput<string, Guid>(input);
        }

        [TestCaseSource(typeof(TestData), "StringToDecimalData")]
        public decimal CreateDelegate_StringToDecimal(string input)
        {
            var converter = ConverterFactory.CreateDelegate<string, decimal>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToDecimalData")]
        public decimal CreateDelegate_StringMemberToDecimalMember(string input)
        {
            return this.AssertConverterOutput<string, decimal>(input, this.numberFormatProvider);
        }

        [TestCaseSource(typeof(TestData), "StringToDateTimeData")]
        public DateTime CreateDelegate_StringToDateTime(string input)
        {
            var converter = ConverterFactory.CreateDelegate<string, DateTime>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToDateTimeData")]
        public DateTime CreateDelegate_StringMemberToDateTimeMember(string input)
        {
            return this.AssertConverterOutput<string, DateTime>(input);
        }

        /************************************************************************/
        /*                                                                      
        /*   Int                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "IntToIntData")]
        public int CreateDelegate_IntToInt(int input)
        {
            var converter = ConverterFactory.CreateDelegate<int, int>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "IntToIntData")]
        public int CreateDelegate_IntMemberToIntMember(int input)
        {
            return this.AssertConverterOutput<int, int>(input);
        }


        [TestCaseSource(typeof(TestData), "IntToStringData")]
        public string CreateDelegate_IntToString(int input)
        {
            var converter = ConverterFactory.CreateDelegate<int, string>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "IntToStringData")]
        public string CreateDelegate_IntMemberToStringMember(int input)
        {
            return this.AssertConverterOutput<int, string>(input);
        }


        [TestCaseSource(typeof(TestData), "IntToLongData")]
        public long CreateDelegate_IntToLong(int input)
        {
            var converter = ConverterFactory.CreateDelegate<int, long>();

            return converter(input);
        }


        /************************************************************************/
        /*                                                                      
        /*   Guid                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "GuidToStringData")]
        public string CreateDelegate_GuidToString(Guid input)
        {
            var converter = ConverterFactory.CreateDelegate<Guid, string>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "GuidToStringData")]
        public string CreateDelegate_GuidMemberToStringMember(Guid input)
        {
            return this.AssertConverterOutput<Guid, string>(input);
        }

        /************************************************************************/
        /*                                                                      
        /*   Decimal                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "DecimalToDecimalData")]
        public decimal CreateDelegate_DecimalToDecimal(decimal input)
        {
            var converter = ConverterFactory.CreateDelegate<decimal, decimal>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToStringData")]
        public string CreateDelegate_DecimalToString(decimal input)
        {
            var converter = ConverterFactory.CreateDelegate<decimal, string>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToStringData")]
        public string CreateDelegate_DecimalMemberToStringMember(decimal input)
        {
            return this.AssertConverterOutput<decimal, string>(input, this.numberFormatProvider);
        }

        [TestCaseSource(typeof(TestData), "DecimalToDoubleData")]
        public double CreateDelegate_DecimalToDouble(decimal input)
        {
            var converter = ConverterFactory.CreateDelegate<decimal, double>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToDoubleData")]
        public double CreateDelegate_DecimalMemberToDoubleMember(decimal input)
        {
            return this.AssertConverterOutput<decimal, double>(input, this.numberFormatProvider);
        }

        /************************************************************************/
        /*                                                                      
        /*   Double                                                              
        /*                                                                      
        /************************************************************************/

        [TestCaseSource(typeof(TestData), "DoubleToDecimalData")]
        public decimal CreateDelegate_DoubleToDecimal(double input)
        {
            var converter = ConverterFactory.CreateDelegate<double, decimal>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "DoubleToDecimalData")]
        public decimal CreateDelegate_DoubleMemberToDecimalMember(double input)
        {
            return this.AssertConverterOutput<double, decimal>(input, this.numberFormatProvider);
        }



        /************************************************************************/
        /*                                                                      
        /*   DateTime                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "DateTimeToStringData")]
        public string CreateDelegate_DateTimeToString(DateTime input)
        {
            var converter = ConverterFactory.CreateDelegate<DateTime, string>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "DateTimeToStringData")]
        public string CreateDelegate_DateTimeMemberToStringMember(DateTime input)
        {
            return this.AssertConverterOutput<DateTime, string>(input, this.numberFormatProvider);
        }

        /************************************************************************/
        /*                                                                      
        /*   Long                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "LongToIntData")]
        public int CreateDelegate_LongToInt(long input)
        {
            var converter = ConverterFactory.CreateDelegate<long, int>();

            return converter(input);
        }


        /************************************************************************/
        /*                                                                      
        /*   Array to array                                                              
        /*                                                                      
        /************************************************************************/

        [Test]
        public void CreateDelegate_IntArrayToIntArray()
        {
            var converter = ConverterFactory.CreateDelegate<int[], int[]>();
            var input = new int[] { 1, 2, 3, 4, 5 };
            var result = converter(input);

            Assert.AreEqual(input, result);
        }

        [Test]
        public void CreateDelegate_IntArrayToStringArray()
        {
            var converter = ConverterFactory.CreateDelegate<int[], string[]>();
            var expected = new string[] { "1", "2", "3", "4", "5" };
            var input = new int[] { 1, 2, 3, 4, 5 };
            var result = converter(input);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateDelegate_StringArrayToIntArray()
        {
            var converter = ConverterFactory.CreateDelegate<string[], int[]>();
            var input = new string[] { "1", "2", "3", "4", "5" };
            var expected = new int[] { 1, 2, 3, 4, 5 };
            var result = converter(input);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateDelegate_IntArrayToDecimalArray()
        {
            var converter = ConverterFactory.CreateDelegate<int[], decimal[]>();
            var input = new int[] { 1, 2, 3, 4, 5 };
            var expected = new decimal[] { 1m, 2m, 3m, 4m, 5m };
            var result = converter(input);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateDelegate_IntArrayMemberToStringArrayMember()
        {
            var input = new int[] { 1, 2, 3, 4, 5 };

            this.AssertConverterOutput<int[], string[]>(input);
        }

        /************************************************************************/
        /*                                                                      
        /*   Array and collections                                                              
        /*                                                                      
        /************************************************************************/

        [Test]
        public void CreateDelegate_StringListToStringArray()
        {
            var converter = ConverterFactory.CreateDelegate<List<string>, string[]>();
            var input = new List<string> { "1", "2", "3", "4", "5" };
            var expected = new string[] { "1", "2", "3", "4", "5" };
            var result = converter(input);

            Assert.IsInstanceOf<string[]>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateDelegate_StringListToIntArray()
        {
            var converter = ConverterFactory.CreateDelegate<List<string>, int[]>();
            var input = new List<string> { "1", "2", "3", "4", "5" };
            var expected = new int[] { 1, 2, 3, 4, 5 };
            var result = converter(input);

            Assert.IsInstanceOf<int[]>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateDelegate_StringListToIntList()
        {
            var converter = ConverterFactory.CreateDelegate<List<string>, List<int>>();
            var expected = new List<int> { 1, 2, 3, 4, 5 };
            var input = new List<string> { "1", "2", "3", "4", "5" };
            var result = converter(input);

            Assert.IsInstanceOf<List<int>>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateDelegate_StringArrayToIntList()
        {
            var converter = ConverterFactory.CreateDelegate<string[], List<int>>();
            var expected = new List<int> { 1, 2, 3, 4, 5 };
            var input = new string[] { "1", "2", "3", "4", "5" };
            var result = converter(input);

            Assert.IsInstanceOf<List<int>>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateDelegate_IntListToDecimalArray()
        {
            var converter = ConverterFactory.CreateDelegate<List<int>, decimal[]>();
            var input = new List<int> { 1, 2, 3, 4, 5 };
            var expected = new decimal[] { 1m, 2m, 3m, 4m, 5m };
            var result = converter(input);

            Assert.IsInstanceOf<decimal[]>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateDelegate_IntListToDecimalList()
        {
            var converter = ConverterFactory.CreateDelegate<List<int>, List<decimal>>();
            var expected = new List<decimal> { 1m, 2m, 3m, 4m, 5m };
            var input = new List<int> { 1, 2, 3, 4, 5 };
            var result = converter(input);

            Assert.IsInstanceOf<List<decimal>>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateDelegate_IntArrayToDecimalList()
        {
            var converter = ConverterFactory.CreateDelegate<int[], List<decimal>>();
            var expected = new List<decimal> { 1m, 2m, 3m, 4m, 5m };
            var input = new int[] { 1, 2, 3, 4, 5 };
            var result = converter(input);

            Assert.IsInstanceOf<List<decimal>>(result);
            Assert.AreEqual(expected, result);
        }


        /************************************************************************/
        /*                                                                      
        /*   Interface collection                                                              
        /*                                                                      
        /************************************************************************/

        [Test]
        public void CreateDelegate_StringICollectionToStringArray()
        {
            var converter = ConverterFactory.CreateDelegate<ICollection<string>, string[]>();
            var input = new List<string> { "1", "2", "3", "4", "5" } as ICollection<string>;
            var expected = new string[] { "1", "2", "3", "4", "5" };
            var result = converter(input);

            Assert.IsInstanceOf<string[]>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateDelegate_StringArrayToStringICollection()
        {
            var converter = ConverterFactory.CreateDelegate<string[], ICollection<string>>();
            var input = new string[] { "1", "2", "3", "4", "5" };
            var expected = new List<string> { "1", "2", "3", "4", "5" } as ICollection<string>;
            var result = converter(input);

            Assert.IsInstanceOf<ICollection<string>>(result);
            Assert.AreEqual(expected, result);
        }

        /************************************************************************/
        /*                                                                      
        /*   Convert From Enum                                                              
        /*                                                                      
        /************************************************************************/

        [TestCase(ByteEnum.Undefined, Result = (byte)0)]
        [TestCase(ByteEnum.Value1, Result = (byte)1)]
        [TestCase(ByteEnum.Value2, Result = (byte)2)]
        [TestCase(ByteEnum.Value3, Result = (byte)3)]
        public byte ToByte_FromByteEnum(ByteEnum input)
        {
            var converter = ConverterFactory.CreateDelegate<ByteEnum, byte>();
            return converter(input);
        }

        [TestCase(ByteEnum.Undefined, Result = (long)0)]
        [TestCase(ByteEnum.Value1, Result = (long)1)]
        [TestCase(ByteEnum.Value2, Result = (long)2)]
        [TestCase(ByteEnum.Value3, Result = (long)3)]
        public long ToInt64_FromByteEnum(ByteEnum input)
        {
            var converter = ConverterFactory.CreateDelegate<ByteEnum, long>();
            return converter(input);
        }


        [TestCase(Int64Enum.Undefined, Result = (byte)0)]
        [TestCase(Int64Enum.Value1, Result = (byte)1)]
        [TestCase(Int64Enum.Min, Result = (byte)0)]
        [TestCase(Int64Enum.Max, Result = (byte)0)]
        public byte ToByte_FromInt64Enum(Int64Enum input)
        {
            var converter = ConverterFactory.CreateDelegate<Int64Enum, byte>();
            return converter(input);
        }

        /************************************************************************/
        /*                                                                      
        /*   Convert To Enum                                                              
        /*                                                                      
        /************************************************************************/

        [TestCase("Undefined", Result = ByteEnum.Undefined)]
        [TestCase("Value1", Result = ByteEnum.Value1)]
        [TestCase("Value2", Result = ByteEnum.Value2)]
        [TestCase("Value3", Result = ByteEnum.Value3)]
        [TestCase("Value4", Result = ByteEnum.Undefined)]
        public ByteEnum ToByteEnum_FromString(string input)
        {
            var converter = ConverterFactory.CreateDelegate<string, ByteEnum>();
            return converter(input);
        }

        [TestCase((byte)0, Result = ByteEnum.Undefined)]
        [TestCase((byte)1, Result = ByteEnum.Value1)]
        [TestCase((byte)2, Result = ByteEnum.Value2)]
        [TestCase((byte)3, Result = ByteEnum.Value3)]
        [TestCase((byte)4, Result = ByteEnum.Undefined)]
        public ByteEnum ToByteEnum_FromByte(byte input)
        {
            var converter = ConverterFactory.CreateDelegate<byte, ByteEnum>();
            return converter(input);
        }

        [TestCase((byte)0, Result = SByteEnum.Undefined)]
        [TestCase((byte)1, Result = SByteEnum.Value1)]
        [TestCase((byte)2, Result = SByteEnum.Value2)]
        [TestCase((byte)3, Result = SByteEnum.Value3)]
        [TestCase((byte)4, Result = SByteEnum.Undefined)]
        public SByteEnum ToSByteEnum_FromByte(byte input)
        {
            var converter = ConverterFactory.CreateDelegate<byte, SByteEnum>();
            return converter(input);
        }

        [TestCase((byte)0, Result = Int16Enum.Undefined)]
        [TestCase((byte)1, Result = Int16Enum.Value1)]
        [TestCase((byte)2, Result = Int16Enum.Value2)]
        [TestCase((byte)3, Result = Int16Enum.Value3)]
        [TestCase((byte)4, Result = Int16Enum.Undefined)]
        public Int16Enum ToInt16Enum_FromByte(byte input)
        {
            var converter = ConverterFactory.CreateDelegate<byte, Int16Enum>();
            return converter(input);
        }

        [TestCase((byte)0, Result = UInt16Enum.Undefined)]
        [TestCase((byte)1, Result = UInt16Enum.Value1)]
        [TestCase((byte)2, Result = UInt16Enum.Value2)]
        [TestCase((byte)3, Result = UInt16Enum.Value3)]
        [TestCase((byte)4, Result = UInt16Enum.Undefined)]
        public UInt16Enum ToUInt16Enum_FromByte(byte input)
        {
            var converter = ConverterFactory.CreateDelegate<byte, UInt16Enum>();
            return converter(input);
        }

        [TestCase(-1, Result = Int32Enum.Undefined)]
        [TestCase(0, Result = Int32Enum.Undefined)]
        [TestCase(1, Result = Int32Enum.Value1)]
        [TestCase(2, Result = Int32Enum.Value2)]
        [TestCase(3, Result = Int32Enum.Value3)]
        [TestCase(4, Result = Int32Enum.Undefined)]
        public Int32Enum ToInt32Enum_FromInt32(int input)
        {
            var converter = ConverterFactory.CreateDelegate<int, Int32Enum>();
            return converter(input);
        }

        [TestCase(-1, Result = UInt32Enum.Undefined)]
        [TestCase(0, Result = UInt32Enum.Undefined)]
        [TestCase(1, Result = UInt32Enum.Value1)]
        [TestCase(2, Result = UInt32Enum.Value2)]
        [TestCase(3, Result = UInt32Enum.Value3)]
        [TestCase(4, Result = UInt32Enum.Undefined)]
        public UInt32Enum ToUInt32Enum_FromInt32(int input)
        {
            var converter = ConverterFactory.CreateDelegate<int, UInt32Enum>();
            return converter(input);
        }

        [TestCase(-1L, Result = Int32Enum.Undefined)]
        [TestCase(0L, Result = Int32Enum.Undefined)]
        [TestCase(1L, Result = Int32Enum.Value1)]
        [TestCase(2L, Result = Int32Enum.Value2)]
        [TestCase(3L, Result = Int32Enum.Value3)]
        [TestCase(4L, Result = Int32Enum.Undefined)]
        public Int32Enum ToInt32Enum_FromInt64(long input)
        {
            var converter = ConverterFactory.CreateDelegate<long, Int32Enum>();
            return converter(input);
        }

        [TestCase(-1L, Result = Int64Enum.Undefined)]
        [TestCase(0L, Result = Int64Enum.Undefined)]
        [TestCase(1L, Result = Int64Enum.Value1)]
        [TestCase(long.MinValue, Result = Int64Enum.Min)]
        [TestCase(long.MaxValue, Result = Int64Enum.Max)]
        [TestCase(4L, Result = Int64Enum.Undefined)]
        public Int64Enum ToInt64Enum_FromInt64(long input)
        {
            var converter = ConverterFactory.CreateDelegate<long, Int64Enum>();
            return converter(input);
        }

        [TestCase(-1L, Result = UInt64Enum.Undefined)]
        [TestCase(0L, Result = UInt64Enum.Undefined)]
        [TestCase(1L, Result = UInt64Enum.Value1)]
        [TestCase(long.MinValue, Result = UInt64Enum.Undefined)]
        [TestCase(long.MaxValue, Result = UInt64Enum.Undefined)]
        [TestCase(4L, Result = UInt64Enum.Undefined)]
        public UInt64Enum ToUInt64Enum_FromInt64(long input)
        {
            var converter = ConverterFactory.CreateDelegate<long, UInt64Enum>();
            return converter(input);
        }

        [TestCase(0UL, Result = UInt64Enum.Undefined)]
        [TestCase(1UL, Result = UInt64Enum.Value1)]
        [TestCase(ulong.MinValue, Result = UInt64Enum.Undefined)]
        [TestCase(ulong.MaxValue, Result = UInt64Enum.Max)]
        [TestCase(4UL, Result = UInt64Enum.Undefined)]
        public UInt64Enum ToUInt64Enum_FromUInt64(ulong input)
        {
            var converter = ConverterFactory.CreateDelegate<ulong, UInt64Enum>();
            return converter(input);
        }

        [TestCase("Odefinerat", Result = DisplayAttributeResourceEnum.Undefined)]
        [TestCase("Värde 1", Result = DisplayAttributeResourceEnum.Value1)]
        [TestCase("Värde 2", Result = DisplayAttributeResourceEnum.Value2)]
        [TestCase("Värde 3", Result = DisplayAttributeResourceEnum.Value3)]
        [TestCase("Värde 4", Result = DisplayAttributeResourceEnum.Undefined)]
        public DisplayAttributeResourceEnum ToDisplayAttributeResourceEnum_FromString_CultureSv(string input)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");
            var converter = ConverterFactory.CreateDelegate<string, DisplayAttributeResourceEnum>();
            return converter(input);
        }

        [TestCase("Undefined", Result = DisplayAttributeResourceEnum.Undefined)]
        [TestCase("Value 1", Result = DisplayAttributeResourceEnum.Value1)]
        [TestCase("Value 2", Result = DisplayAttributeResourceEnum.Value2)]
        [TestCase("Value 3", Result = DisplayAttributeResourceEnum.Value3)]
        [TestCase("Value 4", Result = DisplayAttributeResourceEnum.Undefined)]
        public DisplayAttributeResourceEnum ToDisplayAttributeResourceEnum_FromString_CultureEn(string input)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            var converter = ConverterFactory.CreateDelegate<string, DisplayAttributeResourceEnum>();
            return converter(input);
        }

        [TestCase("Odefinerat", Result = DisplayAttributeResxEnum.Undefined)]
        [TestCase("Värde 1", Result = DisplayAttributeResxEnum.Value1)]
        [TestCase("Värde 2", Result = DisplayAttributeResxEnum.Value2)]
        [TestCase("Värde 3", Result = DisplayAttributeResxEnum.Value3)]
        [TestCase("Värde 4", Result = DisplayAttributeResxEnum.Undefined)]
        public DisplayAttributeResxEnum ToDisplayAttributeResxEnum_FromString_CultureSv(string input)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");
            var converter = ConverterFactory.CreateDelegate<string, DisplayAttributeResxEnum>();
            return converter(input);
        }

        [TestCase("Undefined", Result = DisplayAttributeResxEnum.Undefined)]
        [TestCase("Value 1", Result = DisplayAttributeResxEnum.Value1)]
        [TestCase("Value 2", Result = DisplayAttributeResxEnum.Value2)]
        [TestCase("Value 3", Result = DisplayAttributeResxEnum.Value3)]
        [TestCase("Value 4", Result = DisplayAttributeResxEnum.Undefined)]
        public DisplayAttributeResxEnum ToDisplayAttributeResxEnum_FromString_CultureEn(string input)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            var converter = ConverterFactory.CreateDelegate<string, DisplayAttributeResxEnum>();
            return converter(input);
        }

        [TestCase("Undefined", Result = DescriptionAttributeEnum.Undefined)]
        [TestCase("Value 1", Result = DescriptionAttributeEnum.Value1)]
        [TestCase("Value 2", Result = DescriptionAttributeEnum.Value2)]
        [TestCase("Value 3", Result = DescriptionAttributeEnum.Value3)]
        [TestCase("Value 4", Result = DescriptionAttributeEnum.Undefined)]
        public DescriptionAttributeEnum ToDescriptionEnum_FromString(string input)
        {
            var converter = ConverterFactory.CreateDelegate<string, DescriptionAttributeEnum>();
            return converter(input);
        }

        [TestCase("Undefined", Result = DisplayAttributeEnum.Undefined)]
        [TestCase("Value 1", Result = DisplayAttributeEnum.Value1)]
        [TestCase("Value 2", Result = DisplayAttributeEnum.Value2)]
        [TestCase("Value 3", Result = DisplayAttributeEnum.Value3)]
        [TestCase("Value 4", Result = DisplayAttributeEnum.Undefined)]
        public DisplayAttributeEnum ToDisplayAttributeEnum_FromString(string input)
        {
            var converter = ConverterFactory.CreateDelegate<string, DisplayAttributeEnum>();
            return converter(input);
        }

        [TestCase(DisplayAttributeEnum.Undefined, Result = "Undefined")]
        [TestCase(DisplayAttributeEnum.Value1, Result = "Value 1")]
        [TestCase(DisplayAttributeEnum.Value2, Result = "Value 2")]
        [TestCase(DisplayAttributeEnum.Value3, Result = "Value 3")]
        public string ToString_FromDisplayAttributeEnum(DisplayAttributeEnum input)
        {
            var converter = ConverterFactory.CreateDelegate<DisplayAttributeEnum, string>();
            return converter(input);
        }

        [TestCase(DescriptionAttributeEnum.Undefined, Result = "Undefined")]
        [TestCase(DescriptionAttributeEnum.Value1, Result = "Value 1")]
        [TestCase(DescriptionAttributeEnum.Value2, Result = "Value 2")]
        [TestCase(DescriptionAttributeEnum.Value3, Result = "Value 3")]
        public string ToString_FromDisplayAttributeEnum(DescriptionAttributeEnum input)
        {
            var converter = ConverterFactory.CreateDelegate<DescriptionAttributeEnum, string>();
            return converter(input);
        }

        /************************************************************************/
        /*                                                                      
        /*   Misc                                                              
        /*                                                                      
        /************************************************************************/

        [Test]
        public void CreateDelegate_StringToStringArray()
        {
            var converter = ConverterFactory.CreateDelegate<string, int[]>();
            var input = "1";
            var result = converter(input);

            Assert.AreEqual(new[] { 1 }, result);
        }

        [Test]
        public void CreateDelegate_StringToStringList()
        {
            var converter = ConverterFactory.CreateDelegate<string, List<int>>();
            var input = "1";
            var result = converter(input);

            Assert.AreEqual(new List<int> { 1 }, result);
        }

        [Test]
        public void CreateDelegate_StringArrayToString()
        {
            var converter = ConverterFactory.CreateDelegate<string[], int>();
            var input = new[] { "1", "2", "3" };
            var result = converter(input);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void CreateDelegate_StringListToString()
        {
            var converter = ConverterFactory.CreateDelegate<List<string>, int>();
            var input = new List<string> { "1", "2", "3" };
            var result = converter(input);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void CreateDelegate_NonGenericDateTimeToSqlDateTime()
        {
            var converter = ConverterFactory.CreateDelegate(typeof(DateTime), typeof(SqlDateTime));
            var input = DateTime.MinValue;
            var result = converter(input);

            Assert.AreEqual(SqlDateTime.MinValue, result);
        }

        [Test]
        public void CreateDelegate_NonGenericIntToString()
        {
            var converter = ConverterFactory.CreateDelegate(typeof(int), typeof(string));
            var input = 1;
            var result = converter(input);

            Assert.AreEqual("1", result);
        }

        [Test]
        public void CreateDelegate_NonGenericStringToInt()
        {
            var converter = ConverterFactory.CreateDelegate(typeof(string), typeof(int));
            var input = "1";
            var result = converter(input);

            Assert.AreEqual(1, result);
        }

        [TestCaseSource(typeof(TestData), "NonGenericCollectionTestData")]
        public object CreateDelegate_NonGenericCollectionTestData(object input, Type fromType, Type toType)
        {
            var converter = ConverterFactory.CreateDelegate(fromType, toType);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "NonGenericTestData")]
        public object CreateDelegate_NonGenericTestData(object input, Type fromType, Type toType)
        {
            var converter = ConverterFactory.CreateDelegate(fromType, toType);

            return converter(input);
        }

        [Test]
        public void CreateDelegate_ConvertNameValueCollectionToClassPropertyWithIntArray()
        {
            var converter = ConverterFactory.CreateDelegate<NameValueCollection, ClassProperty<int[]>>();
            var input = new NameValueCollection { { "Value", "1" }, { "Value", "2" }, { "Value", "3" } };

            var result = converter(input);

            Assert.IsInstanceOf<ClassProperty<int[]>>(result);
            Assert.AreEqual(new[] { 1, 2, 3 }, result.Value);
        }

        [Test]
        public void CreateDelegate_ConvertNameValueCollectionToClassPropertyString_ShouldConcatValues()
        {
            var converter = ConverterFactory.CreateDelegate<NameValueCollection, ClassProperty<string>>();
            var input = new NameValueCollection { { "Value", "1" }, { "Value", "2" }, { "Value", "3" } };

            var result = converter(input);

            Assert.IsInstanceOf<ClassProperty<string>>(result);
            Assert.AreEqual("1,2,3", result.Value);
        }

        [Test]
        public void CreateDelegate_ConvertNameValueCollectionToClassPropertyInt_ShouldReturnFirstValue()
        {
            var converter = ConverterFactory.CreateDelegate<NameValueCollection, ClassProperty<int>>();
            var input = new NameValueCollection { { "Value", "1" }, { "Value", "2" }, { "Value", "3" } };

            var result = converter(input);

            Assert.IsInstanceOf<ClassProperty<int>>(result);
            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public void CreateDelegate_ConvertNameValueCollectionWithUnMatchedKey_ShouldReturnObjectWithDefaultValue()
        {
            var converter = ConverterFactory.CreateDelegate<NameValueCollection, ClassProperty<int>>();
            var input = new NameValueCollection { { "Value2", "37" } };

            var result = converter(input);

            Assert.IsInstanceOf<ClassProperty<int>>(result);
            Assert.AreEqual(0, result.Value);
        }

        [Test]
        public void CreateDelegate_ConvertDictionaryToClassProperty_ShouldReturnObjectWithDefaultValue()
        {
            var converter = ConverterFactory.CreateDelegate<Dictionary<string, string>, ClassProperty<int>>();
            var input = new Dictionary<string, string> { { "Value", "37" } };

            var result = converter(input);

            Assert.IsInstanceOf<ClassProperty<int>>(result);
            Assert.AreEqual(37, result.Value);
        }

        [Test]
        public void CreateDelegate_ConvertDictionaryToClassPropertyWithUnmatchedKey_ShouldReturnObjectWithDefaultValue()
        {
            var converter = ConverterFactory.CreateDelegate<Dictionary<string, string>, ClassProperty<int>>();
            var input = new Dictionary<string, string> { { "Value2", "37" } };

            var result = converter(input);

            Assert.IsInstanceOf<ClassProperty<int>>(result);
            Assert.AreEqual(0, result.Value);
        }

        [Test]
        public void CreateDelegate_ConvertClassPropertyWithStringToDictionary()
        {
            var converter = ConverterFactory.CreateDelegate<ClassProperty<string>, Dictionary<string, int>>();
            var input = new ClassProperty<string> { Value = "1337" };

            var result = converter(input);

            Assert.IsInstanceOf<Dictionary<string, int>>(result);
            Assert.AreEqual(1337, result["Value"]);
        }

        [Test]
        public void CreateDelegate_ConvertNameValueCollectionToDictionary()
        {
            var converter = ConverterFactory.CreateDelegate<NameValueCollection, Dictionary<string, int>>();
            var input = new NameValueCollection { { "Value1", "1" }, { "Value2", "2" }, { "Value3", "3" } };

            var result = converter(input);

            Assert.IsInstanceOf<Dictionary<string, int>>(result);
            Assert.AreEqual(1, result["Value1"]);
            Assert.AreEqual(2, result["Value2"]);
            Assert.AreEqual(3, result["Value3"]);
        }

        [Test]
        public void CreateDelegate_ConvertDictionaryToNameValueCollection()
        {
            var converter = ConverterFactory.CreateDelegate<Dictionary<string, int>, NameValueCollection>();
            var input = new Dictionary<string, int> { { "Value1", 1 }, { "Value2", 2 }, { "Value3", 3 } };

            var result = converter(input);

            Assert.IsInstanceOf<NameValueCollection>(result);
            Assert.AreEqual("1", result["Value1"]);
            Assert.AreEqual("2", result["Value2"]);
            Assert.AreEqual("3", result["Value3"]);
        }

        [Test]
        public void CreateDelegate_ConvertNameValueCollectionToPerson_ShouldReturnPersonWithCorrectValues()
        {
            var converter = ConverterFactory.CreateDelegate<NameValueCollection, Person>();
            var person = new Person
            {
                Id = Guid.NewGuid(),
                Name = "Magnus",
                Age = 37,
                Length = 182.5m,
                BirthDate = new DateTime(1977, 03, 04)
            };
            var input = new NameValueCollection();
            input.Add("Id", person.Id.ToString());
            input.Add("Name", person.Name);
            input.Add("Age", person.Age.ToString());
            input.Add("Length", person.Length.ToString());
            input.Add("BirthDate", person.BirthDate.ToString());

            var result = converter(input);

            Assert.IsInstanceOf<Person>(result);
            Assert.AreEqual(person.Id, result.Id);
            Assert.AreEqual(person.Name, result.Name);
            Assert.AreEqual(person.Age, result.Age);
            Assert.AreEqual(person.Length, result.Length);
            Assert.AreEqual(person.BirthDate, result.BirthDate);
        }

        [Test]
        public void CreateDelegate_ConvertPersonToNameValueCollection()
        {
            var converter = ConverterFactory.CreateDelegate<Person, NameValueCollection>();
            var person = new Person
            {
                Id = Guid.NewGuid(),
                Name = "Magnus",
                Age = 37,
                Length = 182.5m,
                BirthDate = new DateTime(1977, 03, 04)
            };

            var result = converter(person);

            Assert.IsInstanceOf<NameValueCollection>(result);
            Assert.AreEqual(person.Id.ToString(), result["Id"]);
            Assert.AreEqual(person.Name, result["Name"]);
            Assert.AreEqual(person.Age.ToString(), result["Age"]);
            Assert.AreEqual(person.Length.ToString(), result["Length"]);
            Assert.AreEqual(person.BirthDate.ToString(), result["BirthDate"]);
        }

        [Test]
        public void CreateDelegate_ConvertPersonToPersonDto_ShouldReturnInstanceOfToTypeWithCorrectValues()
        {
            var converter = ConverterFactory.CreateDelegate<Person, PersonDto>();
            var person = new Person
                             {
                                 Id = Guid.NewGuid(),
                                 Name = "Magnus",
                                 Age = 37,
                                 Length = 182.5m,
                                 BirthDate = new DateTime(1977, 03, 04)
                             };
            var result = converter(person);

            Assert.IsInstanceOf<PersonDto>(result);
            Assert.AreEqual(person.Id, result.Id);
            Assert.AreEqual(person.Name, result.Name);
            Assert.AreEqual(person.Age, result.Age);
            Assert.AreEqual(person.Length, result.Length);
            Assert.AreEqual(person.BirthDate, result.BirthDate);
        }

        [Test]
        public void CreateDelegate_ConvertPersonStringDtoToPerson_ShouldReturnInstanceOfToTypeWithCorrectValues()
        {
            var expectedDecimal = 182.5m;
            var guidStr = "0cb6c00f-fc44-484f-8ddd-823709b74601";
            var converter = ConverterFactory.CreateDelegate<PersonStringDto, Person>();
            var person = new PersonStringDto
            {
                Id = guidStr,
                Name = "Magnus",
                Age = "37",
                Length = expectedDecimal.ToString(),
                BirthDate = "1977-03-04"
            };
            var result = converter(person);

            Assert.AreEqual(new Guid(guidStr), result.Id);
            Assert.AreEqual("Magnus", result.Name);
            Assert.AreEqual(37, result.Age);
            Assert.AreEqual(expectedDecimal, result.Length);
            Assert.AreEqual(DateTime.Parse("1977-03-04"), result.BirthDate);
        }

        [Test]
        public void CreateDelegate_ConvertPersonStructToPerson_ShouldReturnInstanceOfToTypeWithCorrectValues()
        {
            var converter = ConverterFactory.CreateDelegate<PersonStruct, Person>();
            var person = new PersonStruct
            {
                Id = Guid.NewGuid(),
                Name = "Magnus",
                Age = 37,
                Length = 182.5m,
                BirthDate = new DateTime(1977, 03, 04)
            };
            var result = converter(person);

            Assert.IsInstanceOf<Person>(result);
            Assert.AreEqual(person.Id, result.Id);
            Assert.AreEqual(person.Name, result.Name);
            Assert.AreEqual(person.Age, result.Age);
            Assert.AreEqual(person.Length, result.Length);
            Assert.AreEqual(person.BirthDate, result.BirthDate);
        }

        [Test]
        public void CreateDelegate_ConvertPersonToPersonStruct_ShouldReturnInstanceOfToTypeWithCorrectValues()
        {
            var converter = ConverterFactory.CreateDelegate<Person, PersonStruct>();
            var person = new Person
            {
                Id = Guid.NewGuid(),
                Name = "Magnus",
                Age = 37,
                Length = 182.5m,
                BirthDate = new DateTime(1977, 03, 04)
            };
            var result = converter(person);

            Assert.IsInstanceOf<PersonStruct>(result);
            Assert.AreEqual(person.Id, result.Id);
            Assert.AreEqual(person.Name, result.Name);
            Assert.AreEqual(person.Age, result.Age);
            Assert.AreEqual(person.Length, result.Length);
            Assert.AreEqual(person.BirthDate, result.BirthDate);
        }

        [Test]
        public void CreateDelegate_ConvertPersonToPersonStringDto_ShouldReturnInstanceOfToTypeWithCorrectValues()
        {
            var expectedDecimal = 182.5m;
            var guidStr = "0cb6c00f-fc44-484f-8ddd-823709b74601";
            var converter = ConverterFactory.CreateDelegate<Person, PersonStringDto>();
            var person = new Person
            {
                Id = new Guid(guidStr),
                Name = "Magnus",
                Age = 37,
                Length = expectedDecimal,
                BirthDate = DateTime.Parse("1977-03-04")
            };
            var result = converter(person);

            Assert.AreEqual(guidStr, result.Id);
            Assert.AreEqual("Magnus", result.Name);
            Assert.AreEqual("37", result.Age);
            Assert.AreEqual(expectedDecimal.ToString(), result.Length);
            Assert.AreEqual(new DateTime(1977, 03, 04).ToString(), result.BirthDate);
        }

        private TTo AssertConverterOutput<TFrom, TTo>(TFrom input)
        {
            return this.AssertConverterOutput<TFrom, TTo>(input, CultureInfo.CurrentCulture);
        }

        private TTo AssertConverterOutput<TFrom, TTo>(TFrom input, IFormatProvider provider)
        {
            var converter = ConverterFactory.CreateDelegate<TFrom, TTo>(provider);
            var expected = converter(input);
            var converter1 = ConverterFactory.CreateDelegate<ClassProperty<TFrom>, ClassProperty<TTo>>(provider);
            var converter2 = ConverterFactory.CreateDelegate<ClassProperty<TFrom>, ClassField<TTo>>(provider);
            var converter3 = ConverterFactory.CreateDelegate<ClassProperty<TFrom>, StructProperty<TTo>>(provider);
            var converter4 = ConverterFactory.CreateDelegate<ClassProperty<TFrom>, StructField<TTo>>(provider);
            var converter5 = ConverterFactory.CreateDelegate<ClassField<TFrom>, ClassProperty<TTo>>(provider);
            var converter6 = ConverterFactory.CreateDelegate<ClassField<TFrom>, ClassField<TTo>>(provider);
            var converter7 = ConverterFactory.CreateDelegate<ClassField<TFrom>, StructProperty<TTo>>(provider);
            var converter8 = ConverterFactory.CreateDelegate<ClassField<TFrom>, StructField<TTo>>(provider);
            var converter9 = ConverterFactory.CreateDelegate<StructProperty<TFrom>, ClassProperty<TTo>>(provider);
            var converter10 = ConverterFactory.CreateDelegate<StructProperty<TFrom>, ClassField<TTo>>(provider);
            var converter11 = ConverterFactory.CreateDelegate<StructProperty<TFrom>, StructProperty<TTo>>(provider);
            var converter12 = ConverterFactory.CreateDelegate<StructProperty<TFrom>, StructField<TTo>>(provider);
            var converter13 = ConverterFactory.CreateDelegate<StructField<TFrom>, ClassProperty<TTo>>(provider);
            var converter14 = ConverterFactory.CreateDelegate<StructField<TFrom>, ClassField<TTo>>(provider);
            var converter15 = ConverterFactory.CreateDelegate<StructField<TFrom>, StructProperty<TTo>>(provider);
            var converter16 = ConverterFactory.CreateDelegate<StructField<TFrom>, StructField<TTo>>(provider);

            Assert.AreEqual(expected, converter1(new ClassProperty<TFrom> { Value = input }).Value, string.Format("ClassProperty<{0}> to ClassProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
            Assert.AreEqual(expected, converter2(new ClassProperty<TFrom> { Value = input }).Value, string.Format("ClassProperty<{0}> to ClassField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
            Assert.AreEqual(expected, converter3(new ClassProperty<TFrom> { Value = input }).Value, string.Format("ClassProperty<{0}> to StructProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
            Assert.AreEqual(expected, converter4(new ClassProperty<TFrom> { Value = input }).Value, string.Format("ClassProperty<{0}> to StructField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
            Assert.AreEqual(expected, converter5(new ClassField<TFrom> { Value = input }).Value, string.Format("ClassField<{0}> to ClassProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
            Assert.AreEqual(expected, converter6(new ClassField<TFrom> { Value = input }).Value, string.Format("ClassField<{0}> to ClassField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
            Assert.AreEqual(expected, converter7(new ClassField<TFrom> { Value = input }).Value, string.Format("ClassField<{0}> to StructProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
            Assert.AreEqual(expected, converter8(new ClassField<TFrom> { Value = input }).Value, string.Format("ClassField<{0}> to StructField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
            Assert.AreEqual(expected, converter9(new StructProperty<TFrom> { Value = input }).Value, string.Format("StructProperty<{0}> to ClassProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
            Assert.AreEqual(expected, converter10(new StructProperty<TFrom> { Value = input }).Value, string.Format("StructProperty<{0}> to ClassField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
            Assert.AreEqual(expected, converter11(new StructProperty<TFrom> { Value = input }).Value, string.Format("StructProperty<{0}> to StructProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
            Assert.AreEqual(expected, converter12(new StructProperty<TFrom> { Value = input }).Value, string.Format("StructProperty<{0}> to StructField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
            Assert.AreEqual(expected, converter13(new StructField<TFrom> { Value = input }).Value, string.Format("StructField<{0}> to ClassProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
            Assert.AreEqual(expected, converter14(new StructField<TFrom> { Value = input }).Value, string.Format("StructField<{0}> to ClassField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
            Assert.AreEqual(expected, converter15(new StructField<TFrom> { Value = input }).Value, string.Format("StructField<{0}> to StructProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
            Assert.AreEqual(expected, converter16(new StructField<TFrom> { Value = input }).Value, string.Format("StructField<{0}> to StructField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));

            return expected;
        }
    }
}
