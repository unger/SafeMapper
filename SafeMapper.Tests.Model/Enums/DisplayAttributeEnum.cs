namespace SafeMapper.Tests.Model.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum DisplayAttributeEnum
    {
        Undefined = 0,
        [Display(Name = "Value 1")]Value1 = 1,
        [Display(Name = "Value 2")]Value2 = 2,
        [Display(Name = "Value 3")]Value3 = 3,
    }
}
