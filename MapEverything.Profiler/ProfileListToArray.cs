namespace MapEverything.Profiler
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using AutoMapper;

    using EmitMapper;

    using FastMapper;

    using MapEverything.Utils;

    public class ProfileListToArray : ProfileBase
    {
        public override void Execute()
        {
            var rand = new Random();
            var typeMapper = new TypeMapper();
            var fromType = typeof(List<int>);
            var toType = typeof(decimal[]);

            var intList = new List<int>(50);
            for (int i = 0; i < 50; i++)
            {
                intList.Add(rand.Next());
            }

            var typeMapperConverter = typeMapper.GetConverter(fromType, toType);
            var elementConverter = typeMapper.GetConverter(typeof(int), typeof(decimal));
            var toElementType = typeof(decimal);

            var dynamicConverter = ConverterFactory.Create<List<int>, decimal[]>();
            var emitMapper = ObjectMapperManager.DefaultInstance.GetMapper<List<int>, decimal[]>();

            this.WriteHeader();


            this.AddResult("Array.ConvertAll todecimal", i => Array.ConvertAll(intList.ToArray(), Convert.ToDecimal));
            this.AddResult("Array.ConvertAll changetype", i => Array.ConvertAll(intList.ToArray(), v => (decimal)Convert.ChangeType(v, toElementType)));
            this.AddResult("Array.ConvertAll typemapper", i => Array.ConvertAll(intList.ToArray(), v => (decimal)elementConverter(v)));
            this.AddResult("EmitMapper", i => emitMapper.Map(intList));
            this.AddResult("DynamicConverter", i => dynamicConverter(intList));
            this.AddResult("FastMapper", i => TypeAdapter.Adapt(intList, fromType, toType));
            this.AddResult("TypeMapper", i => typeMapper.Convert(intList, fromType, toType));
            this.AddResult("TypeMapper delegate", i => typeMapper.Convert(intList, typeMapperConverter));
            this.AddResult("AutoMapper", i => Mapper.Map(intList, fromType, toType));
        }
    }
}
