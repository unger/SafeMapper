using System;

namespace MapEverything.Profiler
{
    public class ProfileStringConcat : ProfileBase
    {
        protected override void Execute(int iterations)
        {
            var string1 = System.Web.Security.Membership.GeneratePassword(30, 10);
            var string2 = System.Web.Security.Membership.GeneratePassword(30, 10);

            Func<object, object> dummyFunc = o => o;

            this.AddResult(this.Profile("string +", iterations, i => dummyFunc(string1 + string2)));
            this.AddResult(this.Profile("string.Format", iterations, i => dummyFunc(string.Format("{0}{1}", string1, string2))));
            this.AddResult(this.Profile("string.concat", iterations, i => dummyFunc(string.Concat(string1, string2))));
        }
    }
}
