using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Boolean扩展
    /// </summary>
    public static partial class TypeExtension
    {
        #region String:ToBoolean
        /// <summary>
        /// 转换为boolean类型
        /// </summary>
        public static bool ToBoolean(this string value)
        {
            bool result;
            if (Boolean.TryParse(value, out result))
            {
                return result;
            }
            throw new InvalidCastException("\"" + value + "\"不是有效的boolean，请确认。");
        }
        #endregion

        #region String:ToBoolean
        /// <summary>
        /// 转换为安全的boolean类型
        /// </summary>
        public static bool ToSafeBoolean(this string value, bool defaultValue = false)
        {
            bool result;
            if (Boolean.TryParse(value, out result))
            {
                return result;
            }
            return defaultValue;
        }
        #endregion

        #region ToCNString

        /// <summary>
        /// 转换为中文
        /// </summary>
        public static string ToCNString(this Boolean value)
        {
            return value ? "是" : "否";
        }

        /// <summary>
        /// 转换为中文
        /// </summary>
        public static string ToCNString(this bool? value)
        {
            return value.HasValue ? value.ToString() : string.Empty;
        }
        #endregion
    }
}
