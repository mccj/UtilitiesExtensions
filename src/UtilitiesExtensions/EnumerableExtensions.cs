using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                dataTable.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
            }
            foreach (T item in items)
            {
                object[] array2 = new object[properties.Length];
                for (int j = 0; j < properties.Length; j++)
                {
                    array2[j] = properties[j].GetValue(item, null);
                }
                dataTable.Rows.Add(array2);
            }
            return dataTable;
        }

        public static string ConcatWith<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }

        public static string ConcatWith<T>(this IEnumerable<T> source, string separator, string format, IFormatProvider provider = null) where T : IFormattable
        {
            return string.Join(separator, from value in source
                                          select value.ToString(format, provider));
        }

        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector, TFirst default1st, TSecond default2nd)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("selector");
            }
            return _Zip(first, second, resultSelector, default1st, default2nd);
        }

        private static IEnumerable<TResult> _Zip<TFirst, TSecond, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector, TFirst default1st, TSecond default2nd)
        {
            using (IEnumerator<TFirst> enumerator = first.GetEnumerator())
            {
                using (IEnumerator<TSecond> enumerator2 = second.GetEnumerator())
                {
                    while (true)
                    {
                        bool hasValue1st = enumerator.MoveNext();
                        bool hasValue2nd = enumerator2.MoveNext();
                        if (!hasValue1st && !hasValue2nd)
                        {
                            break;
                        }
                        TFirst value1st = hasValue1st ? enumerator.Current : default1st;
                        TSecond value2nd = hasValue2nd ? enumerator2.Current : default2nd;
                        yield return resultSelector(value1st, value2nd);
                    }
                }
            }
        }

        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<int, TFirst, TSecond, TResult> resultSelector, Func<int, TFirst> followingSelector1st, Func<int, TSecond> followingSelector2nd)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("selector");
            }
            if (followingSelector1st == null)
            {
                throw new ArgumentNullException("followingSelector1st");
            }
            if (followingSelector2nd == null)
            {
                throw new ArgumentNullException("followingSelector2nd");
            }
            return _Zip(first, second, resultSelector, followingSelector1st, followingSelector2nd);
        }

        private static IEnumerable<TResult> _Zip<TFirst, TSecond, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<int, TFirst, TSecond, TResult> resultSelector, Func<int, TFirst> followingSelector1st, Func<int, TSecond> followingSelector2nd)
        {
            using (IEnumerator<TFirst> e1st = first.GetEnumerator())
            {
                using (IEnumerator<TSecond> e2nd = second.GetEnumerator())
                {
                    int count = 0;
                    while (true)
                    {
                        bool hasValue1st = e1st.MoveNext();
                        bool hasValue2nd = e2nd.MoveNext();
                        if (!hasValue1st && !hasValue2nd)
                        {
                            break;
                        }
                        TFirst value1st;
                        TSecond value2nd;
                        if (!hasValue1st)
                        {
                            value1st = followingSelector1st(count);
                            value2nd = e2nd.Current;
                        }
                        else if (!hasValue2nd)
                        {
                            value1st = e1st.Current;
                            value2nd = followingSelector2nd(count);
                        }
                        else
                        {
                            value1st = e1st.Current;
                            value2nd = e2nd.Current;
                        }
                        yield return resultSelector(count, value1st, value2nd);
                        count++;
                    }
                }
            }
        }

        public static IEnumerable<T> Infinite<T>(T value)
        {
            while (true)
            {
                yield return value;
            }
        }
    }

}
