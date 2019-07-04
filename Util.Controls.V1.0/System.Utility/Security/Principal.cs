using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace System.Utility.Security
{
    /// <summary>
    /// 安全主题
    /// </summary>
    public class Principal : IPrincipal
    {
        public Principal(IIdentity identity)
        {
            this.Identity = identity;
        }

        public Principal()
            : this(new Identity())
        { }

        /// <summary>
        /// 身份标识
        /// </summary>
        public IIdentity Identity { get; set; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; set; }
    }
}
