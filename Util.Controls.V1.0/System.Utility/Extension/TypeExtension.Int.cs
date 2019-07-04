using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 整数类型的扩展方法
    /// </summary>
    public static partial class TypeExtension
    {
        #region ToSafeInt16
        /// <summary>
        /// 转换为短整形数字
        /// </summary>
        public static Byte ToSafeByte(this string value)
        {
            Byte result;
            if (Byte.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region ToInt16
        /// <summary>
        /// 转换为短整形数字
        /// </summary>
        public static Int16 ToInt16(this string value)
        {
            Int16 result;
            if (Int16.TryParse(value, out result))
            {
                return result;
            }
            throw new InvalidCastException("不能将字符串\"" + value + "\"转换为短整形数字。");
        }
        #endregion

        #region ToSafeInt16
        /// <summary>
        /// 转换为短整形数字
        /// </summary>
        public static Int16 ToSafeInt16(this string value)
        {
            Int16 result;
            if (Int16.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region ToInt
        /// <summary>
        /// 转换为整形数字
        /// </summary>
        public static Int32 ToInt(this string value)
        {
            Int32 result;
            if (Int32.TryParse(value, out result))
            {
                return result;
            }
            throw new InvalidCastException("不能将字符串\"" + value + "\"转换为整形数字。");
        }
        #endregion

        #region ToSafeInt
        /// <summary>
        /// 转换为安全的Int32类型，默认为0
        /// </summary>
        public static int ToSafeInt(this string value)
        {
            if (value.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
            {
                return (Int32)value.ToSafeDouble();
            }
            Int32 result;
            if (Int32.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region ToInt64
        /// <summary>
        /// 转换为长整形数字
        /// </summary>
        public static Int64 ToInt64(this string value)
        {
            Int64 result;
            if (Int64.TryParse(value, out result))
            {
                return result;
            }
            throw new InvalidCastException("不能将字符串\"" + value + "\"转换为长整形数字。");
        }
        #endregion

        #region ToSafeInt64
        /// <summary>
        /// 转换为长整形数字
        /// </summary>
        public static Int64 ToSafeInt64(this string value)
        {
            if (value.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
            {
                return (Int64)value.ToSafeDouble();
            }
            Int64 result;
            if (Int64.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// 转换为长整形数字
        /// </summary>
        public static ulong ToSafeUlong(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }
            if (value.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
            {
                return (ulong)value.ToSafeDouble();
            }
            ulong result;
            if (ulong.TryParse(value, out result))
            {
                return result;
            }

            return 0;
        }
        #endregion

        #region IsGreaterThanZero
        /// <summary>
        /// 判断是否为一个大于0的整数
        /// </summary>
        public static bool IsGreaterThanZero(this int value)
        {
            return value > 0;
        }
        #endregion

        #region IsGreaterThanZero
        /// <summary>
        /// 判断是否为一个大于0的整数
        /// </summary>
        public static bool IsGreaterThanZero(this int? value)
        {
            if (value == null)
            {
                return false;
            }
            return value > 0;
        }
        #endregion

        #region ToByte
        /// <summary>
        /// 转换为byte值,value应小于等于256
        /// </summary>
        public static byte ToByte(this int value)
        {
            try
            {
                return BitConverter.GetBytes(value)[0];
            }
            catch
            {
                return 0;
            }
        }
        #endregion
    }
}
