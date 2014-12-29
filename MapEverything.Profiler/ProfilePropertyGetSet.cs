namespace MapEverything.Profiler
{
    using Fasterflect;

    using MapEverything.Generic;
    using MapEverything.Tests.Model.Person;

    public class ProfilePropertyGetSet : ProfileBase
    {
        protected override void Execute(int iterations)
        {
            var td = new TypeDefinition<Person>();

            var person = new Person();

            var propertyInfo = typeof(Person).GetProperty("Name");
            var memberGetter = propertyInfo.DelegateForGetPropertyValue();
            var typedCompiledGetter = td.GetPropertyGetter<string>("Name");
            var compiledGetter = td.GetPropertyGetter(propertyInfo);

            this.AddResult(this.Profile("Reflection PropertyInfo.GetValue", iterations, i => propertyInfo.GetValue(person)));
            this.AddResult(this.Profile("FasterFlect DelegateForGetPropertyValue", iterations, i => memberGetter(person)));
            this.AddResult(this.Profile("compiled getter", iterations, i => compiledGetter(person)));
            this.AddResult(this.Profile("Typed compiled getter", iterations, i => typedCompiledGetter(person)));
        }
    
    }
}
