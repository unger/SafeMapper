namespace MapEverything.Profiler
{
    using Fasterflect;

    using MapEverything.Generic;
    using MapEverything.Tests.Model.Person;

    public class ProfilePropertyGetSet : ProfileBase
    {
        public override void Execute()
        {
            var td = new TypeDefinition<Person>();

            var person = new Person();

            var propertyInfo = typeof(Person).GetProperty("Name");
            var memberGetter = propertyInfo.DelegateForGetPropertyValue();
            var typedCompiledGetter = td.GetPropertyGetter<string>("Name");
            var compiledGetter = td.GetPropertyGetter(propertyInfo);

            this.WriteHeader();

            this.AddResult("Reflection PropertyInfo.GetValue", i => propertyInfo.GetValue(person));
            this.AddResult("FasterFlect DelegateForGetPropertyValue", i => memberGetter(person));
            this.AddResult("compiled getter", i => compiledGetter(person));
            this.AddResult("Typed compiled getter", i => typedCompiledGetter(person));
        }
    
    }
}
