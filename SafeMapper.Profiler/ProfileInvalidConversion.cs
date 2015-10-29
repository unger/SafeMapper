namespace SafeMapper.Profiler
{
    using System;
    using System.Globalization;

    using AutoMapper;

    using EmitMapper;

    using FastMapper;

    using Omu.ValueInjecter;

    using SafeMapper;
    using SafeMapper.Profiler.AutoMapperHelpers;
    using SafeMapper.Tests.Model;
    using SafeMapper.Tests.Model.Person;
    using SafeMapper.Utils;

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
            //this.ProfileConvert<string, Guid>(stringInvalidArray, formatProvider, i => new Guid(stringInvalidArray[i]));
            this.ProfileConvert<string, int>(stringInvalidArray, formatProvider, i => int.Parse(stringInvalidArray[i], formatProvider));
            //this.ProfileConvert<string, string>(stringInvalidArray, formatProvider, i => int.Parse(stringInvalidArray[i], formatProvider));
            //this.ProfileConvert<string, decimal>(stringInvalidArray, formatProvider, i => SafeConvert.ToDecimal(stringInvalidArray[i], formatProvider));
            //this.ProfileConvert<string, DateTime>(stringInvalidArray, formatProvider, i => Convert.ToDateTime(stringInvalidArray[i]));

            //this.ProfileConvert<PersonStringDto, Person>(personStringArray, CultureInfo.CurrentCulture, null);
        }

        private void ProfileConvert<TSource, TDestination>(TSource[] input, CultureInfo formatProvider, Action<int> compareFunc) where TDestination : new()
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);
            var fastConverter = SafeMap.GetConverter<TSource, TDestination>();

            var emitMapper = ObjectMapperManager.DefaultInstance.GetMapper<TSource, TDestination>();

            Action<Action<int>, int> trycatchDelegate = (Action<int> action, int i) =>
                {
                    try
                    {
                        action(i);
                    }
                    catch (Exception)
                    {
                    }
                };

            if (typeof(TDestination) != typeof(string))
            {
                if (typeof(TDestination) == typeof(DateTime) && typeof(TSource) == typeof(string))
                {
                    AutoMapper.Mapper.CreateMap(typeof(TSource), typeof(TDestination)).ConvertUsing(typeof(AutoMapperDateTimeTypeConverter));
                }
                else
                {
                    AutoMapper.Mapper.CreateMap<TSource, TDestination>();
                }
            }

            AutoMapper.Mapper.CreateMap<Address, AddressDto>();

            this.WriteHeader(string.Format("Profiling convert from {0} to {1}, {2} iterations", typeof(TSource).Name, typeof(TDestination).Name, input.Length));

            if (compareFunc != null)
            {
                this.AddResult("Native", k => trycatchDelegate(compareFunc, k));
            }

            this.AddResult(
                    "SafeMapper",
                    k => trycatchDelegate(i => fastConverter(input[i]), k));

            this.AddResult("EmitMapper", k => trycatchDelegate(i => emitMapper.Map(input[i]), k));

            this.AddResult("FastMapper", k => trycatchDelegate(i => TypeAdapter.Adapt(input[i], sourceType, destinationType), k));


            this.AddResult(
                "ValueInjecter",
                k => trycatchDelegate(
                    i =>
                    {
                        var result = new TDestination();
                        result.InjectFrom(input[i]);
                    }, 
                    k));
            
            /*this.AddResult(
                    "SimpleTypeConverter",
                    i => SimpleTypeConverter.SimpleTypeConverter.ConvertTo(input[i], destinationType, formatProvider));
            */
            /*
            this.AddResult(
                    "UniversalTypeConverter",
                    i => UniversalTypeConverter.Convert(input[i], typeof(TDestination), formatProvider));
            */

            this.AddResult("AutoMapper", k => trycatchDelegate(i => AutoMapper.Mapper.Map<TSource, TDestination>(input[i]), k));

        }
    }
}
