using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Util.Controls
{
    /// <summary>
    /// 系统类型的扩展方法
    /// </summary>
    public static partial class TypeExtension
    {
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

        #region ToDouble
        /// <summary>
        /// 转换为double
        /// </summary>
        public static double ToDouble(this string value)
        {
            if (value.IsInvalid()) return 0.0;
            double result;
            if (double.TryParse(value, out result))
            {
                return result;
            }
            throw new InvalidCastException("不能将字符串\"" + value + "\"转换为double数字。");
        }
        #endregion
    }
}
