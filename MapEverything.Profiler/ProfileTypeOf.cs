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

    public class ProfileTypeOf : ProfileBase
    {
        protected override void Execute(int iterations)
        {

            Func<object, object> dummyFunc = o => o;

            this.AddResult(this.Profile("typeof(int)", iterations, i => dummyFunc(typeof(int))));
            this.AddResult(this.Profile("GetType", iterations, i => dummyFunc(i.GetType())));
            this.AddResult(this.Profile("GetType.FullName", iterations, i => dummyFunc(i.GetType().FullName)));
            this.AddResult(this.Profile("GetTypeCode", iterations, i => dummyFunc(i.GetTypeCode())));

        }
    }
}
