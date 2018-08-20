using System.Collections.Generic;
using System.Windows;

namespace Util.Controls
{
    /// <summary>
    /// Application资源处理辅助类。通过当前Application获取Style、DataTemplate，会静态缓存对象。
    /// </summary>
    public static class ResourceHelper
    {
        private static Dictionary<string, DataTemplate> _DataTemplates = new Dictionary<string, DataTemplate>();
        private static Dictionary<string, Style> _Styles = new Dictionary<string, Style>();
        /// <summary>
        /// 在全局资源中查找DataTemplate资源（会缓存）
        /// </summary>
        public static DataTemplate FindDataTemplate(string key)
        {
            if (!_DataTemplates.ContainsKey(key))
            {
                var dt = Application.Current.FindResource(key) as DataTemplate;
                _DataTemplates.Add(key, dt);
            }
            return _DataTemplates[key];
        }
        /// <summary>
        /// 在全局资源中查找Style资源（会缓存）
        /// </summary>
        public static Style FindStyle(string key)
        {
            if (!_Styles.ContainsKey(key))
            {
                var dt = Application.Current.FindResource(key) as Style;
                _Styles.Add(key, dt);
            }
            return _Styles[key];
        }
    }
}