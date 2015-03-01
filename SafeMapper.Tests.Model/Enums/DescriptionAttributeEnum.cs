namespace SafeMapper.Tests.Model.Enums
{
    using System.ComponentModel;

    public enum DescriptionAttributeEnum
    {
        Undefined = 0,
        [Description("Value 1")]Value1 = 1,
        [Description("Value 2")]Value2 = 2,
        [Description("Value 3")]Value3 = 3,
    }
}
