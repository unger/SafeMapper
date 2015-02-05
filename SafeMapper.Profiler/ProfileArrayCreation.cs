namespace SafeMapper.Profiler
{
    using System;

    using Fasterflect;

    public class ProfileArrayCreation : ProfileBase
    {
        public override void Execute()
        {
            var elementType = typeof(int);

            var fasterflectCctor = typeof(int[]).DelegateForCreateInstance(new[] { typeof(int) });
            var cctor = typeof(int[]).GetConstructor(new[] { typeof(int) });

            Func<object, object> dummyFunc = c => c;

            this.WriteHeader();

            this.AddResult("new int[]", i => dummyFunc(new int[1000]));
            this.AddResult("fasterflect cctor", i => dummyFunc(fasterflectCctor(1000)));
            this.AddResult("Array.CreateInstance", i => dummyFunc(Array.CreateInstance(elementType, 1000)));
            this.AddResult("Reflection", i => dummyFunc(cctor.Invoke(new object[] { 1000 })));
        }
    }
}
