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

        [Test]
        public void EmitMemberMap_ConstructorInfo_ShouldReturnDefaultValue()
        {
            var convertDynamicMethod = new DynamicMethod(
                "TestDenamicMethod",
                typeof(void),
                new[] { typeof(ClassProperty<string>), typeof(ClassProperty<int>) },
                typeof(EmitExtensionsTests).Module);

            var il = new ILGeneratorAdapter(convertDynamicMethod.GetILGenerator());

            var fromLocal = il.DeclareLocal(typeof(ClassProperty<string>));
            var toLocal = il.DeclareLocal(typeof(ClassProperty<int>));

            il.Emit(OpCodes.Ldarg_0);
            il.EmitLocal(OpCodes.Stloc, fromLocal);

            il.Emit(OpCodes.Ldarg_1);
            il.EmitLocal(OpCodes.Stloc, toLocal);

            var fromMemberWrapper = new MemberWrapper(typeof(ClassProperty<string>).GetConstructor(Type.EmptyTypes));
            var toMemberWrapper = new MemberWrapper(typeof(ClassProperty<int>).GetConstructor(Type.EmptyTypes));
            //var fromMemberWrapper = new MemberWrapper(typeof(ClassProperty<string>).GetProperty("Value"));
            //var toMemberWrapper = new MemberWrapper(typeof(ClassProperty<int>).GetProperty("Value"));
            
            il.EmitMemberMap(fromLocal, toLocal, fromMemberWrapper, toMemberWrapper);

            var from = new ClassProperty<string> { Value = "1337" };
            var to = new ClassProperty<int>();

            var membermap = (Action<ClassProperty<string>, ClassProperty<int>>)convertDynamicMethod.CreateDelegate(typeof(Action<ClassProperty<string>, ClassProperty<int>>));

            membermap(from, to);

            Assert.AreEqual(0, to.Value);
        }
    }
}
