using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 浮点类型的扩展方法
    /// </summary>
    public static partial class TypeExtension
    {
        #region ToDecimal
        /// <summary>
        /// 转换为Decimal
        /// </summary>
        public static Decimal ToDecimal(this string value)
        {
            Decimal result;
            if (Decimal.TryParse(value, out result))
            {
                return result;
            }
            throw new InvalidCastException("不能将字符串\"" + value + "\"转换为Decimal数字。");
        }
        #endregion

        #region ToSafeDecimal
        /// <summary>
        /// 转换为安全的Decimal
        /// </summary>
        public static Decimal ToSafeDecimal(this string value)
        {
            Decimal result;
            if (Decimal.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region ToDouble
        /// <summary>
        /// 转换为double
        /// </summary>
        public static double ToDouble(this string value)
        {
            double result;
            if (double.TryParse(value, out result))
            {
                return result;
            }
            throw new InvalidCastException("不能将字符串\"" + value + "\"转换为double数字。");
        }
        #endregion

        #region ToSafeDouble
        /// <summary>
        /// 转换为安全的Double
        /// </summary>
        public static double ToSafeDouble(this string value)
        {
            double result;
            if (double.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        #endregion
    }
}
