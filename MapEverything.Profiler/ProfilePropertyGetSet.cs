namespace MapEverything.Profiler
{
    using Fasterflect;

    using MapEverything.Tests.Model.Person;

    public class ProfilePropertyGetSet : ProfileBase
    {
        public override void Execute()
        {
            var person = new Person();

            var propertyInfo = typeof(Person).GetProperty("Name");
            var memberGetter = propertyInfo.DelegateForGetPropertyValue();

            this.WriteHeader();

            this.AddResult("Reflection PropertyInfo.GetValue", i => propertyInfo.GetValue(person));
            this.AddResult("FasterFlect DelegateForGetPropertyValue", i => memberGetter(person));
        }
    
    }
}
