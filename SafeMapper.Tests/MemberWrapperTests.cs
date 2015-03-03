using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMapper.Tests
{
    using System.Collections.Specialized;

    using NUnit.Framework;

    using SafeMapper.Reflection;

    [TestFixture]
    public class MemberWrapperTests
    {
        [Test]
        public void CanRead_MethodInfo_ReturnsTrue()
        {
            var method = typeof(NameValueCollection).GetMethod("GetValues", new[] { typeof(string) });

            var member = new MemberWrapper(method);

            Assert.True(member.CanRead);
        }

        [Test]
        public void CanRead_ConstructorInfo_ReturnsFalse()
        {
            var constructor = typeof(NameValueCollection).GetConstructor(Type.EmptyTypes);

            var member = new MemberWrapper(constructor);

            Assert.False(member.CanRead);
        }

        [Test]
        public void CanWrite_MethodInfo_ReturnsFalse()
        {
            var method = typeof(NameValueCollection).GetMethod("GetValues", new[] { typeof(string) });

            var member = new MemberWrapper(method);

            Assert.False(member.CanWrite);
        }

        [Test]
        public void MemberWrapper_MethodInfoPropertyIndexer_CanReadAndCanWrite()
        {
            var method = typeof(NameValueCollection).GetMethod("GetValues", new[] { typeof(string) });
            var prop = typeof(NameValueCollection).GetProperty("Item", new[] { typeof(string) });

            var member = new MemberWrapper(method, prop);

            Assert.True(member.CanRead);
            Assert.True(member.CanWrite);
        }
    }
}
