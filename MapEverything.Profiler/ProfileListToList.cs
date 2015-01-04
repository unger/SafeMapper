namespace MapEverything.Profiler
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using AutoMapper;

    using FastMapper;

    public class ProfileListToList : ProfileBase
    {
        public override void Execute()
        {
            var rand = new Random();
            var typeMapper = new TypeMapper();
            var fromType = typeof(List<int>);
            var toType = typeof(List<decimal>);

            var intList = new List<int>(50);
            for (int i = 0; i < 50; i++)
            {
                intList.Add(rand.Next());
            }

            var typeMapperConverter = typeMapper.GetConverter(fromType, toType);
            var elementConverter = typeMapper.GetConverter(typeof(int), typeof(decimal));
            var toElementType = typeof(decimal);

            this.WriteHeader();


            this.AddResult("Array.ConvertAll todecimal", i => intList.ConvertAll(Convert.ToDecimal));
            this.AddResult("Array.ConvertAll changetype", i => intList.ConvertAll(v => (decimal)Convert.ChangeType(v, toElementType)));
            this.AddResult("Array.ConvertAll typemapper", i => intList.ConvertAll(v => (decimal)elementConverter(v)));
            this.AddResult("FastMapper", i => TypeAdapter.Adapt(intList, fromType, toType));
            this.AddResult("TypeMapper", i => typeMapper.Convert(intList, fromType, toType));
            this.AddResult("TypeMapper delegate", i => typeMapper.Convert(intList, typeMapperConverter));
            this.AddResult("AutoMapper", i => Mapper.Map(intList, fromType, toType));
        }
    }
}
