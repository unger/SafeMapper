using System;

namespace SafeMapper.Utils
{
    using System.Reflection.Emit;

    public class ILInstruction
    {
        public ILInstruction(OpCode opcode)
            : this(opcode, null, null)
        {
        }

        public ILInstruction(OpCode opcode, object argument, Type argumentType, ILInstructionType instructionType = ILInstructionType.OpCode)
        {
            this.OpCode = opcode;
            this.Argument = argument;
            this.ArgumentType = argumentType;
            this.InstructionType = instructionType;
        }

        public OpCode OpCode { get; private set; }

        public object Argument { get; private set; }

        public Type ArgumentType { get; private set; }

        public ILInstructionType InstructionType { get; private set; }
    }
}
