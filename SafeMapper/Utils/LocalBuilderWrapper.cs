using System;
using System.Reflection.Emit;

namespace SafeMapper.Utils
{
    public class LocalBuilderWrapper
    {
        private readonly Type _localType;

        public LocalBuilderWrapper(Type localType)
        {
            _localType = localType;
        }

        public LocalBuilder LocalBuilder { get; set; }

        public Type LocalType
        {
            get { return _localType; }
        }
    }
}
