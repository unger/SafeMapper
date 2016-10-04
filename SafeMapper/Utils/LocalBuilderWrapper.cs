using System;
using System.Reflection.Emit;

namespace SafeMapper.Utils
{
    public class LocalBuilderWrapper
    {
        private readonly Type localType;

        public LocalBuilderWrapper(Type localType)
        {
            this.localType = localType;
        }

        public LocalBuilder LocalBuilder { get; set; }

        public Type LocalType
        {
            get { return this.localType; }
        }
    }
}
