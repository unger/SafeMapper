namespace MapEverything.TypeMaps
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Fasterflect;

    public class ClassTypeMap : ITypeMap
    {
        private readonly TypeDefinition fromTypeDef;

        private readonly TypeDefinition toTypeDef;

        private MemberMap[] properties;

        public ClassTypeMap(Type fromType, Type toType, IFormatProvider formatProvider, ITypeMapper typeMapper)
        {
            this.toTypeDef = typeMapper.GetTypeDefinition(toType);
            this.fromTypeDef = typeMapper.GetTypeDefinition(fromType);

            this.properties = this.CreateMemberMaps(formatProvider, typeMapper);

            this.Convert = this.CreateConvert(fromType, toType);
        }

        public Func<object, object> Convert { get; private set; }

        private MemberMap[] CreateMemberMaps(IFormatProvider formatProvider, ITypeMapper typeMapper)
        {
            var memberMaps = new List<MemberMap>();

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

            return memberMaps.ToArray();
        }

        private Func<object, object> CreateConvert(Type fromType, Type toType)
        {
            //return this.CreateCompiledExpression(fromType, toType);

            if (fromType.IsValueType && toType.IsValueType)
            {
                return this.ConvertStructToStruct;
            }

            if (fromType.IsValueType)
            {
                return this.ConvertStructToClass;
            }

            if (toType.IsValueType)
            {
                return this.ConvertClassToStruct;
            }

            return this.ConvertClassToClass;
        }

        private Func<object, object> CreateCompiledExpression(Type fromType, Type toType)
        {
            ParameterExpression fromObjectExpression = Expression.Parameter(typeof(object), "fromObject");

            LabelTarget returnTarget = Expression.Label(toType);
            var expressions = new List<Expression>();
            var toObjectVariable = Expression.Variable(toType, "toObject");

            expressions.Add(Expression.Assign(toObjectVariable, Expression.New(toType)));

            foreach (var key in this.fromTypeDef.MemberGetters.Keys)
            {
                if (this.toTypeDef.MemberSetters.ContainsKey(key))
                {
                    var fromMember = this.fromTypeDef.Members[key];
                    var toMember = this.toTypeDef.Members[key];


                    MemberExpression leftExpression = Expression.Property(toObjectVariable, toMember.Name);
                    MemberExpression rightExpression = Expression.Property(Expression.TypeAs(fromObjectExpression, fromType), fromMember.Name);

                    if (fromMember.Type() == toMember.Type())
                    {
                        expressions.Add(Expression.Assign(leftExpression, rightExpression));
                    }
                    else if (toMember.Type() == typeof(string))
                    {
                        var methodInfo = fromMember.Type().GetMethod(
                            "ToString",
                            new Type[0]);
                        var toStringCall = Expression.Call(rightExpression, methodInfo);

                        expressions.Add(Expression.Assign(leftExpression, toStringCall));
                    }
                    else
                    {
                        var methodInfo = typeof(Convert).GetMethod(
                            "ChangeType",
                            new[] { typeof(object), typeof(Type) });
                        var toTypeExpression = Expression.Constant(toMember.Type());
                        var conversionExpression = Expression.Call(methodInfo, rightExpression, toTypeExpression);

                        expressions.Add(Expression.Assign(leftExpression, conversionExpression));
                    }
                }
            }

            expressions.Add(Expression.Return(returnTarget, toObjectVariable));
            expressions.Add(Expression.Label(returnTarget, toObjectVariable));

            return Expression.Lambda<Func<object, object>>(
                Expression.Block(new[] { toObjectVariable }, expressions.ToArray()),
                new[] { fromObjectExpression }).Compile();
        }

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
