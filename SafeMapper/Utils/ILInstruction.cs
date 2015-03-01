namespace SafeMapper.Utils
{
    using System.Reflection.Emit;

    public class ILInstruction
    {
        private readonly int offset;

        private readonly OpCode opcode;

        private readonly string argument;

        public ILInstruction(int offset, OpCode opcode, string argument = "")
        {
            this.offset = offset;
            this.opcode = opcode;
            this.argument = argument;
        }

        /*public string Offset
        {
            get
            {
                return string.Format("IL_{0:D4}", this.offset);
            }
        }*/

        public OpCode OpCode
        {
            get
            {
                return this.opcode;
            }
        }

        /*public string Argument
        {
            get
            {
                return this.argument;
            }
        }*/
    }
}
