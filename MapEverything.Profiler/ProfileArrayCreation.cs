using MapEverything.Tests.Model.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEverything.Profiler
{
    using System.Collections;
    using System.Data.SqlTypes;

    using Fasterflect;

    public class ProfileArrayCreation : ProfileBase
    {
        protected override void Execute(int iterations)
        {
            var elementType = typeof(int);

            var fasterflectCctor = typeof(int[]).DelegateForCreateInstance(new[] { typeof(int) });
            var cctor = typeof(int[]).GetConstructor(new[] { typeof(int) });

            Func<object, object> dummyFunc = c => c;


            this.AddResult(this.Profile("new int[]", iterations, i => dummyFunc(new int[1000])));
            this.AddResult(this.Profile("fasterflect cctor", iterations, i => dummyFunc(fasterflectCctor(1000))));
            this.AddResult(this.Profile("Array.CreateInstance", iterations, i => dummyFunc(Array.CreateInstance(elementType, 1000))));
            this.AddResult(this.Profile("Reflection", iterations, i => dummyFunc(cctor.Invoke(new object[] { 1000 }))));
        }
    }
}
