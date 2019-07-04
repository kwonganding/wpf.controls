using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Collections;

namespace System
{
    /// <summary>
    /// 集合类型的扩展：IEnumerable；IList
    /// </summary>
    public static partial class TypeExtension
    {
        #region ForEach：循环元素操作
        /// <summary>
        /// 循环元素操作.
        /// 此方法请谨慎使用，特别有钱提示：此处不支持对集合、集合内对象的修改，集合对象的属性可以修改。
        /// 不要问我为什么，只是你还不了解yield.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> souce, Action<T> action)
        {
            if (souce.IsInvalid())
            {
                return;
            }
            foreach (var item in souce)
            {
                action(item);
            }
        }
        #endregion

        #region First（获取序列的第一个元素）

        /// <summary>
        /// 获取序列的第一个元素
        /// </summary>
        public static T FindFirst<T>(this IEnumerable<T> source)
            where T : class
        {
            if (source.IsInvalid()) return null;
            T res;
            foreach (var item in source)
            {
                res = item;
                return res;
            }
            return null;
        }

        /// <summary>
        /// 获取序列的第一个元素
        /// </summary>
        public static object FindFirst(this IEnumerable source)
        {
            if (source.IsInvalid()) return null;
            foreach (var item in source)
            {
                return item;
            }
            return null;
        }

        #endregion

        #region ConvertAll：转换为指定类型Ttarget的集合
        /// <summary>
        /// 转换为指定类型Ttarget的集合
        /// </summary>
        public static IEnumerable<Ttarget> ConvertAll<Ttarget>(this IEnumerable @this)
            where Ttarget : class
        {
            if (@this == null) return null;
            var res = @this as IEnumerable<Ttarget>;
            if (res != null) return res;
            Collection<Ttarget> ress = new Collection<Ttarget>();
            foreach (var item in @this)
            {
                var titem = item as Ttarget;
                if (titem == null) throw new ArgumentException("invalid Target:" + typeof(Ttarget));
                ress.Add(titem);
            }
            return ress;
        }

        public static IEnumerable<Ttarget> ConvertAllyield<Ttarget>(this IEnumerable @this)
            where Ttarget : class
        {
            if (@this == null) yield break;
            foreach (var item in @this)
            {
                var titem = item as Ttarget;
                yield return titem;
            }
        }

        #endregion

        #region IEnumerator To List：转换为List集合
        /// <summary>
        /// 转换为List集合
        /// </summary>
        public static List<T> ToList<T>(this IEnumerator<T> source)
        {
            var res = new List<T>();
            if (source == null)
            {
                return res;
            }
            while (source.MoveNext())
            {
                res.Add(source.Current);
            }
            return res;
        }
        #endregion

        #region IsValid：验证集合是否有效
        /// <summary>
        /// 验证集合是否有效(是否为空，是否包含元素，有效返回true)
        /// 注意：判断是否包含元素，尽量使用Enumerable.Any，而不要用Linq中的Count
        /// </summary>
        public static bool IsValid<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        /// <summary>
        /// 验证集合是否有效(是否为空，是否包含元素，有效返回true)
        /// </summary>
        public static bool IsValid(this IEnumerable @this)
        {
            if (@this == null) return false;
            foreach (var item in @this)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region IsInvalid：验证集合是否无效
        /// <summary>
        /// 验证集合是否无效(是否为空，是否包含元素，若无效返回true)
        /// </summary>
        public static bool IsInvalid<T>(this IEnumerable<T> source)
        {
            return !source.IsValid();
        }

        /// <summary>
        /// 验证集合是否无效(是否为空，是否包含元素，若无效返回true)
        /// </summary>
        public static bool IsInvalid(this IEnumerable @this)
        {
            return !@this.IsValid();
        }

        #endregion

        #region AddRange：添加集合到ICollection
        /// <summary>
        /// AddRange：添加集合到ICollection
        /// </summary>
        public static void AddRange<T>(this ICollection<T> @this, IEnumerable<T> values)
        {
            foreach (var v in values)
            {
                @this.Add(v);
            }
        }
        #endregion

        #region GetSafeValue:  IList:从列表、数组中安全的获取指定索引位置的数据
        /// <summary>
        /// 从列表、数组中安全的获取指定索引位置的数据。
        /// 若数据为空、超出索引返回默认值。
        /// </summary>
        public static T GetSafeValue<T>(this IList<T> @this, int index, T defaultValue = default(T))
        {
            if (@this.IsInvalid()) return defaultValue;
            if (@this.Count > index) return defaultValue;
            return @this[index];
        }
        #endregion
    }
}
