using System;

namespace SafeMapper.Tests.Model.ConvertClasses
{
    public class FakeConvertWithoutSingleton
    {
        public int ToInt32(string value)
        {
            return 1337;
        }

        public string ToString(int value, IFormatProvider formatProvider)
        {
            return "1337";
        }
    }
}
