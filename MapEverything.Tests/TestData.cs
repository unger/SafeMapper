namespace MapEverything.Tests
{
    using System;

    using NUnit.Framework;

    public class TestData
    {
        public TestCaseData[] StringToIntData =
            {
                new TestCaseData("0").Returns(0),
                new TestCaseData("10").Returns(10),
                new TestCaseData("10.0").Returns(0),
                new TestCaseData("10,0").Returns(0),
                new TestCaseData("10.5").Returns(0),
                new TestCaseData("10,5").Returns(0),
                new TestCaseData("-10").Returns(-10),
                new TestCaseData("1 000").Returns(0),
                new TestCaseData("1,000").Returns(0),
                new TestCaseData("1.000").Returns(0),
                new TestCaseData("1 000.00").Returns(0),
                new TestCaseData("2147483647").Returns(2147483647),
                new TestCaseData("2147483648").Returns(0),
                new TestCaseData("-2147483648").Returns(-2147483648),
                new TestCaseData("-2147483649").Returns(0),
                new TestCaseData("abc").Returns(0),
                new TestCaseData("123a").Returns(0),
            };

        public TestCaseData[] DecimalToIntData =
            {
                new TestCaseData(decimal.MaxValue).Returns(0),
                new TestCaseData(decimal.MinValue).Returns(0),
                new TestCaseData(123.5m).Returns(124),
                new TestCaseData(123.49m).Returns(123),
            };

        public TestCaseData[] DoubleToIntData =
            {
                new TestCaseData(double.MaxValue).Returns(0),
                new TestCaseData(double.MinValue).Returns(0),
                new TestCaseData(123.5d).Returns(124),
                new TestCaseData(123.49d).Returns(123),
            };


        public TestCaseData[] LongToIntData =
            {
                new TestCaseData(0).Returns(0),
                new TestCaseData(int.MaxValue).Returns(int.MaxValue),
                new TestCaseData(int.MinValue).Returns(int.MinValue),
                new TestCaseData(long.MaxValue).Returns(0),
                new TestCaseData(long.MinValue).Returns(0),
            };

        public TestCaseData[] IntToLongData =
            {
                new TestCaseData(0).Returns(0),
                new TestCaseData(int.MaxValue).Returns(int.MaxValue),
                new TestCaseData(int.MinValue).Returns(int.MinValue),
            };

        public TestCaseData[] IntToIntData =
            {
                new TestCaseData(0).Returns(0),
                new TestCaseData(int.MaxValue).Returns(int.MaxValue),
                new TestCaseData(int.MinValue).Returns(int.MinValue),
            };

        public TestCaseData[] UIntToIntData =
            {
                new TestCaseData((uint)0).Returns(0),
                new TestCaseData(uint.MaxValue).Returns(0),
                new TestCaseData((uint)int.MaxValue).Returns(int.MaxValue),
                new TestCaseData(uint.MinValue).Returns(0),
            };

        public TestCaseData[] IntToStringData =
            {
                new TestCaseData(0).Returns("0"),
                new TestCaseData(int.MaxValue).Returns(int.MaxValue.ToString()),
                new TestCaseData(int.MinValue).Returns(int.MinValue.ToString()),
            };

        public TestCaseData[] GuidToStringData =
            {
                new TestCaseData(Guid.Empty).Returns("00000000-0000-0000-0000-000000000000"),
                new TestCaseData(new Guid("0cb6c00f-fc44-484f-8ddd-823709b74601")).Returns("0cb6c00f-fc44-484f-8ddd-823709b74601"),
                new TestCaseData(new Guid("0cb6c00ffc44484f8ddd823709b74601")).Returns("0cb6c00f-fc44-484f-8ddd-823709b74601"),
            };

        public TestCaseData[] StringToGuidData =
            {
                new TestCaseData("00000000-0000-0000-0000-000000000000").Returns(Guid.Empty),
                new TestCaseData("0cb6c00f-fc44-484f-8ddd-823709b74601").Returns(new Guid("0cb6c00f-fc44-484f-8ddd-823709b74601")),
                new TestCaseData("0cb6c00ffc44484f8ddd823709b74601").Returns(new Guid("0cb6c00f-fc44-484f-8ddd-823709b74601")),
                new TestCaseData("abc").Returns(Guid.Empty),
                new TestCaseData("123").Returns(Guid.Empty),
            };




    }
}
