namespace MapEverything.Tests
{
    using System;
    using System.Globalization;

    using MapEverything.Tests.Model.Person;
    using MapEverything.Utils;

    using NUnit.Framework;

    [TestFixture]
    public class ConverterFactoryTests
    {
        [Test]
        public void CreateConverter_ConvertStringToInt_ShouldReturnInstanceOfToType()
        {
            var converter = ConverterFactory.Create<string, int>();

            var result = converter("10");

            Assert.IsInstanceOf<int>(result);
        }

        [Test]
        public void CreateConverter_ConvertIntToLong_ShouldReturnInstanceOfLong()
        {
            var converter = ConverterFactory.Create<int, long>();

            var result = converter(1234);

            Assert.IsInstanceOf<long>(result);
            Assert.AreEqual(1234, result);
        }

        [Test]
        public void CreateConverter_ConvertPersonToPersonDto_ShouldReturnInstanceOfToType()
        {
            var converter = ConverterFactory.Create<Person, PersonDto>();

            var result = converter(new Person());

            Assert.IsInstanceOf<PersonDto>(result);
        }

        [Test]
        public void CreateConverter_ConvertStringToInt_ShouldReturnCorrectValue()
        {
            var expected = 10;
            var converter = ConverterFactory.Create<string, int>();

            var result = converter(expected.ToString());

            Assert.AreEqual(expected, result);
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
