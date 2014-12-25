namespace MapEverything.Profiler
{
    using System;
    using System.Xml;

    using AutoMapper;

    using FastMapper;

    public class ProfileArrayConversion : ProfileBase
    {
        protected override void Execute(int iterations)
        {
            var rand = new Random();
            var typeMapper = new TypeMapper();
            var fromType = typeof(int[]);
            var toType = typeof(decimal[]);



            var intArray = new int[50];
            for (int i = 0; i < 50; i++)
            {
                intArray[i] = rand.Next();
            }

            var typeMapperConverter = typeMapper.GetConverter(fromType, toType);

            this.AddResult(this.Profile("FastMapper", iterations, i => TypeAdapter.Adapt(intArray, fromType, toType)));
            this.AddResult(this.Profile("TypeMapper", iterations, i => typeMapper.Convert(intArray, toType)));
            this.AddResult(this.Profile("TypeMapper delegate", iterations, i => typeMapper.Convert(intArray, typeMapperConverter)));
            this.AddResult(this.Profile("AutoMapper", iterations, i => Mapper.Map(intArray, fromType, toType)));
        }
    
    }
}
