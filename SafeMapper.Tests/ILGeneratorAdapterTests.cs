using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMapper.Tests
{
    using System.Reflection.Emit;

    using NUnit.Framework;

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
    }
}
