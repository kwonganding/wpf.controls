using System;

namespace Util.Controls
{
    /// <summary>
    /// 单例模式。
    /// </summary>
    public static class Singleton<T> where T : class, new()
    {
        private static T _Instance;
        private static object _lockObj = new object();

        /// <summary>
        /// 获取单例对象的实例
        /// </summary>
        public static T GetInstance()
        {
            if (_Instance != null) return _Instance;
            lock (_lockObj)
            {
                if (_Instance == null)
                {
                    var temp = Activator.CreateInstance<T>();
                    System.Threading.Interlocked.Exchange(ref _Instance, temp);
                }
            }
            return _Instance;
        }
    }

}