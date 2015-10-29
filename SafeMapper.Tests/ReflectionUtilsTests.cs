namespace SafeMapper.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using Xunit;

    using SafeMapper.Reflection;
    using SafeMapper.Tests.Model.Benchmark;
    using SafeMapper.Tests.Model.Circular;
    using SafeMapper.Tests.Model.GenericClasses;
    using SafeMapper.Tests.Model.Person;

    
    public class ReflectionUtilsTests
    {
        [Fact]
        public void GetMemberSetter_StringPropertyClassExistingMember_ShouldReturnMember()
        {
            var member = ReflectionUtils.GetMemberSetter(typeof(ClassProperty<string>), "Value");

            Assert.NotNull(member);
        }

        [Fact]
        public void GetMemberSetter_StringFieldClassExistingMember_ShouldReturnMember()
        {
            var member = ReflectionUtils.GetMemberSetter(typeof(ClassField<string>), "Value");

            Assert.NotNull(member);
        }

        [Fact]
        public void GetMemberSetter_StringPropertyClassNonExistingMember_ShouldReturnNull()
        {
            var member = ReflectionUtils.GetMemberSetter(typeof(ClassProperty<string>), "NotExisting");

            Assert.Null(member);
        }

        [Fact]
        public void GetMemberGetter_StringFieldClassExistingMember_ShouldReturnMember()
        {
            var member = ReflectionUtils.GetMemberGetter(typeof(ClassField<string>), "Value");

            Assert.NotNull(member);
        }

        [Fact]
        public void GetMemberGetter_StringFieldClassNonExistingMember_ShouldReturnNull()
        {
            var member = ReflectionUtils.GetMemberGetter(typeof(ClassField<string>), "NotExisting");

            Assert.Null(member);
        }

        [Fact]
        public void GetMember_StringObjectDictionary_ShouldReturnStringIndexer()
        {
            var member = ReflectionUtils.GetMemberGetter(typeof(Dictionary<string, string>), "MemberName");

            Assert.NotNull(member);
        }

        [Fact]
        public void GetMemberGetter_IntObjectDictionary_ShouldReturnNull()
        {
            var member = ReflectionUtils.GetMemberGetter(typeof(Dictionary<int, string>), "MemberName");

            Assert.Null(member);
        }

        [Fact]
        public void GetMemberGetter_NameValueCollection_ShouldReturnStringIndexer()
        {
            var member = ReflectionUtils.GetMemberGetter(typeof(NameValueCollection), "MemberName");

            Assert.NotNull(member);
        }

        [Fact]
        public void GetMemberGetter_MethodInfo_ShouldReturnNull()
        {
            var member = ReflectionUtils.GetMemberGetter(typeof(int), "ToString");

            Assert.Null(member);
        }

        [Theory]
        [InlineData(typeof(NameValueCollection), true)]
        public void IsDictionary(Type type, bool expected)
        {
            Assert.Equal(expected, ReflectionUtils.IsDictionary(type));
        }

        [Fact]
        public void GetElementType_ArrayOfInt_ShouldReturnInt()
        {
            var type = ReflectionUtils.GetElementType(typeof(int[]));

            Assert.Equal(typeof(int), type);
        }

        [Fact]
        public void GetElementType_ListOfInt_ShouldReturnInt()
        {
            var type = ReflectionUtils.GetElementType(typeof(List<int>));

            Assert.Equal(typeof(int), type);
        }

        [Fact]
        public void GetElementType_Int_ShouldReturnNull()
        {
            var type = ReflectionUtils.GetElementType(typeof(int));

            Assert.Null(type);
        }

        [Fact]
        public void GetConcreteType_IListOfInt_ShouldReturnListOfInt()
        {
            var type = ReflectionUtils.GetConcreteType(typeof(IList<int>));

            Assert.Equal(typeof(List<int>), type);
        }

        [Fact]
        public void GetConcreteType_IEnumerable_ShouldReturnNull()
        {
            var type = ReflectionUtils.GetConcreteType(typeof(IEnumerable));

            Assert.Null(type);
        }

        [Fact]
        public void GetConcreteType_Int_ShouldReturnInt()
        {
            var type = ReflectionUtils.GetConcreteType(typeof(int));

            Assert.Equal(typeof(int), type);
        }

        [Fact]
        public void GetConcreteTypeDefinition_IList_ShouldReturnList()
        {
            var type = ReflectionUtils.GetConcreteTypeDefinition(typeof(IList<>));

            Assert.Equal(typeof(List<>), type);
        }

        [Fact]
        public void GetConcreteTypeDefinition_List_ShouldReturnList()
        {
            var type = ReflectionUtils.GetConcreteTypeDefinition(typeof(List<>));

            Assert.Equal(typeof(List<>), type);
        }

        [Fact]
        public void GetConcreteTypeDefinition_IEnumerable_ShouldReturnNull()
        {
            var type = ReflectionUtils.GetConcreteTypeDefinition(typeof(IEnumerable));

            Assert.Null(type);
        }

        [Fact]
        public void GetConvertMethod_IEnumerable_ShouldReturnNull()
        {
            var method = ReflectionUtils.GetConvertMethod(typeof(string), typeof(string), new[] { typeof(SafeConvert) });

            Assert.Null(method);
        }

        [Fact]
        public void GetMemberType_ConstructorInfo_ShouldReturnVoid()
        {
            var constructor = typeof(int).GetConstructor(Type.EmptyTypes);
            var type = ReflectionUtils.GetMemberType(constructor);

            Assert.Equal(typeof(void), type);
        }

        [Fact]
        public void CanHaveCircularReference_Person_ShouldReturnFalse()
        {
            var result = ReflectionUtils.CanHaveCircularReference(typeof(Person));

            Assert.False(result);
        }

        [Fact]
        public void CanHaveCircularReference_Parent_ShouldReturnTrue()
        {
            var result = ReflectionUtils.CanHaveCircularReference(typeof(Parent));

            Assert.True(result);
        }

        [Fact]
        public void CanHaveCircularReference_String_ShouldReturnFalse()
        {
            var result = ReflectionUtils.CanHaveCircularReference(typeof(string));

            Assert.False(result);
        }

        [Fact]
        public void CanHaveCircularReference_DateTime_ShouldReturnFalse()
        {
            var result = ReflectionUtils.CanHaveCircularReference(typeof(DateTime));

            Assert.False(result);
        }

        [Fact]
        public void CanHaveCircularReference_ClassFieldPerson_ShouldReturnFalse()
        {
            var result = ReflectionUtils.CanHaveCircularReference(typeof(ClassField<Person>));

            Assert.False(result);
        }

        [Fact]
        public void CanHaveCircularReference_BenchSource_ShouldReturnFalse()
        {
            var result = ReflectionUtils.CanHaveCircularReference(typeof(BenchSource));

            Assert.False(result);
        }

        [Fact]
        public void CanHaveCircularReference_Root_ShouldReturnTrue()
        {
            var result = ReflectionUtils.CanHaveCircularReference(typeof(Root));

            Assert.True(result);
        }

        [Fact]
        public void GetTypeWithGenericTypeDefinition_GenericTypeDefinition_ShouldReturnCorrectType()
        {
            var result = ReflectionUtils.GetTypeWithGenericTypeDefinition(typeof(List<string>), typeof(IEnumerable<>));

            Assert.Equal(typeof(IEnumerable<string>), result);
        }

        [Fact]
        public void GetTypeWithGenericTypeDefinition_NotGenericTypeDefinition_ShouldReturnNull()
        {
            var result = ReflectionUtils.GetTypeWithGenericTypeDefinition(typeof(List<string>), typeof(IEnumerable<string>));

            Assert.Null(result);
        }


        [Fact]
        public void GetMemberMaps_NameValueCollectionAndDictionary_ShouldReturnEmptyList()
        {
            var result = ReflectionUtils.GetMemberMaps(typeof(NameValueCollection), typeof(Dictionary<string, int>));

            Assert.Empty(result);
        }
        
    }
}
