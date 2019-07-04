using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Utility.Resources;

namespace System
{
    /// <summary>
    /// (n. 守卫)提供全局通用验证
    /// </summary>
    public static class Guard
    {
        #region ArgumentNotNull：验证参数是否为NULL
        /// <summary>
        /// 验证参数是否为NULL
        /// </summary>
        public static void ArgumentNotNull(object value, string argumentName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(argumentName,
                    string.Format(DefaultErrorMessages.ArgumentNotNull, argumentName));
            }
        }
        #endregion

        #region ArgumentNotNullOrEmpty：验证参数是否为NULL或空字符串
        /// <summary>
        /// 验证参数是否为NULL或空字符串
        /// </summary>
        public static void ArgumentNotNullOrEmpty(string value, string argumentName)
        {
            if (value.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(argumentName,
                    string.Format(DefaultErrorMessages.ArgumentNotNullOrEmpty, argumentName));
            }
        }
        #endregion
    }
}
