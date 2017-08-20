namespace SafeMapper.Profiler.AutoMapperHelpers
{
    using System;

    using AutoMapper;

    public class AutoMapperDateTimeTypeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            return System.Convert.ToDateTime(source);
        }
    }
}
