namespace MapEverything.TypeMaps
{
    using System;
    using System.Collections.Generic;

    using Fasterflect;

    public class ClassTypeMap : ITypeMap
    {
        private readonly TypeDefinition fromTypeDef;

        private readonly TypeDefinition toTypeDef;

        private MemberMap[] properties;

        public ClassTypeMap(Type fromType, Type toType, IFormatProvider formatProvider, ITypeMapper typeMapper)
        {
            var memberMaps = new List<MemberMap>();
            this.toTypeDef = typeMapper.GetTypeDefinition(toType);
            this.fromTypeDef = typeMapper.GetTypeDefinition(fromType);

            this.Convert = this.ConvertClass;

            foreach (var key in this.fromTypeDef.MemberGetters.Keys)
            {
                if (this.toTypeDef.MemberSetters.ContainsKey(key))
                {
                    var fromMember = this.fromTypeDef.Members[key];
                    var toMember = this.toTypeDef.Members[key];

                    var converter = typeMapper.GetConverter(fromMember.Type(), toMember.Type(), formatProvider);

                    var memberMap = new MemberMap(
                        this.fromTypeDef.MemberGetters[key],
                        this.toTypeDef.MemberSetters[key],
                        converter);

                    memberMaps.Add(memberMap);
                }
            }

            this.properties = memberMaps.ToArray();
        }

        public Func<object, object> Convert { get; private set; }

        private object ConvertClass(object fromObject)
        {
            var toObject = this.toTypeDef.CreateInstanceDelegate();
            for (int i = 0; i < this.properties.Length; i++)
            {
                this.properties[i].Map(fromObject, toObject);
            }

            return toObject;
        }
    }
}
