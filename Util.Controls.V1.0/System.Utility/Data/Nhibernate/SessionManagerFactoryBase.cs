using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Web;

namespace System.Data
{
    /// <summary>
    /// 创建ISession的工厂基类
    /// 子类须实现ISessionManager和ISessionFactory
    /// </summary>
    public abstract class SessionManagerFactoryBase
    {
        /// <summary>
        /// NHibernate配置文件路径
        /// </summary>
        protected string ConfigFile { set; get; }

        protected ISessionManager SessionManager { set; get; }

        protected abstract ISessionFactory SessionFactory { get; }

        /// <summary>
        /// 建立ISessionFactory的实例
        /// </summary>
        public virtual ISession CreateSession()
        {
            var session = SessionManager.Get();
            if (session == null)
            {
                session = this.SessionFactory.OpenSession();
                SessionManager.Set(session);
            }
            if (!session.IsConnected)
            {
                session.Reconnect();
            }
            return session;
        }
    }
}
