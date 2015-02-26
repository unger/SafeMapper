namespace SafeMapper.Tests
{
    using System;
    using System.Reflection.Emit;

    using NUnit.Framework;

    using SafeMapper.Tests.Model.Enums;
    using SafeMapper.Utils;

    [TestFixture]
    public class EmitExtensionsTests
    {
        private ILGenerator ilgenerator;

        [SetUp]
        public void Setup()
        {
            var convertDynamicMethod = new DynamicMethod(
                "TestDenamicMethod",
                typeof(object),
                null,
                typeof(EmitExtensionsTests).Module);

            this.ilgenerator = convertDynamicMethod.GetILGenerator();
            
        }

        [Test]
        public void EmitConvertFromEnum_FromNonEnum_ShouldThrowException()
        {
            Assert.Throws(
                typeof(ArgumentException),
                () => EmitExtensions.EmitConvertFromEnum(this.ilgenerator, typeof(string), typeof(int)));
        }

        [Test]
        public void EmitConvertFromEnum_FromEnum_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(
                () => EmitExtensions.EmitConvertFromEnum(this.ilgenerator, typeof(Int32Enum), typeof(int)));
        }

        [Test]
        public void EmitConvertToEnum_ToNonEnum_ShouldThrowException()
        {
            Assert.Throws(
                typeof(ArgumentException),
                () => EmitExtensions.EmitConvertToEnum(this.ilgenerator, typeof(string), typeof(int)));
        }

        [Test]
        public void EmitConvertToEnum_ToEnum_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(
                () => EmitExtensions.EmitConvertToEnum(this.ilgenerator, typeof(string), typeof(Int32Enum)));
        }


    }
}
