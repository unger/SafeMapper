using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEverything.Tests
{
    using System.Collections.ObjectModel;

    using MapEverything.Tests.Model;

    using NUnit.Framework;

    [TestFixture]
    public class TypeMapperCollectionTests : TypeMapperTestsBase
    {
        [Test]
        public void ConvertIntICollectionToIntList()
        {
            ICollection<int> value = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<ICollection<int>, List<int>>(value);

                Assert.IsInstanceOf<List<int>>(converted);

                Assert.AreEqual(value, converted);
            }
        }

        [Test]
        public void ConvertIntICollectionToDecimalList()
        {
            ICollection<int> value = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<ICollection<int>, List<decimal>>(value);

                Assert.IsInstanceOf<List<decimal>>(converted);

                Assert.AreEqual(value, converted);
            }
        }

        [Test]
        public void ConvertAddressICollectionToAddressDtoList()
        {
            ICollection<Address> value = new List<Address> { new Address { Id = 1, City = "Test" } };
            var expected = new List<AddressDto> { new AddressDto { Id = 1, City = "Test" } };

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<ICollection<Address>, List<AddressDto>>(value);

                Assert.IsInstanceOf<List<AddressDto>>(converted);

                Assert.AreEqual(expected[0].Id, converted[0].Id);
                Assert.AreEqual(expected[0].City, converted[0].City);
            }
        }
    }
}
