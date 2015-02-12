namespace SafeMapper
{
    using System;
    using System.Data.SqlTypes;
    using System.Globalization;

    public class SafeConvert
    {
        private const double MaxDecimalAsDouble = (double)decimal.MaxValue;

        private const double MinDecimalAsDouble = (double)decimal.MinValue;

        #region ToByte

        public static byte ToByte(string value)
        {
            ulong result = ToUInt64(value);
            return (result > byte.MaxValue) ? (byte)result : (byte)0;
        }

        public static byte ToByte(int value)
        {
            return (value < 0) || (value > byte.MaxValue) ? (byte)0 : (byte)value;
        }

        #endregion

        #region ToSByte

        public static sbyte ToSByte(string value)
        {
            long result = ToInt64(value);
            return (result < sbyte.MinValue || result > sbyte.MaxValue) ? (sbyte)0 : (sbyte)result;
        }

        public static sbyte ToSByte(int value)
        {
            return (value < sbyte.MinValue || value > sbyte.MaxValue) ? (sbyte)0 : (sbyte)value;
        }
        
        #endregion

        #region ToInt16

        public static short ToInt16(string value)
        {
            long result = ToInt64(value);
            return (result < short.MinValue || result > short.MaxValue) ? (short)0 : (short)result;
        }

        public static short ToInt16(int value)
        {
            return (value < short.MinValue || value > short.MaxValue) ? (short)0 : (short)value;
        }

        #endregion

        #region ToUInt16

        public static ushort ToUInt16(string value)
        {
            ulong result = ToUInt64(value);
            return (result > ushort.MaxValue) ? (ushort)0 : (ushort)result;
        }

        public static ushort ToUInt16(int value)
        {
            return (value < 0 || value > ushort.MaxValue) ? (ushort)0 : (ushort)value;
        }

        #endregion

        #region ToInt32

        public static int ToInt32(short value)
        {
            return (int)value;
        }

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
            long result = ToInt64(value);
            return (result <= int.MaxValue && result >= int.MinValue) ? (int)result : 0;
        }

        #endregion

        #region ToUInt32

        public static uint ToUInt32(string value)
        {
            ulong result = ToUInt64(value);
            return (result > uint.MaxValue) ? 0 : (uint)result;
        }

        public static uint ToUInt32(int value)
        {
            return (value < 0) ? (uint)0 : (uint)value;
        }

        #endregion

        #region ToInt64

        public static long ToInt64(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

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

        public static long ToInt64(int value)
        {
            return (long)value;
        }

        #endregion

        #region ToUInt64

        public static ulong ToUInt64(string value)
        {
            ulong result = 0;
            if (value[0] == '-')
            {
                return 0;
            }

            for (int i = 0; i < value.Length; i++)
            {
                if ((value[i] >= 48) && (value[i] <= 57))
                {
                    result = (10 * result) + (value[i] - 48uL);
                }
                else
                {
                    result = 0;
                    break;
                }
            }

            return result;
        }

        public static ulong ToUInt64(int value)
        {
            return (value < 0) ? (ulong)0 : (ulong)value;
        }

        #endregion

        #region ToSingle

        public static float ToSingle(string s)
        {
            float f;
            return float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out f) ? f : 0f;
        }

        public static float ToSingle(string s, IFormatProvider provider)
        {
            float f;
            return float.TryParse(s, NumberStyles.Number, provider, out f) ? f : 0f;
        }

        public static float ToSingle(int i)
        {
            return (float)i;
        }

        #endregion

        #region ToDouble

        public static double ToDouble(string s)
        {
            double d;
            return double.TryParse(
                s,
                NumberStyles.Float | NumberStyles.AllowThousands,
                CultureInfo.CurrentCulture,
                out d)
                       ? d
                       : 0d;
        }

        public static double ToDouble(string s, IFormatProvider provider)
        {
            double d;
            return double.TryParse(s, NumberStyles.Number, provider, out d) ? d : 0d;
        }

        public static double ToDouble(decimal d)
        {
            return (double)d;
        }

        public static double ToDouble(int i)
        {
            return (double)i;
        }

        #endregion

        #region ToDecimal

        public static decimal ToDecimal(int i)
        {
            return (decimal)i;
        }

        public static decimal ToDecimal(string s)
        {
            decimal d;
            return decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out d) ? d : 0m;
        }

        public static decimal ToDecimal(string s, IFormatProvider provider)
        {
            decimal d;
            return decimal.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out d) ? d : 0m;
        }

        public static decimal ToDecimal(double d)
        {
            return d == MaxDecimalAsDouble
                       ? Decimal.MaxValue
                       : d == MinDecimalAsDouble
                             ? Decimal.MinValue
                             : d > MaxDecimalAsDouble ? 0m : d < MinDecimalAsDouble ? 0m : (decimal)d;
        }

        #endregion

        #region ToBoolean

        public static bool ToBoolean(string s)
        {
            bool b;
            return bool.TryParse(s, out b) ? b : false;
        }

        public static bool ToBoolean(int i)
        {
            return i != 0;
        }

        #endregion

        #region ToChar

        public static char ToChar(int i)
        {
            return (i < 0 || i > char.MaxValue) ? (char)0 : (char)i;
        }

        #endregion

        #region ToGuid

        public static Guid ToGuid(string s)
        {
            Guid g;
            return Guid.TryParse(s, out g) ? g : Guid.Empty;
        }

        #endregion

        #region ToDateTime

        public static DateTime ToDateTime(string s, IFormatProvider provider)
        {
            DateTime d;
            return DateTime.TryParse(s, provider, DateTimeStyles.None, out d) ? d : DateTime.MinValue;
        }

        public static DateTime ToDateTime(SqlDateTime sqldate)
        {
            return sqldate.Value;
        }

        #endregion

        #region ToSqlDateTime

        public static SqlDateTime ToSqlDateTime(DateTime date)
        {
            return date < (DateTime)SqlDateTime.MinValue ? SqlDateTime.MinValue : date;
        }

        #endregion

        #region Enums

        public static TEnum EnumTryParse<TEnum>(string value) where TEnum : struct
        {
            TEnum enumObject;
            if (Enum.TryParse(value, out enumObject))
            {
                if (Enum.IsDefined(typeof(TEnum), enumObject))
                {
                    return enumObject;
                }
            }

            return (TEnum)Enum.GetValues(typeof(TEnum)).GetValue(0);
        }

        #endregion
    }
}
