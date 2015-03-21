namespace SafeMapper.Tests
{
    using System;
    using System.Globalization;

    using NUnit.Framework;

    using SafeMapper.Tests.Model.Person;

    [TestFixture]
    public class SafeMapTests
    {
        [Test]
        public void Convert_NonGeneric_StringToInt()
        {
            var result = SafeMap.Convert("10", typeof(string), typeof(int));

            Assert.AreEqual(10, result);
        }

        [Test]
        public void Convert_NonGenericWithFormat_StringToInt()
        {
            var result = SafeMap.Convert("10", typeof(string), typeof(int), CultureInfo.CurrentCulture);

            Assert.AreEqual(10, result);
        }

        [Test]
        public void Convert_Generic_StringToInt()
        {
            var result = SafeMap.Convert<string, int>("10");

            Assert.AreEqual(10, result);
        }

        [Test]
        public void Convert_GenericWithFormat_StringToInt()
        {
            var result = SafeMap.Convert<string, int>("10", CultureInfo.CurrentCulture);

            Assert.AreEqual(10, result);
        }

        [Test]
        public void GetConverter_NonGeneric_StringToInt()
        {
            var converter = SafeMap.GetConverter(typeof(string), typeof(int));
            var result = converter("10");

            Assert.AreEqual(10, result);
        }

        [Test]
        public void GetConverter_NonGenericWithFormat_StringToInt()
        {
            var converter = SafeMap.GetConverter(typeof(string), typeof(int), CultureInfo.CurrentCulture);
            var result = converter("10");

            Assert.AreEqual(10, result);
        }

        [Test]
        public void GetConverter_Generic_StringToInt()
        {
            var converter = SafeMap.GetConverter<string, int>();
            var result = converter("10");

            Assert.AreEqual(10, result);
        }

        [Test]
        public void GetConverter_GenericWithFormat_StringToInt()
        {
            var converter = SafeMap.GetConverter<string, int>(CultureInfo.CurrentCulture);
            var result = converter("10");

            Assert.AreEqual(10, result);
        }

        [Test]
        public void CreateMap_PersonToPersonSwedish()
        {
            SafeMap.CreateMap<Person, PersonSwedish>(
                cfg =>
                    {
                        cfg.Map(x => x.Id, x => x.PersonId);
                        cfg.Map(x => x.Name, x => x.Namn);
                        cfg.Map(x => x.Age, x => x.Ålder);
                        cfg.Map(x => x.Length, x => x.Längd);
                        cfg.Map(x => x.BirthDate, x => x.Födelsedag);
                    });

            var person = new Person
                             {
                                 Id = Guid.NewGuid(),
                                 Name = "Magnus Unger",
                                 Age = 38,
                                 Length = 1.85m,
                                 BirthDate = new DateTime(1977, 03, 04),
                             };
            var result = SafeMap.Convert<Person, PersonSwedish>(person);

            Assert.AreEqual(person.Id, result.PersonId);
            Assert.AreEqual(person.Name, result.Namn);
            Assert.AreEqual(person.Age, result.Ålder);
            Assert.AreEqual(person.Length, result.Längd);
            Assert.AreEqual(person.BirthDate, result.Födelsedag);
        }
    }
}
