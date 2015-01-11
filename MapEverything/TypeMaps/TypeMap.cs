namespace MapEverything.TypeMaps
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Fasterflect;

    public class TypeMap : ITypeMap
    {
        private readonly TypeDefinition fromTypeDef;

        private readonly TypeDefinition toTypeDef;

        private List<IMemberMap> memberMaps = new List<IMemberMap>();

        public TypeMap(Type fromType, Type toType, IFormatProvider formatProvider, ITypeMapper typeMapper)
        {
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

                    this.memberMaps.Add(memberMap);
                }
            }
        }

        public Func<object, object> Convert { get; private set; }

        private object ConvertClass(object fromObject)
        {
            var toObject = this.toTypeDef.CreateInstanceDelegate();
            foreach (var member in this.memberMaps)
            {
                member.Map(fromObject, toObject);
            }

            return toObject;
        }
    }
}
