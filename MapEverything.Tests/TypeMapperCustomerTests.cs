﻿namespace MapEverything.Tests
{
    using System;
    using System.Data.SqlTypes;

    using MapEverything.Converters;
    using MapEverything.Profiler.Model;
    using MapEverything.Tests.Model;

    using NUnit.Framework;

    [TestFixture]
    public class TypeMapperCustomerTests : TypeMapperTestsBase
    {
        [Test]
        public void CanConvertPersonStructToPerson()
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
