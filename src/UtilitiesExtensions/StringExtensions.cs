using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Linq
{
    /// <summary>
    /// 字符串<see cref="String"/>类型的扩展辅助操作类
    /// </summary>
    public static class StringExtensions
    {

        #region 正则表达式
        /// <summary>
        /// 指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false</returns>
        public static bool RegexIsMatch(this string value, string pattern)
        {
            if (value == null)
            {
                return false;
            }
            return Regex.IsMatch(value, pattern);
        }
        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的第一个匹配项
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>一个对象，包含有关匹配项的信息</returns>
        public static string RegexMatch(this string value, string pattern, string name = null, RegexOptions options = RegexOptions.None)
        {
            return RegexMatch(value, pattern, options, new[] { name }).FirstOrDefault();
        }
        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的第一个匹配项
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <param name="groupnames"></param>
        /// <returns></returns>
        public static IEnumerable<string> RegexMatch(this string value, string pattern, RegexOptions options = RegexOptions.None, params string[] groupnames)
        {
            if (value == null)
            {
                yield return null;
            }
            var match = Regex.Match(value, pattern, options);
            if (match != null && match.Success)
            {
                if (groupnames == null || groupnames.Length == 0)
                {
                    yield return match.Value;
                }
                else
                {
                    var matchGroups = match.Groups;
                    foreach (var item in groupnames)
                    {
                        var matchItem = matchGroups[item];
                        if (matchItem != null && matchItem.Success)
                            yield return matchItem.Value;
                    }
                }
            }
            else { }
        }
        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的所有匹配项的字符串集合
        /// </summary>
        /// <param name="value"> 要搜索匹配项的字符串 </param>
        /// <param name="pattern"> 要匹配的正则表达式模式 </param>
        /// <returns> 一个集合，包含有关匹配项的字符串值 </returns>
        public static IEnumerable<string> RegexMatches(this string value, string pattern, RegexOptions options = RegexOptions.None)
        {
            if (value == null)
            {
                return new string[] { };
            }
            MatchCollection matches = Regex.Matches(value, pattern, options);
            return from Match match in matches select match.Value;
        }
        /// <summary>
        /// 在指定的输入字符串内，使用 System.Text.RegularExpressions.MatchEvaluator 委托返回的字符串替换与指定正则表达式匹配的所有字符串。指定的选项将修改匹配操作。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string RegexReplace(this string value, string pattern, string replacement, RegexOptions options = RegexOptions.None)
        {
            if (value == null)
            {
                return value;
            }
            return Regex.Replace(value, pattern, replacement, options);
        }
        /// <summary>
        /// 在指定的输入字符串内，使用 System.Text.RegularExpressions.MatchEvaluator 委托返回的字符串替换与指定正则表达式匹配的所有字符串。指定的选项将修改匹配操作。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern"></param>
        /// <param name="evaluator"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string RegexReplace(this string value, string pattern, Func<Match, string> evaluator, RegexOptions options = RegexOptions.None)
        {
            if (value == null)
            {
                return value;
            }
            return Regex.Replace(value, pattern, new MatchEvaluator(evaluator), options);
        }
        /// <summary>
        /// 在由指定正则表达式模式定义的位置将输入字符串拆分为一个子字符串数组。指定的选项将修改匹配操作。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string[] RegexSplit(this string value, string pattern, RegexOptions options = RegexOptions.None)
        {
            if (value == null)
            {
                return new string[] { };
            }
            return Regex.Split(value, pattern, options);
        }
        /// <summary>
        /// 是否电子邮件
        /// </summary>
        public static bool IsEmail(this string value)
        {
            const string pattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return value.RegexIsMatch(pattern);
        }
        /// <summary>
        /// 是否是IP地址
        /// </summary>
        public static bool IsIpAddress(this string value)
        {
            const string pattern = @"^(\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d\.){3}\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d$";
            return value.RegexIsMatch(pattern);
        }
        /// <summary>
        /// 是否是整数
        /// </summary>
        public static bool IsNumeric(this string value)
        {
            const string pattern = @"^\-?[0-9]+$";
            return value.RegexIsMatch(pattern);
        }
        /// <summary>
        /// 是否是Unicode字符串
        /// </summary>
        public static bool IsUnicode(this string value)
        {
            const string pattern = @"^[\u4E00-\u9FA5\uE815-\uFA29]+$";
            return value.RegexIsMatch(pattern);
        }
        /// <summary>
        /// 是否Url字符串
        /// </summary>
        public static bool IsUrl(this string value)
        {
            const string pattern = @"^(http|https|ftp|rtsp|mms):(\/\/|\\\\)[A-Za-z0-9%\-_@]+\.[A-Za-z0-9%\-_@]+[A-Za-z0-9\.\/=\?%\-&_~`@:\+!;]*$";
            return value.RegexIsMatch(pattern);
        }
        /// <summary>
        /// 是否身份证号，验证如下3种情况：
        /// 1.身份证号码为15位数字；
        /// 2.身份证号码为18位数字；
        /// 3.身份证号码为17位数字+1个字母
        /// </summary>
        public static bool IsIdentityCard(this string value)
        {
            const string pattern = @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$";
            return value.RegexIsMatch(pattern);
        }
        /// <summary>
        /// 是否手机号码
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isRestrict">是否按严格格式验证</param>
        public static bool IsMobileNumber(this string value, bool isRestrict = false)
        {
            string pattern = isRestrict ? @"^[1][3-8]\d{9}$" : @"^[1]\d{10}$";
            return value.RegexIsMatch(pattern);
        }
        ///// <summary>
        ///// 是否包含指定的单词
        ///// </summary>
        ///// <param name="source"></param>
        ///// <param name="pattern"></param>
        ///// <returns></returns>
        //public static bool IsLike(this string source, string pattern)
        //{
        //    pattern = Regex.Escape(pattern);
        //    pattern = pattern.Replace("%", ".*?").Replace("_", ".");
        //    pattern = pattern.Replace(@"\[", "[").Replace(@"\]", "]").Replace(@"\^", "^");
        //    return Regex.IsMatch(source, pattern);
        //}
        #endregion

        #region 其他操作
        /// <summary>
        /// 是否包含中文
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsHasChinese(this string value)
        {
            return Regex.IsMatch(value, @"^[\u4e00-\u9fa5]+$");
        }

        /// <summary>
        /// 指示指定的字符串是 null 还是 System.String.Empty 字符串
        /// </summary>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 为指定格式的字符串填充相应对象来生成字符串
        /// </summary>
        /// <param name="format">字符串格式，占位符以{n}表示</param>
        /// <param name="args">用于填充占位符的参数</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatWith(this string format, params object[] args)
        {
            format.CheckNotNull("format");
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        /// 将字符串反转
        /// </summary>
        /// <param name="value">要反转的字符串</param>
        public static string ReverseString(this string value)
        {
            value.CheckNotNull("value");
            return new string(value.Reverse().ToArray());
        }

        /// <summary>
        /// 以指定字符串作为分隔符将指定字符串分隔成数组
        /// </summary>
        /// <param name="value">要分割的字符串</param>
        /// <param name="strSplit">字符串类型的分隔符</param>
        /// <param name="removeEmptyEntries">是否移除数据中元素为空字符串的项</param>
        /// <param name="trim">返回结果是否移除所有前导空白字符和尾部空白字符</param>
        /// <returns>分割后的数据</returns>
        public static string[] Split(this string value, string strSplit, bool removeEmptyEntries = false, bool trim = true)
        {
            var r1 = value.Split(new[] { strSplit }, removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
            var r2 = trim ? r1.Select(f => f.Trim()).ToArray() : r1;
            var r3 = removeEmptyEntries ? r2.Where(f => !string.IsNullOrWhiteSpace(f)).ToArray() : r2;
            return r3;
        }

        /// <summary>
        /// 获取字符串的MD5 Hash值
        /// </summary>
        public static string ToMd5Hash(this string value)
        {
            return UtilitiesExtensions.Utility.Secutiry.HashHelper.GetMd5(value);
        }
        public static string ToBase64Url(this string value)
        {
            return ToBase64(value).Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }
        public static string FromBase64Url(this string value)
        {
            value = value.Replace('-', '+').Replace('_', '/');
            switch (value.Length % 4)
            {
                case 2:
                    value += "==";
                    break;
                case 3:
                    value += "=";
                    break;
            }
            return FromBase64(value);
        }
        public static string ToBase64(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value));
        }
        public static string FromBase64(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;
            try
            {
                return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(value));
            }
            catch (Exception) { }
            return value;
        }
        /// <summary>
        /// 支持汉字的字符串长度，汉字长度计为2
        /// </summary>
        /// <param name="value">参数字符串</param>
        /// <returns>当前字符串的长度，汉字长度为2</returns>
        public static int TextLength(this string value)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] bytes = ascii.GetBytes(value);
            foreach (byte b in bytes)
            {
                if (b == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }
        ///// <summary>
        ///// 将JSON字符串还原为对象
        ///// </summary>
        ///// <typeparam name="T">要转换的目标类型</typeparam>
        ///// <param name="json">JSON字符串 </param>
        ///// <returns></returns>
        //public static T ToJsonEntity<T>(this string json)
        //{
        //    json.CheckNotNull("json");
        //    //return System.Web.Helpers.Json.Decode<T>(json);
        //    return UtilitiesExtensions.Utility.Json.Decode<T>(json);
        //}
        ///// <summary>
        ///// 将JSON字符串还原为对象
        ///// </summary>
        ///// <param name="json">JSON字符串</param>
        ///// <param name="type">要转换的目标类型</param>
        ///// <returns></returns>
        //public static object ToJsonEntity(this string json, Type type)
        //{
        //    json.CheckNotNull("json");
        //    //return System.Web.Helpers.Json.Decode(json, type);
        //    return UtilitiesExtensions.Utility.Json.Decode(json, type);
        //}
        ///// <summary>
        ///// 将JSON字符串还原为对象
        ///// </summary>
        ///// <param name="json"><JSON字符串/param>
        ///// <returns></returns>
        //public static dynamic ToJsonEntity(this string json)
        //{
        //    json.CheckNotNull("json");
        //    //return System.Web.Helpers.Json.Decode(json);
        //    return UtilitiesExtensions.Utility.Json.Decode(json);
        //}
        ///// <summary>
        ///// 将xml字符串还原为对象
        ///// </summary>
        ///// <typeparam name="T">要转换的目标类型</typeparam>
        ///// <param name="xml">xml字符串 </param>
        ///// <returns></returns>
        //public static T ToXmlEntity<T>(this string xml)
        //{
        //    xml.CheckNotNull("xml");
        //    //return System.Web.Helpers.Json.Decode<T>(json);
        //    return UtilitiesExtensions.Utility.SerializationHelper.DeserializeFromXml<T>(xml);
        //}
        ///// <summary>
        ///// 将xml字符串还原为对象
        ///// </summary>
        ///// <param name="xml">xml字符串</param>
        ///// <param name="type">要转换的目标类型</param>
        ///// <returns></returns>
        //public static object ToXmlEntity(this string xml, Type type)
        //{
        //    xml.CheckNotNull("xml");
        //    //return System.Web.Helpers.Json.Decode(json, type);
        //    return UtilitiesExtensions.Utility.SerializationHelper.DeserializeFromXml(xml, type);
        //}
        ////public static dynamic ToXmlEntity(this string xml)
        ////{
        ////    xml.CheckNotNull("xml");
        ////    //return System.Web.Helpers.Json.Decode(json);
        ////    return UtilitiesExtensions.Utility.SerializationHelper.DeserializeFromXml(xml);
        ////}
        /// <summary>
        /// 将字符串转换为<see cref="byte"/>[]数组，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static byte[] ToBytes(this string value, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetBytes(value);
        }

        /// <summary>
        /// 将<see cref="byte"/>[]数组转换为字符串，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static string ToString(this byte[] bytes, Encoding encoding)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetString(bytes);
        }

        #endregion

        public static string ToHtmlId(this string name)
        {
            return name.Replace('.', '_');//.ToHtmlName();
        }
        /// <summary>
        /// 判断指定路径是否图片文件
        /// </summary>
        public static bool IsImageFile(this string filename)
        {
            if (!File.Exists(filename))
            {
                return false;
            }
            byte[] filedata = File.ReadAllBytes(filename);
            if (filedata.Length == 0)
            {
                return false;
            }
            ushort code = BitConverter.ToUInt16(filedata, 0);
            switch (code)
            {
                case 0x4D42: //bmp
                case 0xD8FF: //jpg
                case 0x4947: //gif
                case 0x5089: //png
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// <summary>
        /// 字符串转Unicode
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Unicode编码后的字符串</returns>
        public static string ToUnicodeFormat(this string source)
        {
            //byte[] bytes = Encoding.Unicode.GetBytes(source);
            //StringBuilder stringBuilder = new StringBuilder();
            //for (int i = 0; i < bytes.Length; i += 2)
            //{
            //    stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            //}
            //return stringBuilder.ToString();
            return new Regex(@"([^\x00-\x7f]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(source, x => string.Join("", x.Result("$1").ToCharArray().Select(f => Convert.ToInt16(f)).Select(f => string.Format("\\u{0}", f.ToString("x").PadLeft(4, '0')))));
        }

        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <param name="source">经过Unicode编码的字符串</param>
        /// <returns>正常字符串</returns>
        public static string UnicodeToString(this string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }


        //#region 命名方法
        ///// <summary>
        ///// 骆驼拼写法(CamelCase),除了第一个单词外的其他单词的开头字母大写. 如. testCounter.
        ///// </summary>
        ///// <param name="camel"></param>
        ///// <returns></returns>
        //public static string CamelFriendly(this string camel)
        //{
        //    if (String.IsNullOrWhiteSpace(camel))
        //        return "";

        //    var sb = new StringBuilder(camel);

        //    for (int i = camel.Length - 1; i > 0; i--)
        //    {
        //        if (char.IsUpper(sb[i]))
        //        {
        //            sb.Insert(i, ' ');
        //        }
        //    }

        //    return sb.ToString();
        //}
        //public static string HtmlClassify(this string text)
        //{
        //    if (String.IsNullOrWhiteSpace(text))
        //        return "";

        //    var friendlier = text.CamelFriendly();

        //    var result = new char[friendlier.Length];

        //    var cursor = 0;
        //    var previousIsNotLetter = false;
        //    for (var i = 0; i < friendlier.Length; i++)
        //    {
        //        char current = friendlier[i];
        //        if (IsLetter(current) || (Char.IsDigit(current) && cursor > 0))
        //        {
        //            if (previousIsNotLetter && i != 0 && cursor > 0)
        //            {
        //                result[cursor++] = '-';
        //            }

        //            result[cursor++] = Char.ToLowerInvariant(current);
        //            previousIsNotLetter = false;
        //        }
        //        else
        //        {
        //            previousIsNotLetter = true;
        //        }
        //    }

        //    return new string(result, 0, cursor);
        //}
        ///// <summary>
        ///// Generates a valid technical name.
        ///// </summary>
        ///// <remarks>
        ///// Uses a white list set of chars.
        ///// </remarks>
        //public static string ToSafeName(this string name)
        //{
        //    if (String.IsNullOrWhiteSpace(name))
        //        return String.Empty;

        //    name = RemoveDiacritics(name);
        //    name = name.Strip(c =>
        //        !c.IsLetter()
        //        && !Char.IsDigit(c)
        //        );

        //    name = name.Trim();

        //    // don't allow non A-Z chars as first letter, as they are not allowed in prefixes
        //    while (name.Length > 0 && !IsLetter(name[0]))
        //    {
        //        name = name.Substring(1);
        //    }

        //    if (name.Length > 128)
        //        name = name.Substring(0, 128);

        //    return name;
        //}
        //public static string ToHtmlId(this string name)
        //{
        //    return name.Replace('.', '_');//.ToHtmlName();
        //}
        ///// <summary>
        ///// Generates a valid Html name.
        ///// </summary>
        ///// <remarks>
        ///// Uses a white list set of chars.
        ///// </remarks>
        //public static string ToHtmlName(this string name)
        //{
        //    if (String.IsNullOrWhiteSpace(name))
        //        return String.Empty;

        //    name = RemoveDiacritics(name);
        //    name = name.Strip(c =>
        //        c != '-'
        //        && c != '_'
        //        && !c.IsLetter()
        //        && !Char.IsDigit(c)
        //        );

        //    name = name.Trim();

        //    // don't allow non A-Z chars as first letter, as they are not allowed in prefixes
        //    while (name.Length > 0 && !IsLetter(name[0]))
        //    {
        //        name = name.Substring(1);
        //    }

        //    return name;
        //}

        ///// <summary>
        ///// Whether the char is a letter between A and Z or not
        ///// </summary>
        //public static bool IsLetter(this char c)
        //{
        //    return ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z');
        //}

        //public static bool IsSpace(this char c)
        //{
        //    return (c == '\r' || c == '\n' || c == '\t' || c == '\f' || c == ' ');
        //}
        //public static string RemoveDiacritics(this string name)
        //{
        //    string stFormD = name.Normalize(NormalizationForm.FormD);
        //    var sb = new StringBuilder();

        //    foreach (char t in stFormD)
        //    {
        //        UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(t);
        //        if (uc != UnicodeCategory.NonSpacingMark)
        //        {
        //            sb.Append(t);
        //        }
        //    }

        //    return (sb.ToString().Normalize(NormalizationForm.FormC));
        //}
        ////public static string Strip(this string subject, params char[] stripped)
        ////{
        ////    if (stripped == null || stripped.Length == 0 || String.IsNullOrEmpty(subject))
        ////    {
        ////        return subject;
        ////    }

        ////    var result = new char[subject.Length];

        ////    var cursor = 0;
        ////    for (var i = 0; i < subject.Length; i++)
        ////    {
        ////        char current = subject[i];
        ////        if (Array.IndexOf(stripped, current) < 0)
        ////        {
        ////            result[cursor++] = current;
        ////        }
        ////    }

        ////    return new string(result, 0, cursor);
        ////}

        //public static string Strip(this string subject, Func<char, bool> predicate)
        //{

        //    var result = new char[subject.Length];

        //    var cursor = 0;
        //    for (var i = 0; i < subject.Length; i++)
        //    {
        //        char current = subject[i];
        //        if (!predicate(current))
        //        {
        //            result[cursor++] = current;
        //        }
        //    }

        //    return new string(result, 0, cursor);
        //}
        //#endregion 命名方法

        public static string StripDigits(this string input)
        {
            return Regex.Replace(input, "[\\d-]", string.Empty);
        }

        //public static string Encrypt(this string plainText, string passPhrase)
        //{
        //    return new UtilitiesExtensions.Utility.Encryption().EncryptString(plainText, passPhrase);
        //}

        //public static string Decrypt(this string cipherText, string passPhrase)
        //{
        //    return new UtilitiesExtensions.Utility.Encryption().DecryptString(cipherText, passPhrase);
        //}

        public static string SanitizeArabic(this string input)
        {
            return input.Replace("أ", "ا").Replace("إ", "ا").Replace("آ", "ا")
                .Replace("ي", "ى")
                .Replace("ة", "ه")
                .Replace("لأ", "لا")
                .Replace("لإ", "لا");
        }


        //Essa classe possui códigos adaptados da internet.
        //Para a fonte inicial de alguns desses métodos acesse http://stackoverflow.com/questions/19523913/remove-html-tags-from-string-including-nbsp-in-c-sharp .

        #region Métodos para remoção de tags

        /// <summary> Regex para identificar tags html ou xml.</summary>
        private static readonly Regex tags = new Regex(@"<[^>]+?>", RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary> Remove quaisquer tags (HTML, XML, etc.) encontradas no texto original.
        /// <para>Esse método não elimina o conteúdo interno da tag.</para> </summary>
        /// <param name="source"> String original. </param>
        /// <returns> String contendo o texto original desprovido de tags. </returns>
        public static string RemoverTags(this string source)
        {
            //Caso o método seja chamado em uma string vazia ou nula, retorne uma string vazia
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            return tags.Replace(source, string.Empty);
        }

        ///// <summary> Elimina todas as tags HTML encontradas na string, bem como o conteúdo interno de tags de comentário, script ou style,
        /////  mantendo apenas texto corrido separado por espaços simples. </summary>
        ///// <param name="html"> Texto original cujo HTML será removido. </param>
        ///// <returns> String contendo apenas o texto original, ignorando quaisquer tags ou conteúdo específico de HTML (como comentários, scripts ou css). </returns>
        //public static String RemoverHtml(this String html)
        //{
        //    //Caso o método seja chamado em uma string vazia ou nula, retorne uma string vazia
        //    if (string.IsNullOrEmpty(html))
        //        return string.Empty;

        //    html = System.Web.HttpUtility.UrlDecode(html);
        //    html = System.Web.HttpUtility.HtmlDecode(html);

        //    html = RemoverTag(html, "<!--", "-->");
        //    html = RemoverTag(html, "<script", "</script>");
        //    html = RemoverTag(html, "<style", "</style>");

        //    //Utiliza os Regex para substituir qualquer correspondência (tags) encontrada no texto por espaço
        //    html = tags.Replace(html, " ");
        //    html = ManterEspacoSimples(html);

        //    return html;
        //}

        /// <summary> Remove uma tag específica e todo seu conteúdo interno. </summary>
        /// <param name="html"> Texto original do qual será extraída a tag. </param>
        /// <param name="inicioTag"> Demarcação do início da Tag. </param>
        /// <param name="finalTag"> Demarcação do final da Tag. </param>
        /// <returns> String contendo o texto original menos qualquer ocorrência da tag informada e seu conteúdo interno. </returns>
        public static String RemoverTag(this String html, String inicioTag, String finalTag)
        {
            //Caso o método seja chamado em uma string vazia ou nula, retorne uma string vazia
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            Boolean repetir;
            do
            {
                repetir = false; //Variável para controle da repetição
                //Procura pelo início da tag no texto
                Int32 posicaoInicialTag = html.IndexOf(inicioTag, 0, StringComparison.CurrentCultureIgnoreCase);
                //Caso não seja encontrado o fim da tag, sai do loop.
                if (posicaoInicialTag < 0)
                    break;
                //Procura pelo fim da tag no texto
                Int32 posicaoFinalTag = html.IndexOf(finalTag, posicaoInicialTag + 1, StringComparison.CurrentCultureIgnoreCase);
                //Caso não seja encontrado o fim da tag, sai do loop.
                if (posicaoFinalTag <= posicaoInicialTag)
                    break;
                //Remove todo o trecho entre o início e o final da tag
                html = html.Remove(posicaoInicialTag, posicaoFinalTag - posicaoInicialTag + finalTag.Length);
                //Executa novamente a verificação para garantir que não haja mais nenhuma tag
                repetir = true;
            } while (repetir);
            return html;
        }
        #endregion

        #region Métodos para formatação ou limpeza de texto
        /// <summary> Substitui toda quebra de linha ou espaço longo por espaços simples. </summary>
        /// <param name="textoOriginal"> O texto cujos espaços serão removidos. </param>
        /// <returns> string contendo o texto original sem quebras de linha e apenas com espaços simples entre as palavras. </returns>
        public static string ManterEspacoSimples(this string textoOriginal)
        {
            //Caso o método seja chamado em uma string vazia ou nula, retorne uma string vazia
            if (string.IsNullOrEmpty(textoOriginal))
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            Boolean inBlanks = false; //Define se já foi inserido um espaço antes do caracter atual
            foreach (Char c in textoOriginal)
            {
                //Se o caracter for um espaço vazio (espaços, tabulações, quebras de linha, etc.)
                if (Char.IsWhiteSpace(c))
                {
                    //Apenas se não já tiver sido inserido um espaço anterior, insira um espaço vazio
                    if (!inBlanks)
                    {
                        inBlanks = true;
                        sb.Append(' ');
                    }
                }
                else
                {
                    inBlanks = false;
                    sb.Append(c);
                }
            }
            return sb.ToString().Trim();
        }
        #endregion

        #region sss

        public static byte[] GetBytes(this string source)
        {
            return Encoding.Default.GetBytes(source);
        }

        public static byte[] GetBytes(this string source, Encoding encoding)
        {
            return encoding.GetBytes(source);
        }

        public static string Enclose(this string s, string bothEnds)
        {
            return bothEnds + s + bothEnds;
        }

        public static string Enclose(this string s, string leftEnds, string rightEnds)
        {
            return leftEnds + s + rightEnds;
        }

        public static string MakeXsvField(this string str, IEnumerable<string> delimiters)
        {
            string result = str.Replace("\"", "\"\"");
            string text = result.Trim();
            if (result.Length != text.Length || new string[3]
            {
            "\"",
            "\r",
            "\n"
            }.Concat(delimiters).Any((string s) => result.Contains(s)))
            {
                result = Enclose(result, "\"");
            }
            return result;
        }

        public static TEnum ToEnum<TEnum>(this string str, bool ignoreCase = true) where TEnum : struct
        {
            Type typeFromHandle = typeof(TEnum);
            ThrowIfNotEnumType(typeFromHandle);
            return (TEnum)Enum.Parse(typeFromHandle, str, ignoreCase);
        }

        public static TEnum? ToEnumOrNull<TEnum>(this string str, bool ignoreCase = true) where TEnum : struct
        {
            Type typeFromHandle = typeof(TEnum);
            ThrowIfNotEnumType(typeFromHandle);
            if (TryParseEnum(str, ignoreCase, out TEnum result) && Enum.IsDefined(typeFromHandle, result))
            {
                return result;
            }
            return null;
        }

        public static TEnum ToEnumOrDefault<TEnum>(this string str, TEnum defaultValue = default(TEnum), bool ignoreCase = true) where TEnum : struct
        {
            Type typeFromHandle = typeof(TEnum);
            ThrowIfNotEnumType(typeFromHandle);
            if (TryParseEnum(str, ignoreCase, out TEnum result) && Enum.IsDefined(typeFromHandle, result))
            {
                return result;
            }
            return defaultValue;
        }

        private static void ThrowIfNotEnumType(Type type)
        {
            if (!type.IsEnum)
            {
                throw new TypeAccessException("TEnum must be an enum type.");
            }
        }

        private static bool TryParseEnum<TEnum>(string str, bool ignoreCase, out TEnum result) where TEnum : struct
        {
            return Enum.TryParse(str, ignoreCase, out result);
        }















        public static sbyte ToSByte(this string s)
        {
            return sbyte.Parse(s);
        }

        public static sbyte? ToSByteOrNull(this string s)
        {
            if (sbyte.TryParse(s, out sbyte result))
            {
                return result;
            }
            return null;
        }

        public static sbyte ToSByteOrDefault(this string s, sbyte defaultValue = 0)
        {
            if (sbyte.TryParse(s, out sbyte result))
            {
                return result;
            }
            return defaultValue;
        }

        public static sbyte ToSByte(this string s, IFormatProvider formatProvider)
        {
            return sbyte.Parse(s, formatProvider);
        }

        public static sbyte ToSByte(this string s, NumberStyles numberStyles)
        {
            return sbyte.Parse(s, numberStyles);
        }

        public static sbyte ToSByte(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return sbyte.Parse(s, numberStyles, formatProvider);
        }

        public static sbyte? ToSByteOrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (sbyte.TryParse(s, numberStyles, formatProvider, out sbyte result))
            {
                return result;
            }
            return null;
        }

        public static sbyte? ToSByteOrNull(this string s, NumberStyles numberStyles)
        {
            if (sbyte.TryParse(s, numberStyles, null, out sbyte result))
            {
                return result;
            }
            return null;
        }

        public static sbyte ToSByteOrDefault(this string s, NumberStyles numberStyles, sbyte defaultValue = 0)
        {
            if (sbyte.TryParse(s, numberStyles, null, out sbyte result))
            {
                return result;
            }
            return defaultValue;
        }

        public static sbyte ToSByteOrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, sbyte defaultValue = 0)
        {
            if (sbyte.TryParse(s, numberStyles, formatProvider, out sbyte result))
            {
                return result;
            }
            return defaultValue;
        }

        public static byte ToByte(this string s)
        {
            return byte.Parse(s);
        }

        public static byte? ToByteOrNull(this string s)
        {
            if (byte.TryParse(s, out byte result))
            {
                return result;
            }
            return null;
        }

        public static byte ToByteOrDefault(this string s, byte defaultValue = 0)
        {
            if (byte.TryParse(s, out byte result))
            {
                return result;
            }
            return defaultValue;
        }

        public static byte ToByte(this string s, IFormatProvider formatProvider)
        {
            return byte.Parse(s, formatProvider);
        }

        public static byte ToByte(this string s, NumberStyles numberStyles)
        {
            return byte.Parse(s, numberStyles);
        }

        public static byte ToByte(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return byte.Parse(s, numberStyles, formatProvider);
        }

        public static byte? ToByteOrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (byte.TryParse(s, numberStyles, formatProvider, out byte result))
            {
                return result;
            }
            return null;
        }

        public static byte? ToByteOrNull(this string s, NumberStyles numberStyles)
        {
            if (byte.TryParse(s, numberStyles, null, out byte result))
            {
                return result;
            }
            return null;
        }

        public static byte ToByteOrDefault(this string s, NumberStyles numberStyles, byte defaultValue = 0)
        {
            if (byte.TryParse(s, numberStyles, null, out byte result))
            {
                return result;
            }
            return defaultValue;
        }

        public static byte ToByteOrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, byte defaultValue = 0)
        {
            if (byte.TryParse(s, numberStyles, formatProvider, out byte result))
            {
                return result;
            }
            return defaultValue;
        }

        public static char ToChar(this string s)
        {
            return char.Parse(s);
        }

        public static char? ToCharOrNull(this string s)
        {
            if (char.TryParse(s, out char result))
            {
                return result;
            }
            return null;
        }

        public static char ToCharOrDefault(this string s, char defaultValue = '\0')
        {
            if (char.TryParse(s, out char result))
            {
                return result;
            }
            return defaultValue;
        }

        public static short ToInt16(this string s)
        {
            return short.Parse(s);
        }

        public static short? ToInt16OrNull(this string s)
        {
            if (short.TryParse(s, out short result))
            {
                return result;
            }
            return null;
        }

        public static short ToInt16OrDefault(this string s, short defaultValue = 0)
        {
            if (short.TryParse(s, out short result))
            {
                return result;
            }
            return defaultValue;
        }

        public static short ToInt16(this string s, IFormatProvider formatProvider)
        {
            return short.Parse(s, formatProvider);
        }

        public static short ToInt16(this string s, NumberStyles numberStyles)
        {
            return short.Parse(s, numberStyles);
        }

        public static short ToInt16(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return short.Parse(s, numberStyles, formatProvider);
        }

        public static short? ToInt16OrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (short.TryParse(s, numberStyles, formatProvider, out short result))
            {
                return result;
            }
            return null;
        }

        public static short? ToInt16OrNull(this string s, NumberStyles numberStyles)
        {
            if (short.TryParse(s, numberStyles, null, out short result))
            {
                return result;
            }
            return null;
        }

        public static short ToInt16OrDefault(this string s, NumberStyles numberStyles, short defaultValue = 0)
        {
            if (short.TryParse(s, numberStyles, null, out short result))
            {
                return result;
            }
            return defaultValue;
        }

        public static short ToInt16OrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, short defaultValue = 0)
        {
            if (short.TryParse(s, numberStyles, formatProvider, out short result))
            {
                return result;
            }
            return defaultValue;
        }

        public static ushort ToUInt16(this string s)
        {
            return ushort.Parse(s);
        }

        public static ushort? ToUInt16OrNull(this string s)
        {
            if (ushort.TryParse(s, out ushort result))
            {
                return result;
            }
            return null;
        }

        public static ushort ToUInt16OrDefault(this string s, ushort defaultValue = 0)
        {
            if (ushort.TryParse(s, out ushort result))
            {
                return result;
            }
            return defaultValue;
        }

        public static ushort ToUInt16(this string s, IFormatProvider formatProvider)
        {
            return ushort.Parse(s, formatProvider);
        }

        public static ushort ToUInt16(this string s, NumberStyles numberStyles)
        {
            return ushort.Parse(s, numberStyles);
        }

        public static ushort ToUInt16(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return ushort.Parse(s, numberStyles, formatProvider);
        }

        public static ushort? ToUInt16OrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (ushort.TryParse(s, numberStyles, formatProvider, out ushort result))
            {
                return result;
            }
            return null;
        }

        public static ushort? ToUInt16OrNull(this string s, NumberStyles numberStyles)
        {
            if (ushort.TryParse(s, numberStyles, null, out ushort result))
            {
                return result;
            }
            return null;
        }

        public static ushort ToUInt16OrDefault(this string s, NumberStyles numberStyles, ushort defaultValue = 0)
        {
            if (ushort.TryParse(s, numberStyles, null, out ushort result))
            {
                return result;
            }
            return defaultValue;
        }

        public static ushort ToUInt16OrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, ushort defaultValue = 0)
        {
            if (ushort.TryParse(s, numberStyles, formatProvider, out ushort result))
            {
                return result;
            }
            return defaultValue;
        }

        public static int ToInt32(this string s)
        {
            return int.Parse(s);
        }

        public static int? ToInt32OrNull(this string s)
        {
            if (int.TryParse(s, out int result))
            {
                return result;
            }
            return null;
        }

        public static int ToInt32OrDefault(this string s, int defaultValue = 0)
        {
            if (int.TryParse(s, out int result))
            {
                return result;
            }
            return defaultValue;
        }

        public static int ToInt32(this string s, IFormatProvider formatProvider)
        {
            return int.Parse(s, formatProvider);
        }

        public static int ToInt32(this string s, NumberStyles numberStyles)
        {
            return int.Parse(s, numberStyles);
        }

        public static int ToInt32(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return int.Parse(s, numberStyles, formatProvider);
        }

        public static int? ToInt32OrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (int.TryParse(s, numberStyles, formatProvider, out int result))
            {
                return result;
            }
            return null;
        }

        public static int? ToInt32OrNull(this string s, NumberStyles numberStyles)
        {
            if (int.TryParse(s, numberStyles, null, out int result))
            {
                return result;
            }
            return null;
        }

        public static int ToInt32OrDefault(this string s, NumberStyles numberStyles, int defaultValue = 0)
        {
            if (int.TryParse(s, numberStyles, null, out int result))
            {
                return result;
            }
            return defaultValue;
        }

        public static int ToInt32OrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, int defaultValue = 0)
        {
            if (int.TryParse(s, numberStyles, formatProvider, out int result))
            {
                return result;
            }
            return defaultValue;
        }

        public static uint ToUInt32(this string s)
        {
            return uint.Parse(s);
        }

        public static uint? ToUInt32OrNull(this string s)
        {
            if (uint.TryParse(s, out uint result))
            {
                return result;
            }
            return null;
        }

        public static uint ToUInt32OrDefault(this string s, uint defaultValue = 0u)
        {
            if (uint.TryParse(s, out uint result))
            {
                return result;
            }
            return defaultValue;
        }

        public static uint ToUInt32(this string s, IFormatProvider formatProvider)
        {
            return uint.Parse(s, formatProvider);
        }

        public static uint ToUInt32(this string s, NumberStyles numberStyles)
        {
            return uint.Parse(s, numberStyles);
        }

        public static uint ToUInt32(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return uint.Parse(s, numberStyles, formatProvider);
        }

        public static uint? ToUInt32OrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (uint.TryParse(s, numberStyles, formatProvider, out uint result))
            {
                return result;
            }
            return null;
        }

        public static uint? ToUInt32OrNull(this string s, NumberStyles numberStyles)
        {
            if (uint.TryParse(s, numberStyles, null, out uint result))
            {
                return result;
            }
            return null;
        }

        public static uint ToUInt32OrDefault(this string s, NumberStyles numberStyles, uint defaultValue = 0u)
        {
            if (uint.TryParse(s, numberStyles, null, out uint result))
            {
                return result;
            }
            return defaultValue;
        }

        public static uint ToUInt32OrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, uint defaultValue = 0u)
        {
            if (uint.TryParse(s, numberStyles, formatProvider, out uint result))
            {
                return result;
            }
            return defaultValue;
        }

        public static long ToInt64(this string s)
        {
            return long.Parse(s);
        }

        public static long? ToInt64OrNull(this string s)
        {
            if (long.TryParse(s, out long result))
            {
                return result;
            }
            return null;
        }

        public static long ToInt64OrDefault(this string s, long defaultValue = 0L)
        {
            if (long.TryParse(s, out long result))
            {
                return result;
            }
            return defaultValue;
        }

        public static long ToInt64(this string s, IFormatProvider formatProvider)
        {
            return long.Parse(s, formatProvider);
        }

        public static long ToInt64(this string s, NumberStyles numberStyles)
        {
            return long.Parse(s, numberStyles);
        }

        public static long ToInt64(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return long.Parse(s, numberStyles, formatProvider);
        }

        public static long? ToInt64OrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (long.TryParse(s, numberStyles, formatProvider, out long result))
            {
                return result;
            }
            return null;
        }

        public static long? ToInt64OrNull(this string s, NumberStyles numberStyles)
        {
            if (long.TryParse(s, numberStyles, null, out long result))
            {
                return result;
            }
            return null;
        }

        public static long ToInt64OrDefault(this string s, NumberStyles numberStyles, long defaultValue = 0L)
        {
            if (long.TryParse(s, numberStyles, null, out long result))
            {
                return result;
            }
            return defaultValue;
        }

        public static long ToInt64OrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, long defaultValue = 0L)
        {
            if (long.TryParse(s, numberStyles, formatProvider, out long result))
            {
                return result;
            }
            return defaultValue;
        }

        public static ulong ToUInt64(this string s)
        {
            return ulong.Parse(s);
        }

        public static ulong? ToUInt64OrNull(this string s)
        {
            if (ulong.TryParse(s, out ulong result))
            {
                return result;
            }
            return null;
        }

        public static ulong ToUInt64OrDefault(this string s, ulong defaultValue = 0uL)
        {
            if (ulong.TryParse(s, out ulong result))
            {
                return result;
            }
            return defaultValue;
        }

        public static ulong ToUInt64(this string s, IFormatProvider formatProvider)
        {
            return ulong.Parse(s, formatProvider);
        }

        public static ulong ToUInt64(this string s, NumberStyles numberStyles)
        {
            return ulong.Parse(s, numberStyles);
        }

        public static ulong ToUInt64(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return ulong.Parse(s, numberStyles, formatProvider);
        }

        public static ulong? ToUInt64OrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (ulong.TryParse(s, numberStyles, formatProvider, out ulong result))
            {
                return result;
            }
            return null;
        }

        public static ulong? ToUInt64OrNull(this string s, NumberStyles numberStyles)
        {
            if (ulong.TryParse(s, numberStyles, null, out ulong result))
            {
                return result;
            }
            return null;
        }

        public static ulong ToUInt64OrDefault(this string s, NumberStyles numberStyles, ulong defaultValue = 0uL)
        {
            if (ulong.TryParse(s, numberStyles, null, out ulong result))
            {
                return result;
            }
            return defaultValue;
        }

        public static ulong ToUInt64OrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, ulong defaultValue = 0uL)
        {
            if (ulong.TryParse(s, numberStyles, formatProvider, out ulong result))
            {
                return result;
            }
            return defaultValue;
        }

        public static float ToFloat(this string s)
        {
            return float.Parse(s);
        }

        public static float? ToFloatOrNull(this string s)
        {
            if (float.TryParse(s, out float result))
            {
                return result;
            }
            return null;
        }

        public static float ToFloatOrDefault(this string s, float defaultValue = 0f)
        {
            if (float.TryParse(s, out float result))
            {
                return result;
            }
            return defaultValue;
        }

        public static float ToFloat(this string s, IFormatProvider formatProvider)
        {
            return float.Parse(s, formatProvider);
        }

        public static float ToFloat(this string s, NumberStyles numberStyles)
        {
            return float.Parse(s, numberStyles);
        }

        public static float ToFloat(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return float.Parse(s, numberStyles, formatProvider);
        }

        public static float? ToFloatOrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (float.TryParse(s, numberStyles, formatProvider, out float result))
            {
                return result;
            }
            return null;
        }

        public static float? ToFloatOrNull(this string s, NumberStyles numberStyles)
        {
            if (float.TryParse(s, numberStyles, null, out float result))
            {
                return result;
            }
            return null;
        }

        public static float ToFloatOrDefault(this string s, NumberStyles numberStyles, float defaultValue = 0f)
        {
            if (float.TryParse(s, numberStyles, null, out float result))
            {
                return result;
            }
            return defaultValue;
        }

        public static float ToFloatOrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, float defaultValue = 0f)
        {
            if (float.TryParse(s, numberStyles, formatProvider, out float result))
            {
                return result;
            }
            return defaultValue;
        }

        public static double ToDouble(this string s)
        {
            return double.Parse(s);
        }

        public static double? ToDoubleOrNull(this string s)
        {
            if (double.TryParse(s, out double result))
            {
                return result;
            }
            return null;
        }

        public static double ToDoubleOrDefault(this string s, double defaultValue = 0.0)
        {
            if (double.TryParse(s, out double result))
            {
                return result;
            }
            return defaultValue;
        }

        public static double ToDouble(this string s, IFormatProvider formatProvider)
        {
            return double.Parse(s, formatProvider);
        }

        public static double ToDouble(this string s, NumberStyles numberStyles)
        {
            return double.Parse(s, numberStyles);
        }

        public static double ToDouble(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return double.Parse(s, numberStyles, formatProvider);
        }

        public static double? ToDoubleOrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (double.TryParse(s, numberStyles, formatProvider, out double result))
            {
                return result;
            }
            return null;
        }

        public static double? ToDoubleOrNull(this string s, NumberStyles numberStyles)
        {
            if (double.TryParse(s, numberStyles, null, out double result))
            {
                return result;
            }
            return null;
        }

        public static double ToDoubleOrDefault(this string s, NumberStyles numberStyles, double defaultValue = 0.0)
        {
            if (double.TryParse(s, numberStyles, null, out double result))
            {
                return result;
            }
            return defaultValue;
        }

        public static double ToDoubleOrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, double defaultValue = 0.0)
        {
            if (double.TryParse(s, numberStyles, formatProvider, out double result))
            {
                return result;
            }
            return defaultValue;
        }

        public static decimal ToDecimal(this string s)
        {
            return decimal.Parse(s);
        }

        public static decimal? ToDecimalOrNull(this string s)
        {
            if (decimal.TryParse(s, out decimal result))
            {
                return result;
            }
            return null;
        }

        public static decimal ToDecimalOrDefault(this string s, decimal defaultValue = default(decimal))
        {
            if (decimal.TryParse(s, out decimal result))
            {
                return result;
            }
            return defaultValue;
        }

        public static decimal ToDecimal(this string s, IFormatProvider formatProvider)
        {
            return decimal.Parse(s, formatProvider);
        }

        public static decimal ToDecimal(this string s, NumberStyles numberStyles)
        {
            return decimal.Parse(s, numberStyles);
        }

        public static decimal ToDecimal(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return decimal.Parse(s, numberStyles, formatProvider);
        }

        public static decimal? ToDecimalOrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (decimal.TryParse(s, numberStyles, formatProvider, out decimal result))
            {
                return result;
            }
            return null;
        }

        public static decimal? ToDecimalOrNull(this string s, NumberStyles numberStyles)
        {
            if (decimal.TryParse(s, numberStyles, null, out decimal result))
            {
                return result;
            }
            return null;
        }

        public static decimal ToDecimalOrDefault(this string s, NumberStyles numberStyles, decimal defaultValue = default(decimal))
        {
            if (decimal.TryParse(s, numberStyles, null, out decimal result))
            {
                return result;
            }
            return defaultValue;
        }

        public static decimal ToDecimalOrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, decimal defaultValue = default(decimal))
        {
            if (decimal.TryParse(s, numberStyles, formatProvider, out decimal result))
            {
                return result;
            }
            return defaultValue;
        }

        public static DateTime ToDateTime(this string s)
        {
            return DateTime.Parse(s);
        }

        public static DateTime? ToDateTimeOrNull(this string s)
        {
            if (DateTime.TryParse(s, out DateTime result))
            {
                return result;
            }
            return null;
        }

        public static DateTime ToDateTimeOrDefault(this string s, DateTime defaultValue = default(DateTime))
        {
            if (DateTime.TryParse(s, out DateTime result))
            {
                return result;
            }
            return defaultValue;
        }

        public static DateTime ToDateTime(this string s, IFormatProvider formatProvider)
        {
            return DateTime.Parse(s, formatProvider);
        }

        public static DateTime ToDateTime(this string s, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            return DateTime.Parse(s, formatProvider, dateTimeStyles);
        }

        public static DateTime? ToDateTimeOrNull(this string s, DateTimeStyles dateTimeStyles)
        {
            if (DateTime.TryParse(s, null, dateTimeStyles, out DateTime result))
            {
                return result;
            }
            return null;
        }

        public static DateTime ToDateTimeOrDefault(this string s, DateTimeStyles dateTimeStyles, DateTime defaultValue = default(DateTime))
        {
            if (DateTime.TryParse(s, null, dateTimeStyles, out DateTime result))
            {
                return result;
            }
            return defaultValue;
        }

        public static DateTime? ToDateTimeOrNull(this string s, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            if (DateTime.TryParse(s, formatProvider, dateTimeStyles, out DateTime result))
            {
                return result;
            }
            return null;
        }

        public static DateTime ToDateTimeOrDefault(this string s, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, DateTime defaultValue = default(DateTime))
        {
            if (DateTime.TryParse(s, formatProvider, dateTimeStyles, out DateTime result))
            {
                return result;
            }
            return defaultValue;
        }

        public static bool ToBoolean(this string s)
        {
            return bool.Parse(s);
        }

        public static bool? ToBooleanOrNull(this string s)
        {
            if (bool.TryParse(s, out bool result))
            {
                return result;
            }
            return null;
        }

        public static bool ToBooleanOrDefault(this string s, bool defaultValue = false)
        {
            if (bool.TryParse(s, out bool result))
            {
                return result;
            }
            return defaultValue;
        }

        #endregion

    }
}
