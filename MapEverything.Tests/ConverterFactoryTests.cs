namespace MapEverything.Tests
{
    using System;

    using MapEverything.Tests.Model.Classes;
    using MapEverything.Tests.Model.Person;
    using MapEverything.Utils;

    using NUnit.Framework;

    [TestFixture]
    public class ConverterFactoryTests
    {
        [TestCaseSource(typeof(TestData), "StringToStringData")]
        public string CreateConverter_StringToString(string input)
        {
            var converter = ConverterFactory.Create<string, string>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToIntData")]
        public int CreateConverter_StringToInt(string input)
        {
            var converter = ConverterFactory.Create<string, int>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToIntData")]
        public int CreateConverter_StringPropertyToIntProperty(string input)
        {
            var converter = ConverterFactory.Create<StringPropertyClass, IntPropertyClass>();
            var value = new StringPropertyClass { Value = input };

            return converter(value).Value;
        }

        [TestCaseSource(typeof(TestData), "StringToIntData")]
        public int CreateConverter_StringPropertyToIntField(string input)
        {
            var converter = ConverterFactory.Create<StringPropertyClass, IntFieldClass>();
            var value = new StringPropertyClass { Value = input };

            return converter(value).Value;
        }

        [TestCaseSource(typeof(TestData), "StringToIntData")]
        public int CreateConverter_StringFieldToIntField(string input)
        {
            var converter = ConverterFactory.Create<StringFieldClass, IntFieldClass>();
            var value = new StringFieldClass { Value = input };

            return converter(value).Value;
        }

        [TestCaseSource(typeof(TestData), "StringToIntData")]
        public int CreateConverter_StringFieldToIntProperty(string input)
        {
            var converter = ConverterFactory.Create<StringFieldClass, IntPropertyClass>();
            var value = new StringFieldClass { Value = input };

            return converter(value).Value;
        }

        [TestCaseSource(typeof(TestData), "IntToStringData")]
        public string CreateConverter_IntToString(int input)
        {
            var converter = ConverterFactory.Create<int, string>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "IntToStringData")]
        public string CreateConverter_IntPropertyToStringProperty(int input)
        {
            var converter = ConverterFactory.Create<IntPropertyClass, StringPropertyClass>();
            var value = new IntPropertyClass { Value = input };

            return converter(value).Value;
        }

        [TestCaseSource(typeof(TestData), "IntToStringData")]
        public string CreateConverter_IntPropertyToStringField(int input)
        {
            var converter = ConverterFactory.Create<IntPropertyClass, StringFieldClass>();
            var value = new IntPropertyClass { Value = input };

            return converter(value).Value;
        }

        [TestCaseSource(typeof(TestData), "IntToStringData")]
        public string CreateConverter_IntFieldToStringProperty(int input)
        {
            var converter = ConverterFactory.Create<IntFieldClass, StringPropertyClass>();
            var value = new IntFieldClass { Value = input };

            return converter(value).Value;
        }

        [TestCaseSource(typeof(TestData), "IntToStringData")]
        public string CreateConverter_IntFieldToStringField(int input)
        {
            var converter = ConverterFactory.Create<IntFieldClass, StringFieldClass>();
            var value = new IntFieldClass { Value = input };

            return converter(value).Value;
        }

        [TestCaseSource(typeof(TestData), "GuidToStringData")]
        public string CreateConverter_GuidToString(Guid input)
        {
            var converter = ConverterFactory.Create<Guid, string>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "StringToGuidData")]
        public Guid CreateConverter_StringToGuid(string input)
        {
            var converter = ConverterFactory.Create<string, Guid>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "IntToLongData")]
        public long CreateConverter_IntToLong(int input)
        {
            var converter = ConverterFactory.Create<int, long>();

            return converter(input);
        }

        [TestCaseSource(typeof(TestData), "LongToIntData")]
        public int CreateConverter_LongToInt(long input)
        {
            var converter = ConverterFactory.Create<long, int>();

            return converter(input);
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
    }
}
