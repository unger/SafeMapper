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

            if (fromType.IsValueType && toType.IsValueType)
            {
                this.Convert = this.ConvertStructToStruct;
            }
            else if (fromType.IsValueType)
            {
                this.Convert = this.ConvertStructToClass;
            }
            else if (toType.IsValueType)
            {
                this.Convert = this.ConvertClassToStruct;
            }
            else
            {
                this.Convert = this.ConvertClassToClass;
            }

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

        private object ConvertClassToClass(object fromObject)
        {
            var toObject = this.toTypeDef.CreateInstanceDelegate();
            for (int i = 0; i < this.properties.Length; i++)
            {
                this.properties[i].Map(fromObject, toObject);
            }

            return toObject;
        }

        private object ConvertClassToStruct(object fromObject)
        {
            var toObject = this.toTypeDef.CreateInstanceDelegate().WrapIfValueType();
            for (int i = 0; i < this.properties.Length; i++)
            {
                this.properties[i].Map(fromObject, toObject);
            }

            return toObject.UnwrapIfWrapped();
        }

        private object ConvertStructToStruct(object fromObject)
        {
            var toObject = this.toTypeDef.CreateInstanceDelegate().WrapIfValueType();
            for (int i = 0; i < this.properties.Length; i++)
            {
                this.properties[i].Map(fromObject.WrapIfValueType(), toObject);
            }

            return toObject.UnwrapIfWrapped();
        }

        private object ConvertStructToClass(object fromObject)
        {
            var toObject = this.toTypeDef.CreateInstanceDelegate();
            for (int i = 0; i < this.properties.Length; i++)
            {
                this.properties[i].Map(fromObject.WrapIfValueType(), toObject);
            }

            return toObject;
        }
    }
}
