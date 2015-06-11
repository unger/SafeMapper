namespace SafeMapper.Tests
{
    using System;
    using System.Collections.Specialized;

    using NUnit.Framework;

    using SafeMapper.Reflection;

    [TestFixture]
    public class MemberTests
    {
        [Test]
        public void Ctor_ConstructorInfo_ShouldBeMethodTypeUndefined()
        {
            var constructor = typeof(NameValueCollection).GetConstructor(Type.EmptyTypes);

            var member = new MemberGetter(constructor);

            Assert.AreEqual(MemberType.Undefined, member.MemberType);
        }
    }
}
