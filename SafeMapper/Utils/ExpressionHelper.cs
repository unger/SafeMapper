namespace SafeMapper.Utils
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ExpressionHelper
    {
        public static MemberInfo GetMember<T, TParam>(Expression<Action<T, TParam>> expr)
        {
            return GetMember(expr.Body);
        }

        public static MemberInfo GetMember<T, TReturn>(Expression<Func<T, TReturn>> expr)
        {
            return GetMember(expr.Body);
        }

        public static MemberInfo GetMember<T, TParam>(Expression<Action<T, string, TParam>> expr)
        {
            return GetMember(expr.Body);
        }

        public static MemberInfo GetMember<T, TReturn>(Expression<Func<T, string, TReturn>> expr)
        {
            return GetMember(expr.Body);
        }

        private static MemberInfo GetMember(Expression expr)
        {
            if (expr is MemberExpression)
            {
                var memExpr = expr as MemberExpression;

                return memExpr.Member;
            }

            if (expr is MethodCallExpression)
            {
                var methodExpr = expr as MethodCallExpression;

                return methodExpr.Method;
            }

            return null;
        }
    }
}
