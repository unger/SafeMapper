namespace SafeMapper.Utils
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    public class AttributeHelper
    {
        public static string GetEnumDisplayValue(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = GetAttribute<DisplayAttribute>(fieldInfo);

            return (attribute != null) ? attribute.GetName() : string.Empty;
        }

        public static string GetEnumDescriptionValue(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = GetAttributeWithName(fieldInfo, "DescriptionAttribute");
            var descProp = attribute?.GetType().GetProperty("Description");
            if (descProp != null)
            {
                return descProp.GetValue(attribute).ToString();
            }

            return string.Empty;
        }

        public static T GetAttribute<T>(MemberInfo member) where T : Attribute
        {
            var attributes = GetAttributes<T>(member);
            return (attributes.Length > 0) ? attributes[0] : default(T);
        }

        public static T[] GetAttributes<T>(MemberInfo member) where T : Attribute
        {
            var array = member.GetCustomAttributes(typeof(T), false).ToArray();
            T[] newArray = new T[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = (T)array[i];
            }
            return newArray;
        }

        public static Attribute GetAttributeWithName(MemberInfo member, string attributeName)
        {
            var attributes = GetAttributesWithName(member, attributeName);
            return (attributes.Length > 0) ? attributes[0] : null;
        }

        public static Attribute[] GetAttributesWithName(MemberInfo member, string attributeName)
        {
            return member.GetCustomAttributes().Where(x => x.GetType().Name == attributeName).ToArray();
        }
    }
}
