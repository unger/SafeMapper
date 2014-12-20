namespace MapEverything.Converters
{
    using System;
    using System.ComponentModel;
    using System.Data.SqlTypes;
    using System.Globalization;

    public class SqlDateTimeTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(DateTime))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(DateTime))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is DateTime)
            {
                var datetime = (DateTime)value;

                var minsqlDateTime = (DateTime)SqlDateTime.MinValue;

                if (datetime < minsqlDateTime)
                {
                    return SqlDateTime.MinValue;
                }

                return (SqlDateTime)datetime;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }

            if (destinationType == typeof(DateTime) && value is SqlDateTime)
            {
                return ((SqlDateTime)value).Value;
            }

            if (destinationType == typeof(DateTime) && value is DBNull)
            {
                return DateTime.MinValue;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
