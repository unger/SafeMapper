namespace SafeMapper.Tests
{
    using System;
    using System.Linq;
    using System.Reflection.Emit;

    using Xunit;

    using SafeMapper.Configuration;
    using SafeMapper.Tests.Model.Enums;
    using SafeMapper.Tests.Model.GenericClasses;
    using SafeMapper.Tests.Model.Person;
    using SafeMapper.Utils;

    
    public class ILGeneratorAdapterTests
    {
        private ILGeneratorAdapter ilgenerator;

        public ILGeneratorAdapterTests()
        {
            var convertDynamicMethod = new DynamicMethod(
                "TestDenamicMethod",
                typeof(object),
                null,
                typeof(ILGeneratorAdapterTests).Module);

            this.ilgenerator = new ILGeneratorAdapter(convertDynamicMethod.GetILGenerator(), new MapConfiguration());
        }

        [Fact]
        public void EmitDouble_ShouldResult_Ldc_R8_Opcode()
        {
            this.ilgenerator.EmitDouble(1.0d);

            Assert.Equal(OpCodes.Ldc_R8, this.ilgenerator.Instructions[0].OpCode);
            Assert.Equal(9, this.ilgenerator.Offset);
        }

        [Fact]
        public void EmitFloat_ShouldResult_Ldc_R4_Opcode()
        {
            this.ilgenerator.EmitFloat(1.0f);

            Assert.Equal(OpCodes.Ldc_R4, this.ilgenerator.Instructions[0].OpCode);
            Assert.Equal(5, this.ilgenerator.Offset);
        }

        [Fact]
        public void EmitField_Ldfld_ShouldNotThrowException()
        {
            this.ilgenerator.EmitField(OpCodes.Ldfld, typeof(ClassField<string>).GetField("Value"));
        }

        [Fact]
        public void EmitField_Stfld_ShouldNotThrowException()
        {
            this.ilgenerator.EmitField(OpCodes.Stfld, typeof(ClassField<string>).GetField("Value"));
        }

        [Fact]
        public void EmitField_Ldloc_ShouldThrowException()
        {
            Assert.Throws<Exception>(() => this.ilgenerator.EmitField(OpCodes.Ldloc, typeof(ClassField<string>).GetField("Value")));
        }

        [Fact]
        public void EmitLocal_Ldloc_ShouldNotThrowException()
        {
            this.ilgenerator.EmitLocal(OpCodes.Ldloc, this.ilgenerator.DeclareLocal(typeof(string)));
        }

        [Fact]
        public void EmitLocal_Ldfld_ShouldThrowException()
        {
            Assert.Throws<Exception>(() => this.ilgenerator.EmitLocal(OpCodes.Ldfld, this.ilgenerator.DeclareLocal(typeof(string))));
        }

        [Fact]
        public void EmitBreak_Br_ShouldNotThrowException()
        {
			this.ilgenerator.EmitBreak(OpCodes.Br, this.ilgenerator.DefineLabel());
        }

        [Fact]
        public void EmitBreak_Break_ShouldThrowException()
        {
            Assert.Throws<Exception>(() => this.ilgenerator.EmitBreak(OpCodes.Break, this.ilgenerator.DefineLabel()));
        }

        [Fact]
        public void EmitBreak_Box_ShouldThrowException()
        {
            Assert.Throws<Exception>(() => this.ilgenerator.EmitBreak(OpCodes.Box, this.ilgenerator.DefineLabel()));
        }

        [Fact]
        public void EmitBreak_Ldfld_ShouldThrowException()
        {
            Assert.Throws<Exception>(() => this.ilgenerator.EmitBreak(OpCodes.Ldfld, this.ilgenerator.DefineLabel()));
        }

        [Fact]
        public void EmitConvertFromEnum_FromNonEnum_ShouldThrowException()
        {
            Assert.Throws(
                typeof(ArgumentException),
                () => this.ilgenerator.EmitConvertFromEnum(typeof(string), typeof(int)));
        }

        [Fact]
        public void EmitConvertFromEnum_FromEnum_ShouldNotThrowException()
        {
            this.ilgenerator.EmitConvertFromEnum(typeof(Int32Enum), typeof(int));
        }

        [Fact]
        public void EmitConvertToEnum_ToNonEnum_ShouldThrowException()
        {
            Assert.Throws(
                typeof(ArgumentException),
                () => this.ilgenerator.EmitConvertToEnum(typeof(string), typeof(int)));
        }

        [Fact]
        public void EmitConvertToEnum_ToEnum_ShouldNotThrowException()
        {
            this.ilgenerator.EmitConvertToEnum(typeof(string), typeof(Int32Enum));
        }

        [Fact]
        public void EmitValueTypeBox_FromInt_ShouldLoadLocalAdress()
        {
            this.ilgenerator.EmitValueTypeBox(typeof(int));

            Assert.Equal(OpCodes.Ldloca, this.ilgenerator.Instructions.Last().OpCode);
        }

        [Fact]
        public void EmitValueTypeBox_FromPerson_ShouldNotResultAnyInstructions()
        {
            this.ilgenerator.EmitValueTypeBox(typeof(Person));

            Assert.Equal(0, this.ilgenerator.Instructions.Length);
        }

        [Fact]
        public void EmitValueTypeBox_FromEnum_ShouldBoxValue()
        {
            this.ilgenerator.EmitValueTypeBox(typeof(Int32Enum));

            Assert.Equal(OpCodes.Box, this.ilgenerator.Instructions.Last().OpCode);
        }
    }
}
