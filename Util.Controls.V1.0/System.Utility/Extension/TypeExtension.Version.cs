using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// 版本信息扩展处理方法
    /// </summary>
    public static partial class TypeExtension
    {
        /// <summary>
        /// 严重版本信息的有效性，true有效
        /// </summary>
        public static bool IsValid(this Version value)
        {
            if (value == null)
            {
                return false;
            }
            return value.Major > 0 || value.Minor > 0 || value.Build > 0 || value.Revision > 0;
        }

        /// <summary>
        /// 字符转换为安全的版本信息，若包含”-“则截断
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Version ToSafeVersion(this string value)
        {
            Version ver = new Version();
            if (value.IsInvalid())
            {
                return ver;
            }
            if (Version.TryParse(value, out ver))
            {
                return ver;
            }

            var result = Regex.Matches(value, @"(\d{1,8}\.){1,3}\d{1,8}", RegexOptions.IgnoreCase);
            if (result.Count == 0)
                return ver;
            return result[0].ToSafeString().ToSafeVersion();
        }

        /// <summary>
        /// 转换为短格式的版本字符描述，会去掉后面的0；
        /// </summary>
        public static string ToShortString(this Version value)
        {
            if (!value.IsValid())
            {
                return string.Empty;
            }
            if (value.Minor <= 0 && value.Build <= 0 && value.Revision <= 0)
            {
                return string.Format("{0}", value.Major);
            }
            if (value.Build <= 0 && value.Revision <= 0)
            {
                return string.Format("{0}.{1}", value.Major, value.Minor);
            }
            if (value.Revision <= 0)
            {
                return string.Format("{0}.{1}.{2}", value.Major, value.Minor, value.Build);
            }
            return string.Format("{0}.{1}.{2}.{3}", value.Major, value.Minor, value.Build, value.Revision);
        }
    }
}
