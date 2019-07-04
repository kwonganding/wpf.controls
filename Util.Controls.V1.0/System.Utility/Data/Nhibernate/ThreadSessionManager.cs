using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace System.Data
{
    /// <summary>
    /// 线程ISession管理
    /// </summary>
    public class ThreadSessionManager : ISessionManager
    {
        [ThreadStatic]
        private static ISession _Session;

        #region ISessionManager 成员

        /// <summary>
        /// 获取ISession
        /// </summary>
        public NHibernate.ISession Get()
        {
            if (_Session != null)
            {
                if (!_Session.IsConnected)
                {
                    _Session.Reconnect();
                }
            }
            return _Session;
        }

        /// <summary>
        /// 设置ISession
        /// </summary>
        public void Set(NHibernate.ISession value)
        {
            if (value.IsConnected)
            {
                value.Disconnect();
            }
            _Session = value;
        }

        #endregion
    }
}
