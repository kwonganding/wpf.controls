using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.IO;

namespace System.Utility.Helper
{
    /// <summary>
    /// 反射操作辅助类
    /// </summary>
    public class Reflection
    {
        #region SettingObjectValue：设置对象属性的值
        /// <summary>
        /// 设置对象属性的值
        /// </summary>
        public static void SettingObjectValue<T>(T obj, string propertyName, object value)
        {
            //1.validation
            if (obj == null || string.IsNullOrEmpty(propertyName) || value == null)
            {
                return;
            }
            //2.find properties 
            System.Reflection.PropertyInfo[] pis = obj.GetType().GetProperties();
            int len = pis == null ? 0 : pis.Length;
            //3.setting value
            for (int i = 0; i < len; i++)
            {
                if (string.Equals(pis[i].Name, propertyName))
                {
                    pis[i].SetValue(obj, value, null);
                    return;
                }
            }
        }
        #endregion

        #region SettingObjectValues：批量设置对象属性的值
        /// <summary>
        /// 批量设置对象属性的值
        /// </summary>
        public static void SettingObjectValues<T>(T obj, string[] propertyNames, object[] values)
        {
            //1.validation
            if (obj == null || propertyNames.IsInvalid() || values.IsInvalid())
            {
                return;
            }
            //2.find properties 
            System.Reflection.PropertyInfo[] pis = obj.GetType().GetProperties();
            if (pis.IsInvalid()) return;
            var plen = propertyNames.Length;
            //3.set values
            for (int i = 0; i < plen; i++)
            {
                var fpi = pis.Where(s => string.Equals(s.Name, propertyNames[i]));
                if (fpi.IsInvalid()) continue;
                var pi = fpi.First();
                pi.SetValue(propertyNames[i], values[i], null);
            }
        }
        #endregion

        #region GetObjectValue：获取指定属性的值
        /// <summary>
        /// 获取指定属性的值
        /// </summary>
        public static object GetObjectValue<T>(T obj, string propertyName)
        {
            //1.validation
            if (obj == null || string.IsNullOrEmpty(propertyName))
            {
                return null;
            }
            //2.find properties 
            System.Reflection.PropertyInfo[] pis = obj.GetType().GetProperties();
            int len = pis == null ? 0 : pis.Length;
            //3.get value
            for (int i = 0; i < len; i++)
            {
                if (string.Equals(pis[i].Name, propertyName))
                {
                    var item = pis[i].GetValue(obj, null);
                    return item;
                }
            }
            return null;
        }

        #endregion

        #region GetResourceFile

        /// <summary>
        /// 从程序集的资源文件中获取文本内容
        /// </summary>
        /// <param name="fileUrl">The file URL.</param>
        public static string GetResourceFile(string fileUrl)
        {
            using (
                StreamReader sr =
                    new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(fileUrl))
                )
            {
                return sr.ReadToEnd();
            }
        }

        #endregion
    }
}
