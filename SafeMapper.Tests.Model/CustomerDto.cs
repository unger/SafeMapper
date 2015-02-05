namespace SafeMapper.Tests.Model
{
    using System.Collections.Generic;

    public class CustomerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Address Address { get; set; }

        public AddressDto HomeAddress { get; set; }

        public AddressDto[] Addresses { get; set; }

        public List<AddressDto> WorkAddresses { get; set; }

        public string AddressCity { get; set; }
    }
}
