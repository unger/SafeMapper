using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEverything.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class TypeMapperListTests : TypeMapperTestsBase
    {
        [Test]
        public void ConvertIntListToIntList()
        {
            var value = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<List<int>, List<int>>(value);

                Assert.IsInstanceOf<List<int>>(converted);

                Assert.AreEqual(value, converted);
            }
        }

        [Test]
        public void ConvertIntListToDecimalList()
        {
            var value = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<List<int>, List<decimal>>(value);

                Assert.IsInstanceOf<List<decimal>>(converted);

                Assert.AreEqual(value, converted);
            }
        }

    }
}
