using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// StringBuilder扩展方法
    /// </summary>
    public static partial class TypeExtension
    {
        #region AppendKeyValueNewLine
        /// <summary>
        /// 添加键值数据及换行符
        /// </summary>
        public static void AppendKeyValueNewLine(this StringBuilder sb, string key, object value)
        {
            if (value == null || value.ToString().IsInvalid()) return;
            sb.Append(key).Append("：").Append(value).Append(Environment.NewLine);
        }

        /// <summary>
        /// 添加键值数据及换行符
        /// </summary>
        public static void AppendKeyValueNewLine(this StringBuilder sb, string key, Enum value)
        {
            sb.Append(key).Append("：").Append(value.GetDescription()).Append(Environment.NewLine);
        }

        #endregion

        #region AppendKeyValue
        /// <summary>
        /// 添加键值数据
        /// </summary>
        public static void AppendKeyValue(this StringBuilder builder, string key, object value, string end = " ")
        {
            if (value == null || value.ToString().IsInvalid()) return;

            builder.AppendFormat("{0}：{1}{2}", key, value, end);
        }

        /// <summary>
        /// 添加键值数据
        /// </summary>
        public static void AppendKeyValue(this StringBuilder sb, string key, Enum value, string end = " ")
        {
            sb.Append(key).Append("：").Append(value.GetDescription()).Append(end);
        }

        #endregion
    }
}
