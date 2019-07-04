using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace System.Data
{
    /// <summary>
    /// ISession管理接口
    /// </summary>
    public interface ISessionManager
    {
        /// <summary>
        ///获得ISession 
        /// </summary>
        /// <returns></returns>
        ISession Get();

        /// <summary>
        /// 保存ISession
        /// </summary>
        /// <param name="value"></param>
        void Set(ISession value);
    }
}
