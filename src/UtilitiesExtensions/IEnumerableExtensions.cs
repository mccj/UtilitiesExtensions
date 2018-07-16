namespace System.Linq
{
    using Orchard.Utility;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    [DebuggerStepThrough]
    public static class IEnumerableExtensions
    {
        public static string Uniquify(this IEnumerable<string> inputStrings, string targetString)
        {
            DebugCheck.NotNull(inputStrings);
            DebugCheck.NotEmpty(targetString);

            var uniqueString = targetString;
            var i = 0;

            while (inputStrings.Any(n => string.Equals(n, uniqueString, StringComparison.Ordinal)))
            {
                uniqueString = targetString + ++i;
            }

            return uniqueString;
        }
        public static IEnumerable<T[]> GroupByCount<T>(this IEnumerable<T> source, int count)
        {
            var i = 0;
            var sourcecount = source.Count();
            do
            {
                yield return source.Skip(i).Take(count).ToArray();
                i += count;
            } while (i < sourcecount);
        }
        //public static void Each<TEvents>(this IEnumerable<TEvents> events, Action<TEvents> dispatch, ILogger logger = null)
        //{
        //    DebugCheck.NotNull(events);
        //    DebugCheck.NotNull(dispatch);

        //    foreach (var sink in events)
        //    {
        //        try
        //        {
        //            dispatch(sink);
        //        }
        //        catch (Exception ex)
        //        {
        //            if (IsLogged(ex))
        //            {
        //                logger?.Error(ex, "{2} thrown from {0} by {1}",
        //                    typeof(TEvents).Name,
        //                    sink.GetType().FullName,
        //                    ex.GetType().Name);
        //            }

        //            if (ex.IsFatal())
        //            {
        //                throw;
        //            }
        //        }
        //    }
        //}
        //public static void Each<TEvents>(this IEnumerable<TEvents> events, Action<TEvents, int> dispatch, ILogger logger = null)
        //{
        //    DebugCheck.NotNull(events);
        //    DebugCheck.NotNull(dispatch);
        //    int i = 0;
        //    foreach (var sink in events)
        //    {
        //        try
        //        {
        //            dispatch(sink, i++);
        //        }
        //        catch (Exception ex)
        //        {
        //            if (IsLogged(ex))
        //            {
        //                logger?.Error(ex, "{2} thrown from {0} by {1}",
        //                    typeof(TEvents).Name,
        //                    sink.GetType().FullName,
        //                    ex.GetType().Name);
        //            }

        //            if (ex.IsFatal())
        //            {
        //                throw;
        //            }
        //        }
        //    }
        //}
        //public static IEnumerable<TResult> Each<TEvents, TResult>(this IEnumerable<TEvents> events, Func<TEvents, TResult> dispatch, ILogger logger = null)
        //{
        //    DebugCheck.NotNull(events);
        //    DebugCheck.NotNull(dispatch);

        //    foreach (var sink in events)
        //    {
        //        TResult result = default(TResult);
        //        try
        //        {
        //            result = dispatch(sink);
        //        }
        //        catch (Exception ex)
        //        {
        //            if (IsLogged(ex))
        //            {
        //                logger?.Error(ex, "{2} thrown from {0} by {1}",
        //                    typeof(TEvents).Name,
        //                    sink.GetType().FullName,
        //                    ex.GetType().Name);
        //            }

        //            if (ex.IsFatal())
        //            {
        //                throw;
        //            }
        //        }

        //        yield return result;
        //    }
        //}
        //public static IEnumerable<TResult> Each<TEvents, TResult>(this IEnumerable<TEvents> events, Func<TEvents, int, TResult> dispatch, ILogger logger = null)
        //{
        //    DebugCheck.NotNull(events);
        //    DebugCheck.NotNull(dispatch);

        //    foreach (var sink in events)
        //    {
        //        int i = 0;
        //        TResult result = default(TResult);
        //        try
        //        {
        //            result = dispatch(sink, i++);
        //        }
        //        catch (Exception ex)
        //        {
        //            if (IsLogged(ex))
        //            {
        //                logger?.Error(ex, "{2} thrown from {0} by {1}",
        //                    typeof(TEvents).Name,
        //                    sink.GetType().FullName,
        //                    ex.GetType().Name);
        //            }

        //            if (ex.IsFatal())
        //            {
        //                throw;
        //            }
        //        }

        //        yield return result;
        //    }
        //}
        //private static bool IsLogged(Exception ex)
        //{
        //    return ex is Orchard.Security.OrchardSecurityException || !ex.IsFatal();
        //}


        public static string Join<T>(this IEnumerable<T> ts, Func<T, string> selector = null, string separator = ", ")
        {
            DebugCheck.NotNull(ts);

            selector = selector ?? (t => t.ToString());

            return string.Join(separator, ts.Where(t => !ReferenceEquals(t, null)).Select(selector));
        }

        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            DebugCheck.NotNull(source);

            yield return value;

            foreach (var element in source)
            {
                yield return element;
            }
        }

        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            DebugCheck.NotNull(source);

            foreach (var element in source)
            {
                yield return element;
            }

            yield return value;
        }
        #region IEnumerable的扩展

        /// <summary>
        /// 将集合展开并分别转换成字符串，再以指定的分隔符衔接，拼成一个字符串返回。默认分隔符为逗号
        /// </summary>
        /// <param name="collection"> 要处理的集合 </param>
        /// <param name="separator"> 分隔符，默认为逗号 </param>
        /// <returns> 拼接后的字符串 </returns>
        public static string ExpandAndToString<T>(this IEnumerable<T> collection, string separator = ",")
        {
            return collection.ExpandAndToString(t => t.ToString(), separator);
        }

        /// <summary>
        /// 循环集合的每一项，调用委托生成字符串，返回合并后的字符串。默认分隔符为逗号
        /// </summary>
        /// <param name="collection">待处理的集合</param>
        /// <param name="itemFormatFunc">单个集合项的转换委托</param>
        /// <param name="separetor">分隔符，默认为逗号</param>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <returns></returns>
        public static string ExpandAndToString<T>(this IEnumerable<T> collection, Func<T, string> itemFormatFunc, string separetor = ",")
        {
            collection = collection as IList<T> ?? collection.ToList();
            itemFormatFunc.CheckNotNull("itemFormatFunc");
            if (!collection.Any())
            {
                return null;
            }
            var sb = new Text.StringBuilder();
            int i = 0;
            int count = collection.Count();
            foreach (T t in collection)
            {
                if (i == count - 1)
                {
                    sb.Append(itemFormatFunc(t));
                }
                else
                {
                    sb.Append(itemFormatFunc(t) + separetor);
                }
                i++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 集合是否为空
        /// </summary>
        /// <param name="collection"> 要处理的集合 </param>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <returns> 为空返回True，不为空返回False </returns>
        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            collection = collection as IList<T> ?? collection.ToList();
            return !collection.Any();
        }

        /// <summary>
        /// 根据第三方条件是否为真来决定是否执行指定条件的查询
        /// </summary>
        /// <param name="source"> 要查询的源 </param>
        /// <param name="predicate"> 查询条件 </param>
        /// <param name="condition"> 第三方条件 </param>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <returns> 查询的结果 </returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool condition)
        {
            predicate.CheckNotNull("predicate");
            source = source as IList<T> ?? source.ToList();

            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// 根据指定条件返回集合中不重复的元素
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <typeparam name="TKey">动态筛选条件类型</typeparam>
        /// <param name="source">要操作的源</param>
        /// <param name="keySelector">重复数据筛选条件</param>
        /// <returns>不重复元素的集合</returns>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            keySelector.CheckNotNull("keySelector");
            source = source as IList<T> ?? source.ToList();

            return source.GroupBy(keySelector).Select(group => group.First());
        }
        //public static IEnumerable<T> IntersectBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        //{
        //    keySelector.CheckNotNull("keySelector");
        //    source = source as IList<T> ?? source.ToList();
        //    source.Select(f=>keySelector(f)).d
        //    return source.GroupBy(keySelector).Select(group => group.First());
        //}

        ///// <summary>
        ///// 把<see cref="IEnumerable{T}"/>集合按指定字段与排序方式进行排序
        ///// </summary>
        ///// <typeparam name="T">集合项类型</typeparam>
        ///// <param name="source">要排序的数据集</param>
        ///// <param name="propertyName">排序属性名</param>
        ///// <param name="sortDirection">排序方向</param>
        ///// <returns>排序后的数据集</returns>
        //public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source,
        //    string propertyName,
        //    ListSortDirection sortDirection = ListSortDirection.Ascending)
        //{
        //    propertyName.CheckNotNullOrEmpty("propertyName");
        //    return CollectionPropertySorter<T>.OrderBy(source, propertyName, sortDirection);
        //}

        ///// <summary>
        ///// 把<see cref="IEnumerable{T}"/>集合按指定字段排序条件进行排序
        ///// </summary>
        ///// <typeparam name="T">动态类型</typeparam>
        ///// <param name="source">要排序的数据集</param>
        ///// <param name="sortCondition">列表字段排序条件</param>
        ///// <returns></returns>
        //public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, SortCondition sortCondition)
        //{
        //    sortCondition.CheckNotNull("sortCondition");
        //    return source.OrderBy(sortCondition.SortField, sortCondition.ListSortDirection);
        //}

        ///// <summary>
        ///// 把<see cref="IEnumerable{T}"/>集合按指定字段排序条件进行排序
        ///// </summary>
        ///// <typeparam name="T">动态类型</typeparam>
        ///// <param name="source">要排序的数据集</param>
        ///// <param name="sortCondition">列表字段排序条件</param>
        ///// <returns></returns>
        //public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, SortCondition<T> sortCondition)
        //{
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
        //public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> source,
        //    string propertyName,
        //    ListSortDirection sortDirection = ListSortDirection.Ascending)
        //{
        //    source.CheckNotNull("source");
        //    propertyName.CheckNotNullOrEmpty("propertyName");

        //    return CollectionPropertySorter<T>.ThenBy(source, propertyName, sortDirection);
        //}

        ///// <summary>
        ///// 把<see cref="IOrderedEnumerable{T}"/>集合继续指定字段排序方式进行排序
        ///// </summary>
        ///// <typeparam name="T">动态类型</typeparam>
        ///// <param name="source">要排序的数据集</param>
        ///// <param name="sortCondition">列表字段排序条件</param>
        ///// <returns></returns>
        //public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> source, SortCondition sortCondition)
        //{
        //    source.CheckNotNull("source");
        //    sortCondition.CheckNotNull("sortCondition");

        //    return source.ThenBy(sortCondition.SortField, sortCondition.ListSortDirection);
        //}
        /// <summary>
        /// 连接两个序列。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T selector)
        {
            return source.Concat(new T[] { selector });
        }
        #endregion

        public static TSource GetAt<TSource>(this IEnumerable<TSource> source, int index, TSource defaultValue = default(TSource))
        {
            var value = source.Skip(index).Take(1).FirstOrDefault();
            if (value == null)
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

    }
}
