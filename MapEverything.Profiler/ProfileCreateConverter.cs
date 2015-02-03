using MapEverything.Tests.Model.Person;

namespace MapEverything.Profiler
{
    using System.Globalization;

    using AutoMapper;

    using EmitMapper;

    using MapEverything.Utils;

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
            
            this.AddResult("DynamicConverter", i => ConverterFactory.Create<TSource, TDestination>());

            this.AddResult("EmitMapper", i => ObjectMapperManager.DefaultInstance.GetMapper<TSource, TDestination>());

            this.AddResult("AutoMapper", i => Mapper.CreateMap<TSource, TDestination>());
        }
    }
}
