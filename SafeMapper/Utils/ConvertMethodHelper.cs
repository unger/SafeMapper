using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMapper.Utils
{
    using System.Collections.Concurrent;
    using System.Reflection;

    using SafeMapper.Reflection;

    public class ConvertMethodHelper
    {
        private static readonly ConcurrentDictionary<string, MethodInfo> ConverterCache = new ConcurrentDictionary<string, MethodInfo>();

        public static MethodInfo GetConvertMethod(Type fromType, Type toType)
        {
            return ConverterCache.GetOrAdd(
                string.Concat(toType.FullName, fromType.FullName),
                k => ReflectionUtils.GetConvertMethod(
                    fromType,
                    toType,
                    new[]
                        {
                            typeof(SafeConvert)
                        }));
        }
    }
}
