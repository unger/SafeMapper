namespace MapEverything.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    public class ReflectionUtils
    {
        public static MemberWrapper[] GetMembers(Type type)
        {
            var members = new List<MemberWrapper>();
            var properties = type.GetProperties();
            var fields = type.GetFields();
            
            members.AddRange(properties.Select(p => new MemberWrapper(p)));
            members.AddRange(fields.Select(f => new MemberWrapper(f)));

            return members.ToArray();
        }

        public static MemberWrapper GetMember(Type type, string name)
        {
            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Dictionary<,>))
                {
                    var genericArguments = type.GetGenericArguments();
                    if (genericArguments[0] == typeof(string))
                    {
                        var itemIndexer = type.GetProperty("Item", new[] { typeof(string) });
                        if (itemIndexer != null)
                        {
                            return new MemberWrapper(name, itemIndexer);
                        }
                    }
                }
            }
            else if (type == typeof(NameValueCollection))
            {
                var getValuesMethod = type.GetMethod("GetValues", new[] { typeof(string) });
                if (getValuesMethod != null)
                {
                    return new MemberWrapper(name, getValuesMethod);
                }
            }

            var propertyInfo = type.GetProperty(name);
            if (propertyInfo != null)
            {
                return new MemberWrapper(propertyInfo);
            }

            var fieldInfo = type.GetField(name);
            if (fieldInfo != null)
            {
                return new MemberWrapper(fieldInfo);
            }

            return null;
        }
    }
}
