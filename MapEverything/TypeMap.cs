namespace MapEverything
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Fasterflect;

    public class TypeMap
    {
        private readonly TypeDefinition fromTypeDef;

        private readonly TypeDefinition toTypeDef;

        private readonly ITypeMapper typeMapper;

        private Func<object, object> elementConverter;

        private Dictionary<string, IMemberMap> memberMaps = new Dictionary<string, IMemberMap>();

        public TypeMap(TypeDefinition fromTypeDef, TypeDefinition toTypeDef, ITypeMapper typeMapper)
        {
            this.fromTypeDef = fromTypeDef;
            this.toTypeDef = toTypeDef;
            this.typeMapper = typeMapper;

            this.Convert = this.GenerateConvertDelegate();

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

        public Func<object, object> Convert { get; private set; }

        private Func<object, object> GenerateConvertDelegate()
        {
            if (this.fromTypeDef.IsCollection && this.toTypeDef.IsCollection)
            {
                var fromElementType = this.fromTypeDef.ElementType;
                var toElementType = this.toTypeDef.ElementType;
                this.elementConverter = this.typeMapper.GetConverter(fromElementType, toElementType);

                if (this.toTypeDef.ActualType.IsArray)
                {
                    return this.ConvertArray;
                }

                if (this.toTypeDef.ActualType.IsGenericType)
                {
                    return this.ConvertGenericCollection;
                }
            }

            return this.ConvertClass;
        }

        private object ConvertClass(object fromObject)
        {
            var toObject = this.toTypeDef.CreateInstanceDelegate();
            foreach (var key in this.memberMaps.Keys)
            {
                this.memberMaps[key].Map(fromObject, toObject);
            }

            return toObject;
        }

        private object ConvertGenericCollection(object fromObject)
        {
            var toAddDelegate = this.toTypeDef.AddElementDelegate;

            if (this.fromTypeDef.ActualType.IsArray)
            {
                var values = (Array)fromObject;
                var newElements = this.toTypeDef.CreateInstanceDelegate();

                for (int i = 0; i < values.Length; i++)
                {
                    toAddDelegate(
                        newElements,
                        this.elementConverter(values.GetElement(i)));
                }

                return newElements;
            }

            var collection = fromObject as ICollection;
            if (collection != null)
            {
                var newElements = this.toTypeDef.CreateInstanceDelegate();

                foreach (var elementValue in collection)
                {
                    toAddDelegate(
                        newElements,
                        this.elementConverter(elementValue));
                }

                return newElements;
            }

            return this.toTypeDef.CreateInstanceDelegate();
        }

        private object ConvertArray(object fromObject)
        {
            if (this.fromTypeDef.ActualType.IsArray)
            {
                var array = (IList)fromObject;
                var newArray = (IList)Array.CreateInstance(this.toTypeDef.ElementType ?? typeof(object), array.Count);
                for (int i = 0; i < array.Count; i++)
                {
                    newArray[i] = this.elementConverter(array[i]);
                }

                return newArray;
            }

            var collection = fromObject as ICollection;
            if (collection != null)
            {
                var newElements = Array.CreateInstance(this.toTypeDef.ElementType ?? typeof(object), collection.Count);

                var i = 0;
                foreach (var elementValue in collection)
                {
                    newElements.SetValue(this.elementConverter(elementValue), i);
                    i++;
                }

                return newElements;
            }

            return Array.CreateInstance(this.toTypeDef.ElementType ?? typeof(object), 0);
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
