namespace MapEverything.Profiler
{
    using System;
    using System.Collections;
    using System.Xml;

    using AutoMapper;

    using Fasterflect;

    using FastMapper;

    public class ProfileArrayToArray : ProfileBase
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
            var elementConverter = typeMapper.GetConverter(typeof(int), typeof(decimal));
            var toElementType = typeof(decimal);

            /*
            var genericConvertType = typeof(Converter<,>).MakeGenericType(typeof(int), typeof(decimal));
            var fasterflectConvertAll = typeof(Array).DelegateForCallMethod(
                new Type[] { typeof(int), typeof(decimal) },
                "ConvertAll",
                new Type[] { fromType, genericConvertType });*/

            this.AddResult(this.Profile("Array.ConvertAll todecimal", iterations, i => Array.ConvertAll(intArray, Convert.ToDecimal)));
            this.AddResult(this.Profile("Array.ConvertAll changetype", iterations, i => Array.ConvertAll(intArray, v => Convert.ChangeType(v, toElementType))));
            this.AddResult(this.Profile("Array.ConvertAll typemapper", iterations, i => Array.ConvertAll(intArray, v => elementConverter(v))));
            this.AddResult(this.Profile("FastMapper", iterations, i => TypeAdapter.Adapt(intArray, fromType, toType)));
            this.AddResult(this.Profile("TypeMapper", iterations, i => typeMapper.Convert(intArray, fromType, toType)));
            this.AddResult(this.Profile("TypeMapper delegate", iterations, i => typeMapper.Convert(intArray, typeMapperConverter)));
            this.AddResult(this.Profile("AutoMapper", iterations, i => Mapper.Map(intArray, fromType, toType)));
            this.AddResult(this.Profile("Manual forloop", iterations, i => ConvertArrayManual(intArray, toElementType, v => Convert.ToDecimal(v))));
        }

        private object ConvertArrayManual(object input, Type elementType, Func<object, object> converter)
        {
            var array = (IList)input;
            var newArray = (IList)Array.CreateInstance(elementType, array.Count);
            for (int i = 0; i < array.Count; i++)
            {
                //newArray.SetValue(converter(array.GetValue(i)), i);
                newArray[i] = converter(array[i]);
            }

            return newArray;
        }
    }
}
