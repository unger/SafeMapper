namespace MapEverything.Tests
{
    using System;
    using System.Data.SqlTypes;

    using MapEverything.Converters;
    using MapEverything.Tests.Model;
    using MapEverything.Tests.Model.Person;

    using NUnit.Framework;

    [TestFixture]
    public class TypeMapperStructTests : TypeMapperTestsBase
    {
        [Test]
        public void CanConvertPersonStructToPerson()
        {
            var value = new PersonStruct
                            {
                                Id = Guid.NewGuid(),
                                Name = "Magnus Unger",
                                Age = 37,
                                Length = 1.84m,
                                BirthDate = new DateTime(1977, 03, 04)
                            };

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<PersonStruct, Person>(value);

                Assert.IsInstanceOf<Person>(converted);

                Assert.AreEqual(value.Id, converted.Id);
                Assert.AreEqual(value.Name, converted.Name);
                Assert.AreEqual(value.Age, converted.Age);
                Assert.AreEqual(value.Length, converted.Length);
                Assert.AreEqual(value.BirthDate, converted.BirthDate);
            }
        }

        [Test]
        public void CanConvertPersonToPersonStruct()
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
                var converted = mapper.Convert<Person, PersonStruct>(value);

                Assert.IsInstanceOf<PersonStruct>(converted);

                Assert.AreEqual(value.Id, converted.Id);
                Assert.AreEqual(value.Name, converted.Name);
                Assert.AreEqual(value.Age, converted.Age);
                Assert.AreEqual(value.Length, converted.Length);
                Assert.AreEqual(value.BirthDate, converted.BirthDate);
            }
        }

        [Test]
        public void CanConvertPersonStructToPersonStructDto()
        {
            var value = new PersonStruct
            {
                Id = Guid.NewGuid(),
                Name = "Magnus Unger",
                Age = 37,
                Length = 1.84m,
                BirthDate = new DateTime(1977, 03, 04)
            };

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<PersonStruct, PersonStructDto>(value);

                Assert.IsInstanceOf<PersonStructDto>(converted);

                Assert.AreEqual(value.Id, converted.Id);
                Assert.AreEqual(value.Name, converted.Name);
                Assert.AreEqual(value.Age, converted.Age);
                Assert.AreEqual(value.Length, converted.Length);
                Assert.AreEqual(value.BirthDate, converted.BirthDate);
            }
        }
    }
}
