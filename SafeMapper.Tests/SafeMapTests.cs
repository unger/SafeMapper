using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMapper.Tests
{
    using System.Globalization;

    using NUnit.Framework;

    [TestFixture]
    public class SafeMapTests
    {
        [Test]
        public void Convert_NonGeneric_StringToInt()
        {
            var result = SafeMap.Convert("10", typeof(string), typeof(int));

            Assert.AreEqual(10, result);
        }

        [Test]
        public void Convert_NonGenericWithFormat_StringToInt()
        {
            var result = SafeMap.Convert("10", typeof(string), typeof(int), CultureInfo.CurrentCulture);

            Assert.AreEqual(10, result);
        }

        [Test]
        public void Convert_Generic_StringToInt()
        {
            var result = SafeMap.Convert<string, int>("10");

            Assert.AreEqual(10, result);
        }

        [Test]
        public void Convert_GenericWithFormat_StringToInt()
        {
            var result = SafeMap.Convert<string, int>("10", CultureInfo.CurrentCulture);

            Assert.AreEqual(10, result);
        }

        [Test]
        public void GetConverter_NonGeneric_StringToInt()
        {
            var converter = SafeMap.GetConverter(typeof(string), typeof(int));
            var result = converter("10");

            Assert.AreEqual(10, result);
        }

        [Test]
        public void GetConverter_NonGenericWithFormat_StringToInt()
        {
            var converter = SafeMap.GetConverter(typeof(string), typeof(int), CultureInfo.CurrentCulture);
            var result = converter("10");

            Assert.AreEqual(10, result);
        }

        [Test]
        public void GetConverter_Generic_StringToInt()
        {
            var converter = SafeMap.GetConverter<string, int>();
            var result = converter("10");

            Assert.AreEqual(10, result);
        }

        [Test]
        public void GetConverter_GenericWithFormat_StringToInt()
        {
            var converter = SafeMap.GetConverter<string, int>(CultureInfo.CurrentCulture);
            var result = converter("10");

            Assert.AreEqual(10, result);
        }
    }
}
