namespace SafeMapper.Tests.Model.Enums
{
    using System.ComponentModel.DataAnnotations;

    using SafeMapper.Tests.Model.Resources;

    public enum DisplayAttributeResxEnum
    {
        [Display(Name = "Undefined", ResourceType = typeof(ResourceStrings))]Undefined = 0,
        [Display(Name = "Value1", ResourceType = typeof(ResourceStrings))]Value1 = 1,
        [Display(Name = "Value2", ResourceType = typeof(ResourceStrings))]Value2 = 2,
        [Display(Name = "Value3", ResourceType = typeof(ResourceStrings))]Value3 = 3,
    }
}
