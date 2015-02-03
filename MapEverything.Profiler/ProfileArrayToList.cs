namespace MapEverything.Profiler
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using AutoMapper;

    using EmitMapper;

    using FastMapper;

    using MapEverything.Utils;

    public class ProfileArrayToList : ProfileBase
    {
        public override void Execute()
        {
            var rand = new Random();
            var fromType = typeof(int[]);
            var toType = typeof(List<decimal>);

            var intArray = new int[50];
            for (int i = 0; i < 50; i++)
            {
                intArray[i] = rand.Next();
            }

            var toElementType = typeof(decimal);

            var dynamicConverter = ConverterFactory.Create<int[], List<decimal>>();
            var emitMapper = ObjectMapperManager.DefaultInstance.GetMapper<int[], List<decimal>>();

            this.WriteHeader();


            this.AddResult("Array.ConvertAll todecimal", i => new List<decimal>(Array.ConvertAll(intArray, Convert.ToDecimal)));
            this.AddResult("Array.ConvertAll changetype", i => new List<decimal>(Array.ConvertAll(intArray, v => (decimal)Convert.ChangeType(v, toElementType))));
            this.AddResult("EmitMapper", i => emitMapper.Map(intArray));
            this.AddResult("DynamicConverter", i => dynamicConverter(intArray));
            this.AddResult("FastMapper", i => TypeAdapter.Adapt(intArray, fromType, toType));
            this.AddResult("AutoMapper", i => Mapper.Map(intArray, fromType, toType));
        }
    }
}
