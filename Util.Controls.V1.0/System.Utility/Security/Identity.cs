using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace System.Utility.Security
{
    /// <summary>
    /// 身份标识基类
    /// </summary>
    public class Identity : IIdentity
    {
        public Identity()
        {
            this.IsAuthenticated = false;
            this.Name = string.Empty;
        }

        /// <summary>
        /// 认证类型
        /// </summary>
        public string AuthenticationType
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// 是否通过身份认证
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
    }
}
