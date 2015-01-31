namespace MapEverything.Tests
{
    using System;
    using System.Globalization;

    using MapEverything.Tests.Model.Classes;
    using MapEverything.Tests.Model.Person;
    using MapEverything.Utils;

    using NUnit.Framework;

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
        public string CreateConverter_StringToString(string input)
        {
            var converter = ConverterFactory.Create<string, string>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToStringData")]
        public string CreateConverter_StringMemberToStringMember(string input)
        {
            return this.AssertConverterOutput<string, string>(input);
        }

        [TestCaseSource(typeof(TestData), "StringToIntData")]
        public int CreateConverter_StringToInt(string input)
        {
            var converter = ConverterFactory.Create<string, int>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToIntData")]
        public int CreateConverter_StringMemberToIntMember(string input)
        {
            return this.AssertConverterOutput<string, int>(input);
        }

        [TestCaseSource(typeof(TestData), "StringToGuidData")]
        public Guid CreateConverter_StringToGuid(string input)
        {
            var converter = ConverterFactory.Create<string, Guid>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToGuidData")]
        public Guid CreateConverter_StringMemberToGuidMember(string input)
        {
            return this.AssertConverterOutput<string, Guid>(input);
        }

        [TestCaseSource(typeof(TestData), "StringToDecimalData")]
        public decimal CreateConverter_StringToDecimal(string input)
        {
            var converter = ConverterFactory.Create<string, decimal>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToDecimalData")]
        public decimal CreateConverter_StringMemberToDecimalMember(string input)
        {
            return this.AssertConverterOutput<string, decimal>(input, this.numberFormatProvider);
        }

        [TestCaseSource(typeof(TestData), "StringToDateTimeData")]
        public DateTime CreateConverter_StringToDateTime(string input)
        {
            var converter = ConverterFactory.Create<string, DateTime>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToDateTimeData")]
        public DateTime CreateConverter_StringMemberToDateTimeMember(string input)
        {
            return this.AssertConverterOutput<string, DateTime>(input);
        }

        /************************************************************************/
        /*                                                                      
        /*   Int                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "IntToIntData")]
        public int CreateConverter_IntToInt(int input)
        {
            var converter = ConverterFactory.Create<int, int>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "IntToIntData")]
        public int CreateConverter_IntMemberToIntMember(int input)
        {
            return this.AssertConverterOutput<int, int>(input);
        }


        [TestCaseSource(typeof(TestData), "IntToStringData")]
        public string CreateConverter_IntToString(int input)
        {
            var converter = ConverterFactory.Create<int, string>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "IntToStringData")]
        public string CreateConverter_IntMemberToStringMember(int input)
        {
            return this.AssertConverterOutput<int, string>(input);
        }


        [TestCaseSource(typeof(TestData), "IntToLongData")]
        public long CreateConverter_IntToLong(int input)
        {
            var converter = ConverterFactory.Create<int, long>();

            return converter(input);
        }


        /************************************************************************/
        /*                                                                      
        /*   Guid                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "GuidToStringData")]
        public string CreateConverter_GuidToString(Guid input)
        {
            var converter = ConverterFactory.Create<Guid, string>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "GuidToStringData")]
        public string CreateConverter_GuidMemberToStringMember(Guid input)
        {
            return this.AssertConverterOutput<Guid, string>(input);
        }

        /************************************************************************/
        /*                                                                      
        /*   Decimal                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "DecimalToDecimalData")]
        public decimal CreateConverter_DecimalToDecimal(decimal input)
        {
            var converter = ConverterFactory.Create<decimal, decimal>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToStringData")]
        public string CreateConverter_DecimalToString(decimal input)
        {
            var converter = ConverterFactory.Create<decimal, string>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToStringData")]
        public string CreateConverter_DecimalMemberToStringMember(decimal input)
        {
            return this.AssertConverterOutput<decimal, string>(input, this.numberFormatProvider);
        }

        [TestCaseSource(typeof(TestData), "DecimalToDoubleData")]
        public double CreateConverter_DecimalToDouble(decimal input)
        {
            var converter = ConverterFactory.Create<decimal, double>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "DecimalToDoubleData")]
        public double CreateConverter_DecimalMemberToDoubleMember(decimal input)
        {
            return this.AssertConverterOutput<decimal, double>(input, this.numberFormatProvider);
        }

        /************************************************************************/
        /*                                                                      
        /*   Double                                                              
        /*                                                                      
        /************************************************************************/

        [TestCaseSource(typeof(TestData), "DoubleToDecimalData")]
        public decimal CreateConverter_DoubleToDecimal(double input)
        {
            var converter = ConverterFactory.Create<double, decimal>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "DoubleToDecimalData")]
        public decimal CreateConverter_DoubleMemberToDecimalMember(double input)
        {
            return this.AssertConverterOutput<double, decimal>(input, this.numberFormatProvider);
        }



        /************************************************************************/
        /*                                                                      
        /*   DateTime                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "DateTimeToStringData")]
        public string CreateConverter_DateTimeToString(DateTime input)
        {
            var converter = ConverterFactory.Create<DateTime, string>(this.numberFormatProvider);

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "DateTimeToStringData")]
        public string CreateConverter_DateTimeMemberToStringMember(DateTime input)
        {
            return this.AssertConverterOutput<DateTime, string>(input, this.numberFormatProvider);
        }


        /************************************************************************/
        /*                                                                      
        /*   Long                                                              
        /*                                                                      
        /************************************************************************/


        [TestCaseSource(typeof(TestData), "LongToIntData")]
        public int CreateConverter_LongToInt(long input)
        {
            var converter = ConverterFactory.Create<long, int>();

            return converter(input);
        }


        /************************************************************************/
        /*                                                                      
        /*   Array to array                                                              
        /*                                                                      
        /************************************************************************/

        [Test]
        public void CreateConverter_IntArrayToIntArray()
        {
            var converter = ConverterFactory.Create<int[], int[]>();
            var input = new int[] { 1, 2, 3, 4, 5 };
            var result = converter(input);

            Assert.AreEqual(input, result);
        }

        [Test]
        public void CreateConverter_IntArrayToStringArray()
        {
            var converter = ConverterFactory.Create<int[], string[]>();
            var expected = new string[] { "1", "2", "3", "4", "5" };
            var input = new int[] { 1, 2, 3, 4, 5 };
            var result = converter(input);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateConverter_StringArrayToIntArray()
        {
            var converter = ConverterFactory.Create<string[], int[]>();
            var input = new string[] { "1", "2", "3", "4", "5" };
            var expected = new int[] { 1, 2, 3, 4, 5 };
            var result = converter(input);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateConverter_IntArrayToDecimalArray()
        {
            var converter = ConverterFactory.Create<int[], decimal[]>();
            var input = new int[] { 1, 2, 3, 4, 5 };
            var expected = new decimal[] { 1m, 2m, 3m, 4m, 5m };
            var result = converter(input);

            Assert.AreEqual(expected, result);
        }



        /************************************************************************/
        /*                                                                      
        /*   Misc                                                              
        /*                                                                      
        /************************************************************************/


        [Test]
        public void CreateConverter_ConvertPersonToPersonDto_ShouldReturnInstanceOfToTypeWithCorrectValues()
        {
            var converter = ConverterFactory.Create<Person, PersonDto>();
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
        public void CreateConverter_ConvertPersonStringDtoToPerson_ShouldReturnInstanceOfToTypeWithCorrectValues()
        {
            var expectedDecimal = 182.5m;
            var guidStr = "0cb6c00f-fc44-484f-8ddd-823709b74601";
            var converter = ConverterFactory.Create<PersonStringDto, Person>();
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
        public void CreateConverter_ConvertPersonStructToPerson_ShouldReturnInstanceOfToTypeWithCorrectValues()
        {
            var converter = ConverterFactory.Create<PersonStruct, Person>();
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
        public void CreateConverter_ConvertPersonToPersonStruct_ShouldReturnInstanceOfToTypeWithCorrectValues()
        {
            var converter = ConverterFactory.Create<Person, PersonStruct>();
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
        public void CreateConverter_ConvertPersonToPersonStringDto_ShouldReturnInstanceOfToTypeWithCorrectValues()
        {
            var expectedDecimal = 182.5m;
            var guidStr = "0cb6c00f-fc44-484f-8ddd-823709b74601";
            var converter = ConverterFactory.Create<Person, PersonStringDto>();
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
            var converter = ConverterFactory.Create<TFrom, TTo>(provider);
            var expected = converter(input);
            var converter1 = ConverterFactory.Create<ClassProperty<TFrom>, ClassProperty<TTo>>(provider);
            var converter2 = ConverterFactory.Create<ClassProperty<TFrom>, ClassField<TTo>>(provider);
            var converter3 = ConverterFactory.Create<ClassProperty<TFrom>, StructProperty<TTo>>(provider);
            var converter4 = ConverterFactory.Create<ClassProperty<TFrom>, StructField<TTo>>(provider);
            var converter5 = ConverterFactory.Create<ClassField<TFrom>, ClassProperty<TTo>>(provider);
            var converter6 = ConverterFactory.Create<ClassField<TFrom>, ClassField<TTo>>(provider);
            var converter7 = ConverterFactory.Create<ClassField<TFrom>, StructProperty<TTo>>(provider);
            var converter8 = ConverterFactory.Create<ClassField<TFrom>, StructField<TTo>>(provider);
            var converter9 = ConverterFactory.Create<StructProperty<TFrom>, ClassProperty<TTo>>(provider);
            var converter10 = ConverterFactory.Create<StructProperty<TFrom>, ClassField<TTo>>(provider);
            var converter11 = ConverterFactory.Create<StructProperty<TFrom>, StructProperty<TTo>>(provider);
            var converter12 = ConverterFactory.Create<StructProperty<TFrom>, StructField<TTo>>(provider);
            var converter13 = ConverterFactory.Create<StructField<TFrom>, ClassProperty<TTo>>(provider);
            var converter14 = ConverterFactory.Create<StructField<TFrom>, ClassField<TTo>>(provider);
            var converter15 = ConverterFactory.Create<StructField<TFrom>, StructProperty<TTo>>(provider);
            var converter16 = ConverterFactory.Create<StructField<TFrom>, StructField<TTo>>(provider);

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
