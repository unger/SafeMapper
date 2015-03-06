namespace SafeMapper.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using NUnit.Framework;

    using SafeMapper.Reflection;
    using SafeMapper.Tests.Model.Benchmark;
    using SafeMapper.Tests.Model.Circular;
    using SafeMapper.Tests.Model.GenericClasses;
    using SafeMapper.Tests.Model.Person;

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
        public void GetMember_MethodInfo_ShouldReturnNull()
        {
            var member = ReflectionUtils.GetMember(typeof(int), "ToString");

            Assert.Null(member);
        }

        [TestCase(typeof(NameValueCollection), Result = true)]
        public bool IsDictionary(Type type)
        {
            return ReflectionUtils.IsDictionary(type);
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
        public void GetConcreteType_IEnumerable_ShouldReturnNull()
        {
            var type = ReflectionUtils.GetConcreteType(typeof(IEnumerable));

            Assert.Null(type);
        }

        [Test]
        public void GetConcreteType_Int_ShouldReturnInt()
        {
            var type = ReflectionUtils.GetConcreteType(typeof(int));

            Assert.AreEqual(typeof(int), type);
        }

        [Test]
        public void GetConcreteTypeDefinition_IList_ShouldReturnList()
        {
            var type = ReflectionUtils.GetConcreteTypeDefinition(typeof(IList<>));

            Assert.AreEqual(typeof(List<>), type);
        }

        [Test]
        public void GetConcreteTypeDefinition_List_ShouldReturnList()
        {
            var type = ReflectionUtils.GetConcreteTypeDefinition(typeof(List<>));

            Assert.AreEqual(typeof(List<>), type);
        }

        [Test]
        public void GetConcreteTypeDefinition_IEnumerable_ShouldReturnNull()
        {
            var type = ReflectionUtils.GetConcreteTypeDefinition(typeof(IEnumerable));

            Assert.Null(type);
        }

        [Test]
        public void GetConvertMethod_IEnumerable_ShouldReturnNull()
        {
            var method = ReflectionUtils.GetConvertMethod(typeof(string), typeof(string), new[] { typeof(SafeConvert) });

            Assert.Null(method);
        }

        [Test]
        public void GetMemberType_ConstructorInfo_ShouldReturnVoid()
        {
            var constructor = typeof(int).GetConstructor(Type.EmptyTypes);
            var type = ReflectionUtils.GetMemberType(constructor);

            Assert.AreEqual(typeof(void), type);
        }

        [Test]
        public void CanHaveCircularReference_Person_ShouldReturnFalse()
        {
            var result = ReflectionUtils.CanHaveCircularReference(typeof(Person));

            Assert.False(result);
        }

        [Test]
        public void CanHaveCircularReference_Parent_ShouldReturnTrue()
        {
            var result = ReflectionUtils.CanHaveCircularReference(typeof(Parent));

            Assert.True(result);
        }

        [Test]
        public void CanHaveCircularReference_String_ShouldReturnFalse()
        {
            var result = ReflectionUtils.CanHaveCircularReference(typeof(string));

            Assert.False(result);
        }

        [Test]
        public void CanHaveCircularReference_DateTime_ShouldReturnFalse()
        {
            var result = ReflectionUtils.CanHaveCircularReference(typeof(DateTime));

            Assert.False(result);
        }

        [Test]
        public void CanHaveCircularReference_ClassFieldPerson_ShouldReturnFalse()
        {
            var result = ReflectionUtils.CanHaveCircularReference(typeof(ClassField<Person>));

            Assert.False(result);
        }

        [Test]
        public void CanHaveCircularReference_BenchSource_ShouldReturnFalse()
        {
            var result = ReflectionUtils.CanHaveCircularReference(typeof(BenchSource));

            Assert.False(result);
        }

        [Test]
        public void CanHaveCircularReference_Root_ShouldReturnTrue()
        {
            var result = ReflectionUtils.CanHaveCircularReference(typeof(Root));

            Assert.True(result);
        }
    }
}
