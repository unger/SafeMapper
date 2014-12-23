using MapEverything.Tests.Model.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEverything.Profiler
{
    using Fasterflect;

    public class ProfilePropertyGetSet : ProfileBase
    {
        protected override void Execute(int iterations)
        {
            var td = new TypeDefinition<Person>();

            var person = new Person();

            var propertyInfo = typeof(Person).GetProperty("Name");
            var memberGetter = propertyInfo.DelegateForGetPropertyValue();
            var typedCompiledGetter = td.GetPropertyGetter<string>("Name");
            var compiledGetter = td.GetPropertyGetter(propertyInfo);

            Console.WriteLine("{0}", Profile("Reflection PropertyInfo.GetValue", iterations, i => propertyInfo.GetValue(person)).Item1);
            Console.WriteLine("{0}", Profile("FasterFlect DelegateForGetPropertyValue", iterations, i => memberGetter(person)).Item1);
            Console.WriteLine("{0}", Profile("compiled getter", iterations, i => compiledGetter(person)).Item1);
            Console.WriteLine("{0}", Profile("Typed compiled getter", iterations, i => typedCompiledGetter(person)).Item1);
        }
    
    }
}
