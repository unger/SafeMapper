using System;

namespace SafeMapper.Utils
{
    using System.Reflection.Emit;

    public class ILInstruction
    {
        private readonly OpCode _opcode;

        private readonly object _argument;

        private readonly Type _argumentType;

        public ILInstruction(OpCode opcode) : this(opcode, null, null)
        {
        }

        public ILInstruction(OpCode opcode, object argument, Type argumentType)
        {
            _opcode = opcode;
            _argument = argument;
            _argumentType = argumentType;
        }

        public OpCode OpCode
        {
            get
            {
                return this._opcode;
            }
        }

        public object Argument
        {
            get
            {
                return this._argument;
            }
        }

        public Type ArgumentType
        {
            get
            {
                return this._argumentType;
            }
        }
    }
}
