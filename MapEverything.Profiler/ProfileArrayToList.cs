namespace MapEverything.Profiler
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using AutoMapper;

    using FastMapper;

    public class ProfileArrayToList : ProfileBase
    {
        public override void Execute()
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

            this.WriteHeader();


            this.AddResult("Array.ConvertAll todecimal", i => new List<decimal>(Array.ConvertAll(intArray, Convert.ToDecimal)));
            this.AddResult("Array.ConvertAll changetype", i => new List<decimal>(Array.ConvertAll(intArray, v => (decimal)Convert.ChangeType(v, toElementType))));
            this.AddResult("Array.ConvertAll typemapper", i => new List<decimal>(Array.ConvertAll(intArray, v => (decimal)elementConverter(v))));
            this.AddResult("FastMapper", i => TypeAdapter.Adapt(intArray, fromType, toType));
            this.AddResult("TypeMapper", i => typeMapper.Convert(intArray, fromType, toType));
            this.AddResult("TypeMapper delegate", i => typeMapper.Convert(intArray, typeMapperConverter));
            this.AddResult("AutoMapper", i => Mapper.Map(intArray, fromType, toType));
        }
    }
}
