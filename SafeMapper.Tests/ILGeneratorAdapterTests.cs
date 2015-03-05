using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMapper.Tests
{
    using System.Reflection.Emit;

    using NUnit.Framework;

    using SafeMapper.Reflection;
    using SafeMapper.Tests.Model.GenericClasses;
    using SafeMapper.Utils;

    [TestFixture]
    public class ILGeneratorAdapterTests
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
        public void EmitDouble_ShouldResult_Ldc_R8_opcode()
        {
            this.ilgenerator.EmitDouble(1.0d);

            Assert.AreEqual(OpCodes.Ldc_R8, this.ilgenerator.Instructions[0].OpCode);
            Assert.AreEqual(9, this.ilgenerator.Offset);
        }

        [Test]
        public void EmitFloat_ShouldResult_Ldc_R4_opcode()
        {
            this.ilgenerator.EmitFloat(1.0f);

            Assert.AreEqual(OpCodes.Ldc_R4, this.ilgenerator.Instructions[0].OpCode);
            Assert.AreEqual(5, this.ilgenerator.Offset);
        }

        [Test]
        public void EmitField_Ldfld_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => this.ilgenerator.EmitField(OpCodes.Ldfld, typeof(ClassField<string>).GetField("Value")));
        }

        [Test]
        public void EmitField_Stfld_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => this.ilgenerator.EmitField(OpCodes.Stfld, typeof(ClassField<string>).GetField("Value")));
        }

        [Test]
        public void EmitField_Ldloc_ShouldThrowException()
        {
            Assert.Throws<Exception>(() => this.ilgenerator.EmitField(OpCodes.Ldloc, typeof(ClassField<string>).GetField("Value")));
        }

        [Test]
        public void EmitLocal_Ldloc_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => this.ilgenerator.EmitLocal(OpCodes.Ldloc, this.ilgenerator.DeclareLocal(typeof(string))));
        }

        [Test]
        public void EmitLocal_Ldfld_ShouldThrowException()
        {
            Assert.Throws<Exception>(() => this.ilgenerator.EmitLocal(OpCodes.Ldfld, this.ilgenerator.DeclareLocal(typeof(string))));
        }

        [Test]
        public void EmitBreak_Br_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => this.ilgenerator.EmitBreak(OpCodes.Br, new Label()));
        }

        [Test]
        public void EmitBreak_Break_ShouldThrowException()
        {
            Assert.Throws<Exception>(() => this.ilgenerator.EmitBreak(OpCodes.Break, this.ilgenerator.DefineLabel()));
        }

        [Test]
        public void EmitBreak_Box_ShouldThrowException()
        {
            Assert.Throws<Exception>(() => this.ilgenerator.EmitBreak(OpCodes.Box, this.ilgenerator.DefineLabel()));
        }

        [Test]
        public void EmitBreak_Ldfld_ShouldThrowException()
        {
            Assert.Throws<Exception>(() => this.ilgenerator.EmitBreak(OpCodes.Ldfld, this.ilgenerator.DefineLabel()));
        }
    }
}
