namespace SafeMapper.Tests
{
    using System;
    using System.Linq;
    using System.Reflection.Emit;

    using NUnit.Framework;

    using SafeMapper.Tests.Model.Enums;
    using SafeMapper.Tests.Model.Person;
    using SafeMapper.Utils;

    [TestFixture]
    public class EmitExtensionsTests
    {
        private ILGeneratorAdapter ilgenerator;

        [SetUp]
        public void Setup()
        {
            var convertDynamicMethod = new DynamicMethod(
                "TestDenamicMethod",
                typeof(object),
                null,
                typeof(EmitExtensionsTests).Module);

            this.ilgenerator = new ILGeneratorAdapter(convertDynamicMethod.GetILGenerator());
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

        [Test]
        public void EmitValueTypeBox_FromInt_ShouldLoadLocalAdress()
        {
            EmitExtensions.EmitValueTypeBox(this.ilgenerator, typeof(int));

            Assert.AreEqual(OpCodes.Ldloca, this.ilgenerator.Instructions.Last().OpCode);
        }

        [Test]
        public void EmitValueTypeBox_FromPerson_ShouldNotResultAnyInstructions()
        {
            EmitExtensions.EmitValueTypeBox(this.ilgenerator, typeof(Person));

            Assert.AreEqual(0, this.ilgenerator.Instructions.Length);
        }

        [Test]
        public void EmitValueTypeBox_FromEnum_ShouldBoxValue()
        {
            EmitExtensions.EmitValueTypeBox(this.ilgenerator, typeof(Int32Enum));

            Assert.AreEqual(OpCodes.Box, this.ilgenerator.Instructions.Last().OpCode);
        }
    }
}
