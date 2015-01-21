namespace MapEverything
{
    using System;
    using System.Globalization;

    public class SafeConvert
    {
        public static int ToInt32(long value)
        {
            return (value < int.MinValue || value > int.MaxValue) ? 0 : (int)value;
        }

        public static int ToInt32(uint value)
        {
            return (value > int.MaxValue) ? 0 : (int)value;
        }

        public static int ToInt32(ulong value)
        {
            return (value > int.MaxValue) ? 0 : (int)value;
        }

        public static int ToInt32(double value)
        {
            if (value >= 0)
            {
                if (value < 2147483647.5)
                {
                    int result = (int)value;
                    double dif = value - result;
                    if (dif > 0.5 || (dif == 0.5 && (result & 1) != 0))
                    {
                        result++;
                    }

                    return result;
                }
            }
            else
            {
                if (value >= -2147483648.5)
                {
                    int result = (int)value;
                    double dif = value - result;
                    if (dif < -0.5 || (dif == -0.5 && (result & 1) != 0))
                    {
                        result--;
                    }

                    return result;
                }
            }

            return 0;
        }

        public static int ToInt32(decimal value)
        {
            return (value < int.MinValue || value > int.MaxValue) ? 0 : Convert.ToInt32(value);
        }

        public static int ToInt32(string value)
        {
            int i;
            return int.TryParse(value, NumberStyles.Integer, CultureInfo.CurrentCulture, out i) ? i : 0;
        }

        public static int ToInt32(string value, IFormatProvider provider)
        {
            int i;
            return int.TryParse(value, NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, provider, out i) ? i : 0;
        }

        public static string ToString(IFormattable formattable, IFormatProvider formatProvider)
        {
            return formattable.ToString(null, formatProvider);
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

    }
}
