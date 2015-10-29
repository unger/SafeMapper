namespace SafeMapper.Tests
{
    using System.Collections.Specialized;
    using System.Reflection;

    using Xunit;

    using SafeMapper.Tests.Model.GenericClasses;
    using SafeMapper.Utils;

    
    public class ExpressionHelperTests
    {
        [Fact]
        public void GetMember_ClassProperty()
        {
            var expected = new ClassProperty<string> { Value = "Test text" };
            var member = ExpressionHelper.GetMember<ClassProperty<string>, string>(x => x.Value);

            Assert.NotNull(member);
            Assert.IsAssignableFrom<PropertyInfo>(member);
            Assert.Equal(expected.Value, (member as PropertyInfo).GetValue(expected));
        }

        [Fact]
        public void GetMember_ClassMethod()
        {
            var val = new ClassMethod<string>();
            val.SetValue("Test text");
            var member = ExpressionHelper.GetMember<ClassMethod<string>, string>(x => x.GetValue());

            Assert.NotNull(member);
            Assert.IsAssignableFrom<MethodInfo>(member);
            Assert.Equal("Test text", (member as MethodInfo).Invoke(val, new object[0]));
        }

        [Fact]
        public void GetMember_NameValueCollectionGetter()
        {
            var val = new NameValueCollection { { "Key", "Value" } };
            var member = ExpressionHelper.GetMember<NameValueCollection, string>((x, key) => x.GetValues(key));

            Assert.NotNull(member);
            Assert.IsAssignableFrom<MethodInfo>(member);
            Assert.Equal(new[] { "Value" }, (member as MethodInfo).Invoke(val, new object[] { "Key" }));
        }

        [Fact]
        public void GetMember_NameValueCollectionSetter()
        {
            var val = new NameValueCollection();
            var member = ExpressionHelper.GetMember<NameValueCollection, string>((x, key, v) => x.Add(key, v));

            Assert.NotNull(member);
            Assert.IsAssignableFrom<MethodInfo>(member);

            (member as MethodInfo).Invoke(val, new object[] { "Key", "Value" });

            Assert.Equal("Value", val["Key"]);
        }

        [Fact]
        public void GetMember_MethodBinaryExpression_ShouldReturnNull()
        {
            var member = ExpressionHelper.GetMember<ClassProperty<string>, string>(x => x.Value + "test");

            Assert.Null(member);
        }
    }
}
