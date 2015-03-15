using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMapper.Tests
{
    using System.Reflection;

    using NUnit.Framework;

    using SafeMapper.Tests.Model.GenericClasses;
    using SafeMapper.Tests.Model.Person;
    using SafeMapper.Utils;

    [TestFixture]
    public class ExpressionHelperTests
    {
        [Test]
        public void GetMember_ClassProperty()
        {
            var expected = new ClassProperty<string> { Value = "Test text" };
            var member = ExpressionHelper.GetMember<ClassProperty<string>, string>(x => x.Value);

            Assert.NotNull(member);
            Assert.IsInstanceOf<PropertyInfo>(member);
            Assert.AreEqual(expected.Value, (member as PropertyInfo).GetValue(expected));
        }

        [Test]
        public void GetMember_ClassMethod()
        {
            var val = new ClassMethod<string>();
            val.SetValue("Test text");
            var member = ExpressionHelper.GetMember<ClassMethod<string>, string>(x => x.GetValue());

            Assert.NotNull(member);
            Assert.IsInstanceOf<MethodInfo>(member);
            Assert.AreEqual("Test text", (member as MethodInfo).Invoke(val, new object[0]));
        }
    }
}
