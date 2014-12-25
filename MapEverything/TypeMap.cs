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

        private readonly TypeDefinition fromTypeDef;

        private readonly TypeDefinition toTypeDef;

        private readonly ITypeMapper typeMapper;

        private Dictionary<string, IMemberMap> memberMaps = new Dictionary<string, IMemberMap>();

        public TypeMap(TypeDefinition fromTypeDef, TypeDefinition toTypeDef, ITypeMapper typeMapper)
        {
            this.fromType = fromTypeDef.ActualType;
            this.toType = toTypeDef.ActualType;
            this.fromTypeDef = fromTypeDef;
            this.toTypeDef = toTypeDef;
            this.typeMapper = typeMapper;

            if (fromTypeDef.IsCollection || toTypeDef.IsCollection)
            {
                return;
            }

            foreach (var key in fromTypeDef.MemberGetters.Keys)
            {
                if (toTypeDef.MemberSetters.ContainsKey(key))
                {
                    var fromMember = fromTypeDef.Members[key];
                    var toMember = toTypeDef.Members[key];

                    var memberMap = new MemberMap(
                        fromTypeDef.ActualType,
                        toTypeDef.ActualType,
                        fromMember.Type(),
                        toMember.Type(),
                        fromTypeDef.MemberGetters[key],
                        toTypeDef.MemberSetters[key]);

                    this.AddMemberMap(key, memberMap);
                }
            }
        }

        public object Convert(object fromObject)
        {
            var toObject = this.toTypeDef.CreateInstanceDelegate();
            foreach (var key in this.memberMaps.Keys)
            {
                this.memberMaps[key].Map(fromObject, toObject);
            }

            return toObject;
        }

        private void AddMemberMap(string name, IMemberMap memberMap)
        {
            if (memberMap.IsValid())
            {
                memberMap.SetConverter(this.typeMapper.GetConverter(memberMap.FromMemberType, memberMap.ToMemberType));
                this.memberMaps.Add(name, memberMap);
            }
        }

        private bool IsStringDictionary(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>) && type.GetGenericArguments()[0] == typeof(string);
        }
    }
}
