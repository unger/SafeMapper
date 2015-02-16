using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMapper.Tests
{
    using System.Globalization;
    using System.Reflection;

    using EmitMapper;

    using NUnit.Framework;

    using SafeMapper.Tests.Model.Person;
    using SafeMapper.Tests.Performance.Util;

    [TestFixture]
    public class PerformanceTests
    {
        [Test]
        public void IntToStringPerformance()
        {
            var input = int.MaxValue;
            var output = this.MeasurePerformance<int, string>(() => Convert.ToString(input), input);

            Assert.LessOrEqual(output[1], 0d);
            Assert.LessOrEqual(output[0], 0.15d);
        }

        [Test]
        public void StringToIntPerformance()
        {
            var input = "2147483647";
            var output = this.MeasurePerformance<string, int>(() => Convert.ToInt32(input), input);

            Assert.LessOrEqual(output[1], 0d);
            Assert.LessOrEqual(output[0], 0d);
        }

        [Test]
        public void PersonStringDtoToPerson()
        {
            var input = new PersonStringDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Magnus Unger",
                Age = "38",
                Length = 1.85m.ToString(CultureInfo.CurrentCulture),
                BirthDate = "1977-03-04"
            };
            var output = this.MeasurePerformance<PersonStringDto, Person>(
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

            Assert.LessOrEqual(output[1], 0d);
            Assert.LessOrEqual(output[0], 0d);
        }

        [Test]
        public void PersonToPersonStringDto()
        {
            var input = new Person
            {
                Id = Guid.NewGuid(),
                Name = "Magnus Unger",
                Age = 38,
                Length = 1.85m,
                BirthDate = new DateTime(1977, 03, 04)
            };
            var output = this.MeasurePerformance<Person, PersonStringDto>(
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


            Assert.LessOrEqual(output[1], 0.10d);
            Assert.LessOrEqual(output[0], 0.15d);
        }

        private double[] MeasurePerformance<TFrom, TTo>(Action nativeAction, TFrom input, int iterations = 100000)
        {
            var safeMapConverter = SafeMap.GetConverter<TFrom, TTo>();
            var emitMapper = ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>();

            // Warmup
            Profiler.Profile(100, () => safeMapConverter(input));
            Profiler.Profile(100, nativeAction);
            Profiler.Profile(100, () => emitMapper.Map(input));

            var safemapperTicks = Profiler.Profile(100000, () => safeMapConverter(input));
            var nativeTicks = Profiler.Profile(100000, nativeAction);
            var emitMapperTicks = Profiler.Profile(100000, () => emitMapper.Map(input));

            if (safemapperTicks == -1 || nativeTicks == -1 || emitMapperTicks == -1)
            {
                throw new Exception("invalid conversion");
            } 

            return new[]
                       {
                           (double)(safemapperTicks - nativeTicks) / nativeTicks,
                           (double)(safemapperTicks - emitMapperTicks) / emitMapperTicks
                       };
        }
    }
}
