namespace SafeMapper.Tests.Model.Person
{
    using System;

    public class Person
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public decimal Length { get; set; }

        public DateTime BirthDate { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Name, this.Age);
        }
    }
}