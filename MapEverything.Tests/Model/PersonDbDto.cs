namespace MapEverything.Tests.Model
{
    using System;
    using System.Data.SqlTypes;

    public class PersonDbDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public short Age { get; set; }

        public double Length { get; set; }

        public SqlDateTime BirthDate { get; set; }
    }
}