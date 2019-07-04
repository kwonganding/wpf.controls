using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Utility.Patterns
{
    /// <summary>
    /// 享元模式的实现基类
    /// 共享大量细粒度的对象，复用内存对象，降低内存消耗
    /// </summary>
    public abstract class FlyweightBase<Tkey, TValue> where TValue : class
    {
        private static object _obj = new object();

        /// <summary>
        /// 享元工厂
        /// </summary>
        protected static Dictionary<Tkey, TValue> Cache = new Dictionary<Tkey, TValue>();

        /// <summary>
        /// 对象初始化
        /// </summary>
        /// <param name="key"></param>
        protected abstract void DoInitalize(Tkey key);

        /// <summary>
        /// 获取享元对象实例
        /// </summary>
        public virtual TValue GetInstance(Tkey key)
        {
            Guard.ArgumentNotNull(key, "Tkey");
            lock (_obj)
            {
                if (!Cache.ContainsKey(key))
                {
                    this.DoInitalize(key);
                }
            }

            return Cache[key];
        }

        /// <summary>
        /// 获取享元对象实例
        /// </summary>
        public TValue this[Tkey key]
        {
            get
            {
                return this.GetInstance(key);
            }
        }
    }
}
