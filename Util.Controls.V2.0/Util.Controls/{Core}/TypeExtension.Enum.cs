using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Util.Controls
{
    /// <summary>
    /// TypeExtension for Enum
    /// </summary>
    public static partial class TypeExtension
    {
        #region GetDescription

        /// <summary>
        /// 获取枚举的描述信息(Descripion)。
        /// 支持位域，如果是位域组合值，多个按分隔符组合。
        /// </summary>
        public static string GetDescription(this Enum @this)
        {
            return _ConcurrentDictionary.GetOrAdd(@this, (key) =>
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
            var att = Attribute.GetCustomAttribute(field, typeof (DescriptionAttribute), false);
            return att == null ? field.Name : ((DescriptionAttribute) att).Description;
        }

        private static ConcurrentDictionary<Enum, string> _ConcurrentDictionary =
            new ConcurrentDictionary<Enum, string>();

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
    }
}
