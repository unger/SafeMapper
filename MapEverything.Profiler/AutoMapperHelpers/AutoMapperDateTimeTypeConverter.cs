namespace MapEverything.Profiler.AutoMapperHelpers
{
    using System;

    using AutoMapper;

    public class AutoMapperDateTimeTypeConverter : TypeConverter<string, DateTime>
    {
        protected override DateTime ConvertCore(string source)
        {
            return System.Convert.ToDateTime(source);
        }
    }
}
