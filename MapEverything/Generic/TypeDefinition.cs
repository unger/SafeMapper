namespace MapEverything.Generic
{
    using System;
    using System.Linq.Expressions;

    public class TypeDefinition<T> : TypeDefinition
    {
        public TypeDefinition()
            : base(typeof(T))
        {
        }

        public Func<T, TProperty> GetPropertyGetter<TProperty>(string propertyName)
        {
            ParameterExpression paramExpression = Expression.Parameter(typeof(T), "value");
            Expression propertyGetterExpression = Expression.Property(paramExpression, propertyName);

            return Expression.Lambda<Func<T, TProperty>>(propertyGetterExpression, paramExpression).Compile();
        }
    }
}
