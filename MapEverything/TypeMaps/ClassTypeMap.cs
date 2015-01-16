namespace MapEverything.TypeMaps
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Fasterflect;

    public class ClassTypeMap : ITypeMap
    {
        private readonly TypeDefinition fromTypeDef;

        private readonly TypeDefinition toTypeDef;

        private MemberMap[] properties;
        
        private Action<object, object>[] propertyMappers;

        public ClassTypeMap(Type fromType, Type toType, IFormatProvider formatProvider, ITypeMapper typeMapper)
        {
            /*
            var propertyMapperList = new List<Action<object, object>>();
            this.toTypeDef = typeMapper.GetTypeDefinition(toType);
            this.fromTypeDef = typeMapper.GetTypeDefinition(fromType);

            this.Convert = this.ConvertClass2;
            var members = this.toTypeDef.ActualType.GetMembers();
            for (int i = 0; i < members.Length; i++)
            {
                var toPropInfo = members[i] as PropertyInfo;
                if (toPropInfo != null)
                {
                    if (toPropInfo.CanWrite)
                    {
                        var fromPropInfo = this.fromTypeDef.ActualType.GetProperty(toPropInfo.Name);
                        if (fromPropInfo != null)
                        {
                            if (fromPropInfo.CanRead)
                            {
                                var propertyConverter = typeMapper.GetConverter(
                                    fromPropInfo.PropertyType,
                                    toPropInfo.PropertyType,
                                    formatProvider);
                                var getter = fromPropInfo.DelegateForGetPropertyValue();
                                var setter = toPropInfo.DelegateForSetPropertyValue();

                                if (this.fromTypeDef.ActualType.IsValueType && this.toTypeDef.ActualType.IsValueType)
                                {
                                    propertyMapperList.Add((from, to) => setter(to.WrapIfValueType(), propertyConverter(getter(from.WrapIfValueType()))));
                                }
                                else if (this.fromTypeDef.ActualType.IsValueType)
                                {
                                    propertyMapperList.Add((from, to) => setter(to, propertyConverter(getter(from.WrapIfValueType()))));
                                }
                                else if (this.toTypeDef.ActualType.IsValueType)
                                {
                                    propertyMapperList.Add((from, to) => setter(to.WrapIfValueType(), propertyConverter(getter(from))));
                                }
                                else
                                {
                                    propertyMapperList.Add((from, to) => setter(to, propertyConverter(getter(from))));
                                }
                            }
                        }
                    }
                }
            }

            this.propertyMappers = propertyMapperList.ToArray();
            */
            
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

        private object ConvertClass2(object fromObject)
        {
            var toObject = this.toTypeDef.CreateInstanceDelegate();
            for (int i = 0; i < this.propertyMappers.Length; i++)
            {
                this.propertyMappers[i](fromObject, toObject);
            }

            return toObject;
        }
    }
}
