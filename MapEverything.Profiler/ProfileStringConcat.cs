using System;

namespace MapEverything.Profiler
{
    public class ProfileStringConcat : ProfileBase
    {
        public override void Execute()
        {
            var string1 = System.Web.Security.Membership.GeneratePassword(30, 10);
            var string2 = System.Web.Security.Membership.GeneratePassword(30, 10);

            Func<object, object> dummyFunc = o => o;
            this.WriteHeader();


            this.AddResult("string +", i => dummyFunc(string1 + string2));
            this.AddResult("string.Format", i => dummyFunc(string.Format("{0}{1}", string1, string2)));
            this.AddResult("string.concat", i => dummyFunc(string.Concat(string1, string2)));
        }
    }
}
