namespace SafeMapper.Profiler
{
    public class ProfileIntParse : ProfileBase
    {
        public ProfileIntParse()
        {
            this.iterations = new[] { 1000, 10000, 100000, 1000000 };
        }

        public override void Execute()
        {

            this.WriteHeader();

            this.AddResult("int.Parse", i => int.Parse("12345"));
            this.AddResult("int.TryParse", i => this.TryParse("12345"));
            this.AddResult("int.TryParse cond", i => this.TryParseCond("12345"));
            this.AddResult("int.TryParse static", i => TryParseStatic("12345"));
            this.AddResult("IntParseFast", i => IntParseFast("12345"));
            this.AddResult("SafeIntParseFast", i => SafeIntParseFast("12345"));
            this.AddResult("SafeConvert", i => SafeConvert.ToInt32("12345"));
            this.AddResult("SafeNullableConvert", i => SafeNullableConvert.ToInt32("12345"));
        }

        private static int TryParseStatic(string str)
        {
            int result;
            int.TryParse(str, out result);
            return result;
        }

        private static int IntParseFast(string value)
        {
            // An optimized int parse method.
            int result = 0;
            bool neg = value[0] == '-';
            for (int i = neg ? 1 : 0; i < value.Length; i++)
            {
                result = (10 * result) + (value[i] - 48);
            }

            return neg ? result * -1 : result;
        }

        private static long LongParseFast(string value)
        {
            long result = 0;
            bool neg = value[0] == '-';
            for (int i = neg ? 1 : 0; i < value.Length; i++)
            {
                if ((value[i] >= 48) && (value[i] <= 57))
                {
                    result = (10 * result) + (value[i] - 48);
                }
                else
                {
                    result = 0;
                    break;
                }
            }

            return neg ? result * -1 : result;
        }

        private static int SafeIntParseFast(string value)
        {
            long result = LongParseFast(value);
            return (result <= int.MaxValue && result >= int.MinValue) ? (int)result : 0;
        }


        private int TryParse(string str)
        {
            int result;
            int.TryParse(str, out result);
            return result;
        }

        private int TryParseCond(string str)
        {
            int result;
            return int.TryParse(str, out result) ? result : 0;
        }
    }
}
