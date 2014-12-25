namespace MapEverything.Profiler
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using AutoMapper;

    using FastMapper;

    public class ProfileArrayToList : ProfileBase
    {
        protected override void Execute(int iterations)
        {
            var rand = new Random();
            var typeMapper = new TypeMapper();
            var fromType = typeof(int[]);
            var toType = typeof(List<decimal>);

            var intArray = new int[50];
            for (int i = 0; i < 50; i++)
            {
                intArray[i] = rand.Next();
            }

            var typeMapperConverter = typeMapper.GetConverter(fromType, toType);
            var elementConverter = typeMapper.GetConverter(typeof(int), typeof(decimal));
            var toElementType = typeof(decimal);

            this.AddResult(this.Profile("Array.ConvertAll todecimal", iterations, i => new List<decimal>(Array.ConvertAll(intArray, Convert.ToDecimal))));
            this.AddResult(this.Profile("Array.ConvertAll changetype", iterations, i => new List<decimal>(Array.ConvertAll(intArray, v => (decimal)Convert.ChangeType(v, toElementType)))));
            this.AddResult(this.Profile("Array.ConvertAll typemapper", iterations, i => new List<decimal>(Array.ConvertAll(intArray, v => (decimal)elementConverter(v)))));
            this.AddResult(this.Profile("FastMapper", iterations, i => TypeAdapter.Adapt(intArray, fromType, toType)));
            this.AddResult(this.Profile("TypeMapper", iterations, i => typeMapper.Convert(intArray, toType)));
            this.AddResult(this.Profile("TypeMapper delegate", iterations, i => typeMapper.Convert(intArray, typeMapperConverter)));
            this.AddResult(this.Profile("AutoMapper", iterations, i => Mapper.Map(intArray, fromType, toType)));
        }
    }
}
