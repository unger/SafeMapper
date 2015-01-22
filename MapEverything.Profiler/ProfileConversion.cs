namespace MapEverything.Profiler
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using AutoMapper;

    using EmitMapper;

    using FastMapper;

    using MapEverything.Profiler.AutoMapperHelpers;
    using MapEverything.Tests.Model;
    using MapEverything.Tests.Model.Person;
    using MapEverything.Utils;

    using ServiceStack;

    public class ProfileConversion : ProfileBase
    {
        public override void Execute()
        {
            var formatProvider = CultureInfo.CurrentCulture;
            var maxIterations = this.MaxIterations;

            var stringIntArray = new string[maxIterations];
            var stringDecimalArray = new string[maxIterations];
            var stringGuidArray = new string[maxIterations];
            var stringDateTimeArray = new string[maxIterations];
            var guidArray = new Guid[maxIterations];
            var intArray = new int[maxIterations];
            var decimalArray = new decimal[maxIterations];
            var dateTimeArray = new DateTime[maxIterations];
            var customerArray = new Customer[maxIterations];
            var customerDtoArray = new CustomerDto[maxIterations];
            var personArray = new Person[maxIterations];
            var personStringArray = new PersonStringDto[maxIterations];
            var addressArray = new Address[maxIterations];

            for (int i = 0; i < maxIterations; i++)
            {
                stringIntArray[i] = i.ToString(formatProvider);
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
                    Length = 1.70m + ((i % 20) / 100m),
                    BirthDate = DateTime.Now.AddDays(i)
                };
                personStringArray[i] = new PersonStringDto
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Test Name " + i,
                    Age = (i % 85).ToString(CultureInfo.CurrentCulture),
                    Length = (1.70m + ((i % 20) / 100m)).ToString(CultureInfo.CurrentCulture),
                    BirthDate = DateTime.Now.AddDays(i).ToString(CultureInfo.CurrentCulture)
                };
                customerArray[i] = CustomerFactory.CreateTestCustomer();
                customerDtoArray[i] = TypeAdapter.Adapt<CustomerDto>(customerArray[i]);
                addressArray[i] = new Address
                                      {
                                          Id = i, 
                                          City = "Gbg",
                                          Country = "Sweden",
                                          Street = "Street 1"
                                      };
            }

            // FromString conversions
            //this.ProfileConvert<string, Guid>(stringGuidArray, formatProvider, i => new Guid(stringGuidArray[i]));
            this.ProfileConvert<string, int>(stringIntArray, formatProvider, i => int.Parse(stringIntArray[i], formatProvider));
            /*this.ProfileConvert<string, string>(stringIntArray, formatProvider, i => stringIntArray[i].Clone());
            this.ProfileConvert<string, DateTime>(stringDateTimeArray, formatProvider, i => Convert.ToDateTime(stringDateTimeArray[i]));
            this.ProfileConvert<string, decimal>(stringDecimalArray, formatProvider, i => StringParser.TryParseDecimal(stringDecimalArray[i], formatProvider));

            this.ProfileConvert<int, string>(intArray, formatProvider, i => intArray[i].ToString(formatProvider));
            this.ProfileConvert<int, int>(intArray, formatProvider, i => Convert.ChangeType(i, typeof(int)));

            this.ProfileConvert<decimal, string>(decimalArray, formatProvider, i => decimalArray[i].ToString(formatProvider));

            this.ProfileConvert<Guid, string>(guidArray, CultureInfo.CurrentCulture, i => guidArray[i].ToString());

            this.ProfileConvert<DateTime, string>(dateTimeArray, CultureInfo.CurrentCulture, i => dateTimeArray[i].ToString());
            */
            //this.ProfileConvert<PersonStringDto, Person>(personStringArray, CultureInfo.CurrentCulture, null);
            
            //this.ProfileConvert<Customer, CustomerDto>(customerArray, CultureInfo.CurrentCulture, null);
            
            this.ProfileConvert<CustomerDto, Customer>(customerDtoArray, CultureInfo.CurrentCulture, null);
            
            //this.ProfileConvert<Address, AddressDto>(addressArray, CultureInfo.CurrentCulture, null);

            //this.ProfileConvert<Person, PersonStringDto>(personArray, CultureInfo.CurrentCulture, null);

            //this.ProfileConvert<Person, PersonDto>(personArray, CultureInfo.CurrentCulture, null);

            /*this.ProfileConvert<Person, PersonStruct>(personArray, CultureInfo.CurrentCulture, null);

            this.ProfileConvert<int, decimal>(intArray, formatProvider, i => Convert.ToDecimal(intArray[i], formatProvider));
            */
        }

        private void ProfileConvert<TSource, TDestination>(TSource[] input, CultureInfo formatProvider, Action<int> compareFunc)
        {
            var dynamicConverter = ConverterFactory.Create<TSource, TDestination>();
            var typeMapper = new TypeMapper();
            var typeMapperConverter = typeMapper.GetConverter(typeof(TSource), typeof(TDestination), formatProvider);
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);

            var emitMapper = ObjectMapperManager.DefaultInstance.GetMapper<TSource, TDestination>();

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
            
            if (compareFunc != null)
            {
                this.AddResult("Native", compareFunc);
            }

            this.AddResult("DynamicConverter", i => dynamicConverter(input[i]));

            this.AddResult("EmitMapper", i => emitMapper.Map(input[i]));

            this.AddResult("TypeMapper delegate", i => typeMapper.Convert(input[i], typeMapperConverter));

            this.AddResult(
                  "TypeMapper",
                  i => typeMapper.Convert(input[i], sourceType, destinationType, formatProvider));

            this.AddResult(
                    "FastMapper",
                    i => TypeAdapter.Adapt(input[i], sourceType, destinationType));



            /*this.AddResult(
                    "ServiceStack",
                    i => input[i].ConvertTo<TDestination>());
            
            this.AddResult(
                    "SimpleTypeConverter",
                    i => SimpleTypeConverter.ConvertTo(input[i], typeof(TDestination), formatProvider));


            this.AddResult(
                    "UniversalTypeConverter",
                    i => UniversalTypeConverter.Convert(input[i], typeof(TDestination), formatProvider));


            this.AddResult("AutoMapper", i => Mapper.Map<TSource, TDestination>(input[i]));*/

        }
    }
}
