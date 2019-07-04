#region 命名空间引用

using System.Configuration;

#endregion

namespace System.Utility.Helper
{
    /// <summary>
    /// 配置公共操作类
    /// </summary>
    public class Config
    {
        #region ContainsAppSettings
        /// <summary>
        /// 检测Web.config(App.config)的appSettings配置节是否包含指定键,包含返回true
        /// </summary>
        /// <param name="key">键名</param>
        public static bool ContainsAppSettings(string key)
        {
            try
            {
                return GetAppSettings(key).Length > 0;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region GetAppSettings
        /// <summary>
        /// 读取Web.config(App.config)的appSettings配置节的值
        /// </summary>
        /// <param name="key">键名</param>        
        public static string GetAppSettings(string key)
        {
            //有效性验证
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            //获取配置值
            string value = ConfigurationManager.AppSettings[key];

            //返回值
            return string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        }
        #endregion
    }
}
