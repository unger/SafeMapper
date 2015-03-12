using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMapper.Tests
{
    using NUnit.Framework;

    using SafeMapper.Configuration;
    using SafeMapper.Tests.Model.GenericClasses;
    using SafeMapper.Tests.Model.Person;

    [TestFixture]
    public class TypeMappingTests
    {
        [Test]
        public void TestMappingToDifferentMemberNames()
        {
            var typeMap = new ClassPropertyDictionaryMap();
            TypeMapping.SetTypeMapping(typeMap.GetTypeMapping());

            var value =
                SafeMap.Convert<ClassProperty<string>, Dictionary<string, int>>(
                    new ClassProperty<string> { Value = "1337" });

            Assert.True(value.ContainsKey("Value2"));
            Assert.AreEqual(1337, value["Value2"]);
        }
    }

    public class ClassPropertyDictionaryMap : TypeMap<ClassProperty<string>, Dictionary<string, int>>
    {
        public ClassPropertyDictionaryMap()
        {
            this.Map("Value", "Value2");
        }
    }
}
