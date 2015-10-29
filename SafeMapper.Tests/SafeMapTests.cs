namespace SafeMapper.Tests
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;

    using Xunit;

    using SafeMapper.Configuration;
    using SafeMapper.Tests.Model.GenericClasses;
    using SafeMapper.Tests.Model.Person;

    
    public class SafeMapTests
    {
        [Fact]
        public void Convert_NonGeneric_StringToInt()
        {
            var result = SafeMap.Convert("10", typeof(string), typeof(int));

            Assert.Equal(10, result);
        }

        [Fact]
        public void Convert_NonGenericWithFormat_StringToInt()
        {
            var result = SafeMap.Convert("10", typeof(string), typeof(int), CultureInfo.CurrentCulture);

            Assert.Equal(10, result);
        }

        [Fact]
        public void Convert_Generic_StringToInt()
        {
            var result = SafeMap.Convert<string, int>("10");

            Assert.Equal(10, result);
        }

        [Fact]
        public void Convert_GenericWithFormat_StringToInt()
        {
            var result = SafeMap.Convert<string, int>("10", CultureInfo.CurrentCulture);

            Assert.Equal(10, result);
        }

        [Fact]
        public void GetConverter_NonGeneric_StringToInt()
        {
            var converter = SafeMap.GetConverter(typeof(string), typeof(int));
            var result = converter("10");

            Assert.Equal(10, result);
        }

        [Fact]
        public void GetConverter_NonGenericWithFormat_StringToInt()
        {
            var converter = SafeMap.GetConverter(typeof(string), typeof(int), CultureInfo.CurrentCulture);
            var result = converter("10");

            Assert.Equal(10, result);
        }

        [Fact]
        public void GetConverter_Generic_StringToInt()
        {
            var converter = SafeMap.GetConverter<string, int>();
            var result = converter("10");

            Assert.Equal(10, result);
        }

        [Fact]
        public void GetConverter_GenericWithFormat_StringToInt()
        {
            var converter = SafeMap.GetConverter<string, int>(CultureInfo.CurrentCulture);
            var result = converter("10");

            Assert.Equal(10, result);
        }

        [Fact]
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

            Assert.Equal(person.Id, result.PersonId);
            Assert.Equal(person.Name, result.Namn);
            Assert.Equal(person.Age, result.Ålder);
            Assert.Equal(person.Length, result.Längd);
            Assert.Equal(person.BirthDate, result.Födelsedag);
        }

        [Fact]
        public void CreateMap_NameValueCollectionToClassPropertyInt_GetValues()
        {
            SafeMap.CreateMap<NameValueCollection, ClassProperty<int>>(
                cfg =>
                {
                    cfg.MapGetIndexer((x, key) => x.GetValues(key));
                    cfg.Map<string, int>("Value2", x => x.Value);
                });

            var input = new NameValueCollection { { "Value2", "1337" } };
            var result = SafeMap.Convert<NameValueCollection, ClassProperty<int>>(input);

            Assert.Equal(1337, result.Value);
        }

        [Fact]
        public void CreateMap_NameValueCollectionToClassPropertyInt_StringIndexer()
        {
            SafeMap.CreateMap<NameValueCollection, ClassProperty<int>>(
                cfg =>
                {
                    cfg.MapGetIndexer((x, key) => x[key]);
                    cfg.Map<string, int>("Value2", x => x.Value);
                });

            var input = new NameValueCollection { { "Value2", "1337" } };
            var result = SafeMap.Convert<NameValueCollection, ClassProperty<int>>(input);

            Assert.Equal(1337, result.Value);
        }

        [Fact]
        public void CreateMap_ClassPropertyIntToNameValueCollection_Add()
        {
            SafeMap.Configuration.SetConvertMethod<int, string>(i => i.ToString(CultureInfo.InvariantCulture));
            SafeMap.CreateMap<ClassProperty<int>, NameValueCollection>(
                cfg =>
                {
                    cfg.MapSetIndexer<string>((x, key, val) => x.Add(key, val));
                    cfg.Map<int, string>(x => x.Value, "Value2");
                });

            var input = new ClassProperty<int> { Value = 1337 };
            var result = SafeMap.Convert<ClassProperty<int>, NameValueCollection>(input);

            Assert.Equal("1337", result["Value2"]);
        }

        [Fact]
        public void SetConfiguration_ShouldResetPreviousConfiguredConverters()
        {
            SafeMap.Configuration.SetConvertMethod<int, string>(i => i.ToString(CultureInfo.InvariantCulture) + " pcs");

            var result1 = SafeMap.Convert<int, string>(10);
            SafeMap.Configuration = new MapConfiguration();
            var result2 = SafeMap.Convert<int, string>(10);

            Assert.Equal("10 pcs", result1);
            Assert.Equal("10", result2);
        }
    }
}
