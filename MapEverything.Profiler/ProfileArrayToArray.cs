namespace MapEverything.Profiler
{
    using System;
    using System.Collections;
    using System.Xml;

    using AutoMapper;

    using EmitMapper;

    using Fasterflect;

    using FastMapper;

    using MapEverything.Utils;

    public class ProfileArrayToArray : ProfileBase
    {
        public override void Execute()
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
            var elementConverter = typeMapper.GetConverter(typeof(int), typeof(decimal));
            var toElementType = typeof(decimal);

            this.WriteHeader();

            
            var genericConvertType = typeof(Converter<,>).MakeGenericType(typeof(int), typeof(decimal));
            var fasterflectConvertAll = typeof(Array).DelegateForCallMethod(
                new Type[] { typeof(int), typeof(decimal) },
                "ConvertAll",
                new Type[] { fromType, genericConvertType });

            var getConverterDelegate = typeMapper.GetType().DelegateForCallMethod(
                new Type[] { typeof(int), typeof(decimal) },
                "GetConverter",
                new Type[0]);

            var genericConverter = getConverterDelegate(typeMapper);

            var dynamicConverter = ConverterFactory.Create<int[], decimal[]>();
            var emitMapper = ObjectMapperManager.DefaultInstance.GetMapper<int[], decimal[]>();



            this.AddResult("Array.ConvertAll todecimal", i => Array.ConvertAll(intArray, Convert.ToDecimal));
            this.AddResult("Array.ConvertAll changetype", i => Array.ConvertAll(intArray, v => Convert.ChangeType(v, toElementType)));
            this.AddResult("Array.ConvertAll typemapper", i => Array.ConvertAll(intArray, v => elementConverter(v)));
            this.AddResult("fasterflect", i => fasterflectConvertAll(null, intArray, genericConverter));
            this.AddResult("EmitMapper", i => emitMapper.Map(intArray));
            this.AddResult("DynamicConverter", i => dynamicConverter(intArray));
            this.AddResult("FastMapper", i => TypeAdapter.Adapt(intArray, fromType, toType));
            this.AddResult("TypeMapper", i => typeMapper.Convert(intArray, fromType, toType));
            this.AddResult("TypeMapper delegate", i => typeMapper.Convert(intArray, typeMapperConverter));
            //this.AddResult("AutoMapper", i => Mapper.Map(intArray, fromType, toType));
            this.AddResult("Manual forloop", i => this.ConvertArrayManual(intArray, toElementType, v => Convert.ToDecimal(v)));
            this.AddResult("Manual forloop rev", i => this.ConvertArrayManualReverse(intArray, toElementType, v => Convert.ToDecimal(v)));
            this.AddResult("Manual foreachloop", i => this.ConvertArrayManualForEach(intArray, toElementType, v => Convert.ToDecimal(v)));
        }

        private object ConvertArrayManual(object input, Type elementType, Func<object, object> converter)
        {
            var array = (IList)input;
            var newArray = (IList)Array.CreateInstance(elementType, array.Count);
            for (int i = 0; i < array.Count; i++)
            {
                newArray[i] = converter(array[i]);
            }

            return newArray;
        }

        private object ConvertArrayManualForEach(object input, Type elementType, Func<object, object> converter)
        {
            var array = (IList)input;
            var newArray = (IList)Array.CreateInstance(elementType, array.Count);
            int i = 0;
            foreach (var elem in array)
            {
                newArray[i++] = converter(elem);
            }

            return newArray;
        }


        private object ConvertArrayManualReverse(object input, Type elementType, Func<object, object> converter)
        {
            var array = (IList)input;
            var newArray = (IList)Array.CreateInstance(elementType, array.Count);
            for (int i = array.Count - 1; i >= 0; i--)
            {
                newArray[i] = converter(array[i]);
            }

            return newArray;
        }

    }
}
