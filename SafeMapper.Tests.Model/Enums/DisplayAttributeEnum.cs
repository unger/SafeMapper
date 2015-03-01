namespace SafeMapper.Tests.Model.Enums
{
    using System.ComponentModel.DataAnnotations;

    using SafeMapper.Tests.Model.Resources;

    public enum DisplayAttributeEnum
    {
        [Display(Name = "Undefined")]Undefined = 0,
        [Display(Name = "Value 1")]Value1 = 1,
        [Display(Name = "Value 2")]Value2 = 2,
        [Display(Name = "Value 3")]Value3 = 3,
    }
}
