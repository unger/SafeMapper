using System.Reflection;

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
        public void GetMemberSetter_StringPropertyClassExistingMember_ShouldReturnMember()
        {
            var member = ReflectionUtils.GetMemberSetter(typeof(ClassProperty<string>), "Value");

            Assert.NotNull(member);
        }

        [Test]
        public void GetMemberSetter_StringFieldClassExistingMember_ShouldReturnMember()
        {
            var member = ReflectionUtils.GetMemberSetter(typeof(ClassField<string>), "Value");

            Assert.NotNull(member);
        }

        [Test]
        public void GetMemberSetter_StringPropertyClassNonExistingMember_ShouldReturnNull()
        {
            var member = ReflectionUtils.GetMemberSetter(typeof(ClassProperty<string>), "NotExisting");

            Assert.Null(member);
        }

        [Test]
        public void GetMemberGetter_StringFieldClassExistingMember_ShouldReturnMember()
        {
            var member = ReflectionUtils.GetMemberGetter(typeof(ClassField<string>), "Value");

            Assert.NotNull(member);
        }

        [Test]
        public void GetMemberGetter_StringFieldClassNonExistingMember_ShouldReturnNull()
        {
            var member = ReflectionUtils.GetMemberGetter(typeof(ClassField<string>), "NotExisting");

            Assert.Null(member);
        }

        [Test]
        public void GetMember_StringObjectDictionary_ShouldReturnStringIndexer()
        {
            var member = ReflectionUtils.GetMemberGetter(typeof(Dictionary<string, string>), "MemberName");

            Assert.NotNull(member);
        }

        [Test]
        public void GetMemberGetter_IntObjectDictionary_ShouldReturnNull()
        {
            var member = ReflectionUtils.GetMemberGetter(typeof(Dictionary<int, string>), "MemberName");

            Assert.Null(member);
        }

        [Test]
        public void GetMemberGetter_NameValueCollection_ShouldReturnStringIndexer()
        {
            var member = ReflectionUtils.GetMemberGetter(typeof(NameValueCollection), "MemberName");

            Assert.NotNull(member);
        }

        [Test]
        public void GetMemberGetter_MethodInfo_ShouldReturnNull()
        {
            var member = ReflectionUtils.GetMemberGetter(typeof(int), "ToString");

            Assert.Null(member);
        }

        [TestCase(typeof(NameValueCollection), ExpectedResult = true)]
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

        [Test]
        public void GetTypeWithGenericTypeDefinition_GenericTypeDefinition_ShouldReturnCorrectType()
        {
             var result = ReflectionUtils.GetTypeWithGenericTypeDefinition(typeof(List<string>), typeof(IEnumerable<>));

            Assert.AreEqual(typeof(IEnumerable<string>), result);
        }

        [Test]
        public void GetTypeWithGenericTypeDefinition_NotGenericTypeDefinition_ShouldReturnNull()
        {
             var result = ReflectionUtils.GetTypeWithGenericTypeDefinition(typeof(List<string>), typeof(IEnumerable<string>));

            Assert.IsNull(result);
        }


        [Test]
        public void GetMemberMaps_NameValueCollectionAndDictionary_ShouldReturnEmptyList()
        {
             var result = ReflectionUtils.GetMemberMaps(typeof(NameValueCollection), typeof(Dictionary<string, int>));

            Assert.IsEmpty(result);
        }

        [Test]
        public void GetStaticMemberInfo_OnClassWithSingletonField_ShouldReturnFieldInfo()
        {
             var result = ReflectionUtils.GetStaticMemberInfo(typeof(SingletonFieldClass));

            Assert.IsInstanceOf<FieldInfo>(result);
            Assert.AreEqual(typeof(SingletonFieldClass), (result as FieldInfo).FieldType);
        }

        [Test]
        public void GetStaticMemberInfo_OnClassWithSingletonProperty_ShouldReturnPropertyInfo()
        {
             var result = ReflectionUtils.GetStaticMemberInfo(typeof(SingletonPropertyClass));

            Assert.IsInstanceOf<PropertyInfo>(result);
            Assert.AreEqual(typeof(SingletonPropertyClass), (result as PropertyInfo).PropertyType);
        }

        public class SingletonFieldClass
        {
            public static string StaticString = "Test";

            public static SingletonFieldClass Instance = new SingletonFieldClass();
        }

        public class SingletonPropertyClass
        {
            private static readonly SingletonPropertyClass _instance = new SingletonPropertyClass();

            public static string StaticString
            {
                get { return "Test"; }
            }

            public static SingletonPropertyClass Instance
            {
                get { return _instance; }
            }
        }
    }
}
