namespace SafeMapper.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.Globalization;

    using NUnit.Framework;

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
        /*   Misc                                                              
        /*                                                                      
        /************************************************************************/

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
