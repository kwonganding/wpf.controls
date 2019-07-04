
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace System
{
    /// <summary>
    /// xml操作扩展
    /// </summary>
    public static partial class TypeExtension
    {
        /// <summary>
        /// 安全的获取节点属性值
        /// </summary>
        public static string GetSafeAttributeValue(this XmlNode node, string attribute)
        {
            if (node == null)
            {
                return string.Empty;
            }
            if (node.Attributes != null)
            {
                var att = node.Attributes[attribute];
                if (att == null)
                {
                    return string.Empty;
                }
                return att.Value.Trim();
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取XML枚举属性值，枚举支持位域
        /// </summary>
        public static TEnum GetSafeAttributeEnum<TEnum>(this XmlNode node, string attribute, TEnum defaultValue)
        {
            try
            {
                var value = node.GetSafeAttributeValue(attribute);
                if (value.IsValid()) return value.Replace("，", ",").ToEnum<TEnum>();
            }
            catch
            {
                // ignored
            }
            return defaultValue;
        }
    }
}
