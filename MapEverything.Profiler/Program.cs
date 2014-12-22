namespace MapEverything.Profiler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;

    using AutoMapper;

    using FastMapper;

    using MapEverything.Profiler.AutoMapperHelpers;
    using MapEverything.Profiler.Model;
    using MapEverything.Tests.Model;
    using MapEverything.Utils;

    using TB.ComponentModel;

    public class Program
    {
        public static void Main(string[] args)
        {
            var formatProvider = CultureInfo.CurrentCulture;

            const int Iterations = 10000;
            var stringIntArray = new string[Iterations];
            var stringInvalidArray = new string[Iterations];
            var stringDecimalArray = new string[Iterations];
            var stringGuidArray = new string[Iterations];
            var stringDateTimeArray = new string[Iterations];
            var guidArray = new Guid[Iterations];
            var intArray = new int[Iterations];
            var decimalArray = new decimal[Iterations];
            var dateTimeArray = new DateTime[Iterations];
            var customerArray = new Customer[Iterations];
            var personArray = new Person[Iterations];

            for (int i = 0; i < Iterations; i++)
            {
                stringIntArray[i] = i.ToString(formatProvider);
                stringInvalidArray[i] = System.Web.Security.Membership.GeneratePassword((i % 10) + 1, i % 5);
                stringDecimalArray[i] = (i * 0.9m).ToString(formatProvider);
                stringGuidArray[i] = Guid.NewGuid().ToString();
                intArray[i] = i;
                decimalArray[i] = i * 0.9m;
                guidArray[i] = Guid.NewGuid();
                stringDateTimeArray[i] = DateTime.Now.ToString(formatProvider);
                dateTimeArray[i] = DateTime.Now;
                personArray[i] = new Person
                                     {
                                         Id = Guid.NewGuid(),
                                         Name = "Test Name " + i,
                                         Age = i % 85,
                                         Length = 1.70m + ((i % 20) / 100m)
                                     };
                customerArray[i] = CustomerFactory.CreateTestCustomer();
            }
            /*
            
            ProfileConvert<string, int>(stringIntArray, formatProvider, i => int.Parse(stringIntArray[i], formatProvider));

            //ProfileConvert<string, int>(stringInvalidArray, formatProvider, i => int.Parse(stringInvalidArray[i], formatProvider));

            ProfileConvert<string, decimal>(stringDecimalArray, formatProvider, i => StringParser.TryParseDecimal(stringDecimalArray[i], formatProvider));

            //ProfileConvert<string, decimal>(stringInvalidArray, formatProvider, i => StringParser.TryParseDecimal(stringInvalidArray[i], formatProvider));

            ProfileConvert<string, Guid>(stringGuidArray, formatProvider, i => new Guid(stringGuidArray[i]));

            //ProfileConvert<string, Guid>(stringInvalidArray, formatProvider, i => new Guid(stringInvalidArray[i]));

            ProfileConvert<string, DateTime>(stringDateTimeArray, formatProvider, i => Convert.ToDateTime(stringDateTimeArray[i]));

            //ProfileConvert<string, DateTime>(stringInvalidArray, formatProvider, i => Convert.ToDateTime(stringInvalidArray[i]));


            
            ProfileConvert<int, string>(intArray, formatProvider, i => intArray[i].ToString(formatProvider));
            
            ProfileConvert<decimal, string>(decimalArray, formatProvider, i => decimalArray[i].ToString(formatProvider));
            
            ProfileConvert<Guid, string>(guidArray, CultureInfo.CurrentCulture, i => guidArray[i].ToString());

            ProfileConvert<DateTime, string>(dateTimeArray, CultureInfo.CurrentCulture, i => dateTimeArray[i].ToString());

            ProfileConvert<Customer, CustomerDto>(customerArray, CultureInfo.CurrentCulture, null);*/

            ProfileConvert<Person, PersonDto>(personArray, CultureInfo.CurrentCulture, null);
        }



        private static void ProfileConvert<TSource, TDestination>(TSource[] input, CultureInfo formatProvider, Action<int> compareFunc)
        {
            var typeMapper = new TypeMapper();
            var typeMapperConverter = typeMapper.GetConverter(typeof(TSource), typeof(TDestination), formatProvider);

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

            Console.WriteLine("Profiling convert from {0} to {1}, {2} iterations", typeof(TSource).Name, typeof(TDestination).Name, input.Length);

            var result = new List<Tuple<string, double>>();

            if (compareFunc != null)
            {
                result.Add(Profile("Native", input.Length, compareFunc));
            }

            result.Add(
                Profile(
                    "TypeMapper",
                    input.Length,
                    i => typeMapper.Convert(input[i], typeof(TDestination), formatProvider)));

            result.Add(Profile("TypeMapper delegate", input.Length, i => typeMapper.Convert(input[i], typeMapperConverter)));

            result.Add(
                Profile(
                    "SimpleTypeConverter",
                    input.Length,
                    i => SimpleTypeConverter.ConvertTo(input[i], typeof(TDestination), formatProvider)));


            result.Add(
                Profile(
                    "UniversalTypeConverter",
                    input.Length,
                    i => UniversalTypeConverter.Convert(input[i], typeof(TDestination), formatProvider)));

            result.Add(
                Profile(
                    "FastMapper",
                    input.Length,
                    i => TypeAdapter.Adapt<TSource, TDestination>(input[i])));


            result.Add(Profile("AutoMapper", input.Length, i => Mapper.Map<TSource, TDestination>(input[i])));

            result.Sort((t1, t2) => t1.Item2.CompareTo(t2.Item2));

            foreach (var row in result)
            {
                Console.WriteLine(row.Item1);
            }

            Console.WriteLine();
        }

        private static Tuple<string, double> Profile(string description, int iterations, Action<int> func)
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
                return new Tuple<string, double>(string.Format("{0,-40}{1} ms", description, watch.Elapsed.TotalMilliseconds), watch.Elapsed.TotalMilliseconds);
            }
            catch (Exception e)
            {
                return new Tuple<string, double>(string.Format("{0,-40} throws exception", description, e.Message), double.MaxValue);
            }
        }
    }
}
