namespace MapEverything.Tests.Model
{
    using System;

    public class PersonDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public decimal Length { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
