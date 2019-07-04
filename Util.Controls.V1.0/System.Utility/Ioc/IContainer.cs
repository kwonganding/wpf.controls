using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Utility.Ioc
{
    /// <summary>
    /// Ioc容器接口
    /// </summary>
    internal interface IContainer
    {
        /// <summary>
        /// 加载实例
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        T Load<T>();

        /// <summary>
        /// 加载实例
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <param name="name">实例的命名</param>
        T Load<T>(string name);
    }
}
