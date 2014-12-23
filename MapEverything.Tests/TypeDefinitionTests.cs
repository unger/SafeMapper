namespace MapEverything.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class TypeDefinitionTests
    {
        [Test]
        public void CreateObject_string_ShouldReturnNull()
        {
            var td = new TypeDefinition<string>();

            var obj = td.CreateObject();

            Assert.AreEqual(null, obj);
        }

        [Test]
        public void CreateObject_int_ShouldCreateDefaultForInt()
        {
            var td = new TypeDefinition<int>();

            var obj = td.CreateObject();

            Assert.IsInstanceOf<int>(obj);
            Assert.AreEqual(default(int), obj);
        }

        [Test]
        public void CreateObject_ListOfInt_ShouldReturnEmptyListOfInt()
        {
            var td = new TypeDefinition<List<int>>();

            var obj = td.CreateObject() as List<int>;

            Assert.IsInstanceOf<List<int>>(obj);
            Assert.AreEqual(0, obj.Count);
        }

        [Test]
        public void CreateObject_ArrayOfInt_ShouldReturnNull()
        {
            var td = new TypeDefinition<int[]>();

            var obj = td.CreateObject();

            Assert.AreEqual(null, obj);
        }

        [Test]
        public void CreateObject_IEnumerableOfInt_ShouldReturnEmptyIEnumerableOfInt()
        {
            var td = new TypeDefinition<IEnumerable<int>>();

            var obj = td.CreateObject() as IEnumerable<int>;

            Assert.IsInstanceOf<IEnumerable<int>>(obj);
            Assert.AreEqual(0, obj.Count());
        }

        [Test]
        public void CreateObject_IListOfInt_ShouldReturnEmptyIListOfInt()
        {
            var td = new TypeDefinition<IList<int>>();

            var obj = td.CreateObject() as IList<int>;

            Assert.IsInstanceOf<IList<int>>(obj);
            Assert.AreEqual(0, obj.Count());
        }

        [Test]
        public void CreateObject_HashSetOfString_ShouldReturnEmptyHashSetOfString()
        {
            var td = new TypeDefinition<HashSet<string>>();

            var obj = td.CreateObject() as HashSet<string>;

            Assert.IsInstanceOf<HashSet<string>>(obj);
            Assert.AreEqual(0, obj.Count);
        }

        [Test]
        public void CreateObject_ISetOfString_ShouldReturnEmptyISetOfString()
        {
            var td = new TypeDefinition<ISet<string>>();

            var obj = td.CreateObject() as ISet<string>;

            Assert.IsInstanceOf<ISet<string>>(obj);
            Assert.AreEqual(0, obj.Count);
        }
    }
}
