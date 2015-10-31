using System;

namespace SafeMapper.Tests.Model.ConvertClasses
{
    public class FakeConvertSingletonProperty
    {
        private static readonly FakeConvertSingletonProperty _instance = new FakeConvertSingletonProperty();

        public static FakeConvertSingletonProperty Instance
        {
            get { return _instance; }
        }

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
