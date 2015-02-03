namespace MapEverything.Profiler
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using AutoMapper;

    using FastMapper;

    using MapEverything.Profiler.AutoMapperHelpers;
    using MapEverything.Tests.Model;
    using MapEverything.Tests.Model.Person;
    using MapEverything.Utils;

    using TB.ComponentModel;

    public class ProfileInvalidConversion : ProfileBase
    {
        public override void Execute()
        {
            var formatProvider = CultureInfo.CurrentCulture;
            var maxIterations = this.MaxIterations;

            var stringInvalidArray = new string[maxIterations];
            var personStringArray = new PersonStringDto[maxIterations];

            for (int i = 0; i < maxIterations; i++)
            {
                stringInvalidArray[i] = System.Web.Security.Membership.GeneratePassword((i % 10) + 1, i % 5);
                personStringArray[i] = new PersonStringDto
                {
                    Id = System.Web.Security.Membership.GeneratePassword((i % 10) + 1, i % 5),
                    Name = null,
                    Age = System.Web.Security.Membership.GeneratePassword((i % 10) + 1, i % 5),
                    Length = System.Web.Security.Membership.GeneratePassword((i % 10) + 1, i % 5),
                    BirthDate = System.Web.Security.Membership.GeneratePassword((i % 10) + 1, i % 5)
                };
            }

            // FromString conversions
            this.ProfileConvert<string, Guid>(stringInvalidArray, formatProvider, i => new Guid(stringInvalidArray[i]));
            this.ProfileConvert<string, int>(stringInvalidArray, formatProvider, i => int.Parse(stringInvalidArray[i], formatProvider));
            this.ProfileConvert<string, string>(stringInvalidArray, formatProvider, i => int.Parse(stringInvalidArray[i], formatProvider));
            this.ProfileConvert<string, decimal>(stringInvalidArray, formatProvider, i => SafeConvert.ToDecimal(stringInvalidArray[i], formatProvider));
            this.ProfileConvert<string, DateTime>(stringInvalidArray, formatProvider, i => Convert.ToDateTime(stringInvalidArray[i]));

            this.ProfileConvert<PersonStringDto, Person>(personStringArray, CultureInfo.CurrentCulture, null);
        }

        private void ProfileConvert<TSource, TDestination>(TSource[] input, CultureInfo formatProvider, Action<int> compareFunc)
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);
            var fastConverter = FastConvert.GetConverter<TSource, TDestination>();

            if (typeof(TDestination) != typeof(string))
            {
                if (typeof(TDestination) == typeof(DateTime) && typeof(TSource) == typeof(string))
                {
                    Mapper.CreateMap(typeof(TSource), typeof(TDestination)).ConvertUsing(typeof(AutoMapperDateTimeTypeConverter));
                }
                else
                {
                    Mapper.CreateMap<TSource, TDestination>();
                }
            }

            Mapper.CreateMap<Address, AddressDto>();

            this.WriteHeader(string.Format("Profiling convert from {0} to {1}, {2} iterations", typeof(TSource).Name, typeof(TDestination).Name, input.Length));

            /*if (compareFunc != null)
            {
                this.AddResult("Native", compareFunc);
            }
            */
            this.AddResult(
                    "FastMapper",
                    i => TypeAdapter.Adapt(input[i], sourceType, destinationType));

            this.AddResult(
                    "MapEverything",
                    i => fastConverter(input[i]));
            

            this.AddResult(
                    "SimpleTypeConverter",
                    i => SimpleTypeConverter.ConvertTo(input[i], destinationType, formatProvider));

            /*
            this.AddResult(
                    "UniversalTypeConverter",
                    i => UniversalTypeConverter.Convert(input[i], typeof(TDestination), formatProvider));


            this.AddResult("AutoMapper", i => Mapper.Map<TSource, TDestination>(input[i]));*/

        }
    }
}
