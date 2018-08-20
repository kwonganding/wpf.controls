using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace System
{
    /// <summary>
    /// TypeExtension for String
    /// </summary>
    public static partial class TypeExtension
    {
        #region IsValid 判定是否有效

        /// <summary>
        /// 判定是否有效字符
        /// true：有效；
        /// false：null、empty、空字符
        /// </summary>
        public static bool IsValid(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
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
            return string.IsNullOrWhiteSpace(value);
        }
        #endregion

        #region ToSafeString
        /// <summary>
        /// 安全的字符转换
        /// </summary>
        public static string ToSafeString(this object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }

        /// <summary>
        /// 安全的布尔值转换
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ToSafeBool(this object obj)
        {
            var str = obj.ToSafeString();
            if (str.IsInvalid())
                return false;
            bool result = false;
            bool.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// 安全的double转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToSafeDouble(this object value)
        {
            if (value == null) return 0.0;
            if (value.ToSafeString().IsInvalid()) return 0.0;
            var result = 0.0;
            double.TryParse(value.ToSafeString(), out result);
            return result;
        }

        public static int ToSafeInt(this object value)
        {
            return value.ToSafeString().ToSafeInt();
        }


        /// <summary>
        /// 截取限定长度内的值
        /// </summary>
        /// <param name="value">原始值</param>
        /// <param name="length">限定长度</param>
        /// <returns></returns>
        public static string ToClipLength(this string value, int length)
        {
            if (string.IsNullOrEmpty(value) || value.Length < length)
            {
                return value;
            }

            return value.Substring(0, length);
        }

        #endregion

        #region ToCamelCase

        /// <summary>
        /// 转换为驼峰命名规则的字符串
        /// </summary>
        public static string ToCamelCase(this string @this)
        {
            if (string.IsNullOrEmpty(@this) || !char.IsUpper(@this[0]))
            {
                return @this;
            }
            char[] array = @this.ToCharArray();
            int num = 0;
            while (num < array.Length && (num != 1 || char.IsUpper(array[num])))
            {
                bool flag = num + 1 < array.Length;
                if ((num > 0 & flag) && !char.IsUpper(array[num + 1]))
                {
                    break;
                }
                array[num] = char.ToLower(array[num], CultureInfo.InvariantCulture);
                num++;
            }
            return new string(array);
        }

        #endregion
    }
}
