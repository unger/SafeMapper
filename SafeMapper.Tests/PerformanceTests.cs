namespace SafeMapper.Tests
{
    using System;
    using System.Globalization;

    using EmitMapper;

    using NUnit.Framework;

    using SafeMapper.Profiler;
    using SafeMapper.Tests.Model.Person;

    [Ignore("Ignored")]
    [TestFixture]
    public class PerformanceTests
    {
        [Test]
        public void IntToStringPerformance_ComparedWithNative()
        {
            var input = int.MaxValue;
            var difference = this.ComparePerformance<int, string>(() => Convert.ToString(input), input);

            Assert.LessOrEqual(difference, 0d);
        }

        [Test]
        public void IntToStringPerformance_ComparedWithEmitMapper()
        {
            var input = int.MaxValue;
            var emitMapper = ObjectMapperManager.DefaultInstance.GetMapper<int, string>();
            var difference = this.ComparePerformance<int, string>(() => emitMapper.Map(input), input);

            Assert.LessOrEqual(difference, 0d);
        }

        [Test]
        public void StringToIntPerformance_ComparedWithNative()
        {
            var input = "2147483647";
            var difference = this.ComparePerformance<string, int>(() => Convert.ToInt32(input), input);

            Assert.LessOrEqual(difference, 0d);
        }

        [Test]
        public void StringToIntPerformance_ComparedWithEmitMapper()
        {
            var input = "2147483647";
            var emitMapper = ObjectMapperManager.DefaultInstance.GetMapper<string, int>();
            var difference = this.ComparePerformance<string, int>(() => emitMapper.Map(input), input);

            Assert.LessOrEqual(difference, 0d);
        }

        [Test]
        public void PersonStringDtoToPerson_ComparedWithNative()
        {
            var input = new PersonStringDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Magnus Unger",
                Age = "38",
                Length = 1.85m.ToString(CultureInfo.CurrentCulture),
                BirthDate = "1977-03-04"
            };
            var difference = this.ComparePerformance<PersonStringDto, Person>(
                () =>
                {
                    var personDto = new Person();
                    personDto.Id = new Guid(input.Id);
                    personDto.Name = input.Name;
                    personDto.Age = int.Parse(input.Age);
                    personDto.Length = decimal.Parse(input.Length);
                    personDto.BirthDate = DateTime.Parse(input.BirthDate);
                },
                input);

            Assert.LessOrEqual(difference, 0d);
        }

        [Test]
        public void PersonStringDtoToPerson_ComparedWithEmitMapper()
        {
            var input = new PersonStringDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Magnus Unger",
                Age = "38",
                Length = 1.85m.ToString(CultureInfo.CurrentCulture),
                BirthDate = "1977-03-04"
            };
            var emitMapper = ObjectMapperManager.DefaultInstance.GetMapper<PersonStringDto, Person>();
            var difference = this.ComparePerformance<PersonStringDto, Person>(() => emitMapper.Map(input), input);

            Assert.LessOrEqual(difference, 0d);
        }

        [Test]
        public void PersonToPersonStringDto_ComparedWithNative()
        {
            var input = new Person
            {
                Id = Guid.NewGuid(),
                Name = "Magnus Unger",
                Age = 38,
                Length = 1.85m,
                BirthDate = new DateTime(1977, 03, 04)
            };
            var difference = this.ComparePerformance<Person, PersonStringDto>(
                () =>
                {
                    var personDto = new PersonStringDto();
                    personDto.Id = input.Id.ToString();
                    personDto.Name = input.Name;
                    personDto.Age = input.Age.ToString();
                    personDto.Length = input.Length.ToString();
                    personDto.BirthDate = input.BirthDate.ToString();
                },
                input);

            Assert.LessOrEqual(difference, 0d);
        }

        [Test]
        public void PersonToPersonStringDto_ComparedWithEmitMapper()
        {
            var input = new Person
            {
                Id = Guid.NewGuid(),
                Name = "Magnus Unger",
                Age = 38,
                Length = 1.85m,
                BirthDate = new DateTime(1977, 03, 04)
            };
            var emitMapper = ObjectMapperManager.DefaultInstance.GetMapper<Person, PersonStringDto>();
            var difference = this.ComparePerformance<Person, PersonStringDto>(() => emitMapper.Map(input), input);

            Assert.LessOrEqual(difference, 0d);
        }

        private double ComparePerformance<TFrom, TTo>(Action compareAction, TFrom input, int iterations = 100000)
        {
            var safeMapConverter = SafeMap.GetConverter<TFrom, TTo>();

            var compareTime = Profiler.Profile(compareAction, iterations, 100);
            var safeMapperTime = Profiler.Profile(() => safeMapConverter(input), iterations, 100);

            return (safeMapperTime - compareTime) / compareTime;
        }
    }
}
