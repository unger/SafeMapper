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
        public void CreateConverter_CallDelegateWithPrimitive_ShouldReturnInstanceOfToType()
        {
            var converter = ConverterFactory.Create<string, int>();

            var result = converter("10");

            Assert.IsInstanceOf<int>(result);
        }

        [Test]
        public void CreateConverter_CallDelegateWithClass_ShouldReturnInstanceOfToType()
        {
            var converter = ConverterFactory.Create<Person, PersonDto>();

            var result = converter(new Person());

            Assert.IsInstanceOf<PersonDto>(result);
        }

        [Test]
        public void CreateConverter_CallDelegateWithPrimitive_ShouldReturnCorrectValue()
        {
            var expected = 10;
            var converter = ConverterFactory.Create<string, int>();

            var result = converter(expected.ToString());

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateConverter_CallDelegatePersonToPersonDto_ShouldReturnInstanceOfToTypeWithCorrectValues()
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
        public void CreateConverter_CallDelegatePersonStringDtoToPerson_ShouldReturnInstanceOfToTypeWithCorrectValues()
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
    }
}
