using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
    public static class ExpressionLambda操作扩展
    {
        #region 替换Linq表达式参数
        /// <summary>
        /// 替换Linq表达式参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">Linq表达式</param>
        /// <param name="parameters">名称对应参数</param>
        /// <returns></returns>
        public static T ReplaceParameterExpression<T>(this T expression, params ReplaceParameter[] parameters) where T : Expression
        {
            if (expression == null) return null;
            if (expression is LambdaExpression)
            {
                var call = expression as LambdaExpression;
                return Expression.Lambda(ReplaceParameterExpression(call.Body, parameters), call.Parameters.Select(f =>
                {
                    var param = parameters.Where(ff => ff.Name.Contains(call.Name)).Select(ff => ff.Parameter).FirstOrDefault();
                    if (param?.Type == f.Type)
                    {
                        return param;
                    }
                    else
                    {
                        return f;
                    }
                })) as T;
            }
            else if (expression is ParameterExpression)
            {
                var call = expression as ParameterExpression;
                var param = parameters.Where(f => f.Name.Contains(call.Name)).Select(f => f.Parameter).FirstOrDefault();
                if (param?.Type == call.Type)
                {
                    return param as T;
                }
                else
                {
                    return call as T;
                }
            }
            else if (expression is BinaryExpression)
            {
                var call = expression as BinaryExpression;
                return call.Update(ReplaceParameterExpression(call.Left, parameters), ReplaceParameterExpression(call.Conversion, parameters), ReplaceParameterExpression(call.Right, parameters)) as T;
            }
            else if (expression is BlockExpression)
            {
                var call = expression as BlockExpression;
                return call.Update(call.Variables.Select(f => ReplaceParameterExpression(f, parameters)), call.Expressions.Select(f => ReplaceParameterExpression(f, parameters))) as T;
            }
            else if (expression is ConditionalExpression)
            {
                var call = expression as ConditionalExpression;
                return call.Update(ReplaceParameterExpression(call.Test, parameters), ReplaceParameterExpression(call.IfTrue, parameters), ReplaceParameterExpression(call.IfFalse, parameters)) as T;
            }
            else if (expression is ConstantExpression)
            {
                var call = expression as ConstantExpression;
                return expression;
            }
            else if (expression is DebugInfoExpression)
            {
                var call = expression as DebugInfoExpression;
                return expression;
            }
            else if (expression is DefaultExpression)
            {
                var call = expression as DefaultExpression;
                return expression;
            }
            else if (expression is DynamicExpression)
            {
                var call = expression as DynamicExpression;
                return call.Update(call.Arguments.Select(f => ReplaceParameterExpression(f, parameters))) as T;
            }
            else if (expression is GotoExpression)
            {
                var call = expression as GotoExpression;
                return call.Update(call.Target, ReplaceParameterExpression(call.Value, parameters)) as T;
            }
            else if (expression is IndexExpression)
            {
                var call = expression as IndexExpression;
                return call.Update(ReplaceParameterExpression(call.Object, parameters), call.Arguments.Select(f => ReplaceParameterExpression(f, parameters))) as T;
            }
            else if (expression is InvocationExpression)
            {
                var call = expression as InvocationExpression;
                return call.Update(ReplaceParameterExpression(call.Expression, parameters), call.Arguments.Select(f => ReplaceParameterExpression(f, parameters))) as T;
            }
            else if (expression is LabelExpression)
            {
                var call = expression as LabelExpression;
                return call.Update(call.Target, ReplaceParameterExpression(call.DefaultValue, parameters)) as T;
            }
            else if (expression is ListInitExpression)
            {
                var call = expression as ListInitExpression;
                return call.Update(ReplaceParameterExpression(call.NewExpression, parameters), call.Initializers) as T;
            }
            else if (expression is LoopExpression)
            {
                var call = expression as LoopExpression;
                return call.Update(call.BreakLabel, call.ContinueLabel, ReplaceParameterExpression(call.Body, parameters)) as T;
            }
            else if (expression is MemberExpression)
            {
                var call = expression as MemberExpression;
                return call.Update(ReplaceParameterExpression(call.Expression, parameters)) as T;
            }
            else if (expression is MemberInitExpression)
            {
                var call = expression as MemberInitExpression;
                return call.Update(ReplaceParameterExpression(call.NewExpression, parameters), call.Bindings) as T;
            }
            else if (expression is MethodCallExpression)
            {
                var call = expression as MethodCallExpression;
                return call.Update(ReplaceParameterExpression(call.Object, parameters), call.Arguments.Select(f => ReplaceParameterExpression(f, parameters))) as T;
            }
            else if (expression is NewArrayExpression)
            {
                var call = expression as NewArrayExpression;
                return call.Update(call.Expressions.Select(f => ReplaceParameterExpression(f, parameters))) as T;
            }
            else if (expression is NewExpression)
            {
                var call = expression as NewExpression;
                return call.Update(call.Arguments.Select(f => ReplaceParameterExpression(f, parameters))) as T;
            }
            else if (expression is RuntimeVariablesExpression)
            {
                var call = expression as RuntimeVariablesExpression;
                return call.Update(call.Variables.Select(f => ReplaceParameterExpression(f, parameters))) as T;
            }
            else if (expression is SwitchExpression)
            {
                var call = expression as SwitchExpression;
                return call.Update(ReplaceParameterExpression(call.SwitchValue, parameters), call.Cases, ReplaceParameterExpression(call.DefaultBody, parameters)) as T;
            }
            else if (expression is TryExpression)
            {
                var call = expression as TryExpression;
                return call.Update(ReplaceParameterExpression(call.Body, parameters), call.Handlers, ReplaceParameterExpression(call.Finally, parameters), ReplaceParameterExpression(call.Fault, parameters)) as T;
            }
            else if (expression is TypeBinaryExpression)
            {
                var call = expression as TypeBinaryExpression;
                return call.Update(ReplaceParameterExpression(call.Expression, parameters)) as T;
            }
            else if (expression is UnaryExpression)
            {
                var call = expression as UnaryExpression;
                return call.Update(ReplaceParameterExpression(call.Operand, parameters)) as T;
            }

            throw new Exception("IsParameterExpression   " + expression.NodeType);
        }
        #endregion 替换Linq表达式参数
        #region 合并Where条件
        private class MergeWhereResult
        {
            public MergeWhereResult(Expression body, ParameterExpression[] parameters)
            {
                this.Body = body;
                this.Parameters = parameters;
            }
            public Expression Body { get; set; }
            public ParameterExpression[] Parameters { get; set; }

        }
        private class Eererere2
        {
            public Expression topExpression { get; set; }
            public List<MergeWhereResult> sdddddddd { get; set; } = new List<MergeWhereResult>();
        }

        public static Expression<Func<TSource, bool>> MergeWhere<TSource>(this IQueryable<TSource> source)
        {
            var sdfdg = MergeWhere(source.Expression, new Eererere2());
            return Expression.Lambda<Func<TSource, bool>>(sdfdg.Body, sdfdg.Parameters);
        }
        private static MergeWhereResult MergeWhere(this Expression expression, Eererere2 parameters)
        {
            if (expression == null) return null;
            if (expression is MethodCallExpression)
            {
                var call = expression as MethodCallExpression;
                if (call.Type.IsGenericType && call.Type.GetGenericTypeDefinition() == typeof(IQueryable<>) && call.Method.Name == nameof(Queryable.Where))
                {
                    if (parameters.topExpression == null)
                        parameters.topExpression = expression;

                    MergeWhere(call.Object, parameters);
                    var ssss = call.Arguments.Select(f => MergeWhere(f, parameters)).ToArray();

                    if (parameters.topExpression == expression)
                    {
                        var s2 = parameters.sdddddddd.SelectMany(f => f.Parameters).FirstOrDefault();
                        var predicate = parameters.sdddddddd.Select(f => f.Body.ReplaceParameterExpression(new ReplaceParameter(s2, new[] { f.Parameters.Single().Name }))).ToArray();

                        var ddddddd = predicate[0];
                        for (int i = 1; i < predicate.Length; i++)
                        {
                            ddddddd = Expression.AndAlso(ddddddd, predicate[i]);
                        }
                        return new MergeWhereResult(ddddddd, new[] { s2 });
                    }
                    return null;
                }
                else
                {
                    throw new Exception("dddddddddddddd");
                }
            }
            else if (expression is LambdaExpression)
            {
                var call = expression as LambdaExpression;
                parameters.sdddddddd.Add(new MergeWhereResult(call.Body, call.Parameters.ToArray()));
                return null;
            }
            else
            {
                var ssddfdfd = expression.GetType().GetProperties().Where(f => f.PropertyType.IsAssignableFrom(typeof(Expression))).ToArray();
                foreach (var item in ssddfdfd)
                {
                    var dddfdffdfdf = item.GetValue(expression) as Expression;
                    MergeWhere(dddfdffdfdf, parameters);
                }
                return null;
            }
            throw new Exception("IsParameterExpression   " + expression.NodeType);
        }
        #endregion 合并Where条件
    }
    /// <summary>
    /// 名称对应参数
    /// </summary>
    public class ReplaceParameter
    {
        //public ReplaceParameter() { }
        public ReplaceParameter(ParameterExpression parameter, params string[] name)
        {
            this.Name = name;
            this.Parameter = parameter;
        }
        /// <summary>
        /// 原名称
        /// </summary>
        public string[] Name { get; set; }
        /// <summary>
        /// 目标参数
        /// </summary>
        public ParameterExpression Parameter { get; set; }
    }
}
