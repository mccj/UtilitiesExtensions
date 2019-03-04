using System.Globalization;

namespace System.Linq
{
    /// <summary>
    /// 字符串<see cref="String"/>类型的扩展辅助操作类
    /// 类型转换-提供用于将字符串值转换为其他数据类型的实用工具方法。
    /// </summary>
    public static class StringIsAsExtensions
    {
        #region Is   
        /// <summary>检查字符串值是否为 null 或空。</summary>
        /// <returns>如果 <paramref name="value" /> 为 null 或零长度字符串 ("")，则为 true；否则为 false。</returns>
        /// <param name="value">要测试的字符串值。</param>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>检查字符串是否可以转换为 Boolean (true/false) 类型。</summary>
        /// <returns>如果 <paramref name="value" /> 可以转换为指定的类型，则为 true；否则为 false。</returns>
        /// <param name="value">要测试的字符串值。</param>
        public static bool IsBool(this string value)
        {
            bool flag;
            return bool.TryParse(value, out flag);
        }

        /// <summary>检查字符串是否可以转换为整数。</summary>
        /// <returns>如果 <paramref name="value" /> 可以转换为指定的类型，则为 true；否则为 false。</returns>
        /// <param name="value">要测试的字符串值。</param>
        public static bool IsInt(this string value)
        {
            int num;
            return int.TryParse(value, out num);
        }

        /// <summary>检查字符串是否可以转换为 <see cref="T:System.Decimal" /> 类型。</summary>
        /// <returns>如果 <paramref name="value" /> 可以转换为指定的类型，则为 true；否则为 false。</returns>
        /// <param name="value">要测试的字符串值。</param>
        public static bool IsDecimal(this string value)
        {
            return value.Is<decimal>();
        }

        /// <summary>检查字符串是否可以转换为 <see cref="T:System.Single" /> 类型。</summary>
        /// <returns>如果 <paramref name="value" /> 可以转换为指定的类型，则为 true；否则为 false。</returns>
        /// <param name="value">要测试的字符串值。</param>
        public static bool IsFloat(this string value)
        {
            float num;
            return float.TryParse(value, out num);
        }

        /// <summary>检查字符串是否可以转换为 <see cref="T:System.DateTime" /> 类型。</summary>
        /// <returns>如果 <paramref name="value" /> 可以转换为指定的类型，则为 true；否则为 false。</returns>
        /// <param name="value">要测试的字符串值。</param>
        public static bool IsDateTime(this string value)
        {
            DateTime dateTime;
            return DateTime.TryParse(value, out dateTime);
        }

        /// <summary>检查字符串是否可以转换为指定的数据类型。</summary>
        /// <returns>如果 <paramref name="value" /> 可以转换为指定的类型，则为 true；否则为 false。</returns>
        /// <param name="value">要测试的值。</param>
        /// <typeparam name="TValue">要转换为的数据类型。</typeparam>
        public static bool Is<TValue>(this string value)
        {
            var converter = ComponentModel.TypeDescriptor.GetConverter(typeof(TValue));
            if (converter != null)
            {
                try
                {
                    if (value == null || converter.CanConvertFrom(null, value.GetType()))
                    {
                        converter.ConvertFrom(null, CultureInfo.CurrentCulture, value);
                        return true;
                    }
                }
                catch
                {
                }
                return false;
            }
            return false;
        }
        #endregion Is
        #region As
        /// <summary>将字符串转换为整数。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        public static int AsInt(this string value)
        {
            return value.AsInt(0);
        }


        /// <summary>将字符串转换为整数，并指定默认值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">当 <paramref name="value" /> 为 null 或无效的值时要返回的值。</param>
        public static int AsInt(this string value, int defaultValue)
        {
            int result;
            if (!int.TryParse(value, out result))
            {
                return defaultValue;
            }
            return result;
        }

        /// <summary>将字符串转换为 <see cref="T:System.Decimal" /> 数字。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        public static decimal AsDecimal(this string value)
        {
            return value.As<decimal>();
        }

        /// <summary>将字符串转换为 <see cref="T:System.Decimal" /> 数字，并指定默认值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">当 <paramref name="value" /> 为 null 或无效时要返回的值。</param>
        public static decimal AsDecimal(this string value, decimal defaultValue)
        {
            return value.As(defaultValue);
        }

        /// <summary>将字符串转换为 <see cref="T:System.Single" /> 数字。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        public static float AsFloat(this string value)
        {
            return value.AsFloat(0f);
        }

        /// <summary>将字符串转换为 <see cref="T:System.Single" /> 数字，并指定默认值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">当 <paramref name="value" /> 为 null 时要返回的值。</param>
        public static float AsFloat(this string value, float defaultValue)
        {
            float result;
            if (!float.TryParse(value, out result))
            {
                return defaultValue;
            }
            return result;
        }

        /// <summary>将字符串转换为 <see cref="T:System.DateTime" /> 值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        public static DateTime AsDateTime(this string value)
        {
            return value.AsDateTime(default(DateTime));
        }

        /// <summary>将字符串转换为 <see cref="T:System.DateTime" /> 值，并指定默认值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">当 <paramref name="value" /> 为 null 或无效的值时要返回的值。 默认值为系统的最小时间值。</param>
        public static DateTime AsDateTime(this string value, DateTime defaultValue)
        {
            DateTime result;
            if (!DateTime.TryParse(value, out result))
            {
                return defaultValue;
            }
            return result;
        }


        /// <summary>将字符串转换为布尔值 (true/false)。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        public static bool AsBool(this string value)
        {
            return value.AsBool(false);
        }

        /// <summary>将字符串转换为布尔值 (true/false)，并指定默认值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">当 <paramref name="value" /> 为 null 或无效的值时要返回的值。</param>
        public static bool AsBool(this string value, bool defaultValue)
        {
            bool result;
            if (!bool.TryParse(value, out result))
            {
                return defaultValue;
            }
            return result;
        }

        /// <summary>将字符串转换为指定数据类型的强类型值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        /// <typeparam name="TValue"> 要转换为的数据类型。</typeparam>
        public static TValue As<TValue>(this string value)
        {
            return value.As(default(TValue));
        }
        /// <summary>将字符串转换为指定的数据类型，并指定默认值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">当 <paramref name="value" /> 为 null 时要返回的值。</param>
        /// <typeparam name="TValue">要转换为的数据类型。</typeparam>
        public static TValue As<TValue>(this string value, TValue defaultValue)
        {
            try
            {
                var converter = ComponentModel.TypeDescriptor.GetConverter(typeof(TValue));
                if (converter.CanConvertFrom(typeof(string)))
                {
                    TValue result = (TValue)((object)converter.ConvertFrom(value));
                    return result;
                }
                converter = ComponentModel.TypeDescriptor.GetConverter(typeof(string));
                if (converter.CanConvertTo(typeof(TValue)))
                {
                    TValue result = (TValue)((object)converter.ConvertTo(value, typeof(TValue)));
                    return result;
                }
            }
            catch
            {
            }
            return defaultValue;
        }
        #endregion As
    }
}
