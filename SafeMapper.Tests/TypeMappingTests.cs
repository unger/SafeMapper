namespace SafeMapper.Tests
{
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using NUnit.Framework;

    using SafeMapper.Configuration;
    using SafeMapper.Tests.Model.GenericClasses;

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

        [Test]
        public void Map_PropertyToNameValueCollection()
        {
            var typeMap = new ClassPropertyNameValueCollectionMap();
            TypeMapping.SetTypeMapping(typeMap.GetTypeMapping());

            var input = new ClassProperty<string> { Value = "1337" };

            var result = SafeMap.Convert<ClassProperty<string>, NameValueCollection>(input);

            Assert.AreEqual("1337", result["Value2"]);
        }

        [Test]
        public void Map_NameValueCollectionToClassProperty()
        {
            var typeMap = new NameValueCollectionClassPropertyMap();
            TypeMapping.SetTypeMapping(typeMap.GetTypeMapping());

            var input = new NameValueCollection { { "Value2", "1337" } };

            var result = SafeMap.Convert<NameValueCollection, ClassProperty<string>>(input);

            Assert.AreEqual("1337", result.Value);
        }

        [Test]
        public void Map_NameValueCollectionToClassMethod()
        {
            var typeMap = new NameValueCollectionClassMethodMap();
            TypeMapping.SetTypeMapping(typeMap.GetTypeMapping());

            var input = new NameValueCollection { { "Value2", "1337" } };

            var result = SafeMap.Convert<NameValueCollection, ClassMethod<string>>(input);

            Assert.AreEqual("1337", result.GetValue());
        }
    }

    

    public class ClassPropertyDictionaryMap : TypeMap<ClassProperty<string>, Dictionary<string, int>>
    {
        public ClassPropertyDictionaryMap()
        {
            this.Map("Value", "Value2");
        }
    }

    public class ClassPropertyNameValueCollectionMap : TypeMap<ClassProperty<string>, NameValueCollection>
    {
        public ClassPropertyNameValueCollectionMap()
        {
            this.Map<string, string>(x => x.Value, "Value2");
        }
    }

    public class NameValueCollectionClassPropertyMap : TypeMap<NameValueCollection, ClassProperty<string>>
    {
        public NameValueCollectionClassPropertyMap()
        {
            this.Map<string, string>("Value2", x => x.Value);
        }
    }

    public class NameValueCollectionClassMethodMap : TypeMap<NameValueCollection, ClassMethod<string>>
    {
        public NameValueCollectionClassMethodMap()
        {
            this.Map<string, string>("Value2", (x, v) => x.SetValue(v));
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
