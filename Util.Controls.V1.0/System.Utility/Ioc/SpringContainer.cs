using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Utility.Ioc
{
    /// <summary>
    /// Spring容器
    /// </summary>
    internal class SpringContainer : IContainer
    {
        #region 字段
        /// <summary>
        /// 容器上下文
        /// </summary>
        private Spring.Context.IApplicationContext _context;
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化
        /// </summary>
        public SpringContainer()
        {
            _context = Spring.Context.Support.ContextRegistry.GetContext();
        }
        #endregion

        #region 加载实例

        #region 重载1
        /// <summary>
        /// 加载实例
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        public T Load<T>()
        {
            return _context.GetObject<T>();
        }
        #endregion

        #region 重载2
        /// <summary>
        /// 加载实例
        /// </summary>
        /// <param name="name">实例的命名</param>
        /// <typeparam name="T">实例类型</typeparam>
        public T Load<T>(string name)
        {
            
            return _context.GetObject<T>(name);
        }
        #endregion

        #endregion
    }
}
