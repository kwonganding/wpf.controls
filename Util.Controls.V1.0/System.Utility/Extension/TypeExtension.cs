using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace System
{
    /// <summary>
    /// 系统类型的扩展方法
    /// </summary>
    public static partial class TypeExtension
    {
        #region IsBetween: 比较指定数据是否在两者之间
        /// <summary>
        /// 比较指定数据是否在两者之间
        /// </summary>
        public static bool IsBetween<T>(this T @this, T lower, T upper, bool includeLower = true, bool includeUpper = true)
            where T : IComparable<T>
        {
            if (@this == null) return false;
            var flower = @this.CompareTo(lower);
            var fupper = @this.CompareTo(upper);
            return (includeLower && flower == 0) || (includeUpper && fupper == 0) || (flower > 0 && fupper < 0);
        }

        /// <summary>
        /// 比较指定数据是否在两者之间,指定比较器comparer
        /// </summary>
        public static bool IsBetween<T>(this T @this, T lower, T upper, IComparer<T> comparer, bool includeLower = true, bool includeUpper = true)
        {
            if (@this == null) return false;
            var flower = comparer.Compare(@this, lower);
            var fupper = comparer.Compare(@this, upper);
            return (includeLower && flower == 0) || (includeUpper && fupper == 0) || (flower > 0 && fupper < 0);
        }

        #endregion

        /// <summary>
        /// 判断类型是否（Nullable）可空类型
        /// </summary>
        public static bool IsNullableType(this Type @type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == Constants.NullableType;
        }

        /// <summary>
        /// 获取Nullable-T-的泛型参数类型
        /// </summary>
        public static Type GetNullableGenericType(this Type @type)
        {
            return @type.IsNullableType() ? type.GetGenericArguments()[0] : @type;
        }

        /// <summary>
        /// 安全的获取可空类型的值，若null返回默认值
        /// </summary>
        public static T SafeValue<T>(this T? @this)
            where T : struct
        {
            return @this ?? default(T);
        }
    }
}
