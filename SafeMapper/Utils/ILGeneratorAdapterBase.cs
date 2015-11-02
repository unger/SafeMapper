namespace SafeMapper.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;

    public class ILGeneratorAdapterBase
    {
        private List<ILInstruction> instructions = new List<ILInstruction>();

        public ILInstruction[] Instructions
        {
            get
            {
                return this.instructions.ToArray();
            }
        }

        public LocalBuilderWrapper DeclareLocal(Type type)
        {
            var local = new LocalBuilderWrapper(type);
            AddInstruction(new ILInstruction(OpCodes.Nop, local, typeof(LocalBuilderWrapper), ILInstructionType.DeclareLocal));
            return local;
        }

        public LabelWrapper DefineLabel()
        {
            var label = new LabelWrapper();
            AddInstruction(new ILInstruction(OpCodes.Nop, label, typeof(LabelWrapper), ILInstructionType.DefineLabel));
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
            if (opcode == OpCodes.Ldfld || opcode == OpCodes.Stfld || opcode == OpCodes.Ldsfld)
            {
                this.AddInstruction(opcode, field);
            }
            else
            {
                throw new Exception("Unsupported Opcode, only Ldfld and Stfld supported");
            }
        }

        public void EmitLocal(OpCode opcode, LocalBuilderWrapper local)
        {
            if (opcode == OpCodes.Ldloc || opcode == OpCodes.Stloc || opcode == OpCodes.Ldloca)
            {
                this.AddInstruction(opcode, local);
            }
            else
            {
                throw new Exception("Unsupported Opcode, only Ldloc, Ldloca and Stloc supported");
            }
        }

        public void EmitNewobj(ConstructorInfo con)
        {
            this.AddInstruction(OpCodes.Newobj, con);
        }

        public void EmitBreak(OpCode opcode, LabelWrapper label)
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

        public void EmitCall(OpCode opcode, MethodInfo methodInfo)
        {
            this.AddInstruction(opcode, methodInfo, null);
        }

        public void EmitInstructions(ILInstruction[] ins)
        {
            foreach (var instruction in ins)
            {
                AddInstruction(instruction);
            }
        }

        public void MarkLabel(LabelWrapper label)
        {
            AddInstruction(new ILInstruction(OpCodes.Nop, label, typeof(LabelWrapper), ILInstructionType.MarkLabel));
        }

        private void AddInstruction(OpCode opcode)
        {
            AddInstruction(new ILInstruction(opcode));
        }

        private void AddInstruction(OpCode opcode, ConstructorInfo con)
        {
            AddInstruction(new ILInstruction(opcode, con, typeof(ConstructorInfo)));
        }

        private void AddInstruction(OpCode opcode, FieldInfo field)
        {
            AddInstruction(new ILInstruction(opcode, field, typeof(FieldInfo)));
        }

        private void AddInstruction(OpCode opcode, LabelWrapper label)
        {
            AddInstruction(new ILInstruction(opcode, label, typeof(LabelWrapper)));
        }

        private void AddInstruction(OpCode opcode, Type type)
        {
            AddInstruction(new ILInstruction(opcode, type, typeof(Type)));
        }

        private void AddInstruction(OpCode opcode, int value)
        {
            AddInstruction(new ILInstruction(opcode, value, typeof(int)));
        }

        private void AddInstruction(OpCode opcode, long value)
        {
            AddInstruction(new ILInstruction(opcode, value, typeof(long)));
        }

        private void AddInstruction(OpCode opcode, double value)
        {
            AddInstruction(new ILInstruction(opcode, value, typeof(double)));
        }

        private void AddInstruction(OpCode opcode, float value)
        {
            AddInstruction(new ILInstruction(opcode, value, typeof(float)));
        }

        private void AddInstruction(OpCode opcode, LocalBuilderWrapper local)
        {
            AddInstruction(new ILInstruction(opcode, local, typeof(LocalBuilderWrapper)));
        }

        private void AddInstruction(OpCode opcode, string argument)
        {
            AddInstruction(new ILInstruction(opcode, argument, typeof(string)));
        }

        private void AddInstruction(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
        {
            AddInstruction(new ILInstruction(opcode, methodInfo, typeof(MethodInfo)));
        }

        private void AddInstruction(ILInstruction instruction)
        {
            this.instructions.Add(instruction);
        }
    }
}
