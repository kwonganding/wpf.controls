using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Utility.Ioc
{
    /// <summary>
    /// 依赖注入容器
    /// Denpendency Injection(DI)
    /// </summary>
    public static class Ioc
    {
        /// <summary>
        /// 创建对象
        /// </summary>
        public static T Create<T>(string name = "")
        {
            IContainer container = Singleton<SpringContainer>.GetInstance();
            if (name.IsValid())
            {
                return container.Load<T>(name);
            }
            return container.Load<T>();
        }
    }
}
