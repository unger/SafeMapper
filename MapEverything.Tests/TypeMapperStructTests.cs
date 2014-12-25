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
        [Ignore]
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
    }
}
