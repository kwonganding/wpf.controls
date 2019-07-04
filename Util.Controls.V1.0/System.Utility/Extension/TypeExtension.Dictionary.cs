using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;

namespace System
{
    /// <summary>
    /// Dictionary、Hashtable相关的扩展
    /// </summary>
    public static partial class TypeExtension
    {
        #region HttpContext: ToDictionary

        /// <summary>
        /// 从HttpContext的请求信息中Request获取的数据，并通过字典返回键值数据
        /// </summary>
        public static Dictionary<string, string> ToDictionaryH(this System.Web.HttpContext context)
        {
            //1.validation
            if (context == null || context.Request == null)
            {
                return new Dictionary<string, string>();
            }
            //2.get nameValuecollection from HttpContext
            string httpMode = context.Request.HttpMethod;
            System.Collections.Specialized.NameValueCollection values = new System.Collections.Specialized.NameValueCollection();
            if (httpMode.Equals(EnumHttpMode.Post.GetDescription()))
            {
                values = context.Request.Form;
            }
            else if (httpMode.Equals(EnumHttpMode.Get.GetDescription()))
            {
                values = context.Request.QueryString;
            }
            //convert to dictionary
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (values != null && values.Count > 0)
            {
                foreach (string key in values)
                {
                    dic.Add(key, values[key]);
                }
            }
            //return result
            return dic;
        }

        #endregion

        #region GetSafeValue:从字典中安全获取键Key的值，若Key不存在，则返回默认值

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
    }
}
