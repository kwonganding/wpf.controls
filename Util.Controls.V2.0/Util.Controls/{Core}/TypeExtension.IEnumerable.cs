using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Collections;
using System.Linq.Expressions;

namespace Util.Controls
{
    /// <summary>
    /// 集合类型的扩展：IEnumerable；IList
    /// </summary>
    public static partial class TypeExtension
    {
        #region IEnumerable<T>：ForEach：循环元素操作
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

        #region IEnumerable<T>：First（获取序列的第一个元素）

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

        #region IEnumerable：ConvertAll：转换为指定类型Ttarget的集合
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
                if (titem == null) throw new ArgumentException("ConvertAll Failure，Invalid Target:" + typeof(Ttarget));
                ress.Add(titem);
            }
            return ress;
        }

        /// <summary>
        /// 值类型转换
        /// </summary>
        public static IEnumerable<Ttarget> ConvertAllValueType<Ttarget>(this IEnumerable @this)
            where Ttarget : struct
        {
            if (@this == null) return null;
            var res = @this as IEnumerable<Ttarget>;
            if (res != null) return res;
            Collection<Ttarget> ress = new Collection<Ttarget>();
            foreach (var item in @this)
            {
                if (item is Ttarget)
                {
                    var titem = (Ttarget)item;
                    ress.Add(titem);
                }
            }
            return ress;
        }

        /// <summary>
        /// 转换到指定类型，并按需返回
        /// </summary>
        /// <typeparam name="Ttarget"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IEnumerable<Ttarget> ConvertAllyield<Ttarget>(this IEnumerable @this)
            where Ttarget : class
        {
            if (@this == null) yield break;
            foreach (var item in @this)
            {
                var titem = item as Ttarget;
                if (titem == null) continue;
                yield return titem;
            }
        }

        /// <summary>
        /// 该方法用于过滤不合法的数据
        /// </summary>
        /// <typeparam name="Ttarget"></typeparam>
        /// <param name="this"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<Ttarget> ConvertAllyield<Ttarget>(this IEnumerable @this, Func<Ttarget, bool> predicate) where Ttarget : class
        {
            if (@this == null) yield break;
            foreach (var item in @this)
            {
                var titem = item as Ttarget;
                if (predicate(titem))
                {
                    yield return titem;
                }
            }
        }

        #endregion

        #region IEnumerable：IEnumerator To List：转换为List集合
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

        #region IEnumerable<T>：IsValid：验证集合是否有效
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

        #region IEnumerable<T>：IsInvalid：验证集合是否无效
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

        #region ICollection<T>：AddRange：添加集合到ICollection
        /// <summary>
        /// AddRange：添加集合到ICollection
        /// </summary>
        public static void AddRange<T>(this ICollection<T> @this, IEnumerable<T> values)
        {
            var list = values.ToList();
            foreach (var v in list)
            {
                @this.Add(v);
            }
        }
        #endregion

        #region IList<T>：GetSafeValue:  IList:从列表、数组中安全的获取指定索引位置的数据
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

        #region ICollection<T>：RemoveWhere

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that removes value that satisfy the predicate.
        /// 从集合移除制定条件的项
        /// </summary>
        public static void RemoveWhere<T>(this ICollection<T> @this, Func<T, bool> predicate)
        {
            var list = @this.Where(predicate).ToList();
            foreach (T item in list)
            {
                @this.Remove(item);
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        public static void RemoveRange<T>(this ICollection<T> @this, IEnumerable<T> items)
        {
            var list = items.ToList();
            foreach (T item in list)
            {
                @this.Remove(item);
            }
        }

        /// <summary>
        /// 更新或添加,满足第一个条件的对象。index可以指定插入的索引位置，默认-1,追加到最后。
        /// </summary>
        public static void InsertOrUpdate<T>(this IList<T> @this, T model, Func<T, bool> predicate, int index = -1)
        {
            var item = @this.FirstOrDefault(predicate);
            if (item == null)
            {
                if (index >= 0)
                    @this.Insert(index, model);
                else
                    @this.Add(model);
            }
            else
            {
                index = @this.IndexOf(item);
                @this[index] = model;
            }
        }

        #endregion

        #region Dictionary<TKey, TValue>：GetSafeValue:从字典中安全获取键Key的值，若Key不存在，则返回默认值

        /// <summary>
        /// 从字典中安全获取键Key的值，若Key不存在，则返回默认值。
        /// </summary>
        /// <param name="dictionary">字段。</param>
        /// <param name="key">key</param>
        /// <param name="defalut">默认值。</param>
        /// <returns>返回value</returns>
        public static TValue GetSafeValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defalut = default(TValue))
        {
            if (dictionary.IsInvalid())
            {
                return defalut;
            }

            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return defalut;
        }

        #endregion

        #region IQueryable<T>：WhereIf

        /// <summary>
        /// 如果条件condition为ture时执行查询谓词predicate 
        /// predicate 查询谓词['predɪkət]
        /// </summary>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> @this, Expression<Func<T, bool>> predicate,
            bool condition)
        {
            return condition ? @this.Where(predicate) : @this;
        }

        #endregion

    }
}
