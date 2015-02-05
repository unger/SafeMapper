namespace MapEverything.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.Globalization;

    using MapEverything.Tests.Model.Classes;
    using MapEverything.Tests.Model.Person;

    using NUnit.Framework;

    [TestFixture]
    public class FastConvertTests
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
        public string GetConverter_StringToString(string input)
        {
            var converter = FastConvert.GetConverter<string, string>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToStringData")]
        public string GetConverter_StringMemberToStringMember(string input)
        {
            return this.AssertConverterOutput<string, string>(input);
        }

        [TestCaseSource(typeof(TestData), "StringToIntData")]
        public int GetConverter_StringToInt(string input)
        {
            var converter = FastConvert.GetConverter<string, int>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToIntData")]
        public int GetConverter_StringMemberToIntMember(string input)
        {
            return this.AssertConverterOutput<string, int>(input);
        }

        [TestCaseSource(typeof(TestData), "StringToGuidData")]
        public Guid GetConverter_StringToGuid(string input)
        {
            var converter = FastConvert.GetConverter<string, Guid>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToGuidData")]
        public Guid GetConverter_StringMemberToGuidMember(string input)
        {
            return this.AssertConverterOutput<string, Guid>(input);
        }

        [TestCaseSource(typeof(TestData), "StringToDecimalData")]
        public decimal GetConverter_StringToDecimal(string input)
        {
            var converter = FastConvert.GetConverter<string, decimal>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToDecimalData")]
        public decimal GetConverter_StringMemberToDecimalMember(string input)
        {
            return this.AssertConverterOutput<string, decimal>(input, this.numberFormatProvider);
        }

        [TestCaseSource(typeof(TestData), "StringToDateTimeData")]
        public DateTime GetConverter_StringToDateTime(string input)
        {
            var converter = FastConvert.GetConverter<string, DateTime>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToDateTimeData")]
        public DateTime GetConverter_StringMemberToDateTimeMember(string input)
        {
            return this.AssertConverterOutput<string, DateTime>(input);
        }

        /************************************************************************/
        /*                                                                      
        /*   Int                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "IntToIntData")]
        public int GetConverter_IntToInt(int input)
        {
            var converter = FastConvert.GetConverter<int, int>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "IntToIntData")]
        public int GetConverter_IntMemberToIntMember(int input)
        {
            return this.AssertConverterOutput<int, int>(input);
        }


        [TestCaseSource(typeof(TestData), "IntToStringData")]
        public string GetConverter_IntToString(int input)
        {
            var converter = FastConvert.GetConverter<int, string>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "IntToStringData")]
        public string GetConverter_IntMemberToStringMember(int input)
        {
            return this.AssertConverterOutput<int, string>(input);
        }


        [TestCaseSource(typeof(TestData), "IntToLongData")]
        public long GetConverter_IntToLong(int input)
        {
            var converter = FastConvert.GetConverter<int, long>();

            return converter(input);
        }


        /************************************************************************/
        /*                                                                      
        /*   Guid                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "GuidToStringData")]
        public string GetConverter_GuidToString(Guid input)
        {
            var converter = FastConvert.GetConverter<Guid, string>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "GuidToStringData")]
        public string GetConverter_GuidMemberToStringMember(Guid input)
        {
            return this.AssertConverterOutput<Guid, string>(input);
        }

        /************************************************************************/
        /*                                                                      
        /*   Decimal                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "DecimalToDecimalData")]
        public decimal GetConverter_DecimalToDecimal(decimal input)
        {
            var converter = FastConvert.GetConverter<decimal, decimal>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToStringData")]
        public string GetConverter_DecimalToString(decimal input)
        {
            var converter = FastConvert.GetConverter<decimal, string>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToStringData")]
        public string GetConverter_DecimalMemberToStringMember(decimal input)
        {
            return this.AssertConverterOutput<decimal, string>(input, this.numberFormatProvider);
        }

        [TestCaseSource(typeof(TestData), "DecimalToDoubleData")]
        public double GetConverter_DecimalToDouble(decimal input)
        {
            var converter = FastConvert.GetConverter<decimal, double>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToDoubleData")]
        public double GetConverter_DecimalMemberToDoubleMember(decimal input)
        {
            return this.AssertConverterOutput<decimal, double>(input, this.numberFormatProvider);
        }

        /************************************************************************/
        /*                                                                      
        /*   Double                                                              
        /*                                                                      
        /************************************************************************/

        [TestCaseSource(typeof(TestData), "DoubleToDecimalData")]
        public decimal GetConverter_DoubleToDecimal(double input)
        {
            var converter = FastConvert.GetConverter<double, decimal>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "DoubleToDecimalData")]
        public decimal GetConverter_DoubleMemberToDecimalMember(double input)
        {
            return this.AssertConverterOutput<double, decimal>(input, this.numberFormatProvider);
        }



        /************************************************************************/
        /*                                                                      
        /*   DateTime                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "DateTimeToStringData")]
        public string GetConverter_DateTimeToString(DateTime input)
        {
            var converter = FastConvert.GetConverter<DateTime, string>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "DateTimeToStringData")]
        public string GetConverter_DateTimeMemberToStringMember(DateTime input)
        {
            return this.AssertConverterOutput<DateTime, string>(input, this.numberFormatProvider);
        }

        /************************************************************************/
        /*                                                                      
        /*   Long                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "LongToIntData")]
        public int GetConverter_LongToInt(long input)
        {
            var converter = FastConvert.GetConverter<long, int>();

            return converter(input);
        }


        /************************************************************************/
        /*                                                                      
        /*   Array to array                                                              
        /*                                                                      
        /************************************************************************/

        [Test]
        public void GetConverter_IntArrayToIntArray()
        {
            var converter = FastConvert.GetConverter<int[], int[]>();
            var input = new int[] { 1, 2, 3, 4, 5 };
            var result = converter(input);

            Assert.AreEqual(input, result);
        }

        [Test]
        public void GetConverter_IntArrayToStringArray()
        {
            var converter = FastConvert.GetConverter<int[], string[]>();
            var expected = new string[] { "1", "2", "3", "4", "5" };
            var input = new int[] { 1, 2, 3, 4, 5 };
            var result = converter(input);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetConverter_StringArrayToIntArray()
        {
            var converter = FastConvert.GetConverter<string[], int[]>();
            var input = new string[] { "1", "2", "3", "4", "5" };
            var expected = new int[] { 1, 2, 3, 4, 5 };
            var result = converter(input);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetConverter_IntArrayToDecimalArray()
        {
            var converter = FastConvert.GetConverter<int[], decimal[]>();
            var input = new int[] { 1, 2, 3, 4, 5 };
            var expected = new decimal[] { 1m, 2m, 3m, 4m, 5m };
            var result = converter(input);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetConverter_IntArrayMemberToStringArrayMember()
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
        public void GetConverter_StringListToStringArray()
        {
            var converter = FastConvert.GetConverter<List<string>, string[]>();
            var input = new List<string> { "1", "2", "3", "4", "5" };
            var expected = new string[] { "1", "2", "3", "4", "5" };
            var result = converter(input);

            Assert.IsInstanceOf<string[]>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetConverter_StringListToIntArray()
        {
            var converter = FastConvert.GetConverter<List<string>, int[]>();
            var input = new List<string> { "1", "2", "3", "4", "5" };
            var expected = new int[] { 1, 2, 3, 4, 5 };
            var result = converter(input);

            Assert.IsInstanceOf<int[]>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetConverter_StringListToIntList()
        {
            var converter = FastConvert.GetConverter<List<string>, List<int>>();
            var expected = new List<int> { 1, 2, 3, 4, 5 };
            var input = new List<string> { "1", "2", "3", "4", "5" };
            var result = converter(input);

            Assert.IsInstanceOf<List<int>>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetConverter_StringArrayToIntList()
        {
            var converter = FastConvert.GetConverter<string[], List<int>>();
            var expected = new List<int> { 1, 2, 3, 4, 5 };
            var input = new string[] { "1", "2", "3", "4", "5" };
            var result = converter(input);

            Assert.IsInstanceOf<List<int>>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetConverter_IntListToDecimalArray()
        {
            var converter = FastConvert.GetConverter<List<int>, decimal[]>();
            var input = new List<int> { 1, 2, 3, 4, 5 };
            var expected = new decimal[] { 1m, 2m, 3m, 4m, 5m };
            var result = converter(input);

            Assert.IsInstanceOf<decimal[]>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetConverter_IntListToDecimalList()
        {
            var converter = FastConvert.GetConverter<List<int>, List<decimal>>();
            var expected = new List<decimal> { 1m, 2m, 3m, 4m, 5m };
            var input = new List<int> { 1, 2, 3, 4, 5 };
            var result = converter(input);

            Assert.IsInstanceOf<List<decimal>>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetConverter_IntArrayToDecimalList()
        {
            var converter = FastConvert.GetConverter<int[], List<decimal>>();
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
        public void GetConverter_StringICollectionToStringArray()
        {
            var converter = FastConvert.GetConverter<ICollection<string>, string[]>();
            var input = new List<string> { "1", "2", "3", "4", "5" } as ICollection<string>;
            var expected = new string[] { "1", "2", "3", "4", "5" };
            var result = converter(input);

            Assert.IsInstanceOf<string[]>(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetConverter_StringArrayToStringICollection()
        {
            var converter = FastConvert.GetConverter<string[], ICollection<string>>();
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
        public void GetConverter_NonGenericDateTimeToSqlDateTime()
        {
            var converter = FastConvert.GetConverter(typeof(DateTime), typeof(SqlDateTime));
            var input = DateTime.MinValue;
            var result = converter(input);

            Assert.AreEqual(SqlDateTime.MinValue, result);
        }

        [Test]
        public void GetConverter_NonGenericIntToString()
        {
            var converter = FastConvert.GetConverter(typeof(int), typeof(string));
            var input = 1;
            var result = converter(input);

            Assert.AreEqual("1", result);
        }
        [Test]
        public void GetConverter_NonGenericStringToInt()
        {
            var converter = FastConvert.GetConverter(typeof(string), typeof(int));
            var input = "1";
            var result = converter(input);

            Assert.AreEqual(1, result);
        }

        [TestCaseSource(typeof(TestData), "NonGenericTestData")]
        public object GetConverter_NonGenericTestData(object input, Type fromType, Type toType)
        {
            var converter = FastConvert.GetConverter(fromType, toType);

            return converter(input);
        }

        [Test]
        public void GetConverter_ConvertPersonToPersonDto_ShouldReturnInstanceOfToTypeWithCorrectValues()
        {
            var converter = FastConvert.GetConverter<Person, PersonDto>();
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
        public void GetConverter_ConvertPersonStringDtoToPerson_ShouldReturnInstanceOfToTypeWithCorrectValues()
        {
            var expectedDecimal = 182.5m;
            var guidStr = "0cb6c00f-fc44-484f-8ddd-823709b74601";
            var converter = FastConvert.GetConverter<PersonStringDto, Person>();
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
        public void GetConverter_ConvertPersonStructToPerson_ShouldReturnInstanceOfToTypeWithCorrectValues()
        {
            var converter = FastConvert.GetConverter<PersonStruct, Person>();
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
        public void GetConverter_ConvertPersonToPersonStruct_ShouldReturnInstanceOfToTypeWithCorrectValues()
        {
            var converter = FastConvert.GetConverter<Person, PersonStruct>();
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
        public void GetConverter_ConvertPersonToPersonStringDto_ShouldReturnInstanceOfToTypeWithCorrectValues()
        {
            var expectedDecimal = 182.5m;
            var guidStr = "0cb6c00f-fc44-484f-8ddd-823709b74601";
            var converter = FastConvert.GetConverter<Person, PersonStringDto>();
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
            Assert.AreEqual("1977-03-04 00:00:00", result.BirthDate);
        }

        private TTo AssertConverterOutput<TFrom, TTo>(TFrom input)
        {
            return this.AssertConverterOutput<TFrom, TTo>(input, CultureInfo.CurrentCulture);
        }

        private TTo AssertConverterOutput<TFrom, TTo>(TFrom input, IFormatProvider provider)
        {
            var converter = FastConvert.GetConverter<TFrom, TTo>(provider);
            var expected = converter(input);
            var converter1 = FastConvert.GetConverter<ClassProperty<TFrom>, ClassProperty<TTo>>(provider);
            var converter2 = FastConvert.GetConverter<ClassProperty<TFrom>, ClassField<TTo>>(provider);
            var converter3 = FastConvert.GetConverter<ClassProperty<TFrom>, StructProperty<TTo>>(provider);
            var converter4 = FastConvert.GetConverter<ClassProperty<TFrom>, StructField<TTo>>(provider);
            var converter5 = FastConvert.GetConverter<ClassField<TFrom>, ClassProperty<TTo>>(provider);
            var converter6 = FastConvert.GetConverter<ClassField<TFrom>, ClassField<TTo>>(provider);
            var converter7 = FastConvert.GetConverter<ClassField<TFrom>, StructProperty<TTo>>(provider);
            var converter8 = FastConvert.GetConverter<ClassField<TFrom>, StructField<TTo>>(provider);
            var converter9 = FastConvert.GetConverter<StructProperty<TFrom>, ClassProperty<TTo>>(provider);
            var converter10 = FastConvert.GetConverter<StructProperty<TFrom>, ClassField<TTo>>(provider);
            var converter11 = FastConvert.GetConverter<StructProperty<TFrom>, StructProperty<TTo>>(provider);
            var converter12 = FastConvert.GetConverter<StructProperty<TFrom>, StructField<TTo>>(provider);
            var converter13 = FastConvert.GetConverter<StructField<TFrom>, ClassProperty<TTo>>(provider);
            var converter14 = FastConvert.GetConverter<StructField<TFrom>, ClassField<TTo>>(provider);
            var converter15 = FastConvert.GetConverter<StructField<TFrom>, StructProperty<TTo>>(provider);
            var converter16 = FastConvert.GetConverter<StructField<TFrom>, StructField<TTo>>(provider);

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
