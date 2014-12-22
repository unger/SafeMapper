namespace MapEverything.Profiler.Model
{
    using System.Collections.Generic;

    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal? Credit { get; set; }

        public Address Address { get; set; }

        public Address HomeAddress { get; set; }

        public Address[] Addresses { get; set; }

        public ICollection<Address> WorkAddresses { get; set; }
    }
}