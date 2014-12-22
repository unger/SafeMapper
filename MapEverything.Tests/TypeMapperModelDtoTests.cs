namespace MapEverything.Tests
{
    using System;
    using System.Data.SqlTypes;

    using MapEverything.Converters;
    using MapEverything.Tests.Model;

    using NUnit.Framework;

    [TestFixture]
    public class TypeMapperModelDtoTests : TypeMapperTestsBase
    {
        [Test]
        public void CanConvertPersonToPersonDto()
        {
            var value = new Person
            {
                Id = Guid.NewGuid(),
                Name = "Magnus Unger",
                Age = 37,
                Length = 1.84m,
                BirthDate = new DateTime(1977, 03, 04)
            };

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<Person, PersonDto>(value);

                Assert.IsInstanceOf<PersonDto>(converted);

                Assert.AreEqual(value.Id, converted.Id);
                Assert.AreEqual(value.Name, converted.Name);
                Assert.AreEqual(value.Age, converted.Age);
                Assert.AreEqual(value.Length, converted.Length);
                Assert.AreEqual(value.BirthDate, converted.BirthDate);
            }
        }

        [Test]
        public void CanConvertPersonToPersonStringDto()
        {
            var value = new Person
            {
                Id = Guid.NewGuid(),
                Name = "Magnus Unger",
                Age = 37,
                Length = 1.84m,
                BirthDate = new DateTime(1977, 03, 04)
            };

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<Person, PersonStringDto>(value);

                Assert.IsInstanceOf<PersonStringDto>(converted);

                Assert.AreEqual(value.Id.ToString(), converted.Id);
                Assert.AreEqual(value.Name, converted.Name);
                Assert.AreEqual(value.Age.ToString(), converted.Age);
                Assert.AreEqual(value.Length.ToString(), converted.Length);
                Assert.AreEqual(value.BirthDate.ToString(), converted.BirthDate);
            }
        }

        [Test]
        public void CanConvertPersonStringDtoToPerson()
        {
            var expectedValue = new Person
            {
                Id = Guid.NewGuid(),
                Name = "Magnus Unger",
                Age = 37,
                Length = 1.84m,
                BirthDate = new DateTime(1977, 03, 04)
            };

            var value = new PersonStringDto
            {
                Id = expectedValue.Id.ToString(),
                Name = expectedValue.Name,
                Age = expectedValue.Age.ToString(),
                Length = expectedValue.Length.ToString(),
                BirthDate = expectedValue.BirthDate.ToString()
            };

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<PersonStringDto, Person>(value);

                Assert.IsInstanceOf<Person>(converted);

                Assert.AreEqual(expectedValue.Id, converted.Id);
                Assert.AreEqual(expectedValue.Name, converted.Name);
                Assert.AreEqual(expectedValue.Age, converted.Age);
                Assert.AreEqual(expectedValue.Length, converted.Length);
                Assert.AreEqual(expectedValue.BirthDate, converted.BirthDate);
            }
        }

        [Test]
        public void CanConvertPersonToPersonDbDto()
        {
            var value = new Person
            {
                Id = Guid.NewGuid(),
                Name = "Magnus Unger",
                Age = 37,
                Length = 1.84m,
                BirthDate = new DateTime(1977, 03, 04)
            };

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<Person, PersonDbDto>(value);

                Assert.IsInstanceOf<PersonDbDto>(converted);

                Assert.AreEqual(value.Id, converted.Id);
                Assert.AreEqual(value.Name, converted.Name);
                Assert.AreEqual(value.Age, converted.Age);
                Assert.AreEqual(value.Length, converted.Length);
                Assert.AreEqual(value.BirthDate, converted.BirthDate.Value);
            }
        }

        [Test]
        public void CanConvertPersonDbToPerson()
        {
            var value = new PersonDbDto
            {
                Id = Guid.NewGuid(),
                Name = "Magnus Unger",
                Age = 37,
                Length = 1.84,
                BirthDate = new SqlDateTime(1977, 03, 04)
            };

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<PersonDbDto, Person>(value);

                Assert.IsInstanceOf<Person>(converted);

                Assert.AreEqual(value.Id, converted.Id);
                Assert.AreEqual(value.Name, converted.Name);
                Assert.AreEqual(value.Age, converted.Age);
                Assert.AreEqual(value.Length, converted.Length);
                Assert.AreEqual(value.BirthDate.Value, converted.BirthDate);
            }
        }

    }
}
