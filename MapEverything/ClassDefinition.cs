namespace MapEverything
{
    using System;
    using System.Linq.Expressions;

    public class ClassDefinition<T> : ITypeDefinition
    {
        public ClassDefinition()
        {
            if (typeof(T).IsInterface)
            {

            }
            else
            {
                this.CreateObject = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
            }
        }

        public Func<T> CreateObject { get; private set; }
    }

    public interface ITypeDefinition
    {
    }
}
