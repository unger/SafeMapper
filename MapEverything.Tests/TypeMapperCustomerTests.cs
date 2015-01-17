namespace MapEverything.Tests
{
    using MapEverything.Tests.Model;

    using NUnit.Framework;

    [TestFixture]
    public class TypeMapperCustomerTests : TypeMapperTestsBase
    {
        [Test]
        public void CanConvertCustomerToCustomerDto()
        {
            var value = CustomerFactory.CreateTestCustomer();

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<Customer, CustomerDto>(value);

                Assert.IsInstanceOf<CustomerDto>(converted);

                Assert.AreEqual(value.Id, converted.Id);
                Assert.AreEqual(value.Name, converted.Name);
            }
        }
    }
}
