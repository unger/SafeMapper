namespace MapEverything.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using MapEverything.Tests.Model.Person;
    using MapEverything.Utils;

    using NUnit.Framework;

    [TestFixture]
    public class ConverterFactoryTests
    {
        [TestCase("0", Result = 0)]
        [TestCase("10", Result = 10)]
        [TestCase("10.5", Result = 0)]
        [TestCase("10,5", Result = 0)]
        [TestCase("-10", Result = -10)]
        [TestCase("1 000", Result = 0)]
        [TestCase("1,000", Result = 0)]
        [TestCase("1.000", Result = 0)]
        [TestCase("2147483647", Result = 2147483647)]
        [TestCase("2147483648", Result = 0)]
        [TestCase("-2147483648", Result = -2147483648)]
        [TestCase("-2147483649", Result = 0)]
        public int CreateConverter_StringToInt(string input)
        {
            var converter = ConverterFactory.Create<string, int>();

            return converter(input);
        }

        [TestCase(0, Result = "0")]
        [TestCase(2147483647, Result = "2147483647")]
        [TestCase(-2147483648, Result = "-2147483648")]
        public string CreateConverter_IntToString(int input)
        {
            var converter = ConverterFactory.Create<int, string>();

            return converter(input);
        }

        public IEnumerable<TestCaseData> GuidToStringData
        {
            get
            {
                var guidStr = "0cb6c00f-fc44-484f-8ddd-823709b74601";
                var guidStr2 = "0cb6c00ffc44484f8ddd823709b74601";

                yield return new TestCaseData(Guid.Empty).Returns("00000000-0000-0000-0000-000000000000");
                yield return new TestCaseData(new Guid(guidStr)).Returns(guidStr);
                yield return new TestCaseData(new Guid(guidStr2)).Returns(guidStr);
            }
        }
        
        [TestCaseSource("GuidToStringData")]
        public string CreateConverter_GuidToString(Guid input)
        {
            var converter = ConverterFactory.Create<Guid, string>();

            return converter(input);
        }

        public IEnumerable<TestCaseData> StringToGuidData
        {
            get
            {
                var guidStr = "0cb6c00f-fc44-484f-8ddd-823709b74601";
                var guidStr2 = "0cb6c00ffc44484f8ddd823709b74601";

                yield return new TestCaseData("00000000-0000-0000-0000-000000000000").Returns(Guid.Empty);
                yield return new TestCaseData(guidStr).Returns(new Guid(guidStr));
                yield return new TestCaseData(guidStr2).Returns(new Guid(guidStr));
                yield return new TestCaseData("abc").Returns(Guid.Empty);
                yield return new TestCaseData("123").Returns(Guid.Empty);
            }
        }

        [TestCaseSource("StringToGuidData")]
        public Guid CreateConverter_StringToGuid(string input)
        {
            var converter = ConverterFactory.Create<string, Guid>();

            return converter(input);
        }


        [TestCase(0, Result = 0)]
        [TestCase(int.MaxValue, Result = int.MaxValue)]
        [TestCase(int.MinValue, Result = int.MinValue)]
        public long CreateConverter_IntToLong(int input)
        {
            var converter = ConverterFactory.Create<int, long>();

            return converter(input);
        }

        [TestCase(0, Result = 0)]
        [TestCase(long.MaxValue, Result = 0)]
        [TestCase(long.MinValue, Result = 0)]
        [TestCase(2147483647, Result = 2147483647)]
        [TestCase(2147483648, Result = 0)]
        [TestCase(-2147483648, Result = -2147483648)]
        [TestCase(-2147483649, Result = 0)]
        public int CreateConverter_LongToInt(long input)
        {
            var converter = ConverterFactory.Create<long, int>();

            return converter(input);
        }

        [Test]
        public void CreateConverter_ConvertPersonToPersonDto_ShouldReturnInstanceOfToType()
        {
            var converter = ConverterFactory.Create<Person, PersonDto>();

            var result = converter(new Person());

            Assert.IsInstanceOf<PersonDto>(result);
        }

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

        [Test]
        public void CreateConverter_ConvertIntToString_ShouldReturnCorrectValue()
        {
            var expected = "10";
            var converter = ConverterFactory.Create<int, string>();

            var result = converter(10);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateConverter_ConvertGuidToString_ShouldReturnCorrectValue()
        {
            var guidStr = "0cb6c00f-fc44-484f-8ddd-823709b74601";
            var converter = ConverterFactory.Create<Guid, string>();

            var result = converter(new Guid(guidStr));

            Assert.AreEqual(guidStr, result);
        }

    }
}
