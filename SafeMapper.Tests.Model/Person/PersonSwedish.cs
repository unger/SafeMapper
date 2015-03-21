namespace SafeMapper.Tests.Model.Person
{
    using System;

    public class PersonSwedish
    {
        public Guid PersonId { get; set; }

        public string Namn { get; set; }

        public int Ålder { get; set; }

        public decimal Längd { get; set; }

        public DateTime Födelsedag { get; set; }
    }
}
