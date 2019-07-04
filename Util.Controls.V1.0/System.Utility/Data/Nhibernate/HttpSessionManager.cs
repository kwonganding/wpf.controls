using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Web;

namespace System.Data
{
    /// <summary>
    /// Http请求的Isession管理
    /// </summary>
    public class HttpSessionManager : ISessionManager
    {
        private const string KeyName = "NHSession";

        #region ISessionManager 成员

        /// <summary>
        /// 获取ISession
        /// </summary>
        public NHibernate.ISession Get()
        {
            return (ISession)HttpContext.Current.Items[KeyName];
        }

        /// <summary>
        /// 设置ISession
        /// </summary>
        public void Set(NHibernate.ISession value)
        {
            if (value != null)
            {
                HttpContext.Current.Items.Add(KeyName, value);
            }
            else
            {
                HttpContext.Current.Items.Remove(KeyName);
            }
        }

        #endregion
    }
}
