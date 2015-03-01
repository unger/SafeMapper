namespace SafeMapper.Tests.Model.Enums
{
    using System.ComponentModel.DataAnnotations;

    using SafeMapper.Tests.Model.Resources;

    public enum DisplayAttributeResourceEnum
    {
        [Display(Name = "Undefined", ResourceType = typeof(ResourceStringsClass))]Undefined = 0,
        [Display(Name = "Value1", ResourceType = typeof(ResourceStringsClass))]Value1 = 1,
        [Display(Name = "Value2", ResourceType = typeof(ResourceStringsClass))]Value2 = 2,
        [Display(Name = "Value3", ResourceType = typeof(ResourceStringsClass))]Value3 = 3,
    }
}
