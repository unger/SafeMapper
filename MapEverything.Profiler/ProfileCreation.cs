using MapEverything.Tests.Model.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEverything.Profiler
{
    using System.Data.SqlTypes;

    using Fasterflect;

    public class ProfileCreation : ProfileBase
    {
        protected override void Execute(int iterations)
        {
            var td = new TypeDefinition<Person>();
            var fasterflectCctor = typeof(Person).DelegateForCreateInstance(Type.EmptyTypes);
            var cctor = typeof(Person).GetConstructor(Type.EmptyTypes);

            Console.WriteLine("{0}", Profile("new Person()", iterations, i => new Person()).Item1);
            Console.WriteLine("{0}", Profile("fasterflect cctor", iterations, i => fasterflectCctor()).Item1);
            Console.WriteLine("{0}", Profile("compiled expression", iterations, i => td.CreateInstanceDelegate()).Item1);
            Console.WriteLine("{0}", Profile("Activator.CreateInstance", iterations, i => Activator.CreateInstance<Person>()).Item1);
            Console.WriteLine("{0}", Profile("Reflection", iterations, i => cctor.Invoke(new object[] { })).Item1);
            Console.WriteLine();
        }
    }
}
