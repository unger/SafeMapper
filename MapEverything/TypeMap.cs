namespace MapEverything
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Fasterflect;

    public class TypeMap
    {
        private readonly Type fromType;

        private readonly Type toType;

        private readonly ITypeMapper typeMapper;

        private Dictionary<string, IMemberMap> memberMaps = new Dictionary<string, IMemberMap>();

        private Dictionary<string, MemberInfo> fromMembers = new Dictionary<string, MemberInfo>();
        private Dictionary<string, MemberInfo> toMembers = new Dictionary<string, MemberInfo>();

        public TypeMap(Type fromType, Type toType, ITypeMapper typeMapper)
        {
            this.fromType = fromType;
            this.toType = toType;
            this.typeMapper = typeMapper;
            var isFromTypeDictionary = this.IsStringDictionary(fromType);
            var isToTypeDictionary = this.IsStringDictionary(toType);

            if (!isFromTypeDictionary)
            {
                foreach (var member in fromType.GetMembers())
                {
                    if (member.IsReadable() && !member.IsInvokable())
                    {
                        this.fromMembers.Add(member.Name, member);
                    }
                }
            }
            
            if (!isToTypeDictionary)
            {
                foreach (var member in toType.GetMembers())
                {
                    if (member.IsReadable() && !member.IsInvokable())
                    {
                        this.toMembers.Add(member.Name, member);
                    }
                }
            }

            if (isFromTypeDictionary && isToTypeDictionary)
            {
                // Both Dictionaries
            }
            else if (isFromTypeDictionary)
            {
                foreach (var key in this.toMembers.Keys)
                {
                    var member = this.toMembers[key];
                    var memberMap = new DictionaryMemberMap(fromType, toType, member.Name, member.Name);

                    this.AddMemberMap(member.Name, memberMap);
                }
            }
            else if (isToTypeDictionary)
            {
                foreach (var key in this.fromMembers.Keys)
                {
                    var member = this.fromMembers[key];
                    var memberMap = new DictionaryMemberMap(fromType, toType, member.Name, member.Name);

                    this.AddMemberMap(member.Name, memberMap);
                }
            }
            else
            {
                // No Dictionary
                foreach (var key in this.fromMembers.Keys)
                {
                    if (this.toMembers.ContainsKey(key))
                    {
                        var member = this.fromMembers[key];
                        var memberMap = new MemberMap(fromType, toType, member.Name, member.Name);

                        this.AddMemberMap(member.Name, memberMap);
                    }
                }
            }
        }

        public void Map(object fromObject, object toObject)
        {
            foreach (var key in this.memberMaps.Keys)
            {
                this.memberMaps[key].Map(fromObject, toObject);
            }
        }

        private void AddMemberMap(string name, IMemberMap memberMap)
        {
            if (memberMap.IsValid())
            {
                memberMap.SetConverter(this.typeMapper.GetConverter(memberMap.FromPropertyType, memberMap.ToPropertyType));
                this.memberMaps.Add(name, memberMap);
            }
        }

        private bool IsStringDictionary(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>) && type.GetGenericArguments()[0] == typeof(string);
        }
    }
}
