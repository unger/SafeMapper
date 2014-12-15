using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEverything.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class TypeMapperTests
    {
        [Test]
        public void CanConvertStringToInt()
        {
            var mapper = new TypeMapper();
            var value = "12345";

            var converted = mapper.ConvertTo(value, typeof(int));

            Assert.AreEqual(12345, converted);
        }

    }
}
