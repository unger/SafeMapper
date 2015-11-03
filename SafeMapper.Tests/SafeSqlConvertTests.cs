namespace SafeMapper.Tests
{
    using System;
    using System.Data.SqlTypes;
    using System.Globalization;

    using NUnit.Framework;

    [TestFixture]
    public class SafeSqlConvertTests
    {
        [TestCaseSource(typeof(TestData), "SqlDateTimeToDateTimeData")]
        public DateTime ToDateTime_FromSqlDateTime(SqlDateTime input)
        {
            return SafeSqlConvert.ToDateTime(input);
        }

        [TestCaseSource(typeof(TestData), "DateTimeToSqlDateTimeData")]
        public SqlDateTime ToSqlDateTime_FromSqlDateTime(DateTime input)
        {
            return SafeSqlConvert.ToSqlDateTime(input);
        }

        [TestCaseSource(typeof(TestData), "SqlDateTimeToDateTimeData")]
        public DateTime? ToNullableDateTime_FromSqlDateTime(SqlDateTime input)
        {
            return SafeSqlConvert.ToNullableDateTime(input);
        }

        [TestCaseSource(typeof(TestData), "DateTimeToSqlDateTimeData")]
        public SqlDateTime? ToNullableSqlDateTime_FromSqlDateTime(DateTime input)
        {
            return SafeSqlConvert.ToNullableSqlDateTime(input);
        }

    }
}
