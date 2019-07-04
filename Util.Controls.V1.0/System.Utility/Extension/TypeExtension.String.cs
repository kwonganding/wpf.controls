using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Security.Cryptography;

namespace System
{
    /// <summary>
    /// 字符类型的扩展方法
    /// </summary>
    public static partial class TypeExtension
    {
        #region ToBytes：把字符转换为指定编码的序列
        /// <summary>
        /// 把字符转换为指定编码的序列
        /// </summary>
        /// <param name="value">字符值</param>
        /// <param name="encode">编码</param>
        /// <returns>序列</returns>
        public static byte[] ToBytes(this string value, System.Text.Encoding encode)
        {
            return encode.GetBytes(value);
        }
        #endregion

        #region ToBytes：把字符转换为默认编码(System.Text.Encoding.Default)的序列
        /// <summary>
        /// 把字符转换为默认编码(System.Text.Encoding.Default)的序列
        /// </summary>
        public static byte[] ToBytes(this string value)
        {
            return value.ToBytes(System.Text.Encoding.Default);
        }
        #endregion

        #region ToASCIIBytes：把字符转换为ASCII编码序列
        /// <summary>
        /// 把字符转换为ASCII编码序列
        /// </summary>
        public static byte[] ToASCIIBytes(this string value)
        {
            return value.ToBytes(System.Text.Encoding.ASCII);
        }
        #endregion

        #region Encode：把字符转换为制定的编码格式的字符
        /// <summary>
        /// 把字符转换为制定的编码格式的字符
        /// </summary>
        public static string Encode(this string value, System.Text.Encoding encode)
        {
            Guard.ArgumentNotNull(value, "value");
            var utf16 = System.Text.Encoding.Unicode;
            var bytes = System.Text.Encoding.Convert(utf16, encode, value.ToBytes(utf16));
            return bytes.GetString(encode);
        }
        #endregion

        #region Encode：把字符转换为制定的编码格式的字符
        /// <summary>
        /// 把字符转换为制定的编码格式的字符
        /// </summary>
        public static string Encode(this string value, System.Text.Encoding sencode, System.Text.Encoding tencode)
        {
            Guard.ArgumentNotNull(value, "value");
            var bytes = System.Text.Encoding.Convert(sencode, tencode, value.ToBytes(sencode));
            return bytes.GetString(tencode);
        }
        #endregion

        #region string:Url(Html) string Encoding and Decode

        #region UrlEncode
        /// <summary>
        /// Url地址字符编码
        /// </summary>
        public static string UrlEncode(this string source, EnumEncodingType encodingType = EnumEncodingType.UTF_8)
        {
            if (!string.IsNullOrEmpty(source))
            {
                var chs = Encoding.GetEncoding(encodingType.GetDescription());
                return System.Web.HttpUtility.UrlEncode(source, chs);
            }
            return string.Empty;
        }
        #endregion

        #region UrlDecode
        /// <summary>
        /// Url 地址字符解码
        /// </summary>
        public static string UrlDecode(this string source, EnumEncodingType encodingType = EnumEncodingType.UTF_8)
        {
            if (!string.IsNullOrEmpty(source))
            {
                var chs = Encoding.GetEncoding(encodingType.GetDescription());
                return System.Web.HttpUtility.UrlDecode(source, chs);
            }
            return string.Empty;
        }
        #endregion

        #region HtmlDecode
        /// <summary>
        /// HTML解码
        /// </summary>
        public static string HtmlDecode(this string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                return System.Web.HttpUtility.HtmlDecode(source);
            }
            return string.Empty;
        }
        #endregion

        #region HtmlEncode
        /// <summary>
        /// HTML编码
        /// </summary>
        public static string HtmlEncode(this string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                return System.Web.HttpUtility.HtmlEncode(source);
            }
            return string.Empty;
        }
        #endregion

        #endregion

        #region IsNullOrEmpty
        /// <summary>
        /// 验证字符是否为NULL或空字符
        /// </summary>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        #endregion

        #region IsNullOrEmptyOrWhiteSpace
        /// <summary>
        /// 验证字符是否为NULL或空字符、空格字符
        /// </summary>
        public static bool IsNullOrEmptyOrWhiteSpace(this string value)
        {
            var flag = value.IsNullOrEmpty();
            if (!flag)
            {
                flag = value.Trim().IsNullOrEmpty();
            }
            return flag;
        }
        #endregion

        #region IsValid 判定是否有效
        /// <summary>
        /// 判定是否有效字符
        /// true：有效；
        /// false：null、empty、空字符
        /// </summary>
        public static bool IsValid(this string value)
        {
            return !value.IsNullOrEmptyOrWhiteSpace();
        }
        #endregion

        #region IsInvalid 判定是否是无效字符
        /// <summary>
        /// 判定是否是无效字符
        /// true：无效；
        /// false：有效
        /// </summary>
        public static bool IsInvalid(this string value)
        {
            return value.IsNullOrEmptyOrWhiteSpace();
        }
        #endregion

        #region ToSafeString
        /// <summary>
        /// 安全的字符转换
        /// </summary>
        public static string ToSafeString(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return obj.ToString();
        }
        #endregion

        #region ToReverse：字符串翻转
        /// <summary>
        /// 字符串翻转
        /// </summary>
        public static string ToReverse(this string obj)
        {
            char[] arr = obj.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        #endregion

        #region TrimEnd（去掉字符末尾指定字符值）

        /// <summary>
        /// 去掉字符末尾指定字符值
        /// </summary>
        public static string TrimEnd(this string value, string end, StringComparison comparison = StringComparison.Ordinal)
        {
            if (value.IsInvalid())
            {
                return value;
            }
            if (end.IsInvalid())
            {
                return value.TrimEnd();
            }
            if (!value.EndsWith(end, comparison))
            {
                return value;
            }
            var index = value.LastIndexOf(end, comparison);
            return value.Substring(0, index);
        }

        #endregion

        #region TrimStart:去掉开始位置指定字符串
        /// <summary>
        /// 去掉开始位置指定字符串
        /// </summary>
        /// <returns></returns>
        public static string TrimStart(this string value, string star)
        {
            if (star.IsInvalid())
            {
                return value.TrimStart();
            }
            if (!value.StartsWith(star))
            {
                return value;
            }
            return value.Substring(star.Length);
        }
        #endregion

        #region IsMatch/Match：正则验证
        /// <summary>
        ///  正则验证字符是否符合规则，返回true，符合规则，否则不符合。
        /// </summary>
        public static bool IsMatch(this string source, String pattern, RegexOptions options)
        {
            if (source.IsInvalid()) return false;
            return Regex.IsMatch(source, pattern, options);
        }

        /// <summary>
        ///  正则验证字符是否符合规则（默认单行模式，并忽略大小写），返回true，符合规则，否则不符合。
        /// </summary>
        public static bool IsMatch(this string source, String pattern)
        {
            if (source.IsInvalid()) return false;
            return IsMatch(source, pattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 对字符串进行正则匹配，默认单行模式，并忽略大小写。
        /// </summary>
        public static Match Match(this String source, String pattern)
        {
            return Match(source, pattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 对字符串进行正则匹配
        /// </summary>
        public static Match Match(this String source, String pattern, RegexOptions options)
        {
            return Regex.Match(source, pattern, options);
        }

        #endregion

        #region Contains
        /// <summary>
        /// 判断字符串是否在另一个字符串中包含
        /// 支持某些重载要使用的区域、大小写和排序规则。
        /// </summary>
        public static bool Contains(this string value, string match, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return value.IndexOf(match, comparison) > -1;
        }
        #endregion

        #region IsChinese

        private const int LOW_CHINESE = 0x4e00;
        private const int HEIGHT_CHINESE = 0x9fbb;

        /// <summary>
        /// 判断字符是否中文
        /// </summary>
        public static bool IsChinese(this char value)
        {
            return value >= LOW_CHINESE && value <= HEIGHT_CHINESE;
        }
        #endregion

        #region IsHasChinese
        /// <summary>
        /// 验证字符串中是否包含中文字符
        /// </summary>
        public static bool IsHasChinese(this string value)
        {
            return value.Any(c => c.IsChinese());
        }

        #endregion

        #region Format：字符串参数化

        /// <summary>
        /// 字符串参数化：一个参数
        /// </summary>
        public static string Format(this string @this, object arg0)
        {
            return string.Format(@this, arg0);
        }

        /// <summary>
        /// 字符串参数化：2个参数
        /// </summary>
        public static string Format(this string @this, object arg0, object arg1)
        {
            return string.Format(@this, arg0, arg1);
        }

        /// <summary>
        /// 字符串参数化：3个参数
        /// </summary>
        public static string Format(this string @this, object arg0, object arg1, object arg2)
        {
            return string.Format(@this, arg0, arg1, arg2);
        }

        /// <summary>
        /// 字符串参数化：多个参数
        /// </summary>
        public static string Format(this string @this, params object[] arg0)
        {
            return string.Format(@this, arg0);
        }
        #endregion
    }
}
