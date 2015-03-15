namespace SafeMapper.Profiler
{
    using System.Globalization;

    using AutoMapper;

    using EmitMapper;

    using SafeMapper.Tests.Model.Person;
    using SafeMapper.Utils;

    public class ProfileCreateConverter : ProfileBase
    {
        public override void Execute()
        {
            this.ProfileConvert<Person, PersonDto>(CultureInfo.CurrentCulture);
        }

        private void ProfileConvert<TSource, TDestination>(CultureInfo formatProvider)
        {
            var fromType = typeof(TSource);
            var toType = typeof(TDestination);

            this.WriteHeader(string.Format("Profiling convert from {0} to {1}", typeof(TSource).Name, typeof(TDestination).Name));

            this.AddResult("SafeMapper", i => ConverterFactory.CreateDelegate<TSource, TDestination>(formatProvider));

            //this.AddResult("EmitMapper", i => ObjectMapperManager.DefaultInstance.GetMapper<TSource, TDestination>());

            //this.AddResult("AutoMapper", i => Mapper.CreateMap<TSource, TDestination>());
        }
    }
}
