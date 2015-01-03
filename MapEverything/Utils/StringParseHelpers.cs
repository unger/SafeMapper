namespace MapEverything.Utils
{
    using System;
    using System.Globalization;

    public class StringParser
    {
        public static short TryParseInt16(string s, IFormatProvider provider)
        {
            short sh;
            return short.TryParse(s, NumberStyles.Integer, provider, out sh) ? sh : (short)0;
        }

        public static int TryParseInt32(string s, IFormatProvider provider)
        {
            int i;
            return int.TryParse(s, NumberStyles.Integer, provider, out i) ? i : 0;
        }

        public static long TryParseInt64(string s, IFormatProvider provider)
        {
            long l;
            return long.TryParse(s, NumberStyles.Integer, provider, out l) ? l : 0;
        }

        public static ushort TryParseUInt16(string s, IFormatProvider provider)
        {
            ushort sh;
            return ushort.TryParse(s, NumberStyles.Integer, provider, out sh) ? sh : (ushort)0;
        }

        public static uint TryParseUInt32(string s, IFormatProvider provider)
        {
            uint i;
            return uint.TryParse(s, NumberStyles.Integer, provider, out i) ? i : 0;
        }

        public static ulong TryParseUInt64(string s, IFormatProvider provider)
        {
            ulong l;
            return ulong.TryParse(s, NumberStyles.Integer, provider, out l) ? l : 0;
        }

        public static decimal TryParseDecimal(string s, IFormatProvider provider)
        {
            decimal d;
            return decimal.TryParse(s, NumberStyles.Number, provider, out d) ? d : 0m;
        }

        public static float TryParseSingle(string s, IFormatProvider provider)
        {
            float d;
            return float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out d) ? d : 0;
        }

        public static double TryParseDouble(string s, IFormatProvider provider)
        {
            double d;
            return double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out d) ? d : 0;
        }

        public static byte TryParseByte(string s, IFormatProvider provider)
        {
            byte b;
            return byte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out b) ? b : (byte)0;
        }

        public static sbyte TryParseSByte(string s, IFormatProvider provider)
        {
            sbyte b;
            return sbyte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out b) ? b : (sbyte)0;
        }

        public static Guid TryParseGuid(string s, IFormatProvider provider)
        {
            Guid g;
            return Guid.TryParse(s, out g) ? g : Guid.Empty;
        }

        public static DateTime TryParseDateTime(string s, IFormatProvider provider)
        {
            DateTime d;
            return DateTime.TryParse(s, provider, DateTimeStyles.None, out d) ? d : DateTime.MinValue;
        }

        public static bool TryParseBoolean(string s)
        {
            bool b;
            return bool.TryParse(s, out b) ? b : false;
        }
    }
}
