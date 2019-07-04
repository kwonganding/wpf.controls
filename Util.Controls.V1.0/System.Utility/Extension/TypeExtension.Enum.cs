using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Dynamic;

namespace System
{
    /// <summary>
    /// 枚举类型的扩展方法
    /// </summary>
    public static partial class TypeExtension
    {
        private static ConcurrentDictionary<Enum, string> _CacheDescriptions = new ConcurrentDictionary<Enum, string>();

        #region Enum: GetDescription

        /// <summary>
        /// 获取枚举的描述信息(Descripion)。
        /// 支持位域，如果是位域组合值，多个按分隔符组合。
        /// </summary>
        public static string GetDescription(this Enum @this)
        {
            return _CacheDescriptions.GetOrAdd(@this, (key) =>
            {
                var type = key.GetType();
                var field = type.GetField(key.ToString());
                //如果field为null则应该是组合位域值，
                return field == null ? key.GetDescriptions() : GetDescription(field);
            });
        }

        /// <summary>
        /// 获取位域枚举的描述，多个按分隔符组合
        /// </summary>
        public static string GetDescriptions(this Enum @this, string separator = ",")
        {
            var names = @this.ToString().Split(',');
            string[] res = new string[names.Length];
            var type = @this.GetType();
            for (int i = 0; i < names.Length; i++)
            {
                var field = type.GetField(names[i].Trim());
                if (field == null) continue;
                res[i] = GetDescription(field);
            }
            return string.Join(separator, res);
        }
        private static string GetDescription(FieldInfo field)
        {
            var att = System.Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false);
            return att == null ? string.Empty : ((DescriptionAttribute)att).Description;
        }

        #endregion

        #region Has：[位域]（位域枚举是否包含指定的值）

        /// <summary>
        /// 位域枚举是否包含指定的值，true：包含。
        /// .net中可以直接使用HasFlag判定
        /// </summary>
        public static bool Has(this Enum value, Enum target)
        {
            Guard.ArgumentNotNull(value, "Enum value");
            Guard.ArgumentNotNull(target, "Enum target");
            if (value.GetType() != target.GetType())
            {
                return false;
            }
            return (value.GetHashCode() & target.GetHashCode()) == target.GetHashCode();
        }
        #endregion

        #region Remove：[位域]除去位域枚举指定的一个枚举
        /// <summary>
        /// 除去位域枚举指定的一个枚举，返回处理后的枚举值
        /// </summary>
        public static T Remove<T>(this Enum value, Enum target)
        {
            Guard.ArgumentNotNull(value, "Enum value");
            Guard.ArgumentNotNull(target, "Enum target");

            var a = value.GetHashCode() & (~target.GetHashCode());
            return a.ToEnum<T>();
        }
        #endregion

        #region GetEnumNameByValue：通过枚举的值获取对应的枚举名称
        /// <summary>
        /// 通过枚举的值获取对应的枚举名称
        /// </summary>
        public static string GetEnumNameByValue<T>(this object value)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Enum.GetName(type, value);
            }
            throw new InvalidCastException("必须是枚举类型才能获取枚举名称。");
        }
        #endregion

        #region ToEnumByValue
        /// <summary>
        /// 转换为枚举对象(不适用于位域值)
        /// </summary>
        public static T ToEnumByValue<T>(this object value)
        {
            return value.GetEnumNameByValue<T>().ToEnum<T>();
        }
        #endregion

        #region ToEnum
        /// <summary>
        /// 将一个或多个枚举常数的名称或数字值的字符串表示转换成等效的枚举对象。
        /// </summary>
        public static T ToEnum<T>(this string name)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return (T)Enum.Parse(typeof(T), name, true);
            }
            throw new InvalidCastException("必须是枚举类型才能转换。");
        }

        /// <summary>
        /// 将一个或多个枚举常数的名称或数字值的字符串表示转换成等效的枚举对象。
        /// </summary>
        public static T ToEnum<T>(this int value)
        {
            return value.ToString().ToEnum<T>();
        }

        #endregion

        #region ToSafeEnum
        /// <summary>
        /// 将枚举常数的名称或数字值的字符串表示安全的转换成等效的枚举对象。
        /// </summary>
        public static T ToSafeEnum<T>(this string value, T defaultValue)
        {
            try
            {
                return value.ToEnum<T>();
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion

        #region ToEnum：[位域]把枚举数值或名称的数组抓换为枚举值
        /// <summary>
        /// 把枚举数值或名称的数组抓换为枚举值
        /// </summary>
        public static T ToEnum<T>(this string[] values)
        {
            //有效性验证
            Guard.ArgumentNotNull(values, "values");
            Type type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidCastException("必须是枚举类型才能转换。");
            }

            List<string> res = new List<string>();
            foreach (var item in values)
            {
                res.Add(item.ToEnum<T>().ToString());
            }
            //转换名字字符为枚举值，此处也可以使用单个枚举的的亦或来得到结果
            return string.Join(",", res.ToArray()).ToEnum<T>();
        }
        #endregion

        #region Enum: ToValueTextItems

        /// <summary>
        /// 获取枚举的值文本集合，参数emptyItem为默认添加的空节点文本值（值为-1）
        /// 属性：Text，Value
        /// </summary>
        public static List<dynamic> ToValueTextItems(this System.Enum value, string emptyItem = "")
        {
            Type type = value.GetType();
            return type.ToValueTextItems(emptyItem);
        }

        /// <summary>
        /// 获取枚举类型的枚举值、文本集合
        /// 属性：Text，Value
        /// </summary>
        public static List<dynamic> ToValueTextItems(this Type enumType, string emptyItem = "")
        {
            if (enumType == null || !enumType.IsEnum)
            {
                return null;
            }
            List<dynamic> items = new List<dynamic>();
            dynamic item = new ExpandoObject();
            item.Name = '1';
            if (!emptyItem.IsNullOrEmptyOrWhiteSpace())
            {
                item = new ExpandoObject();
                item.Text = emptyItem;
                item.Value = -1;
                items.Add(item);
            }
            //添加所有节点
            foreach (int i in Enum.GetValues(enumType))
            {
                string text = ((Enum)Enum.Parse(enumType, i.ToString())).GetDescription();
                if (!string.IsNullOrEmpty(text))
                {
                    item = new ExpandoObject();
                    item.Text = text;
                    item.Value = i;
                    items.Add(item);
                }
            }
            return items;
        }

        #endregion
    }
}
