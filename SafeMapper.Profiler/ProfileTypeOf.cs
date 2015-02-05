namespace SafeMapper.Profiler
{
    using System;

    public class ProfileTypeOf : ProfileBase
    {
        public override void Execute()
        {

            Func<object, object> dummyFunc = o => o;
            this.WriteHeader();

            this.AddResult("typeof(int)", i => dummyFunc(typeof(int)));
            this.AddResult("GetType", i => dummyFunc(i.GetType()));
            this.AddResult("GetType.FullName", i => dummyFunc(i.GetType().FullName));
            this.AddResult("GetTypeCode", i => dummyFunc(i.GetTypeCode()));

        }
    }
}
