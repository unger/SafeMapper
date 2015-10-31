namespace SafeMapper.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;

    using NUnit.Framework;

    using SafeMapper.Configuration;
    using SafeMapper.Tests.Model.GenericClasses;
    using SafeMapper.Utils;

    [TestFixture]
    public class MapConfigurationTests
    {
        private SafeMapService safeMapService;

        [SetUp]
        public void SetUp()
        {
            this.safeMapService = new SafeMapService(new ConverterFactory());
        }

        [Test]
        public void TestMappingToDifferentMemberNames()
        {
            var typeMap = new ClassPropertyDictionaryMap();
            this.safeMapService.Configuration.SetTypeMapping(typeMap.GetTypeMapping());

            var value =
                this.safeMapService.Convert<ClassProperty<string>, Dictionary<string, int>>(
                    new ClassProperty<string> { Value = "1337" });

            Assert.True(value.ContainsKey("Value2"));
            Assert.AreEqual(1337, value["Value2"]);
        }

        [Test]
        public void Map_MethodToProperty()
        {
            var typeMap = new ClassMethodClassPropertyMap();
            this.safeMapService.Configuration.SetTypeMapping(typeMap.GetTypeMapping());

            var input = new ClassMethod<int>();
            input.SetValue(1337);

            var result = this.safeMapService.Convert<ClassMethod<int>, ClassProperty<string>>(input);

            Assert.AreEqual("1337", result.Value);
        }

        [Test]
        public void Map_PropertyToMethod()
        {
            var typeMap = new ClassPropertyClassMethodMap();
            this.safeMapService.Configuration.SetTypeMapping(typeMap.GetTypeMapping());

            var input = new ClassProperty<string> { Value = "1337" };

            var result = this.safeMapService.Convert<ClassProperty<string>, ClassMethod<int>>(input);

            Assert.AreEqual(1337, result.GetValue());
        }

        [Test]
        public void Map_PropertyToNameValueCollection()
        {
            var typeMap = new ClassPropertyNameValueCollectionMap();
            this.safeMapService.Configuration.SetTypeMapping(typeMap.GetTypeMapping());

            var input = new ClassProperty<string> { Value = "1337" };

            var result = this.safeMapService.Convert<ClassProperty<string>, NameValueCollection>(input);

            Assert.AreEqual("1337", result["Value2"]);
        }

        [Test]
        public void Map_NameValueCollectionToClassProperty()
        {
            var typeMap = new NameValueCollectionClassPropertyMap();
            this.safeMapService.Configuration.SetTypeMapping(typeMap.GetTypeMapping());

            var input = new NameValueCollection { { "Value2", "1337" } };

            var result = this.safeMapService.Convert<NameValueCollection, ClassProperty<string>>(input);

            Assert.AreEqual("1337", result.Value);
        }

        [Test]
        public void Map_NameValueCollectionToClassMethod()
        {
            var typeMap = new NameValueCollectionClassMethodMap();
            this.safeMapService.Configuration.SetTypeMapping(typeMap.GetTypeMapping());

            var input = new NameValueCollection { { "Value2", "1337" } };

            var result = this.safeMapService.Convert<NameValueCollection, ClassMethod<string>>(input);

            Assert.AreEqual("1337", result.GetValue());
        }

        [Test]
        public void SetConvertMethod_IntToStringWithCustomConverter()
        {
            this.safeMapService.Configuration.SetConvertMethod<int, string>(x => string.Format("{0}pcs", x));

            var input = 1337;

            var result = this.safeMapService.Convert<int, string>(input);

            Assert.AreEqual("1337pcs", result);
        }

        [Test]
        public void SetConvertMethod_DecimalToStringWithLamdaUsingLocalVariabel()
        {
            var decimals = 2;

            this.safeMapService.Configuration.SetConvertMethod<decimal, string>(
                x => Math.Round(x, decimals).ToString(CultureInfo.InvariantCulture));

            var result = this.safeMapService.Convert<decimal, string>(1337.1337m);

            Assert.AreEqual("1337.13", result);
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