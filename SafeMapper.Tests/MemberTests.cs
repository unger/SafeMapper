namespace SafeMapper.Tests
{
    using System;
    using System.Collections.Specialized;

    using Xunit;

    using SafeMapper.Reflection;

    
    public class MemberTests
    {
        [Fact]
        public void Ctor_ConstructorInfo_ShouldBeMethodTypeUndefined()
        {
            var constructor = typeof(NameValueCollection).GetConstructor(Type.EmptyTypes);

            var member = new MemberGetter(constructor);

            Assert.Equal(MemberType.Undefined, member.MemberType);
        }
    }
}
