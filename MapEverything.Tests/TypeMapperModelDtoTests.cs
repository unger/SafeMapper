namespace MapEverything.Tests
{
    using System;

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
                mapper.AddTypeConverter<Person>(new GenericTypeConverter<Person, PersonDto>(mapper));

                var converted = mapper.Convert<Person, PersonDto>(value);

                Assert.IsInstanceOf<PersonDto>(converted);

                Assert.AreEqual(value.Id, converted.Id);
                Assert.AreEqual(value.Name, converted.Name);
                Assert.AreEqual(value.Age, converted.Age);
                Assert.AreEqual(value.Length, converted.Length);
                Assert.AreEqual(value.BirthDate, converted.BirthDate);
            }
        }
    }
}
