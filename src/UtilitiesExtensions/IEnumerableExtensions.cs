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
        #region IEnumerable����չ

        /// <summary>
        /// ������չ�����ֱ�ת�����ַ���������ָ���ķָ����νӣ�ƴ��һ���ַ������ء�Ĭ�Ϸָ���Ϊ����
        /// </summary>
        /// <param name="collection"> Ҫ����ļ��� </param>
        /// <param name="separator"> �ָ�����Ĭ��Ϊ���� </param>
        /// <returns> ƴ�Ӻ���ַ��� </returns>
        public static string ExpandAndToString<T>(this IEnumerable<T> collection, string separator = ",")
        {
            return collection.ExpandAndToString(t => t.ToString(), separator);
        }

        /// <summary>
        /// ѭ�����ϵ�ÿһ�����ί�������ַ��������غϲ�����ַ�����Ĭ�Ϸָ���Ϊ����
        /// </summary>
        /// <param name="collection">������ļ���</param>
        /// <param name="itemFormatFunc">�����������ת��ί��</param>
        /// <param name="separetor">�ָ�����Ĭ��Ϊ����</param>
        /// <typeparam name="T">��������</typeparam>
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
        /// �����Ƿ�Ϊ��
        /// </summary>
        /// <param name="collection"> Ҫ����ļ��� </param>
        /// <typeparam name="T"> ��̬���� </typeparam>
        /// <returns> Ϊ�շ���True����Ϊ�շ���False </returns>
        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            collection = collection as IList<T> ?? collection.ToList();
            return !collection.Any();
        }

        /// <summary>
        /// ���ݵ����������Ƿ�Ϊ���������Ƿ�ִ��ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="source"> Ҫ��ѯ��Դ </param>
        /// <param name="predicate"> ��ѯ���� </param>
        /// <param name="condition"> ���������� </param>
        /// <typeparam name="T"> ��̬���� </typeparam>
        /// <returns> ��ѯ�Ľ�� </returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool condition)
        {
            predicate.CheckNotNull("predicate");
            source = source as IList<T> ?? source.ToList();

            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// ����ָ���������ؼ����в��ظ���Ԫ��
        /// </summary>
        /// <typeparam name="T">��̬����</typeparam>
        /// <typeparam name="TKey">��̬ɸѡ��������</typeparam>
        /// <param name="source">Ҫ������Դ</param>
        /// <param name="keySelector">�ظ�����ɸѡ����</param>
        /// <returns>���ظ�Ԫ�صļ���</returns>
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
        ///// ��<see cref="IEnumerable{T}"/>���ϰ�ָ���ֶ�������ʽ��������
        ///// </summary>
        ///// <typeparam name="T">����������</typeparam>
        ///// <param name="source">Ҫ��������ݼ�</param>
        ///// <param name="propertyName">����������</param>
        ///// <param name="sortDirection">������</param>
        ///// <returns>���������ݼ�</returns>
        //public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source,
        //    string propertyName,
        //    ListSortDirection sortDirection = ListSortDirection.Ascending)
        //{
        //    propertyName.CheckNotNullOrEmpty("propertyName");
        //    return CollectionPropertySorter<T>.OrderBy(source, propertyName, sortDirection);
        //}

        ///// <summary>
        ///// ��<see cref="IEnumerable{T}"/>���ϰ�ָ���ֶ�����������������
        ///// </summary>
        ///// <typeparam name="T">��̬����</typeparam>
        ///// <param name="source">Ҫ��������ݼ�</param>
        ///// <param name="sortCondition">�б��ֶ���������</param>
        ///// <returns></returns>
        //public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, SortCondition sortCondition)
        //{
        //    sortCondition.CheckNotNull("sortCondition");
        //    return source.OrderBy(sortCondition.SortField, sortCondition.ListSortDirection);
        //}

        ///// <summary>
        ///// ��<see cref="IEnumerable{T}"/>���ϰ�ָ���ֶ�����������������
        ///// </summary>
        ///// <typeparam name="T">��̬����</typeparam>
        ///// <param name="source">Ҫ��������ݼ�</param>
        ///// <param name="sortCondition">�б��ֶ���������</param>
        ///// <returns></returns>
        //public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, SortCondition<T> sortCondition)
        //{
        //    sortCondition.CheckNotNull("sortCondition");
        //    return source.OrderBy(sortCondition.SortField, sortCondition.ListSortDirection);
        //}

        ///// <summary>
        ///// ��<see cref="IOrderedQueryable{T}"/>���ϼ�����ָ���ֶ�����ʽ��������
        ///// </summary>
        ///// <typeparam name="T">��̬����</typeparam>
        ///// <param name="source">Ҫ��������ݼ�</param>
        ///// <param name="propertyName">����������</param>
        ///// <param name="sortDirection">������</param>
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
        ///// ��<see cref="IOrderedEnumerable{T}"/>���ϼ���ָ���ֶ�����ʽ��������
        ///// </summary>
        ///// <typeparam name="T">��̬����</typeparam>
        ///// <param name="source">Ҫ��������ݼ�</param>
        ///// <param name="sortCondition">�б��ֶ���������</param>
        ///// <returns></returns>
        //public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> source, SortCondition sortCondition)
        //{
        //    source.CheckNotNull("source");
        //    sortCondition.CheckNotNull("sortCondition");

        //    return source.ThenBy(sortCondition.SortField, sortCondition.ListSortDirection);
        //}
        /// <summary>
        /// �����������С�
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
