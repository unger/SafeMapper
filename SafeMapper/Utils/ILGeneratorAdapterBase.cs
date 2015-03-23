namespace SafeMapper.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    public class ILGeneratorAdapterBase
    {
        private readonly ILGenerator il;

        private List<ILInstruction> instructions = new List<ILInstruction>();

        private List<Label> labels = new List<Label>();

        public ILGeneratorAdapterBase(ILGenerator il)
        {
            this.il = il;
        }

        public int Offset
        {
            get
            {
                return this.il.ILOffset;
            }
        }

        public ILInstruction[] Instructions
        {
            get
            {
                return this.instructions.ToArray();
            }
        }

        public LocalBuilder DeclareLocal(Type type)
        {
            return this.il.DeclareLocal(type);
        }

        public Label DefineLabel()
        {
            var label = this.il.DefineLabel();
            this.labels.Add(label);
            return label;
        }

        public void Emit(OpCode opcode)
        {
            this.AddInstruction(opcode);
        }

        public void Emit(OpCode opcode, Type type)
        {
            this.AddInstruction(opcode, type);
        }

        public void EmitField(OpCode opcode, FieldInfo field)
        {
            var supportedOpcodes = new[] { OpCodes.Ldfld, OpCodes.Stfld };
            if (!supportedOpcodes.Contains(opcode))
            {
                throw new Exception("Unsupported Opcode, only Ldfld and Stfld supported");
            }

            this.AddInstruction(opcode, field);
        }

        public void EmitLocal(OpCode opcode, LocalBuilder local)
        {
            var supportedOpcodes = new[] { OpCodes.Ldloc, OpCodes.Stloc, OpCodes.Ldloca };
            if (!supportedOpcodes.Contains(opcode))
            {
                throw new Exception("Unsupported Opcode, only Ldloc, Ldloca and Stloc supported");
            }

            this.AddInstruction(opcode, local);
        }

        public void EmitNewobj(ConstructorInfo con)
        {
            this.AddInstruction(OpCodes.Newobj, con);
        }

        public void EmitBreak(OpCode opcode, Label label)
        {
            if (!opcode.Name.StartsWith("b") || opcode == OpCodes.Break || opcode == OpCodes.Box)
            {
                throw new Exception("Unsupported Opcode, only 'break'-opcodes supported");
            }

            this.AddInstruction(opcode, label);
        }

        public void EmitByte(byte value)
        {
            this.AddInstruction(OpCodes.Ldc_I4, (int)value);
            this.AddInstruction(OpCodes.Conv_U1);
        }

        public void EmitDouble(double value)
        {
            this.AddInstruction(OpCodes.Ldc_R8, value);
        }

        public void EmitFloat(float value)
        {
            this.AddInstruction(OpCodes.Ldc_R4, value);
        }

        public void EmitInt(int value)
        {
            this.AddInstruction(OpCodes.Ldc_I4, value);
        }

        public void EmitUInt(uint value)
        {
            this.AddInstruction(OpCodes.Ldc_I4, (int)value);
            this.AddInstruction(OpCodes.Conv_U4);
        }

        public void EmitLong(long value)
        {
            this.AddInstruction(OpCodes.Ldc_I8, value);
        }

        public void EmitULong(ulong value)
        {
            this.AddInstruction(OpCodes.Ldc_I8, (long)value);
            this.AddInstruction(OpCodes.Conv_U8);
        }

        public void EmitSByte(sbyte value)
        {
            this.AddInstruction(OpCodes.Ldc_I4, (int)value);
            this.AddInstruction(OpCodes.Conv_I1);
        }

        public void EmitShort(short value)
        {
            this.AddInstruction(OpCodes.Ldc_I4, (int)value);
            this.AddInstruction(OpCodes.Conv_I2);
        }

        public void EmitUShort(ushort value)
        {
            this.AddInstruction(OpCodes.Ldc_I4, (int)value);
            this.AddInstruction(OpCodes.Conv_U2);
        }

        public void EmitString(string value)
        {
            this.AddInstruction(OpCodes.Ldstr, value);
        }

        public void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
        {
            this.AddInstruction(opcode, methodInfo, optionalParameterTypes);
        }

        public void MarkLabel(Label label)
        {
            var index = this.labels.IndexOf(label);
            this.instructions.Add(new ILInstruction(this.il.ILOffset, OpCodes.Nop, "Label_" + index));
            this.il.MarkLabel(label);
        }

        private void AddInstruction(OpCode opcode)
        {
            this.il.Emit(opcode);
            this.instructions.Add(new ILInstruction(this.il.ILOffset, opcode));
        }

        private void AddInstruction(OpCode opcode, ConstructorInfo con)
        {
            this.il.Emit(opcode, con);
            this.instructions.Add(new ILInstruction(this.il.ILOffset, opcode, con.DeclaringType + "." + con.Name));
        }

        private void AddInstruction(OpCode opcode, FieldInfo field)
        {
            this.il.Emit(opcode, field);
            this.instructions.Add(new ILInstruction(this.il.ILOffset, opcode, field.DeclaringType + "." + field.Name));
        }

        private void AddInstruction(OpCode opcode, Label label)
        {
            var index = this.labels.IndexOf(label);
            this.il.Emit(opcode, label);
            this.instructions.Add(new ILInstruction(this.il.ILOffset, opcode, "Label_" + index));
        }

        private void AddInstruction(OpCode opcode, Type type)
        {
            this.il.Emit(opcode, type);
            this.instructions.Add(new ILInstruction(this.il.ILOffset, opcode, type.FullName));
        }

        private void AddInstruction(OpCode opcode, int value)
        {
            this.il.Emit(opcode, value);
            this.instructions.Add(new ILInstruction(this.il.ILOffset, opcode, value.ToString(CultureInfo.InvariantCulture)));
        }

        private void AddInstruction(OpCode opcode, long value)
        {
            this.il.Emit(opcode, value);
            this.instructions.Add(new ILInstruction(this.il.ILOffset, opcode, value.ToString(CultureInfo.InvariantCulture)));
        }

        private void AddInstruction(OpCode opcode, double value)
        {
            this.il.Emit(opcode, value);
            this.instructions.Add(new ILInstruction(this.il.ILOffset, opcode, value.ToString(CultureInfo.InvariantCulture)));
        }

        private void AddInstruction(OpCode opcode, float value)
        {
            this.il.Emit(opcode, value);
            this.instructions.Add(new ILInstruction(this.il.ILOffset, opcode, value.ToString(CultureInfo.InvariantCulture)));
        }

        private void AddInstruction(OpCode opcode, LocalBuilder local)
        {
            this.il.Emit(opcode, local);
            this.instructions.Add(new ILInstruction(this.il.ILOffset, opcode, local.LocalIndex.ToString(CultureInfo.InvariantCulture)));
        }

        private void AddInstruction(OpCode opcode, string argument)
        {
            this.il.Emit(opcode, argument);
            this.instructions.Add(new ILInstruction(this.il.ILOffset, opcode, argument));
        }

        private void AddInstruction(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
        {
            this.il.EmitCall(opcode, methodInfo, optionalParameterTypes);
            this.instructions.Add(new ILInstruction(this.il.ILOffset, opcode, methodInfo.DeclaringType + "." + methodInfo.Name));
        }
     }
}
