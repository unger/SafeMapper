using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEverything.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class TypeMapperArrayTests : TypeMapperTestsBase
    {
        [Test]
        public void ConvertIntArrayToIntArray()
        {
            var value = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<int[], int[]>(value);

                Assert.IsInstanceOf<int[]>(converted);

                Assert.AreEqual(value, converted);
            }
        }

        [Test]
        public void ConvertIntArrayToDecimalArray()
        {
            var value = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<int[], decimal[]>(value);

                Assert.IsInstanceOf<decimal[]>(converted);

                Assert.AreEqual(value, converted);
            }
        }

        [Test]
        public void ConvertIntArrayToIntList()
        {
            var value = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            foreach (var mapper in this.Mappers)
            {
                var converted = mapper.Convert<int[], List<int>>(value);

                Assert.IsInstanceOf<List<int>>(converted);

                Assert.AreEqual(value, converted);
            }
        }


    }
}
