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

            this.AddResult(this.Profile("new Person()", iterations, i => new Person()));
            this.AddResult(this.Profile("fasterflect cctor", iterations, i => fasterflectCctor()));
            this.AddResult(this.Profile("compiled expression", iterations, i => td.CreateInstanceDelegate()));
            this.AddResult(this.Profile("Activator.CreateInstance", iterations, i => Activator.CreateInstance<Person>()));
            this.AddResult(this.Profile("Reflection", iterations, i => cctor.Invoke(new object[] { })));
        }
    }
}
