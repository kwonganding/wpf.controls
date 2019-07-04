using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Utility.Patterns
{
    /// <summary>
    /// 简单工厂的抽象接口
    /// </summary>
    /// <typeparam name="TKey">key键值类型</typeparam>
    /// <typeparam name="TValue">工厂实例类型</typeparam>
    public interface ISimpleFactory<TKey,TValue>
    {
        /// <summary>
        /// 根据key获取实例
        /// </summary>
        TValue GetInstance(TKey key);
    }
}
