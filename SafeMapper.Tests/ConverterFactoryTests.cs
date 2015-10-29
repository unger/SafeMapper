//namespace SafeMapper.Tests
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Collections.Specialized;
//    using System.Data.SqlTypes;
//    using System.Globalization;
//    using System.Linq;
//    using System.Threading;

//    using Xunit;

//    using SafeMapper.Configuration;
//    using SafeMapper.Tests.Model.Circular;
//    using SafeMapper.Tests.Model.Enums;
//    using SafeMapper.Tests.Model.GenericClasses;
//    using SafeMapper.Tests.Model.Person;
//    using SafeMapper.Utils;

    
//    public class ConverterFactoryTests
//    {
//        private IFormatProvider numberFormatProvider;

//        private ConverterFactory converterFactory;

//        public ConverterFactoryTests()
//        {
//            var numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();

//            numberFormat.NumberDecimalSeparator = ".";
//            numberFormat.CurrencyDecimalSeparator = ".";
//            numberFormat.NumberGroupSeparator = " ";
//            numberFormat.CurrencyGroupSeparator = " ";
//            this.numberFormatProvider = numberFormat;

//            this.converterFactory = new ConverterFactory(new MapConfiguration());
//        }

//        /************************************************************************/
//        /*                                                                      
//        /*   String                                                              
//        /*                                                                      
//        /************************************************************************/

//        [TestCaseSource(typeof(TestData), "StringToStringData")]
//        public string CreateDelegate_StringToString(string input)
//        {
//            var converter = this.converterFactory.CreateDelegate<string, string>();

//            return converter(input);
//        }

//        [TestCaseSource(typeof(TestData), "StringToStringData")]
//        public string CreateDelegate_StringMemberToStringMember(string input)
//        {
//            return this.AssertConverterOutput<string, string>(input);
//        }

//        [TestCaseSource(typeof(TestData), "StringToIntData")]
//        public int CreateDelegate_StringToInt(string input)
//        {
//            var converter = this.converterFactory.CreateDelegate<string, int>();

//            return converter(input);
//        }

//        [TestCaseSource(typeof(TestData), "StringToIntData")]
//        public int CreateDelegate_StringMemberToIntMember(string input)
//        {
//            return this.AssertConverterOutput<string, int>(input);
//        }

//        [TestCaseSource(typeof(TestData), "StringToGuidData")]
//        public Guid CreateDelegate_StringToGuid(string input)
//        {
//            var converter = this.converterFactory.CreateDelegate<string, Guid>();

//            return converter(input);
//        }

//        [TestCaseSource(typeof(TestData), "StringToGuidData")]
//        public Guid CreateDelegate_StringMemberToGuidMember(string input)
//        {
//            return this.AssertConverterOutput<string, Guid>(input);
//        }

//        [TestCaseSource(typeof(TestData), "StringToDecimalData")]
//        public decimal CreateDelegate_StringToDecimal(string input)
//        {
//            var converter = this.converterFactory.CreateDelegate<string, decimal>(this.numberFormatProvider);

//            return converter(input);
//        }

//        [TestCaseSource(typeof(TestData), "StringToDecimalData")]
//        public decimal CreateDelegate_StringMemberToDecimalMember(string input)
//        {
//            return this.AssertConverterOutput<string, decimal>(input, this.numberFormatProvider);
//        }

//        [TestCaseSource(typeof(TestData), "StringToDateTimeData")]
//        public DateTime CreateDelegate_StringToDateTime(string input)
//        {
//            var converter = this.converterFactory.CreateDelegate<string, DateTime>();

//            return converter(input);
//        }

//        [TestCaseSource(typeof(TestData), "StringToDateTimeData")]
//        public DateTime CreateDelegate_StringMemberToDateTimeMember(string input)
//        {
//            return this.AssertConverterOutput<string, DateTime>(input);
//        }

//        /************************************************************************/
//        /*                                                                      
//        /*   Int                                                              
//        /*                                                                      
//        /************************************************************************/

//        [TestCaseSource(typeof(TestData), "IntToIntData")]
//        public int CreateDelegate_IntToInt(int input)
//        {
//            var converter = this.converterFactory.CreateDelegate<int, int>();

//            return converter(input);
//        }

//        [TestCaseSource(typeof(TestData), "IntToIntData")]
//        public int CreateDelegate_IntMemberToIntMember(int input)
//        {
//            return this.AssertConverterOutput<int, int>(input);
//        }

//        [TestCaseSource(typeof(TestData), "IntToStringData")]
//        public string CreateDelegate_IntToString(int input)
//        {
//            var converter = this.converterFactory.CreateDelegate<int, string>();

//            return converter(input);
//        }

//        [TestCaseSource(typeof(TestData), "IntToStringData")]
//        public string CreateDelegate_IntMemberToStringMember(int input)
//        {
//            return this.AssertConverterOutput<int, string>(input);
//        }

//        [TestCaseSource(typeof(TestData), "IntToLongData")]
//        public long CreateDelegate_IntToLong(int input)
//        {
//            var converter = this.converterFactory.CreateDelegate<int, long>();

//            return converter(input);
//        }

//        /************************************************************************/
//        /*                                                                      
//        /*   Guid                                                              
//        /*                                                                      
//        /************************************************************************/

//        [TestCaseSource(typeof(TestData), "GuidToStringData")]
//        public string CreateDelegate_GuidToString(Guid input)
//        {
//            var converter = this.converterFactory.CreateDelegate<Guid, string>();

//            return converter(input);
//        }

//        [TestCaseSource(typeof(TestData), "GuidToStringData")]
//        public string CreateDelegate_GuidMemberToStringMember(Guid input)
//        {
//            return this.AssertConverterOutput<Guid, string>(input);
//        }

//        /************************************************************************/
//        /*                                                                      
//        /*   Decimal                                                              
//        /*                                                                      
//        /************************************************************************/

//        [TestCaseSource(typeof(TestData), "DecimalToDecimalData")]
//        public decimal CreateDelegate_DecimalToDecimal(decimal input)
//        {
//            var converter = this.converterFactory.CreateDelegate<decimal, decimal>(this.numberFormatProvider);

//            return converter(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToStringData")]
//        public string CreateDelegate_DecimalToString(decimal input)
//        {
//            var converter = this.converterFactory.CreateDelegate<decimal, string>(this.numberFormatProvider);

//            return converter(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToStringData")]
//        public string CreateDelegate_DecimalMemberToStringMember(decimal input)
//        {
//            return this.AssertConverterOutput<decimal, string>(input, this.numberFormatProvider);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToDoubleData")]
//        public double CreateDelegate_DecimalToDouble(decimal input)
//        {
//            var converter = this.converterFactory.CreateDelegate<decimal, double>(this.numberFormatProvider);

//            return converter(input);
//        }

//        [TestCaseSource(typeof(TestData), "DecimalToDoubleData")]
//        public double CreateDelegate_DecimalMemberToDoubleMember(decimal input)
//        {
//            return this.AssertConverterOutput<decimal, double>(input, this.numberFormatProvider);
//        }

//        /************************************************************************/
//        /*                                                                      
//        /*   Double                                                              
//        /*                                                                      
//        /************************************************************************/

//        [TestCaseSource(typeof(TestData), "DoubleToDecimalData")]
//        public decimal CreateDelegate_DoubleToDecimal(double input)
//        {
//            var converter = this.converterFactory.CreateDelegate<double, decimal>(this.numberFormatProvider);

//            return converter(input);
//        }

//        [TestCaseSource(typeof(TestData), "DoubleToDecimalData")]
//        public decimal CreateDelegate_DoubleMemberToDecimalMember(double input)
//        {
//            return this.AssertConverterOutput<double, decimal>(input, this.numberFormatProvider);
//        }

//        /************************************************************************/
//        /*                                                                      
//        /*   DateTime                                                              
//        /*                                                                      
//        /************************************************************************/

//        [TestCaseSource(typeof(TestData), "DateTimeToStringData")]
//        public string CreateDelegate_DateTimeToString(DateTime input)
//        {
//            var converter = this.converterFactory.CreateDelegate<DateTime, string>(this.numberFormatProvider);

//            return converter(input);
//        }

//        [TestCaseSource(typeof(TestData), "DateTimeToStringData")]
//        public string CreateDelegate_DateTimeMemberToStringMember(DateTime input)
//        {
//            return this.AssertConverterOutput<DateTime, string>(input, this.numberFormatProvider);
//        }

//        /************************************************************************/
//        /*                                                                      
//        /*   Long                                                              
//        /*                                                                      
//        /************************************************************************/

//        [TestCaseSource(typeof(TestData), "LongToIntData")]
//        public int CreateDelegate_LongToInt(long input)
//        {
//            var converter = this.converterFactory.CreateDelegate<long, int>();

//            return converter(input);
//        }

//        /************************************************************************/
//        /*                                                                      
//        /*   Array to array                                                              
//        /*                                                                      
//        /************************************************************************/

//        [Fact]
//        public void CreateDelegate_IntArrayToIntArray()
//        {
//            var converter = this.converterFactory.CreateDelegate<int[], int[]>();
//            var input = new[] { 1, 2, 3, 4, 5 };
//            var result = converter(input);

//            Assert.Equal(input, result);
//        }

//        [Fact]
//        public void CreateDelegate_IntArrayToStringArray()
//        {
//            var converter = this.converterFactory.CreateDelegate<int[], string[]>();
//            var expected = new[] { "1", "2", "3", "4", "5" };
//            var input = new[] { 1, 2, 3, 4, 5 };
//            var result = converter(input);

//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void CreateDelegate_StringArrayToIntArray()
//        {
//            var converter = this.converterFactory.CreateDelegate<string[], int[]>();
//            var input = new[] { "1", "2", "3", "4", "5" };
//            var expected = new[] { 1, 2, 3, 4, 5 };
//            var result = converter(input);

//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void CreateDelegate_IntArrayToDecimalArray()
//        {
//            var converter = this.converterFactory.CreateDelegate<int[], decimal[]>();
//            var input = new[] { 1, 2, 3, 4, 5 };
//            var expected = new[] { 1m, 2m, 3m, 4m, 5m };
//            var result = converter(input);

//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void CreateDelegate_IntArrayMemberToStringArrayMember()
//        {
//            var input = new[] { 1, 2, 3, 4, 5 };

//            this.AssertConverterOutput<int[], string[]>(input);
//        }

//        /************************************************************************/
//        /*                                                                      
//        /*   Array and collections                                                              
//        /*                                                                      
//        /************************************************************************/

//        [Fact]
//        public void CreateDelegate_StringListToStringArray()
//        {
//            var converter = this.converterFactory.CreateDelegate<List<string>, string[]>();
//            var input = new List<string> { "1", "2", "3", "4", "5" };
//            var expected = new[] { "1", "2", "3", "4", "5" };
//            var result = converter(input);

//            Assert.IsAssignableFrom<string[]>(result);
//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void CreateDelegate_StringListToIntArray()
//        {
//            var converter = this.converterFactory.CreateDelegate<List<string>, int[]>();
//            var input = new List<string> { "1", "2", "3", "4", "5" };
//            var expected = new[] { 1, 2, 3, 4, 5 };
//            var result = converter(input);

//            Assert.IsAssignableFrom<int[]>(result);
//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void CreateDelegate_StringListToIntList()
//        {
//            var converter = this.converterFactory.CreateDelegate<List<string>, List<int>>();
//            var expected = new List<int> { 1, 2, 3, 4, 5 };
//            var input = new List<string> { "1", "2", "3", "4", "5" };
//            var result = converter(input);

//            Assert.IsAssignableFrom<List<int>>(result);
//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void CreateDelegate_StringArrayToIntList()
//        {
//            var converter = this.converterFactory.CreateDelegate<string[], List<int>>();
//            var expected = new List<int> { 1, 2, 3, 4, 5 };
//            var input = new[] { "1", "2", "3", "4", "5" };
//            var result = converter(input);

//            Assert.IsAssignableFrom<List<int>>(result);
//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void CreateDelegate_IntListToDecimalArray()
//        {
//            var converter = this.converterFactory.CreateDelegate<List<int>, decimal[]>();
//            var input = new List<int> { 1, 2, 3, 4, 5 };
//            var expected = new[] { 1m, 2m, 3m, 4m, 5m };
//            var result = converter(input);

//            Assert.IsAssignableFrom<decimal[]>(result);
//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void CreateDelegate_IntListToDecimalList()
//        {
//            var converter = this.converterFactory.CreateDelegate<List<int>, List<decimal>>();
//            var expected = new List<decimal> { 1m, 2m, 3m, 4m, 5m };
//            var input = new List<int> { 1, 2, 3, 4, 5 };
//            var result = converter(input);

//            Assert.IsAssignableFrom<List<decimal>>(result);
//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void CreateDelegate_IntArrayToDecimalList()
//        {
//            var converter = this.converterFactory.CreateDelegate<int[], List<decimal>>();
//            var expected = new List<decimal> { 1m, 2m, 3m, 4m, 5m };
//            var input = new[] { 1, 2, 3, 4, 5 };
//            var result = converter(input);

//            Assert.IsAssignableFrom<List<decimal>>(result);
//            Assert.Equal(expected, result);
//        }

//        /************************************************************************/
//        /*                                                                      
//        /*   Interface collection                                                              
//        /*                                                                      
//        /************************************************************************/

//        [Fact]
//        public void CreateDelegate_StringICollectionToStringArray()
//        {
//            var converter = this.converterFactory.CreateDelegate<ICollection<string>, string[]>();
//            var input = new List<string> { "1", "2", "3", "4", "5" } as ICollection<string>;
//            var expected = new[] { "1", "2", "3", "4", "5" };
//            var result = converter(input);

//            Assert.IsAssignableFrom<string[]>(result);
//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void CreateDelegate_StringArrayToStringICollection()
//        {
//            var converter = this.converterFactory.CreateDelegate<string[], ICollection<string>>();
//            var input = new[] { "1", "2", "3", "4", "5" };
//            var expected = new List<string> { "1", "2", "3", "4", "5" } as ICollection<string>;
//            var result = converter(input);

//            Assert.IsAssignableFrom<ICollection<string>>(result);
//            Assert.Equal(expected, result);
//        }

//        /************************************************************************/
//        /*                                                                      
//        /*   Convert From Enum                                                              
//        /*                                                                      
//        /************************************************************************/

//        [TestCase(ByteEnum.Undefined, ExpectedResult = (byte)0)]
//        [TestCase(ByteEnum.Value1, ExpectedResult = (byte)1)]
//        [TestCase(ByteEnum.Value2, ExpectedResult = (byte)2)]
//        [TestCase(ByteEnum.Value3, ExpectedResult = (byte)3)]
//        public byte ToByte_FromByteEnum(ByteEnum input)
//        {
//            var converter = this.converterFactory.CreateDelegate<ByteEnum, byte>();
//            return converter(input);
//        }

//        [TestCase(ByteEnum.Undefined, ExpectedResult = (long)0)]
//        [TestCase(ByteEnum.Value1, ExpectedResult = (long)1)]
//        [TestCase(ByteEnum.Value2, ExpectedResult = (long)2)]
//        [TestCase(ByteEnum.Value3, ExpectedResult = (long)3)]
//        public long ToInt64_FromByteEnum(ByteEnum input)
//        {
//            var converter = this.converterFactory.CreateDelegate<ByteEnum, long>();
//            return converter(input);
//        }

//        [TestCase(Int64Enum.Undefined, ExpectedResult = (byte)0)]
//        [TestCase(Int64Enum.Value1, ExpectedResult = (byte)1)]
//        [TestCase(Int64Enum.Min, ExpectedResult = (byte)0)]
//        [TestCase(Int64Enum.Max, ExpectedResult = (byte)0)]
//        public byte ToByte_FromInt64Enum(Int64Enum input)
//        {
//            var converter = this.converterFactory.CreateDelegate<Int64Enum, byte>();
//            return converter(input);
//        }

//        /************************************************************************/
//        /*                                                                      
//        /*   Convert To Enum                                                              
//        /*                                                                      
//        /************************************************************************/

//        [TestCase("Undefined", ExpectedResult = ByteEnum.Undefined)]
//        [TestCase("Value1", ExpectedResult = ByteEnum.Value1)]
//        [TestCase("Value2", ExpectedResult = ByteEnum.Value2)]
//        [TestCase("Value3", ExpectedResult = ByteEnum.Value3)]
//        [TestCase("Value4", ExpectedResult = ByteEnum.Undefined)]
//        public ByteEnum ToByteEnum_FromString(string input)
//        {
//            var converter = this.converterFactory.CreateDelegate<string, ByteEnum>();
//            return converter(input);
//        }

//        [TestCase((byte)0, ExpectedResult = ByteEnum.Undefined)]
//        [TestCase((byte)1, ExpectedResult = ByteEnum.Value1)]
//        [TestCase((byte)2, ExpectedResult = ByteEnum.Value2)]
//        [TestCase((byte)3, ExpectedResult = ByteEnum.Value3)]
//        [TestCase((byte)4, ExpectedResult = ByteEnum.Undefined)]
//        public ByteEnum ToByteEnum_FromByte(byte input)
//        {
//            var converter = this.converterFactory.CreateDelegate<byte, ByteEnum>();
//            return converter(input);
//        }

//        [TestCase((byte)0, ExpectedResult = SByteEnum.Undefined)]
//        [TestCase((byte)1, ExpectedResult = SByteEnum.Value1)]
//        [TestCase((byte)2, ExpectedResult = SByteEnum.Value2)]
//        [TestCase((byte)3, ExpectedResult = SByteEnum.Value3)]
//        [TestCase((byte)4, ExpectedResult = SByteEnum.Undefined)]
//        public SByteEnum ToSByteEnum_FromByte(byte input)
//        {
//            var converter = this.converterFactory.CreateDelegate<byte, SByteEnum>();
//            return converter(input);
//        }

//        [TestCase((byte)0, ExpectedResult = Int16Enum.Undefined)]
//        [TestCase((byte)1, ExpectedResult = Int16Enum.Value1)]
//        [TestCase((byte)2, ExpectedResult = Int16Enum.Value2)]
//        [TestCase((byte)3, ExpectedResult = Int16Enum.Value3)]
//        [TestCase((byte)4, ExpectedResult = Int16Enum.Undefined)]
//        public Int16Enum ToInt16Enum_FromByte(byte input)
//        {
//            var converter = this.converterFactory.CreateDelegate<byte, Int16Enum>();
//            return converter(input);
//        }

//        [TestCase((byte)0, ExpectedResult = UInt16Enum.Undefined)]
//        [TestCase((byte)1, ExpectedResult = UInt16Enum.Value1)]
//        [TestCase((byte)2, ExpectedResult = UInt16Enum.Value2)]
//        [TestCase((byte)3, ExpectedResult = UInt16Enum.Value3)]
//        [TestCase((byte)4, ExpectedResult = UInt16Enum.Undefined)]
//        public UInt16Enum ToUInt16Enum_FromByte(byte input)
//        {
//            var converter = this.converterFactory.CreateDelegate<byte, UInt16Enum>();
//            return converter(input);
//        }

//        [TestCase(-1, ExpectedResult = Int32Enum.Undefined)]
//        [TestCase(0, ExpectedResult = Int32Enum.Undefined)]
//        [TestCase(1, ExpectedResult = Int32Enum.Value1)]
//        [TestCase(2, ExpectedResult = Int32Enum.Value2)]
//        [TestCase(3, ExpectedResult = Int32Enum.Value3)]
//        [TestCase(4, ExpectedResult = Int32Enum.Undefined)]
//        public Int32Enum ToInt32Enum_FromInt32(int input)
//        {
//            var converter = this.converterFactory.CreateDelegate<int, Int32Enum>();
//            return converter(input);
//        }

//        [TestCase(-1, ExpectedResult = UInt32Enum.Undefined)]
//        [TestCase(0, ExpectedResult = UInt32Enum.Undefined)]
//        [TestCase(1, ExpectedResult = UInt32Enum.Value1)]
//        [TestCase(2, ExpectedResult = UInt32Enum.Value2)]
//        [TestCase(3, ExpectedResult = UInt32Enum.Value3)]
//        [TestCase(4, ExpectedResult = UInt32Enum.Undefined)]
//        public UInt32Enum ToUInt32Enum_FromInt32(int input)
//        {
//            var converter = this.converterFactory.CreateDelegate<int, UInt32Enum>();
//            return converter(input);
//        }

//        [TestCase(-1L, ExpectedResult = Int32Enum.Undefined)]
//        [TestCase(0L, ExpectedResult = Int32Enum.Undefined)]
//        [TestCase(1L, ExpectedResult = Int32Enum.Value1)]
//        [TestCase(2L, ExpectedResult = Int32Enum.Value2)]
//        [TestCase(3L, ExpectedResult = Int32Enum.Value3)]
//        [TestCase(4L, ExpectedResult = Int32Enum.Undefined)]
//        public Int32Enum ToInt32Enum_FromInt64(long input)
//        {
//            var converter = this.converterFactory.CreateDelegate<long, Int32Enum>();
//            return converter(input);
//        }

//        [TestCase(-1L, ExpectedResult = Int64Enum.Undefined)]
//        [TestCase(0L, ExpectedResult = Int64Enum.Undefined)]
//        [TestCase(1L, ExpectedResult = Int64Enum.Value1)]
//        [TestCase(long.MinValue, ExpectedResult = Int64Enum.Min)]
//        [TestCase(long.MaxValue, ExpectedResult = Int64Enum.Max)]
//        [TestCase(4L, ExpectedResult = Int64Enum.Undefined)]
//        public Int64Enum ToInt64Enum_FromInt64(long input)
//        {
//            var converter = this.converterFactory.CreateDelegate<long, Int64Enum>();
//            return converter(input);
//        }

//        [TestCase(-1L, ExpectedResult = UInt64Enum.Undefined)]
//        [TestCase(0L, ExpectedResult = UInt64Enum.Undefined)]
//        [TestCase(1L, ExpectedResult = UInt64Enum.Value1)]
//        [TestCase(long.MinValue, ExpectedResult = UInt64Enum.Undefined)]
//        [TestCase(long.MaxValue, ExpectedResult = UInt64Enum.Undefined)]
//        [TestCase(4L, ExpectedResult = UInt64Enum.Undefined)]
//        public UInt64Enum ToUInt64Enum_FromInt64(long input)
//        {
//            var converter = this.converterFactory.CreateDelegate<long, UInt64Enum>();
//            return converter(input);
//        }

//        [TestCase(0UL, ExpectedResult = UInt64Enum.Undefined)]
//        [TestCase(1UL, ExpectedResult = UInt64Enum.Value1)]
//        [TestCase(ulong.MinValue, ExpectedResult = UInt64Enum.Undefined)]
//        [TestCase(ulong.MaxValue, ExpectedResult = UInt64Enum.Max)]
//        [TestCase(4UL, ExpectedResult = UInt64Enum.Undefined)]
//        public UInt64Enum ToUInt64Enum_FromUInt64(ulong input)
//        {
//            var converter = this.converterFactory.CreateDelegate<ulong, UInt64Enum>();
//            return converter(input);
//        }

//        [TestCase("Odefinerat", ExpectedResult = DisplayAttributeResourceEnum.Undefined)]
//        [TestCase("Värde 1", ExpectedResult = DisplayAttributeResourceEnum.Value1)]
//        [TestCase("Värde 2", ExpectedResult = DisplayAttributeResourceEnum.Value2)]
//        [TestCase("Värde 3", ExpectedResult = DisplayAttributeResourceEnum.Value3)]
//        [TestCase("Värde 4", ExpectedResult = DisplayAttributeResourceEnum.Undefined)]
//        public DisplayAttributeResourceEnum ToDisplayAttributeResourceEnum_FromString_CultureSv(string input)
//        {
//            Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");
//            var converter = this.converterFactory.CreateDelegate<string, DisplayAttributeResourceEnum>();
//            return converter(input);
//        }

//        [TestCase("Undefined", ExpectedResult = DisplayAttributeResourceEnum.Undefined)]
//        [TestCase("Value 1", ExpectedResult = DisplayAttributeResourceEnum.Value1)]
//        [TestCase("Value 2", ExpectedResult = DisplayAttributeResourceEnum.Value2)]
//        [TestCase("Value 3", ExpectedResult = DisplayAttributeResourceEnum.Value3)]
//        [TestCase("Value 4", ExpectedResult = DisplayAttributeResourceEnum.Undefined)]
//        public DisplayAttributeResourceEnum ToDisplayAttributeResourceEnum_FromString_CultureEn(string input)
//        {
//            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
//            var converter = this.converterFactory.CreateDelegate<string, DisplayAttributeResourceEnum>();
//            return converter(input);
//        }

//        [TestCase("Odefinerat", ExpectedResult = DisplayAttributeResxEnum.Undefined)]
//        [TestCase("Värde 1", ExpectedResult = DisplayAttributeResxEnum.Value1)]
//        [TestCase("Värde 2", ExpectedResult = DisplayAttributeResxEnum.Value2)]
//        [TestCase("Värde 3", ExpectedResult = DisplayAttributeResxEnum.Value3)]
//        [TestCase("Värde 4", ExpectedResult = DisplayAttributeResxEnum.Undefined)]
//        public DisplayAttributeResxEnum ToDisplayAttributeResxEnum_FromString_CultureSv(string input)
//        {
//            Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");
//            var converter = this.converterFactory.CreateDelegate<string, DisplayAttributeResxEnum>();
//            return converter(input);
//        }

//        [TestCase("Undefined", ExpectedResult = DisplayAttributeResxEnum.Undefined)]
//        [TestCase("Value 1", ExpectedResult = DisplayAttributeResxEnum.Value1)]
//        [TestCase("Value 2", ExpectedResult = DisplayAttributeResxEnum.Value2)]
//        [TestCase("Value 3", ExpectedResult = DisplayAttributeResxEnum.Value3)]
//        [TestCase("Value 4", ExpectedResult = DisplayAttributeResxEnum.Undefined)]
//        public DisplayAttributeResxEnum ToDisplayAttributeResxEnum_FromString_CultureEn(string input)
//        {
//            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
//            var converter = this.converterFactory.CreateDelegate<string, DisplayAttributeResxEnum>();
//            return converter(input);
//        }

//        [TestCase("Undefined", ExpectedResult = DescriptionAttributeEnum.Undefined)]
//        [TestCase("Value 1", ExpectedResult = DescriptionAttributeEnum.Value1)]
//        [TestCase("Value 2", ExpectedResult = DescriptionAttributeEnum.Value2)]
//        [TestCase("Value 3", ExpectedResult = DescriptionAttributeEnum.Value3)]
//        [TestCase("Value 4", ExpectedResult = DescriptionAttributeEnum.Undefined)]
//        public DescriptionAttributeEnum ToDescriptionEnum_FromString(string input)
//        {
//            var converter = this.converterFactory.CreateDelegate<string, DescriptionAttributeEnum>();
//            return converter(input);
//        }

//        [TestCase("Undefined", ExpectedResult = DisplayAttributeEnum.Undefined)]
//        [TestCase("Value 1", ExpectedResult = DisplayAttributeEnum.Value1)]
//        [TestCase("Value 2", ExpectedResult = DisplayAttributeEnum.Value2)]
//        [TestCase("Value 3", ExpectedResult = DisplayAttributeEnum.Value3)]
//        [TestCase("Value 4", ExpectedResult = DisplayAttributeEnum.Undefined)]
//        public DisplayAttributeEnum ToDisplayAttributeEnum_FromString(string input)
//        {
//            var converter = this.converterFactory.CreateDelegate<string, DisplayAttributeEnum>();
//            return converter(input);
//        }

//        [TestCase(DisplayAttributeEnum.Undefined, ExpectedResult = "Undefined")]
//        [TestCase(DisplayAttributeEnum.Value1, ExpectedResult = "Value 1")]
//        [TestCase(DisplayAttributeEnum.Value2, ExpectedResult = "Value 2")]
//        [TestCase(DisplayAttributeEnum.Value3, ExpectedResult = "Value 3")]
//        public string ToString_FromDisplayAttributeEnum(DisplayAttributeEnum input)
//        {
//            var converter = this.converterFactory.CreateDelegate<DisplayAttributeEnum, string>();
//            return converter(input);
//        }

//        [TestCase(DescriptionAttributeEnum.Undefined, ExpectedResult = "Undefined")]
//        [TestCase(DescriptionAttributeEnum.Value1, ExpectedResult = "Value 1")]
//        [TestCase(DescriptionAttributeEnum.Value2, ExpectedResult = "Value 2")]
//        [TestCase(DescriptionAttributeEnum.Value3, ExpectedResult = "Value 3")]
//        public string ToString_FromDisplayAttributeEnum(DescriptionAttributeEnum input)
//        {
//            var converter = this.converterFactory.CreateDelegate<DescriptionAttributeEnum, string>();
//            return converter(input);
//        }

//        [TestCase(-1, ExpectedResult = LargeEnum.Undefined)]
//        [TestCase(0, ExpectedResult = LargeEnum.Undefined)]
//        [TestCase(16, ExpectedResult = LargeEnum.Value16)]
//        public LargeEnum ToLargeEnum_FromInt32(int input)
//        {
//            var converter = this.converterFactory.CreateDelegate<int, LargeEnum>();
//            return converter(input);
//        }

//        /************************************************************************/
//        /*                                                                      
//        /*   Misc                                                              
//        /*                                                                      
//        /************************************************************************/

//        [Fact]
//        public void CreateDelegate_StringToStringArray()
//        {
//            var converter = this.converterFactory.CreateDelegate<string, int[]>();
//            var input = "1";
//            var result = converter(input);

//            Assert.Equal(new[] { 1 }, result);
//        }

//        [Fact]
//        public void CreateDelegate_StringToStringList()
//        {
//            var converter = this.converterFactory.CreateDelegate<string, List<int>>();
//            var input = "1";
//            var result = converter(input);

//            Assert.Equal(new List<int> { 1 }, result);
//        }

//        [Fact]
//        public void CreateDelegate_StringArrayToString()
//        {
//            var converter = this.converterFactory.CreateDelegate<string[], int>();
//            var input = new[] { "1", "2", "3" };
//            var result = converter(input);

//            Assert.Equal(1, result);
//        }

//        [Fact]
//        public void CreateDelegate_StringListToString()
//        {
//            var converter = this.converterFactory.CreateDelegate<List<string>, int>();
//            var input = new List<string> { "1", "2", "3" };
//            var result = converter(input);

//            Assert.Equal(1, result);
//        }

//        [Fact]
//        public void CreateDelegate_NonGenericDateTimeToSqlDateTime()
//        {
//            var converter = this.converterFactory.CreateDelegate(typeof(DateTime), typeof(SqlDateTime));
//            var input = DateTime.MinValue;
//            var result = converter(input);

//            Assert.Equal(SqlDateTime.MinValue, result);
//        }

//        [Fact]
//        public void CreateDelegate_NonGenericIntToString()
//        {
//            var converter = this.converterFactory.CreateDelegate(typeof(int), typeof(string));
//            var input = 1;
//            var result = converter(input);

//            Assert.Equal("1", result);
//        }

//        [Fact]
//        public void CreateDelegate_NonGenericStringToInt()
//        {
//            var converter = this.converterFactory.CreateDelegate(typeof(string), typeof(int));
//            var input = "1";
//            var result = converter(input);

//            Assert.Equal(1, result);
//        }

//        [Fact]
//        public void CreateDelegate_IEnumerableDictionaryToListOfClassProperty()
//        {
//            var input =
//                new List<Dictionary<string, string>>
//                    {
//                        new Dictionary<string, string> { { "Value", "1" } },
//                        new Dictionary<string, string> { { "Value", "12" } },
//                        new Dictionary<string, string> { { "Value", "123" } },
//                        new Dictionary<string, string> { { "Value", "1234" } },
//                    }

//                    as IEnumerable<Dictionary<string, string>>;

//            var converter = this.converterFactory.CreateDelegate<IEnumerable<Dictionary<string, string>>, IList<ClassProperty<int>>>();
//            var result = converter(input);

//            Assert.Equal(4, result.Count);
//            Assert.Equal(1, result[0].Value);
//            Assert.Equal(12, result[1].Value);
//            Assert.Equal(123, result[2].Value);
//            Assert.Equal(1234, result[3].Value);

//        }

//        [TestCaseSource(typeof(TestData), "NonGenericCollectionTestData")]
//        public object CreateDelegate_NonGenericCollectionTestData(object input, Type fromType, Type toType)
//        {
//            var converter = this.converterFactory.CreateDelegate(fromType, toType);

//            return converter(input);
//        }

//        [TestCaseSource(typeof(TestData), "NonGenericTestData")]
//        public object CreateDelegate_NonGenericTestData(object input, Type fromType, Type toType)
//        {
//            var converter = this.converterFactory.CreateDelegate(fromType, toType);

//            return converter(input);
//        }

//        [TestCase(null, ExpectedResult = 0)]
//        [TestCase(0, ExpectedResult = 0)]
//        [TestCase(int.MaxValue, ExpectedResult = int.MaxValue)]
//        [TestCase(int.MinValue, ExpectedResult = int.MinValue)]
//        public int CreateDelegate_NullableIntToInt(int? input)
//        {
//            var converter = this.converterFactory.CreateDelegate<int?, int>();

//            return converter(input);
//        }

//        [TestCase(null, ExpectedResult = 0)]
//        [TestCase((long)0, ExpectedResult = 0)]
//        [TestCase(long.MaxValue, ExpectedResult = 0)]
//        [TestCase(long.MinValue, ExpectedResult = 0)]
//        [TestCase((long)int.MaxValue, ExpectedResult = int.MaxValue)]
//        [TestCase((long)int.MinValue, ExpectedResult = int.MinValue)]
//        public int CreateDelegate_NullableLongToInt(long? input)
//        {
//            var converter = this.converterFactory.CreateDelegate<long?, int>();

//            return converter(input);
//        }

//        [TestCase(null, ExpectedResult = 0)]
//        [TestCase(byte.MaxValue, ExpectedResult = (int)byte.MaxValue)]
//        [TestCase(byte.MinValue, ExpectedResult = (int)byte.MinValue)]
//        public int CreateDelegate_NullableByteToInt(byte? input)
//        {
//            var converter = this.converterFactory.CreateDelegate<long?, int>();

//            return converter(input);
//        }

//        [TestCase(null, ExpectedResult = null)]
//        [TestCase(0, ExpectedResult = "0")]
//        [TestCase(int.MaxValue, ExpectedResult = "2147483647")]
//        [TestCase(int.MinValue, ExpectedResult = "-2147483648")]
//        public string CreateDelegate_NullableIntToString(int? input)
//        {
//            var converter = this.converterFactory.CreateDelegate<int?, string>();

//            return converter(input);
//        }

//        [TestCase(null, ExpectedResult = null)]
//        [TestCase("0", ExpectedResult = 0)]
//        [TestCase("2147483647", ExpectedResult = int.MaxValue)]
//        [TestCase("-2147483648", ExpectedResult = int.MinValue)]
//        public int? CreateDelegate_StringToNullableInt(string input)
//        {
//            var converter = this.converterFactory.CreateDelegate<string, int?>();

//            return converter(input);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertNameValueCollectionToClassPropertyWithIntArray()
//        {
//            var converter = this.converterFactory.CreateDelegate<NameValueCollection, ClassProperty<int[]>>();
//            var input = new NameValueCollection { { "Value", "1" }, { "Value", "2" }, { "Value", "3" } };

//            var result = converter(input);

//            Assert.IsAssignableFrom<ClassProperty<int[]>>(result);
//            Assert.Equal(new[] { 1, 2, 3 }, result.Value);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertNameValueCollectionToClassPropertyString_ShouldConcatValues()
//        {
//            var converter = this.converterFactory.CreateDelegate<NameValueCollection, ClassProperty<string>>();
//            var input = new NameValueCollection { { "Value", "1" }, { "Value", "2" }, { "Value", "3" } };

//            var result = converter(input);

//            Assert.IsAssignableFrom<ClassProperty<string>>(result);
//            Assert.Equal("1,2,3", result.Value);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertNameValueCollectionToClassPropertyInt_ShouldReturnFirstValue()
//        {
//            var converter = this.converterFactory.CreateDelegate<NameValueCollection, ClassProperty<int>>();
//            var input = new NameValueCollection { { "Value", "1" }, { "Value", "2" }, { "Value", "3" } };

//            var result = converter(input);

//            Assert.IsAssignableFrom<ClassProperty<int>>(result);
//            Assert.Equal(1, result.Value);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertNameValueCollectionWithUnMatchedKey_ShouldReturnObjectWithDefaultValue()
//        {
//            var converter = this.converterFactory.CreateDelegate<NameValueCollection, ClassProperty<int>>();
//            var input = new NameValueCollection { { "Value2", "37" } };

//            var result = converter(input);

//            Assert.IsAssignableFrom<ClassProperty<int>>(result);
//            Assert.Equal(0, result.Value);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertDictionaryToClassProperty_ShouldReturnObjectWithDefaultValue()
//        {
//            var converter = this.converterFactory.CreateDelegate<Dictionary<string, string>, ClassProperty<int>>();
//            var input = new Dictionary<string, string> { { "Value", "37" } };

//            var result = converter(input);

//            Assert.IsAssignableFrom<ClassProperty<int>>(result);
//            Assert.Equal(37, result.Value);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertDictionaryToClassPropertyWithUnmatchedKey_ShouldReturnObjectWithDefaultValue()
//        {
//            var converter = this.converterFactory.CreateDelegate<Dictionary<string, string>, ClassProperty<int>>();
//            var input = new Dictionary<string, string> { { "Value2", "37" } };

//            var result = converter(input);

//            Assert.IsAssignableFrom<ClassProperty<int>>(result);
//            Assert.Equal(0, result.Value);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertClassPropertyWithStringToDictionary()
//        {
//            var converter = this.converterFactory.CreateDelegate<ClassProperty<string>, Dictionary<string, int>>();
//            var input = new ClassProperty<string> { Value = "1337" };

//            var result = converter(input);

//            Assert.IsAssignableFrom<Dictionary<string, int>>(result);
//            Assert.Equal(1337, result["Value"]);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertNameValueCollectionToDictionary()
//        {
//            var converter = this.converterFactory.CreateDelegate<NameValueCollection, Dictionary<string, int>>();
//            var input = new NameValueCollection { { "Value1", "1" }, { "Value2", "2" }, { "Value3", "3" } };

//            var result = converter(input);

//            Assert.IsAssignableFrom<Dictionary<string, int>>(result);
//            Assert.Equal(1, result["Value1"]);
//            Assert.Equal(2, result["Value2"]);
//            Assert.Equal(3, result["Value3"]);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertDictionaryToNameValueCollection()
//        {
//            var converter = this.converterFactory.CreateDelegate<Dictionary<string, int>, NameValueCollection>();
//            var input = new Dictionary<string, int> { { "Value1", 1 }, { "Value2", 2 }, { "Value3", 3 } };

//            var result = converter(input);

//            Assert.IsAssignableFrom<NameValueCollection>(result);
//            Assert.Equal("1", result["Value1"]);
//            Assert.Equal("2", result["Value2"]);
//            Assert.Equal("3", result["Value3"]);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertNameValueCollectionMultiKeysToDictionary()
//        {
//            var converter = this.converterFactory.CreateDelegate<NameValueCollection, Dictionary<string, int[]>>();
//            var input = new NameValueCollection { { "Value", "1" }, { "Value", "2" }, { "Value2", "3" } };

//            var result = converter(input);

//            Assert.IsAssignableFrom<Dictionary<string, int[]>>(result);
//            Assert.Equal(new[] { 1, 2 }, result["Value"]);
//            Assert.Equal(new[] { 3 }, result["Value2"]);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertCircularReference_Parent_Child()
//        {
//            var converter = this.converterFactory.CreateDelegate<Parent, ParentDto>();
//            var input = new Parent();
//            input.Children = new[] { new Child { Parent = input } };

//            var result = converter(input);

//            Assert.IsAssignableFrom<ParentDto>(result);
//            Assert.Equal(1, result.Children.Length);
//            Assert.Null(result.Children[0].Parent);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertNameValueCollectionToPerson_ShouldReturnPersonWithCorrectValues()
//        {
//            var converter = this.converterFactory.CreateDelegate<NameValueCollection, Person>();
//            var person = new Person
//            {
//                Id = Guid.NewGuid(),
//                Name = "Magnus",
//                Age = 37,
//                Length = 182.5m,
//                BirthDate = new DateTime(1977, 03, 04)
//            };
//            var input = new NameValueCollection();
//            input.Add("Id", person.Id.ToString());
//            input.Add("Name", person.Name);
//            input.Add("Age", person.Age.ToString(CultureInfo.CurrentCulture));
//            input.Add("Length", person.Length.ToString(CultureInfo.CurrentCulture));
//            input.Add("BirthDate", person.BirthDate.ToString(CultureInfo.CurrentCulture));

//            var result = converter(input);

//            Assert.IsAssignableFrom<Person>(result);
//            Assert.Equal(person.Id, result.Id);
//            Assert.Equal(person.Name, result.Name);
//            Assert.Equal(person.Age, result.Age);
//            Assert.Equal(person.Length, result.Length);
//            Assert.Equal(person.BirthDate, result.BirthDate);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertPersonToNameValueCollection()
//        {
//            var converter = this.converterFactory.CreateDelegate<Person, NameValueCollection>();
//            var person = new Person
//            {
//                Id = Guid.NewGuid(),
//                Name = "Magnus",
//                Age = 37,
//                Length = 182.5m,
//                BirthDate = new DateTime(1977, 03, 04)
//            };

//            var result = converter(person);

//            Assert.IsAssignableFrom<NameValueCollection>(result);
//            Assert.Equal(person.Id.ToString(), result["Id"]);
//            Assert.Equal(person.Name, result["Name"]);
//            Assert.Equal(person.Age.ToString(CultureInfo.CurrentCulture), result["Age"]);
//            Assert.Equal(person.Length.ToString(CultureInfo.CurrentCulture), result["Length"]);
//            Assert.Equal(person.BirthDate.ToString(CultureInfo.CurrentCulture), result["BirthDate"]);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertPersonToPersonDto_ShouldReturnInstanceOfToTypeWithCorrectValues()
//        {
//            var converter = this.converterFactory.CreateDelegate<Person, PersonDto>();
//            var person = new Person
//                             {
//                                 Id = Guid.NewGuid(),
//                                 Name = "Magnus",
//                                 Age = 37,
//                                 Length = 182.5m,
//                                 BirthDate = new DateTime(1977, 03, 04)
//                             };
//            var result = converter(person);

//            Assert.IsAssignableFrom<PersonDto>(result);
//            Assert.Equal(person.Id, result.Id);
//            Assert.Equal(person.Name, result.Name);
//            Assert.Equal(person.Age, result.Age);
//            Assert.Equal(person.Length, result.Length);
//            Assert.Equal(person.BirthDate, result.BirthDate);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertPersonStringDtoToPerson_ShouldReturnInstanceOfToTypeWithCorrectValues()
//        {
//            var expectedDecimal = 182.5m;
//            var guidStr = "0cb6c00f-fc44-484f-8ddd-823709b74601";
//            var converter = this.converterFactory.CreateDelegate<PersonStringDto, Person>();
//            var person = new PersonStringDto
//            {
//                Id = guidStr,
//                Name = "Magnus",
//                Age = "37",
//                Length = expectedDecimal.ToString(CultureInfo.CurrentCulture),
//                BirthDate = "1977-03-04"
//            };
//            var result = converter(person);

//            Assert.Equal(new Guid(guidStr), result.Id);
//            Assert.Equal("Magnus", result.Name);
//            Assert.Equal(37, result.Age);
//            Assert.Equal(expectedDecimal, result.Length);
//            Assert.Equal(DateTime.Parse("1977-03-04"), result.BirthDate);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertPersonStructToPerson_ShouldReturnInstanceOfToTypeWithCorrectValues()
//        {
//            var converter = this.converterFactory.CreateDelegate<PersonStruct, Person>();
//            var person = new PersonStruct
//            {
//                Id = Guid.NewGuid(),
//                Name = "Magnus",
//                Age = 37,
//                Length = 182.5m,
//                BirthDate = new DateTime(1977, 03, 04)
//            };
//            var result = converter(person);

//            Assert.IsAssignableFrom<Person>(result);
//            Assert.Equal(person.Id, result.Id);
//            Assert.Equal(person.Name, result.Name);
//            Assert.Equal(person.Age, result.Age);
//            Assert.Equal(person.Length, result.Length);
//            Assert.Equal(person.BirthDate, result.BirthDate);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertPersonToPersonStruct_ShouldReturnInstanceOfToTypeWithCorrectValues()
//        {
//            var converter = this.converterFactory.CreateDelegate<Person, PersonStruct>();
//            var person = new Person
//            {
//                Id = Guid.NewGuid(),
//                Name = "Magnus",
//                Age = 37,
//                Length = 182.5m,
//                BirthDate = new DateTime(1977, 03, 04)
//            };
//            var result = converter(person);

//            Assert.IsAssignableFrom<PersonStruct>(result);
//            Assert.Equal(person.Id, result.Id);
//            Assert.Equal(person.Name, result.Name);
//            Assert.Equal(person.Age, result.Age);
//            Assert.Equal(person.Length, result.Length);
//            Assert.Equal(person.BirthDate, result.BirthDate);
//        }

//        [Fact]
//        public void CreateDelegate_ConvertPersonToPersonStringDto_ShouldReturnInstanceOfToTypeWithCorrectValues()
//        {
//            var expectedDecimal = 182.5m;
//            var guidStr = "0cb6c00f-fc44-484f-8ddd-823709b74601";
//            var converter = this.converterFactory.CreateDelegate<Person, PersonStringDto>();
//            var person = new Person
//            {
//                Id = new Guid(guidStr),
//                Name = "Magnus",
//                Age = 37,
//                Length = expectedDecimal,
//                BirthDate = DateTime.Parse("1977-03-04")
//            };
//            var result = converter(person);

//            Assert.Equal(guidStr, result.Id);
//            Assert.Equal("Magnus", result.Name);
//            Assert.Equal("37", result.Age);
//            Assert.Equal(expectedDecimal.ToString(CultureInfo.CurrentCulture), result.Length);
//            Assert.Equal(new DateTime(1977, 03, 04).ToString(CultureInfo.CurrentCulture), result.BirthDate);
//        }

//        private TTo AssertConverterOutput<TFrom, TTo>(TFrom input)
//        {
//            return this.AssertConverterOutput<TFrom, TTo>(input, CultureInfo.CurrentCulture);
//        }

//        private TTo AssertConverterOutput<TFrom, TTo>(TFrom input, IFormatProvider provider)
//        {
//            var converter = this.converterFactory.CreateDelegate<TFrom, TTo>(provider);
//            var expected = converter(input);
//            var converter1 = this.converterFactory.CreateDelegate<ClassProperty<TFrom>, ClassProperty<TTo>>(provider);
//            var converter2 = this.converterFactory.CreateDelegate<ClassProperty<TFrom>, ClassField<TTo>>(provider);
//            var converter3 = this.converterFactory.CreateDelegate<ClassProperty<TFrom>, StructProperty<TTo>>(provider);
//            var converter4 = this.converterFactory.CreateDelegate<ClassProperty<TFrom>, StructField<TTo>>(provider);
//            var converter5 = this.converterFactory.CreateDelegate<ClassField<TFrom>, ClassProperty<TTo>>(provider);
//            var converter6 = this.converterFactory.CreateDelegate<ClassField<TFrom>, ClassField<TTo>>(provider);
//            var converter7 = this.converterFactory.CreateDelegate<ClassField<TFrom>, StructProperty<TTo>>(provider);
//            var converter8 = this.converterFactory.CreateDelegate<ClassField<TFrom>, StructField<TTo>>(provider);
//            var converter9 = this.converterFactory.CreateDelegate<StructProperty<TFrom>, ClassProperty<TTo>>(provider);
//            var converter10 = this.converterFactory.CreateDelegate<StructProperty<TFrom>, ClassField<TTo>>(provider);
//            var converter11 = this.converterFactory.CreateDelegate<StructProperty<TFrom>, StructProperty<TTo>>(provider);
//            var converter12 = this.converterFactory.CreateDelegate<StructProperty<TFrom>, StructField<TTo>>(provider);
//            var converter13 = this.converterFactory.CreateDelegate<StructField<TFrom>, ClassProperty<TTo>>(provider);
//            var converter14 = this.converterFactory.CreateDelegate<StructField<TFrom>, ClassField<TTo>>(provider);
//            var converter15 = this.converterFactory.CreateDelegate<StructField<TFrom>, StructProperty<TTo>>(provider);
//            var converter16 = this.converterFactory.CreateDelegate<StructField<TFrom>, StructField<TTo>>(provider);

//            Assert.Equal(expected, converter1(new ClassProperty<TFrom> { Value = input }).Value, string.Format("ClassProperty<{0}> to ClassProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
//            Assert.Equal(expected, converter2(new ClassProperty<TFrom> { Value = input }).Value, string.Format("ClassProperty<{0}> to ClassField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
//            Assert.Equal(expected, converter3(new ClassProperty<TFrom> { Value = input }).Value, string.Format("ClassProperty<{0}> to StructProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
//            Assert.Equal(expected, converter4(new ClassProperty<TFrom> { Value = input }).Value, string.Format("ClassProperty<{0}> to StructField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
//            Assert.Equal(expected, converter5(new ClassField<TFrom> { Value = input }).Value, string.Format("ClassField<{0}> to ClassProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
//            Assert.Equal(expected, converter6(new ClassField<TFrom> { Value = input }).Value, string.Format("ClassField<{0}> to ClassField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
//            Assert.Equal(expected, converter7(new ClassField<TFrom> { Value = input }).Value, string.Format("ClassField<{0}> to StructProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
//            Assert.Equal(expected, converter8(new ClassField<TFrom> { Value = input }).Value, string.Format("ClassField<{0}> to StructField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
//            Assert.Equal(expected, converter9(new StructProperty<TFrom> { Value = input }).Value, string.Format("StructProperty<{0}> to ClassProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
//            Assert.Equal(expected, converter10(new StructProperty<TFrom> { Value = input }).Value, string.Format("StructProperty<{0}> to ClassField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
//            Assert.Equal(expected, converter11(new StructProperty<TFrom> { Value = input }).Value, string.Format("StructProperty<{0}> to StructProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
//            Assert.Equal(expected, converter12(new StructProperty<TFrom> { Value = input }).Value, string.Format("StructProperty<{0}> to StructField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
//            Assert.Equal(expected, converter13(new StructField<TFrom> { Value = input }).Value, string.Format("StructField<{0}> to ClassProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
//            Assert.Equal(expected, converter14(new StructField<TFrom> { Value = input }).Value, string.Format("StructField<{0}> to ClassField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
//            Assert.Equal(expected, converter15(new StructField<TFrom> { Value = input }).Value, string.Format("StructField<{0}> to StructProperty<{1}>", typeof(TFrom).Name, typeof(TTo).Name));
//            Assert.Equal(expected, converter16(new StructField<TFrom> { Value = input }).Value, string.Format("StructField<{0}> to StructField<{1}>", typeof(TFrom).Name, typeof(TTo).Name));

//            return expected;
//        }
//    }
//}
