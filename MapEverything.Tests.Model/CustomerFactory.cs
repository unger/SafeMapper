namespace MapEverything.Tests.Model
{
    using System.Collections.Generic;

    using MapEverything.Profiler.Model;

    public class CustomerFactory
    {
        public static Customer CreateTestCustomer()
        {
            return new Customer
                       {
                           Id = 1,
                           Name = "Magnus Unger",
                           Credit = 234.7m,
                           Address =
                               new Address
                                   {
                                       City = "Göteborg",
                                       Country = "Sweden",
                                       Id = 1,
                                       Street = "Testgatan"
                                   },
                           HomeAddress =
                               new Address
                                   {
                                       City = "Göteborg",
                                       Country = "Sweden",
                                       Id = 2,
                                       Street = "Testgatan"
                                   },
                           WorkAddresses =
                               new List<Address>
                                   {
                                       new Address
                                           {
                                               City = "Göteborg",
                                               Country = "Sweden",
                                               Id = 5,
                                               Street = "Testgatan"
                                           },
                                       new Address
                                           {
                                               City = "Göteborg",
                                               Country = "Sweden",
                                               Id = 6,
                                               Street = "Testgatan"
                                           }
                                   },
                           Addresses =
                               new List<Address>
                                   {
                                       new Address
                                           {
                                               City = "Göteborg",
                                               Country = "Sweden",
                                               Id = 3,
                                               Street = "Testgatan"
                                           },
                                       new Address
                                           {
                                               City = "Göteborg",
                                               Country = "Sweden",
                                               Id = 4,
                                               Street = "Testgatan"
                                           }
                                   }.ToArray()
                       };
        }
    }
}
