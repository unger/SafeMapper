namespace SafeMapper.Tests
{
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using NUnit.Framework;

    using SafeMapper.Reflection;
    using SafeMapper.Tests.Model.GenericClasses;

    [TestFixture]
    public class ReflectionUtilsTests
    {
        [Test]
        public void GetMember_StringPropertyClassExistingMember_ShouldReturnMember()
        {
            var member = ReflectionUtils.GetMember(typeof(ClassProperty<string>), "Value");

            Assert.NotNull(member);
        }

        [Test]
        public void GetMember_StringPropertyClassNonExistingMember_ShouldReturnNull()
        {
            var member = ReflectionUtils.GetMember(typeof(ClassProperty<string>), "NotExisting");

            Assert.Null(member);
        }

        [Test]
        public void GetMember_StringObjectDictionary_ShouldReturnStringIndexer()
        {
            var member = ReflectionUtils.GetMember(typeof(Dictionary<string, string>), "MemberName");

            Assert.NotNull(member);
        }

        [Test]
        public void GetMember_NameValueCollection_ShouldReturnStringIndexer()
        {
            var member = ReflectionUtils.GetMember(typeof(NameValueCollection), "MemberName");

            Assert.NotNull(member);
        }


    }
}
