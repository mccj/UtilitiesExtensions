using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
    /// <summary>
    /// 集合扩展方法类
    /// </summary>
    public static class QueryableExtensions
    {
        #region IQueryable的扩展

        /// <summary>
        /// 根据第三方条件是否为真来决定是否执行指定条件的查询
        /// </summary>
        /// <param name="source"> 要查询的源 </param>
        /// <param name="predicate"> 查询条件 </param>
        /// <param name="condition"> 第三方条件 </param>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <returns> 查询的结果 </returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool condition)
        {
            source.CheckNotNull("source");
            predicate.CheckNotNull("predicate");

            return condition ? source.Where(predicate) : source;
        }

        ///// <summary>
        ///// 把<see cref="IQueryable{T}"/>集合按指定字段与排序方式进行排序
        ///// </summary>
        ///// <param name="source">要排序的数据集</param>
        ///// <param name="propertyName">排序属性名</param>
        ///// <param name="sortDirection">排序方向</param>
        ///// <typeparam name="T">动态类型</typeparam>
        ///// <returns>排序后的数据集</returns>
        //public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source,
        //    string propertyName,
        //    ListSortDirection sortDirection = ListSortDirection.Ascending)
        //{
        //    source.CheckNotNull("source");
        //    propertyName.CheckNotNullOrEmpty("propertyName");

        //    return CollectionPropertySorter<T>.OrderBy(source, propertyName, sortDirection);
        //}

        ///// <summary>
        ///// 把<see cref="IQueryable{T}"/>集合按指定字段排序条件进行排序
        ///// </summary>
        ///// <typeparam name="T">动态类型</typeparam>
        ///// <param name="source">要排序的数据集</param>
        ///// <param name="sortCondition">列表字段排序条件</param>
        ///// <returns></returns>
        //public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, SortCondition sortCondition)
        //{
        //    source.CheckNotNull("source");
        //    sortCondition.CheckNotNull("sortCondition");

        //    return source.OrderBy(sortCondition.SortField, sortCondition.ListSortDirection);
        //}

        ///// <summary>
        ///// 把<see cref="IQueryable{T}"/>集合按指定字段排序条件进行排序
        ///// </summary>
        ///// <typeparam name="T">动态类型</typeparam>
        ///// <param name="source">要排序的数据集</param>
        ///// <param name="sortCondition">列表字段排序条件</param>
        ///// <returns></returns>
        //public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, SortCondition<T> sortCondition)
        //{
        //    source.CheckNotNull("source");
        //    sortCondition.CheckNotNull("sortCondition");
        //    return source.OrderBy(sortCondition.SortField, sortCondition.ListSortDirection);
        //}

        ///// <summary>
        ///// 把<see cref="IOrderedQueryable{T}"/>集合继续按指定字段排序方式进行排序
        ///// </summary>
        ///// <typeparam name="T">动态类型</typeparam>
        ///// <param name="source">要排序的数据集</param>
        ///// <param name="propertyName">排序属性名</param>
        ///// <param name="sortDirection">排序方向</param>
        ///// <returns></returns>
        //public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source,
        //    string propertyName,
        //    ListSortDirection sortDirection = ListSortDirection.Ascending)
        //{
        //    source.CheckNotNull("source");
        //    propertyName.CheckNotNullOrEmpty("propertyName");

        //    return CollectionPropertySorter<T>.ThenBy(source, propertyName, sortDirection);
        //}

        ///// <summary>
        ///// 把<see cref="IOrderedQueryable{T}"/>集合继续指定字段排序方式进行排序
        ///// </summary>
        ///// <typeparam name="T">动态类型</typeparam>
        ///// <param name="source">要排序的数据集</param>
        ///// <param name="sortCondition">列表字段排序条件</param>
        ///// <returns></returns>
        //public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, SortCondition sortCondition)
        //{
        //    source.CheckNotNull("source");
        //    sortCondition.CheckNotNull("sortCondition");

        //    return source.ThenBy(sortCondition.SortField, sortCondition.ListSortDirection);
        //}

        /// <summary>
        /// 从指定<see cref="IQueryable{T}"/>集合中筛选指定键范围内的子数据集
        /// </summary>
        /// <typeparam name="TSource">集合元素类型</typeparam>
        /// <typeparam name="TKey">筛选键类型</typeparam>
        /// <param name="source">要筛选的数据源</param>
        /// <param name="keySelector">筛选键的范围表达式</param>
        /// <param name="start">筛选范围起始值</param>
        /// <param name="end">筛选范围结束值</param>
        /// <param name="startEqual">是否等于起始值</param>
        /// <param name="endEqual">是否等于结束集</param>
        /// <returns></returns>
        public static IQueryable<TSource> Between<TSource, TKey>(this IQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector,
            TKey start,
            TKey end,
            bool startEqual = false,
            bool endEqual = false) where TKey : IComparable<TKey>
        {
            Expression[] paramters = keySelector.Parameters.Cast<Expression>().ToArray();
            Expression key = Expression.Invoke(keySelector, paramters);
            Expression startBound = startEqual
                ? Expression.GreaterThanOrEqual(key, Expression.Constant(start))
                : Expression.GreaterThan(key, Expression.Constant(start));
            Expression endBound = endEqual
                ? Expression.LessThanOrEqual(key, Expression.Constant(end))
                : Expression.LessThan(key, Expression.Constant(end));
            Expression and = Expression.AndAlso(startBound, endBound);
            Expression<Func<TSource, bool>> lambda = Expression.Lambda<Func<TSource, bool>>(and, keySelector.Parameters);
            return source.Where(lambda);
        }
        /// <summary>
        /// 只读集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IList<T> ToReadOnlyList<T>(this IEnumerable<T> enumerable)
        {//AsReadOnly
            return new Collections.ObjectModel.ReadOnlyCollection<T>(enumerable.ToList());
        }
        public static T[] ToReadOnlyArray<T>(this IEnumerable<T> enumerable)
        {//AsReadOnly
            return new Collections.ObjectModel.ReadOnlyCollection<T>(enumerable.ToList()).ToArray();
        }
        #endregion


    }
}
