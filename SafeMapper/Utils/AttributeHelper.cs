namespace SafeMapper.Utils
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    public class AttributeHelper
    {
        public static string GetDisplayValue(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var displayAttribute = GetAttribute<DisplayAttribute>(fieldInfo);

            return (displayAttribute != null) ? displayAttribute.Name : string.Empty;
        }

        public static string GetDescriptionValue(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var displayAttribute = GetAttribute<DescriptionAttribute>(fieldInfo);

            return (displayAttribute != null) ? displayAttribute.Description : string.Empty;
        }

        public static T GetAttribute<T>(MemberInfo member) where T : Attribute
        {
            var attributes = GetAttributes<T>(member);
            return (attributes.Length > 0) ? attributes[0] : default(T);
        }

        public static T[] GetAttributes<T>(MemberInfo member) where T : Attribute
        {
            return Array.ConvertAll(member.GetCustomAttributes(typeof(T), false), input => (T)input);
        }
    }
}
