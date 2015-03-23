namespace SafeMapper.Tests
{
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using NUnit.Framework;

    using SafeMapper.Configuration;
    using SafeMapper.Tests.Model.GenericClasses;

    [TestFixture]
    public class MapConfigurationTests
    {
        [SetUp]
        public void SetUp()
        {
            SafeMap.Configuration = new MapConfiguration();
        }

        [Test]
        public void TestMappingToDifferentMemberNames()
        {
            var typeMap = new ClassPropertyDictionaryMap();
            SafeMap.Configuration.SetTypeMapping(typeMap.GetTypeMapping());

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
            SafeMap.Configuration.SetTypeMapping(typeMap.GetTypeMapping());

            var input = new ClassMethod<int>();
            input.SetValue(1337);

            var result = SafeMap.Convert<ClassMethod<int>, ClassProperty<string>>(input);

            Assert.AreEqual("1337", result.Value);
        }

        [Test]
        public void Map_PropertyToMethod()
        {
            var typeMap = new ClassPropertyClassMethodMap();
            SafeMap.Configuration.SetTypeMapping(typeMap.GetTypeMapping());

            var input = new ClassProperty<string> { Value = "1337" };

            var result = SafeMap.Convert<ClassProperty<string>, ClassMethod<int>>(input);

            Assert.AreEqual(1337, result.GetValue());
        }

        [Test]
        public void Map_PropertyToNameValueCollection()
        {
            var typeMap = new ClassPropertyNameValueCollectionMap();
            SafeMap.Configuration.SetTypeMapping(typeMap.GetTypeMapping());

            var input = new ClassProperty<string> { Value = "1337" };

            var result = SafeMap.Convert<ClassProperty<string>, NameValueCollection>(input);

            Assert.AreEqual("1337", result["Value2"]);
        }

        [Test]
        public void Map_NameValueCollectionToClassProperty()
        {
            var typeMap = new NameValueCollectionClassPropertyMap();
            SafeMap.Configuration.SetTypeMapping(typeMap.GetTypeMapping());

            var input = new NameValueCollection { { "Value2", "1337" } };

            var result = SafeMap.Convert<NameValueCollection, ClassProperty<string>>(input);

            Assert.AreEqual("1337", result.Value);
        }

        [Test]
        public void Map_NameValueCollectionToClassMethod()
        {
            var typeMap = new NameValueCollectionClassMethodMap();
            SafeMap.Configuration.SetTypeMapping(typeMap.GetTypeMapping());

            var input = new NameValueCollection { { "Value2", "1337" } };

            var result = SafeMap.Convert<NameValueCollection, ClassMethod<string>>(input);

            Assert.AreEqual("1337", result.GetValue());
        }

        [Test]
        public void SetConvertMethod_IntToStringWithCustomConverter()
        {
            var method = this.GetType().GetMethod("ConvertIntToStringWithSuffix");
            SafeMap.Configuration.SetConvertMethod(typeof(int), typeof(string), method);

            var input = 1337;

            var result = SafeMap.Convert<int, string>(input);

            Assert.AreEqual("1337pcs", result);
        }

        public static string ConvertIntToStringWithSuffix(int value)
        {
            return string.Format("{0}pcs", value);
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
}