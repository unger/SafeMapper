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
            long result = ToLong(value);
            return (result <= int.MaxValue && result >= int.MinValue) ? (int)result : 0;
        }

        public static string ToString(IFormattable formattable, IFormatProvider formatProvider)
        {
            return formattable.ToString(null, formatProvider);
        }

        public static Guid ToGuid(string s)
        {
            Guid g;
            return Guid.TryParse(s, out g) ? g : Guid.Empty;
        }

        public static long ToLong(string value)
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
    }
}
