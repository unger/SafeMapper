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

        private List<LabelWrapper> labels = new List<LabelWrapper>();

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

        public LocalBuilderWrapper DeclareLocal(Type type)
        {
            var local = new LocalBuilderWrapper(type)
            {
                LocalBuilder = this.il.DeclareLocal(type)
            };

            this.AddInstruction(OpCodes.Nop, local);
            return local;
        }

        public LabelWrapper DefineLabel()
        {
            var label = new LabelWrapper
            {
                Label = this.il.DefineLabel()
            };

            this.AddInstruction(OpCodes.Nop, label);
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

        //public void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
        //{
        //    this.AddInstruction(opcode, methodInfo, optionalParameterTypes);
        //}

        public void MarkLabel(LabelWrapper label)
        {
            this.AddInstruction(OpCodes.Nop, label);
            this.il.MarkLabel(label.Label);
        }

        private void AddInstruction(OpCode opcode)
        {
            this.il.Emit(opcode);
            this.instructions.Add(new ILInstruction(opcode));
        }

        private void AddInstruction(OpCode opcode, ConstructorInfo con)
        {
            this.il.Emit(opcode, con);
            this.instructions.Add(new ILInstruction(opcode, con, typeof(ConstructorInfo)));
        }

        private void AddInstruction(OpCode opcode, FieldInfo field)
        {
            this.il.Emit(opcode, field);
            this.instructions.Add(new ILInstruction(opcode, field, typeof(FieldInfo)));
        }

        private void AddInstruction(OpCode opcode, LabelWrapper label)
        {
            if (opcode != OpCodes.Nop)
            {
                this.il.Emit(opcode, label.Label);
            }
            this.instructions.Add(new ILInstruction(opcode, label, typeof(Label)));
        }

        private void AddInstruction(OpCode opcode, Type type)
        {
            this.il.Emit(opcode, type);
            this.instructions.Add(new ILInstruction(opcode, type, typeof(Type)));
        }

        private void AddInstruction(OpCode opcode, int value)
        {
            this.il.Emit(opcode, value);
            this.instructions.Add(new ILInstruction(opcode, value, typeof(int)));
        }

        private void AddInstruction(OpCode opcode, long value)
        {
            this.il.Emit(opcode, value);
            this.instructions.Add(new ILInstruction(opcode, value, typeof(long)));
        }

        private void AddInstruction(OpCode opcode, double value)
        {
            this.il.Emit(opcode, value);
            this.instructions.Add(new ILInstruction(opcode, value, typeof(double)));
        }

        private void AddInstruction(OpCode opcode, float value)
        {
            this.il.Emit(opcode, value);
            this.instructions.Add(new ILInstruction(opcode, value, typeof(float)));
        }

        private void AddInstruction(OpCode opcode, LocalBuilderWrapper local)
        {
            if (opcode != OpCodes.Nop)
            {
                this.il.Emit(opcode, local.LocalBuilder);
            }
            this.instructions.Add(new ILInstruction(opcode, local, typeof(LocalBuilderWrapper)));
        }

        private void AddInstruction(OpCode opcode, string argument)
        {
            this.il.Emit(opcode, argument);
            this.instructions.Add(new ILInstruction(opcode, argument, typeof(string)));
        }

        private void AddInstruction(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
        {
            this.il.EmitCall(opcode, methodInfo, optionalParameterTypes);
            this.instructions.Add(new ILInstruction(opcode, methodInfo, typeof(MethodInfo)));
        }
     }
}
