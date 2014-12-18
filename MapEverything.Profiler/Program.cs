namespace MapEverything.Profiler
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using AutoMapper;

    using LuceneNetExtensions;

    using MapEverything.Converters;

    using TB.ComponentModel;

    public class Program
    {
        public static void Main(string[] args)
        {
            var formatProvider = CultureInfo.CurrentCulture;

            const int Iterations = 100000;
            var stringIntArray = new string[Iterations];
            var stringDecimalArray = new string[Iterations];
            var stringGuidArray = new string[Iterations];
            var guidArray = new Guid[Iterations];
            var intArray = new int[Iterations];
            var decimalArray = new decimal[Iterations];

            for (int i = 0; i < Iterations; i++)
            {
                stringIntArray[i] = i.ToString(formatProvider);
                stringDecimalArray[i] = (i * 0.9m).ToString(formatProvider);
                stringGuidArray[i] = Guid.NewGuid().ToString();
                intArray[i] = i;
                decimalArray[i] = i * 0.9m;
                guidArray[i] = Guid.NewGuid();
            }

            ProfileConvert<string, int>(stringIntArray, formatProvider, i => int.Parse(stringIntArray[i], formatProvider));

            ProfileConvert<string, decimal>(stringIntArray, formatProvider, i => decimal.Parse(stringDecimalArray[i], formatProvider));

            ProfileConvert<string, Guid>(stringGuidArray, formatProvider, i => new Guid(stringGuidArray[i]));

            ProfileConvert<int, string>(intArray, formatProvider, i => intArray[i].ToString(formatProvider));

            ProfileConvert<decimal, string>(decimalArray, formatProvider, i => decimalArray[i].ToString(formatProvider));
            
            ProfileConvert<Guid, string>(guidArray, CultureInfo.CurrentCulture, i => guidArray[i].ToString());
        }

        private static void ProfileConvert<TSource, TDestination>(TSource[] input, CultureInfo formatProvider, Action<int> compareFunc)
        {
            var reflectionMapper = new ReflectionTypeMapper();
            var standardMapper = new TypeMapper();

            var reflectionConverter = reflectionMapper.GetConverter(typeof(TSource), typeof(TDestination), formatProvider);
            var standardConverter = reflectionMapper.GetConverter(typeof(TSource), typeof(TDestination), formatProvider);

            Console.WriteLine("Profiling convert from {0} to {1}, {2} iterations", typeof(TSource).Name, typeof(TDestination).Name, input.Length);

            Profile(
                "Native",
                input.Length,
                compareFunc);


            Profile(
                "StandardTypeMapper",
                input.Length,
                i => standardMapper.Convert(input[i], typeof(TDestination), formatProvider));

            Profile(
                "StandardTypeMapper delegate",
                input.Length,
                i => standardConverter(input[i]));

            Profile(
                "ReflectionTypeMapper",
                input.Length,
                i => reflectionMapper.Convert(input[i], typeof(TDestination), formatProvider));

            Profile(
                "ReflectionTypeMapper delegate",
                input.Length,
                i => reflectionConverter(input[i]));


            Profile(
                "SimpleTypeConverter",
                input.Length,
                i => SimpleTypeConverter.ConvertTo(input[i], typeof(TDestination), formatProvider));


            Profile(
                "UniversalTypeConverter",
                input.Length,
                i => UniversalTypeConverter.Convert(input[i], typeof(TDestination), formatProvider));


            Profile(
                "AutoMapper",
                input.Length,
                i => Mapper.Map<TSource, TDestination>(input[i]));

            Console.WriteLine();
        }

        private static void Profile(string description, int iterations, Action<int> func)
        {
            try
            {
                // warm up 
                func(0);

                var watch = new Stopwatch();

                // clean up
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                watch.Start();
                for (int i = 0; i < iterations; i++)
                {
                    func(i);
                }

                watch.Stop();
                Console.WriteLine("{0,-40}{1} ms", description, watch.Elapsed.TotalMilliseconds);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0,-40} throws exception {1}", description, e.Message);
            }
        }

    }
}
