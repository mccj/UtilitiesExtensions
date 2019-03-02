
using System.ComponentModel;
using System.Reflection;

namespace System.Linq
{
    /// <summary>
    /// 枚举<see cref="Enum"/>的扩展辅助操作方法
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举项上的<see cref="DescriptionAttribute"/>特性的文字描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum value)
        {
            Type type = value.GetType();
            var member = type.GetMember(value.ToString()).FirstOrDefault();
            return member != null ? member.ToDescription() : value.ToString();
        }
        public static TEnum? AsEnum<TEnum>(this string str) where TEnum : struct
        {
            TEnum ooo;
            if (Enum.TryParse(str, true, out ooo))
                return ooo;
            else
                return default(TEnum?);
        }

        //public static string ToDescription(this Enum value)
        //{
        //    Type type = value.GetType();
        //    var member = type.GetMember(value.ToString()).FirstOrDefault();
        //    return member != null ? member.ToDescription() : value.ToString();
        //}

        public static string GetDescription(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] member = type.GetMember(en.ToString());
            if (member != null && member.Length != 0)
            {
                object[] customAttributes = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (customAttributes != null && customAttributes.Length != 0)
                {
                    return ((DescriptionAttribute)customAttributes[0]).Description;
                }
            }
            return en.ToString();
        }
    }
}
