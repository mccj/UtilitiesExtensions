using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class LambdaExpression操作扩展
    {
        public static LambdaExpression OrElse(this LambdaExpression predicate1, LambdaExpression predicate2, params ReplaceParameter[] parameters)
        {
            return Expression.Lambda(Expression.OrElse(
                                  predicate1.ReplaceParameterExpression(parameters).Body,
                                  predicate2.ReplaceParameterExpression(parameters).Body
                              ), parameters.Select(f => f.Parameter));
        }
        public static LambdaExpression AndAlso(this LambdaExpression predicate1, LambdaExpression predicate2, params ReplaceParameter[] parameters)
        {
            return Expression.Lambda(Expression.AndAlso(
                                  predicate1.ReplaceParameterExpression(parameters).Body,
                                  predicate2.ReplaceParameterExpression(parameters).Body
                              ), parameters.Select(f => f.Parameter));
        }
        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>>[] predicate)
        {
            var ddddddd = predicate[0];
            for (int i = 1; i < predicate.Length; i++)
            {
                ddddddd = ddddddd.OrElse(predicate[i]);
            }
            return ddddddd;
        }
        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> predicate1, Expression<Func<T, bool>> predicate2)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(
                        predicate1.Body,
                        predicate2.ReplaceParameterExpression(new ReplaceParameter(predicate1.Parameters.FirstOrDefault(), new[] { predicate2.Parameters.FirstOrDefault()?.Name })).Body
                    ), predicate1.Parameters.FirstOrDefault());
        }
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>>[] predicate)
        {
            var ddddddd = predicate[0];
            for (int i = 1; i < predicate.Length; i++)
            {
                ddddddd = ddddddd.AndAlso(predicate[i]);
            }
            return ddddddd;
        }
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> predicate1, Expression<Func<T, bool>> predicate2)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(
                        predicate1.Body,
                        predicate2.ReplaceParameterExpression(new ReplaceParameter(predicate1.Parameters.FirstOrDefault(), new[] { predicate2.Parameters.FirstOrDefault()?.Name })).Body
                    ), predicate1.Parameters.FirstOrDefault());
        }
    }
}
