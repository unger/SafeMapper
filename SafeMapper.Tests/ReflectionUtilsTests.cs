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
        public void GetMember_StringFieldClassExistingMember_ShouldReturnMember()
        {
            var member = ReflectionUtils.GetMember(typeof(ClassField<string>), "Value");

            Assert.NotNull(member);
        }

        [Test]
        public void GetMember_StringFieldClassNonExistingMember_ShouldReturnNull()
        {
            var member = ReflectionUtils.GetMember(typeof(ClassField<string>), "NotExisting");

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

        [Test]
        public void GetElementType_ArrayOfInt_ShouldReturnInt()
        {
            var type = ReflectionUtils.GetElementType(typeof(int[]));

            Assert.AreEqual(typeof(int), type);
        }

        [Test]
        public void GetElementType_ListOfInt_ShouldReturnInt()
        {
            var type = ReflectionUtils.GetElementType(typeof(List<int>));

            Assert.AreEqual(typeof(int), type);
        }

        [Test]
        public void GetElementType_Int_ShouldReturnNull()
        {
            var type = ReflectionUtils.GetElementType(typeof(int));

            Assert.Null(type);
        }

        [Test]
        public void GetConcreteType_IListOfInt_ShouldReturnListOfInt()
        {
            var type = ReflectionUtils.GetConcreteType(typeof(IList<int>));

            Assert.AreEqual(typeof(List<int>), type);
        }

        [Test]
        public void GetConcreteType_Int_ShouldReturnInt()
        {
            var type = ReflectionUtils.GetConcreteType(typeof(int));

            Assert.AreEqual(typeof(int), type);
        }
        

    }
}
