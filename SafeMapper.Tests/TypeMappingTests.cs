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
    public class Map_PropertyToDifferentDictionaryKey
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

        [Test]
        public void Map_MethodToProperty()
        {
            var typeMap = new ClassMethodClassPropertyMap();
            TypeMapping.SetTypeMapping(typeMap.GetTypeMapping());

            var input = new ClassMethod<int>();
            input.SetValue(1337);

            var result = SafeMap.Convert<ClassMethod<int>, ClassProperty<string>>(input);

            Assert.AreEqual("1337", result.Value);
        }

        [Test]
        public void Map_PropertyToMethod()
        {
            var typeMap = new ClassPropertyClassMethodMap();
            TypeMapping.SetTypeMapping(typeMap.GetTypeMapping());

            var input = new ClassProperty<string> { Value = "1337" };

            var result = SafeMap.Convert<ClassProperty<string>, ClassMethod<int>>(input);

            Assert.AreEqual(1337, result.GetValue());
        }

    }

    public class ClassPropertyDictionaryMap : TypeMap<ClassProperty<string>, Dictionary<string, int>>
    {
        public ClassPropertyDictionaryMap()
        {
            this.Map("Value", "Value2");
        }
    }

    public class ClassMethodClassPropertyMap : TypeMap<ClassMethod<int>, ClassProperty<string>>
    {
        public ClassMethodClassPropertyMap()
        {
            this.Map(x => x.GetValue(), x => x.Value);
        }
    }


    public class ClassPropertyClassMethodMap : TypeMap<ClassProperty<string>, ClassMethod<int>>
    {
        public ClassPropertyClassMethodMap()
        {
            this.Map<string, int>(x => x.Value, (x, y) => x.SetValue(y));
        }
    }
}
