using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Controls
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
                throw new ArgumentNullException(argumentName, $@"The argument[{argumentName}] cannot be null");
            }
        }

        #endregion

        #region ArgumentNotNullOrEmpty：验证参数是否为NULL或空字符串

        /// <summary>
        /// 验证参数是否为NULL或空字符串
        /// </summary>
        public static void ArgumentNotNullOrEmpty(string value, string argumentName)
        {
            if (value.IsInvalid())
            {
                throw new ArgumentNullException(argumentName, $@"The argument[{argumentName}] cannot be null or empty");
            }
        }

        #endregion
    }
}
