using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 单例模式。
    /// 注意：如果对象创建比较昂贵或需要特殊的销毁处理，请谨慎使用此单例实现，原因参考Interlocked提供的原子操作原理。
    /// </summary>
    public static class Singleton<TItem> where TItem : class,new()
    {
        private static TItem _Instance = null;

        /// <summary>
        /// 获取单例对象的实例
        /// </summary>
        /// <returns></returns>
        public static TItem GetInstance()
        {
            if (_Instance == null)
            {
                //提供线程互斥的原子操作
                System.Threading.Interlocked.CompareExchange<TItem>(ref _Instance, Activator.CreateInstance<TItem>(), null);
            }

            return _Instance;
        }
    }
}
