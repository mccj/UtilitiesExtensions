using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
    /// <summary>
    /// Expression表达式扩展操作类
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// 以特定的条件运行组合两个Expression表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型</typeparam>
        /// <param name="first">第一个Expression表达式</param>
        /// <param name="second">要组合的Expression表达式</param>
        /// <param name="merge">组合条件运算方式</param>
        /// <returns>组合后的表达式</returns>
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            Dictionary<ParameterExpression, ParameterExpression> map =
                first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            Expression secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// 以 Expression.AndAlso 组合两个Expression表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型</typeparam>
        /// <param name="first">第一个Expression表达式</param>
        /// <param name="second">要组合的Expression表达式</param>
        /// <returns>组合后的表达式</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        /// <summary>
        /// 以 Expression.OrElse 组合两个Expression表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型</typeparam>
        /// <param name="first">第一个Expression表达式</param>
        /// <param name="second">要组合的Expression表达式</param>
        /// <returns>组合后的表达式</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }


        //public static PropertyPath GetSimplePropertyAccess(this LambdaExpression propertyAccessExpression)
        //{
        //    DebugCheck.NotNull(propertyAccessExpression);
        //    Debug.Assert(propertyAccessExpression.Parameters.Count == 1);

        //    var propertyPath
        //        = propertyAccessExpression
        //            .Parameters
        //            .Single()
        //            .MatchSimplePropertyAccess(propertyAccessExpression.Body);

        //    if (propertyPath == null)
        //    {
        //        throw Error.InvalidPropertyExpression(propertyAccessExpression);
        //    }

        //    return propertyPath;
        //}

        //public static PropertyPath GetComplexPropertyAccess(this LambdaExpression propertyAccessExpression)
        //{
        //    DebugCheck.NotNull(propertyAccessExpression);
        //    Debug.Assert(propertyAccessExpression.Parameters.Count == 1);

        //    var propertyPath
        //        = propertyAccessExpression
        //            .Parameters
        //            .Single()
        //            .MatchComplexPropertyAccess(propertyAccessExpression.Body);

        //    if (propertyPath == null)
        //    {
        //        throw Error.InvalidComplexPropertyExpression(propertyAccessExpression);
        //    }

        //    return propertyPath;
        //}

        //public static IEnumerable<PropertyPath> GetSimplePropertyAccessList(this LambdaExpression propertyAccessExpression)
        //{
        //    DebugCheck.NotNull(propertyAccessExpression);
        //    Debug.Assert(propertyAccessExpression.Parameters.Count == 1);

        //    var propertyPaths
        //        = MatchPropertyAccessList(propertyAccessExpression, (p, e) => e.MatchSimplePropertyAccess(p));

        //    if (propertyPaths == null)
        //    {
        //        throw Error.InvalidPropertiesExpression(propertyAccessExpression);
        //    }

        //    return propertyPaths;
        //}

        //public static IEnumerable<PropertyPath> GetComplexPropertyAccessList(this LambdaExpression propertyAccessExpression)
        //{
        //    DebugCheck.NotNull(propertyAccessExpression);
        //    Debug.Assert(propertyAccessExpression.Parameters.Count == 1);

        //    var propertyPaths
        //        = MatchPropertyAccessList(propertyAccessExpression, (p, e) => e.MatchComplexPropertyAccess(p));

        //    if (propertyPaths == null)
        //    {
        //        throw Error.InvalidComplexPropertiesExpression(propertyAccessExpression);
        //    }

        //    return propertyPaths;
        //}

        //private static IEnumerable<PropertyPath> MatchPropertyAccessList(
        //    this LambdaExpression lambdaExpression, Func<Expression, Expression, PropertyPath> propertyMatcher)
        //{
        //    DebugCheck.NotNull(lambdaExpression);
        //    DebugCheck.NotNull(propertyMatcher);
        //    Debug.Assert(lambdaExpression.Body != null);

        //    var newExpression
        //        = RemoveConvert(lambdaExpression.Body) as NewExpression;

        //    if (newExpression != null)
        //    {
        //        var parameterExpression
        //            = lambdaExpression.Parameters.Single();

        //        var propertyPaths
        //            = newExpression.Arguments
        //                           .Select(a => propertyMatcher(a, parameterExpression))
        //                           .Where(p => p != null);

        //        if (propertyPaths.Count()
        //            == newExpression.Arguments.Count())
        //        {
        //            return newExpression.HasDefaultMembersOnly(propertyPaths) ? propertyPaths : null;
        //        }
        //    }

        //    var propertyPath = propertyMatcher(lambdaExpression.Body, lambdaExpression.Parameters.Single());

        //    return (propertyPath != null) ? new[] { propertyPath } : null;
        //}

        //private static bool HasDefaultMembersOnly(
        //    this NewExpression newExpression, IEnumerable<PropertyPath> propertyPaths)
        //{
        //    DebugCheck.NotNull(newExpression);
        //    DebugCheck.NotNull(propertyPaths);

        //    return !newExpression.Members
        //                         .Where(
        //                             (t, i) =>
        //                             !string.Equals(t.Name, propertyPaths.ElementAt(i).Last().Name, StringComparison.Ordinal))
        //                         .Any();
        //}

        //private static PropertyPath MatchSimplePropertyAccess(
        //    this Expression parameterExpression, Expression propertyAccessExpression)
        //{
        //    DebugCheck.NotNull(propertyAccessExpression);

        //    var propertyPath = MatchPropertyAccess(parameterExpression, propertyAccessExpression);

        //    return propertyPath != null && propertyPath.Count == 1 ? propertyPath : null;
        //}

        //private static PropertyPath MatchComplexPropertyAccess(
        //    this Expression parameterExpression, Expression propertyAccessExpression)
        //{
        //    DebugCheck.NotNull(propertyAccessExpression);

        //    var propertyPath = MatchPropertyAccess(parameterExpression, propertyAccessExpression);

        //    return propertyPath;
        //}

        //private static PropertyPath MatchPropertyAccess(
        //    this Expression parameterExpression, Expression propertyAccessExpression)
        //{
        //    DebugCheck.NotNull(parameterExpression);
        //    DebugCheck.NotNull(propertyAccessExpression);

        //    var propertyInfos = new List<PropertyInfo>();

        //    MemberExpression memberExpression;

        //    do
        //    {
        //        memberExpression = RemoveConvert(propertyAccessExpression) as MemberExpression;

        //        if (memberExpression == null)
        //        {
        //            return null;
        //        }

        //        var propertyInfo = memberExpression.Member as PropertyInfo;

        //        if (propertyInfo == null)
        //        {
        //            return null;
        //        }

        //        propertyInfos.Insert(0, propertyInfo);

        //        propertyAccessExpression = memberExpression.Expression;
        //    }
        //    while (memberExpression.Expression != parameterExpression);

        //    return new PropertyPath(propertyInfos);
        //}

        //public static Expression RemoveConvert(this Expression expression)
        //{
        //    UtilitiesExtensions.Utility.DebugCheck.NotNull(expression);

        //    while (expression.NodeType == ExpressionType.Convert
        //           || expression.NodeType == ExpressionType.ConvertChecked)
        //    {
        //        expression = ((UnaryExpression)expression).Operand;
        //    }

        //    return expression;
        //}

        //public static bool IsNullConstant(this Expression expression)
        //{
        //    // convert statements introduced by compiler should not affect nullness
        //    expression = expression.RemoveConvert();

        //    // check if the unwrapped expression is a null constant
        //    if (expression.NodeType != ExpressionType.Constant)
        //    {
        //        return false;
        //    }

        //    return ((ConstantExpression)expression).Value == null;
        //}

        public static bool IsStringAddExpression(this Expression expression)
        {
            var linq = expression as BinaryExpression;
            if (linq == null)
            {
                return false;
            }

            if (linq.Method == null || linq.NodeType != ExpressionType.Add)
            {
                return false;
            }

            return linq.Method.DeclaringType == typeof(string) &&
                   string.Equals(linq.Method.Name, "Concat", StringComparison.Ordinal);
        }

        private class ParameterRebinder : ExpressionVisitor
        {
            private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

            private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                ParameterExpression replacement;
                if (_map.TryGetValue(node, out replacement))
                {
                    node = replacement;
                }
                return base.VisitParameter(node);
            }
        }
    }
}
