using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Util.Cache
{
    public sealed class DefaultCache : ICache
    {
        private MemoryCache _Cache = MemoryCache.Default;
        private List<string> _Keys = new List<string>();

        public bool Contains(string key)
        {
            return _Cache.Contains(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void Add(string key, object target, int time)
        {
            _Cache.Add(key, target, new DateTimeOffset(DateTime.Now.AddSeconds(time)));
            _Keys.Add(key);
        }

        public T Get<T>(string key) where T : class
        {
            return (T)_Cache.Get(key);
        }

        public void Remove(string key)
        {
            _Cache.Remove(key);
            _Keys.Remove(key);
        }

        public void ClearAll()
        {
            foreach (var key in _Keys)
            {
                _Cache.Remove(key);
            }
            _Keys.Clear();
        }
    }
}