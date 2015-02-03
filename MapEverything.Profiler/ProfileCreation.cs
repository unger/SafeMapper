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
        public override void Execute()
        {
            var cctor = typeof(Person).GetConstructor(Type.EmptyTypes);
            var fasterflectCctor = cctor.DelegateForCreateInstance();
            
            this.WriteHeader();


            this.AddResult("new Person()", i => new Person());
            this.AddResult("fasterflect cctor", i => fasterflectCctor());
            this.AddResult("Activator.CreateInstance", i => Activator.CreateInstance<Person>());
            this.AddResult("Reflection", i => cctor.Invoke(new object[] { }));


        }
    }
}
