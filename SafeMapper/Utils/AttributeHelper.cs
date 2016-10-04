﻿namespace SafeMapper.Utils
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    public class AttributeHelper
    {
        public static string GetEnumDisplayValue(Enum value)
        {
            var fieldInfo = value.GetType().GetRuntimeField(value.ToString());
            var attribute = GetAttribute<DisplayAttribute>(fieldInfo);

            return (attribute != null) ? attribute.GetName() : string.Empty;
        }

        public static string GetEnumDescriptionValue(Enum value)
        {
            var fieldInfo = value.GetType().GetRuntimeField(value.ToString());
            var attribute = GetAttribute<DescriptionAttribute>(fieldInfo);

            return (attribute != null) ? attribute.Description : string.Empty;
        }

        public static T GetAttribute<T>(MemberInfo member) where T : Attribute
        {
            var attributes = GetAttributes<T>(member);
            return (attributes.Length > 0) ? attributes[0] : default(T);
        }

        public static T[] GetAttributes<T>(MemberInfo member) where T : Attribute
        {
            var array = member.GetCustomAttributes(typeof(T), false);
            T[] newArray = new T[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = (T)array[i];
            }
            return newArray;
        }
    }
}
