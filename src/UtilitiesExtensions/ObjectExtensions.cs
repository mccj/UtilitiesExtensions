using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.ComponentModel;
using System.Collections;
using System.Reflection;

namespace System.Linq
{
    /// <summary>
    /// 基类型<see cref="Object"/>扩展辅助操作类
    /// </summary>
    public static class ObjectExtensions
    {

        #region 公共方法

        /// <summary>
        /// 把对象类型转换为指定类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object CastTo(this object value, Type conversionType)
        {
            if (value == null)
            {
                return null;
            }
            if (conversionType.IsNullableType())
            {
                conversionType = conversionType.GetUnNullableType();
            }
            if (conversionType.IsEnum)
            {
                //return Enum.Parse(conversionType, value.ToString());
                if (value is string)
                    return Enum.Parse(conversionType, value as string);
                else
                    return Enum.ToObject(conversionType, value);
            }
            if (conversionType.IsAnonymousType())
            {
                var bindingFlags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance;
                System.Reflection.PropertyInfo[] fromPropertyInfo = value.GetType().GetProperties(bindingFlags);
                var innerValue = Activator.CreateInstance(conversionType, conversionType.GetConstructors().First().GetParameters().Select(f => fromPropertyInfo.FirstOrDefault(ff => ff.Name == f.Name)?.GetValue(value)).ToArray());
                return innerValue;
            }

            if (!conversionType.IsInterface && conversionType.IsGenericType)
            {
                var bindingFlags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance;
                System.Reflection.PropertyInfo[] fromPropertyInfo = value.GetType().GetProperties(bindingFlags);
                System.Reflection.PropertyInfo[] toPropertyInfo = conversionType.GetProperties(bindingFlags);
                var s12 = conversionType.GetConstructors().FirstOrDefault()?.GetParameters();
                if (s12.All(f => fromPropertyInfo.Any(ff => ff.Name.Equals(f.Name, StringComparison.OrdinalIgnoreCase))))
                {
                    var ss = s12.Select(f => fromPropertyInfo.FirstOrDefault(ff => ff.Name.Equals(f.Name, StringComparison.OrdinalIgnoreCase))?.GetValue(value)).ToArray();
                    return Activator.CreateInstance(conversionType, ss);
                }
                var innerValue = Activator.CreateInstance(conversionType, new object[conversionType.GetConstructors().First().GetParameters().Count()]);
                foreach (var item in toPropertyInfo)
                {
                    var property = fromPropertyInfo.FirstOrDefault(f => f.Name == item.Name);
                    if (item.CanWrite && property != null && property.CanRead)
                        item.SetValue(innerValue, CastTo(property.GetValue(value), item.PropertyType));
                }
                return innerValue;
            }
            if (conversionType == typeof(Guid)) return Guid.Parse(value.ToString());
            if (conversionType == typeof(Version)) return new Version(value.ToString());
            if (value is IConvertible) return Convert.ChangeType(value, conversionType);
            return value;
        }

        /// <summary>
        /// 把对象类型转化为指定类型
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <returns> 转化后的指定类型的对象，转化失败引发异常。 </returns>
        public static T CastTo<T>(this object value)
        {
            object result = CastTo(value, typeof(T));
            return (T)result;
        }

        /// <summary>
        /// 把对象类型转化为指定类型，转化失败时返回指定的默认值
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <param name="defaultValue"> 转化失败返回的指定默认值 </param>
        /// <returns> 转化后的指定类型对象，转化失败时返回指定的默认值 </returns>
        public static T CastTo<T>(this object value, T defaultValue)
        {
            try
            {
                return CastTo<T>(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 判断当前值是否介于指定范围内
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 动态类型对象 </param>
        /// <param name="start"> 范围起点 </param>
        /// <param name="end"> 范围终点 </param>
        /// <param name="leftEqual"> 是否可等于上限（默认等于） </param>
        /// <param name="rightEqual"> 是否可等于下限（默认等于） </param>
        /// <returns> 是否介于 </returns>
        public static bool IsBetween<T>(this IComparable<T> value, T start, T end, bool leftEqual = false, bool rightEqual = false) where T : IComparable
        {
            bool flag = leftEqual ? value.CompareTo(start) >= 0 : value.CompareTo(start) > 0;
            return flag && (rightEqual ? value.CompareTo(end) <= 0 : value.CompareTo(end) < 0);
        }

        ///// <summary>
        ///// 将对象序列化为JSON字符串，不支持存在循环引用的对象
        ///// </summary>
        ///// <typeparam name="T">动态类型</typeparam>
        ///// <param name="value">动态类型对象</param>
        ///// <returns>JSON字符串</returns>
        //public static string ToJson<T>(this T value)
        //{
        //    //return System.Web.Helpers.Json.Encode(value);
        //    return Orchard.Utility.Json.Encode(value);
        //}
        //public static T MergerExpression<T>(this T source, object obj, bool 只映射值类型 = false, Type[] 排除类型 = null, object[] 空不赋值 = null, params Expression<Func<T, object>>[] excludeExpressions) where T : class
        //{
        //    return source.Merger(obj, 只映射值类型, 排除类型, 空不赋值, excludeExpressions.Select(f => ExpressionHelper2.GetExpressionText(f)).ToArray());
        //}
        //public static T MergerExpression<T>(this T source, object obj, object[] 空不赋值 = null, params Expression<Func<T, object>>[] excludeExpressions) where T : class
        //{
        //    return source.Merger(obj, false, null, 空不赋值, excludeExpressions.Select(f => ExpressionHelper2.GetExpressionText(f)).ToArray());
        //}
        //public static T MergerExpression<T>(this T source, object obj, bool bool空不赋值 = true, params Expression<Func<T, object>>[] excludeExpressions) where T : class
        //{
        //    return source.Merger(obj, false, null, bool空不赋值 ? new object[] { null } : new object[] { }, excludeExpressions.Select(f => ExpressionHelper2.GetExpressionText(f)).ToArray());
        //}
        //public static T MergerExpression<T>(this T source, object obj, params Expression<Func<T, object>>[] excludeExpressions) where T : class
        //{
        //    return source.Merger(obj, true, excludeExpressions.Select(f => ExpressionHelper2.GetExpressionText(f)).ToArray());
        //}

        //public static T Merger<T>(this T source, object obj, bool 只映射值类型 = false, Type[] 排除类型 = null, object[] 空不赋值 = null, params string[] excludeNames) where T : class
        //{
        //    if (source == null) return source;
        //    //TODO:数组合并有问题
        //    if (source is IEnumerable) { return obj as T; }
        //    excludeNames = excludeNames.Where(f => !string.IsNullOrWhiteSpace(f)).ToArray();
        //    var fromType = source.GetType();
        //    var toType = obj.GetType();
        //    var expando = new Dictionary<string, object>();

        //    var bindingFlags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance;
        //    System.Reflection.PropertyInfo[] fromPropertyInfo = fromType.GetProperties(bindingFlags);
        //    System.Reflection.PropertyInfo[] toPropertyInfo = toType.GetProperties(bindingFlags);
        //    foreach (var property in fromPropertyInfo)
        //    {
        //        if (toType.IsGenericType)
        //        {
        //            expando.Add(property.Name, property.GetValue(source));
        //        }


        //        if (!property.CanWrite || (excludeNames != null && excludeNames.Any(f => f.TrimStart('.') == property.Name))) continue;
        //        var subExcludeNames = excludeNames
        //            .Where(f => f.StartsWith(property.Name + ".") || (f.StartsWith(".") && f.IndexOf(".", 1) == -1))
        //            .Select(f => (f.StartsWith(".") && f.IndexOf(".", 1) == -1) ? f : f.Remove(0, property.Name.Length + 1)).ToArray();
        //        var itemPropertyInfo = toPropertyInfo.FirstOrDefault(f => f.Name == property.Name);
        //        if (itemPropertyInfo != null)
        //        {
        //            if (排除类型 != null)
        //                if (排除类型.Any(f => f.IsAssignableFrom(itemPropertyInfo.PropertyType)))
        //                    continue;

        //            var value = itemPropertyInfo.GetValue(obj);
        //            if (!(空不赋值 != null && 空不赋值.Contains(value)))
        //                if (itemPropertyInfo.PropertyType.IsValueType || itemPropertyInfo.PropertyType == typeof(string))
        //                {
        //                    if (toType.IsGenericType)
        //                        expando[property.Name] = value;
        //                    else
        //                        property.SetValue(source, value);
        //                }
        //                else if (!只映射值类型)
        //                {
        //                    var sourceValue = property.GetValue(source);
        //                    if (sourceValue == null && value != null)
        //                    {
        //                        sourceValue = Activator.CreateInstance(value.GetType());
        //                        sourceValue = sourceValue.Merger(value, 只映射值类型, 排除类型, 空不赋值, subExcludeNames);
        //                        if (toType.IsGenericType)
        //                            expando[property.Name] = sourceValue;
        //                        else
        //                            property.SetValue(source, sourceValue);
        //                    }
        //                    else
        //                    {
        //                        sourceValue.Merger(value, 只映射值类型, 排除类型, 空不赋值, subExcludeNames);
        //                    }


        //                }
        //        }
        //    }
        //    return toType.IsGenericType ? new System.Web.Helpers.DynamicJsonObject(expando) as T : source;
        //}

        //public static T Merger<T>(this T source, object obj, bool 空不赋值 = true, params string[] excludeNames) where T : class
        //{
        //    return source.Merger(obj, false, null, 空不赋值 ? new object[] { null } : new object[] { }, excludeNames);
        //}
        //public static T Merger<T>(this T source, object obj, params string[] excludeNames) where T : class
        //{
        //    return source.Merger(obj, true, excludeNames);
        //}
        ///// <summary>
        ///// 将对象[主要是匿名对象]转换为dynamic
        ///// </summary>
        //public static dynamic ToDynamic(this object source, params string[] excludeNames)
        //{
        //    if (source == null) return null;
        //    excludeNames = excludeNames.Where(f => !string.IsNullOrWhiteSpace(f)).ToArray();
        //    if (source is IEnumerable)
        //    {
        //        var lst = new List<object>();
        //        foreach (var item in source as IEnumerable)
        //        {
        //            lst.Add(item.ToDynamic(excludeNames));
        //        }
        //        return lst;
        //    }
        //    var expando = new Dictionary<string, object>();
        //    var type = source.GetType();
        //    var properties = TypeDescriptor.GetProperties(type);
        //    foreach (PropertyDescriptor property in properties)
        //    {
        //        if (excludeNames != null && excludeNames.Any(f => f.TrimStart('.') == property.Name)) continue;
        //        var subExcludeNames = excludeNames
        //            .Where(f => f.StartsWith(property.Name + ".") || (f.StartsWith(".") && f.IndexOf(".", 1) == -1))
        //            .Select(f => (f.StartsWith(".") && f.IndexOf(".", 1) == -1) ? f : f.Remove(0, property.Name.Length + 1)).ToArray();
        //        var val = property.GetValue(source);
        //        //if (property.PropertyType.FullName.StartsWith("<>f__AnonymousType"))
        //        if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
        //        {
        //            expando.Add(property.Name, val);
        //        }
        //        else
        //        {//
        //            //excludeNames.Where(f=>f.StartsWith(property.Name+"[")||f.StartsWith(property.Name+".")).Select(f=>f.Trim("]").Trim()).FirstOrDefault()
        //            dynamic dval = val.ToDynamic(subExcludeNames);
        //            expando.Add(property.Name, dval);
        //        }
        //    }
        //    return new System.Web.Helpers.DynamicJsonObject(expando);
        //}
        ///// <summary>
        ///// 将对象[主要是匿名对象]转换为dynamic
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="TProperty"></typeparam>
        ///// <param name="source"></param>
        ///// <param name="excludeExpressions"></param>
        ///// <returns></returns>
        //public static dynamic ToDynamic<T>(this T source, params Expression<Func<T, object>>[] excludeExpressions)
        //{
        //    return source.ToDynamic(excludeExpressions.Select(f => ExpressionHelper2.GetExpressionText(f)).ToArray());
        //}
        //public static Object ToObject<T>(this T source, params string[] excludeNames)
        //{
        //    return source.ToDynamic(excludeNames);
        //}

        //public static Object ToObject<T>(this T source, params Expression<Func<T, object>>[] excludeExpressions)
        //{
        //    return source.ToDynamic(excludeExpressions.Select(f => ExpressionHelper2.GetExpressionText(f)).ToArray());
        //}
        #endregion

        #region 类型转换
        private static T ConvertValue<T>(object obj, Func<object, T> func, T defaultValue = default(T))
        {
            try
            {
                return (obj == null ? defaultValue : (obj == System.DBNull.Value ? defaultValue : func(obj)));
            }
            catch (Exception) { return defaultValue; }
        }
        public static bool AsBoolean(this object obj, bool defaultValue = default(bool))
        {
            return ConvertValue(obj, f => Convert.ToBoolean(f), defaultValue);
        }
        public static string AsString(this object obj, string defaultValue = default(string))
        {
            return ConvertValue(obj, f => Convert.ToString(f), defaultValue);
        }
        public static short AsInt16(this object obj, short defaultValue = default(short))
        {
            return ConvertValue(obj, f => Convert.ToInt16(f), defaultValue);
        }
        public static int AsInt32(this object obj, int defaultValue = default(int))
        {
            return ConvertValue(obj, f => Convert.ToInt32(f), defaultValue);
        }
        public static long AsInt64(this object obj, long defaultValue = default(long))
        {
            return ConvertValue(obj, f => Convert.ToInt64(f), defaultValue);
        }
        public static ushort AsUInt16(this object obj, ushort defaultValue = default(ushort))
        {
            return ConvertValue(obj, f => Convert.ToUInt16(f), defaultValue);
        }
        public static uint AsUInt32(this object obj, uint defaultValue = default(uint))
        {
            return ConvertValue(obj, f => Convert.ToUInt32(f), defaultValue);
        }
        public static ulong AsUInt64(this object obj, ulong defaultValue = default(ulong))
        {
            return ConvertValue(obj, f => Convert.ToUInt64(f), defaultValue);
        }
        public static float AsSingle(this object obj, float defaultValue = default(float))
        {
            return ConvertValue(obj, f => Convert.ToSingle(f), defaultValue);
        }
        public static double AsDouble(this object obj, double defaultValue = default(double))
        {
            return ConvertValue(obj, f => Convert.ToDouble(f), defaultValue);
        }
        public static decimal AsDecimal(this object obj, decimal defaultValue = default(decimal))
        {
            return ConvertValue(obj, f => Convert.ToDecimal(f), defaultValue);
        }


        //public static bool? AsBoolean(this object obj, bool? defaultValue = null)
        //{
        //    return ConvertValue(obj, f => Convert.ToBoolean(f), defaultValue);
        //}
        //public static short? AsInt16(this object obj, short? defaultValue = null)
        //{
        //    return ConvertValue(obj, f => Convert.ToInt16(f), defaultValue);
        //}
        //public static int? AsInt32(this object obj, int? defaultValue = null)
        //{
        //    return ConvertValue(obj, f => Convert.ToInt32(f), defaultValue);
        //}
        //public static long? AsInt64(this object obj, long? defaultValue = null)
        //{
        //    return ConvertValue(obj, f => Convert.ToInt64(f), defaultValue);
        //}
        //public static float? AsSingle(this object obj, float? defaultValue = null)
        //{
        //    return ConvertValue(obj, f => Convert.ToSingle(f), defaultValue);
        //}
        //public static double? AsDouble(this object obj, double? defaultValue = null)
        //{
        //    return ConvertValue(obj, f => Convert.ToDouble(f), defaultValue);
        //}
        //public static decimal? AsDecimal(this object obj, decimal? defaultValue = null)
        //{
        //    return ConvertValue(obj, f => Convert.ToDecimal(f), defaultValue);
        //}

        #endregion

        public static bool IsAnonymousType(this object value)
        {
            return value == null ? false : value.GetType().IsAnonymousType();
        }
        public static bool IsDBNull(this object value)
        {
            return value == null || value == System.DBNull.Value;
        }
    }
}