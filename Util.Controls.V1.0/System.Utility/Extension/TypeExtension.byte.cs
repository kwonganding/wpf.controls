using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 对byte类型的扩展
    /// </summary>
    public static partial class TypeExtension
    {
        #region GetString： 把byte数组转换为制定编码类型的字符串
        /// <summary>
        /// 把byte数组转换为制定编码类型的字符串
        /// </summary>
        /// <param name="bytes">值</param>
        /// <param name="encode">编码类型</param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes, System.Text.Encoding encode)
        {
            if (bytes == null || bytes.Count() <= 0)
            {
                return string.Empty;
            }
            return encode.GetString(bytes);
        }
        #endregion

        #region GetString： 把byte数组转换为系统默认编码(System.Text.Encoding.Default)类型的字符串
        /// <summary>
        /// 把byte数组转换为系统默认编码(System.Text.Encoding.Default)类型的字符串
        /// </summary>
        /// <param name="bytes">值</param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes)
        {
            return bytes.GetString(System.Text.Encoding.Default);
        }
        #endregion

        #region ToBinary
        /// <summary>
        /// 转换为2进制字符串
        /// </summary>
        public static string ToBinary(this byte value)
        {
            return Convert.ToString(value, 2);
        }

        /// <summary>
        /// 转换为2进制字符串
        /// </summary>
        public static string ToBinary(this byte[] values, string separator = " ")
        {
            if (values == null || values.Length <= 0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var b in values)
            {
                sb.Append(b.ToBinary()).Append(separator);
            }
            return sb.ToString();
        }

        #endregion

        #region To16Bland
        /// <summary>
        /// 转换为16进制字符串
        /// </summary>
        public static string To16Bland(this byte value)
        {
            return value.ToString("X2");
        }

        /// <summary>
        /// 转换为16进制字符串
        /// </summary>
        public static string To16Bland(this byte[] values, string separator = " ")
        {
            if (values == null || values.Length <= 0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var b in values)
            {
                sb.Append(b.To16Bland()).Append(separator);
            }
            return sb.ToString();
        }
        #endregion
    }

}

