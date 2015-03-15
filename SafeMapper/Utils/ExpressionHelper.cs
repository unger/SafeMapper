namespace SafeMapper.Utils
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ExpressionHelper
    {
        public static MemberInfo GetMember<T, TParam>(Expression<Action<T, TParam>> expr)
        {
            if (expr.Body is MethodCallExpression)
            {
                var methodExpr = expr.Body as MethodCallExpression;

                return methodExpr.Method;
            }

            return null;
        }

        public static MemberInfo GetMember<T, TReturn>(Expression<Func<T, TReturn>> expr)
        {
            if (expr.Body is MemberExpression)
            {
                var memExpr = expr.Body as MemberExpression;

                return memExpr.Member;
            }

            if (expr.Body is MethodCallExpression)
            {
                var methodExpr = expr.Body as MethodCallExpression;

                return methodExpr.Method;
            }

            return null;
        }
    }
}
