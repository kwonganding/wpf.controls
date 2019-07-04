using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data
{
    /// <summary>
    /// 缓存处理中心，提供简单缓存的封装处理。
    /// 若缓存有文件、数据库依赖，需要单独使用NetCache处理
    /// </summary>
    public static class CacheManager
    {
        private static readonly ICache _CacheService;

        static CacheManager()
        {
            _CacheService = new NetCache();
        }

        /// <summary>
        /// 添加一个对象，放入缓存。
        /// </summary>
        /// <typeparam name="TInput">对象类型。</typeparam>
        /// <param name="key">存入的键值。</param>
        /// <param name="ob">对象</param>
        /// <param name="duration">缓存有效时间。</param>
        public static void AddObject<TInput>(string key, TInput ob, int duration) where TInput : class
        {
            _CacheService.Add(key, ob, duration);
        }

        /// <summary>
        /// 更新缓存对象
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="key"></param>
        /// <param name="ob"></param>
        /// <param name="duration"></param>
        public static void Update<TInput>(string key, TInput ob, int duration) where TInput : class
        {
            if (key.IsInvalid())
            {
                return;
            }
            if(_CacheService.Contains(key))
            {
                _CacheService.Remove(key);
                AddObject(key, ob, duration);
            }
        }

        /// <summary>
        /// 获取缓存对象。
        /// </summary>
        /// <typeparam name="TResult">对象类型。</typeparam>
        /// <param name="key">缓存的键。</param>
        /// <returns>返回键对应的值。</returns>
        public static TResult GetObject<TResult>(string key) where TResult : class
        {
            if (_CacheService.Contains(key))
            {
                return _CacheService.Get<TResult>(key);
            }

            return null;
        }

        #region 获取缓存对象-Refactoring by Ryan Kuang/2012.5.35

        #region 获取缓存对象：无参数

        /// <summary>
        /// 获取缓存对象：无参数
        /// </summary>
        /// <returns></returns>
        /// User:Ryan  CreateTime:2012-5-25 1:22.
        public static TResult GetCache<TResult>(string key, int duration, Func<TResult> func)
            where TResult : class
        {
            //若存在缓存对象，返回
            if (_CacheService.Contains(key))
            {
                var res = _CacheService.Get<TResult>(key);
                if (res != null)
                {
                    return res;
                }
            }
            //否则执行缓存获取方法，加载缓存
            TResult result = func();
            if (result != null)
            {
                _CacheService.Add(key, result, duration);
            }
            return result;
        }

        #endregion

        #region 获取缓存对象：1个参数

        /// <summary>
        /// 获取缓存对象：1个参数
        /// </summary>
        /// <returns></returns>
        /// User:Ryan  CreateTime:2012-5-25 1:22.
        public static TResult GetCache<T1, TResult>(string key, int duration, Func<T1, TResult> func, T1 arg)
            where TResult : class
        {
            //若存在缓存对象，返回
            if (_CacheService.Contains(key))
            {
                var res = _CacheService.Get<TResult>(key);
                if (res != null)
                {
                    return res;
                }
            }
            //否则执行缓存获取方法，加载缓存
            TResult result = func(arg);
            if (result != null)
            {
                _CacheService.Add(key, result, duration);
            }
            return result;
        }

        #endregion

        #region 获取缓存对象：2个参数

        /// <summary>
        /// 获取缓存对象：2个参数
        /// </summary>
        /// <returns></returns>
        /// User:Ryan  CreateTime:2012-5-25 1:22.
        public static TResult GetCache<T1, T2, TResult>(string key, int duration, Func<T1, T2, TResult> func, T1 arg1, T2 arg2)
            where TResult : class
        {
            //若存在缓存对象，返回
            if (_CacheService.Contains(key))
            {
                var res = _CacheService.Get<TResult>(key);
                if (res != null)
                {
                    return res;
                }
            }
            //否则执行缓存获取方法，加载缓存
            TResult result = func(arg1, arg2);
            if (result != null)
            {
                _CacheService.Add(key, result, duration);
            }
            return result;
        }

        #endregion

        #region 获取缓存对象：3个参数

        /// <summary>
        /// 获取缓存对象：3个参数
        /// </summary>
        /// <returns></returns>
        /// User:Ryan  CreateTime:2012-5-25 1:22.
        public static TResult GetCache<T1, T2, T3, TResult>(string key, int duration, Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3)
            where TResult : class
        {
            //若存在缓存对象，返回
            if (_CacheService.Contains(key))
            {
                var res = _CacheService.Get<TResult>(key);
                if (res != null)
                {
                    return res;
                }
            }
            //否则执行缓存获取方法，加载缓存
            TResult result = func(arg1, arg2, arg3);
            if (result != null)
            {
                _CacheService.Add(key, result, duration);
            }
            return result;
        }

        #endregion

        #endregion

        #region Remove：移除指定键值的缓存对象
        /// <summary>
        /// 移除指定键值的缓存对象
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            if (_CacheService.Contains(key))
            {
                _CacheService.Remove(key);
            }
        }
        #endregion


        #region ClearAll：清空所有缓存
        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public static void ClearAll()
        {
            _CacheService.ClearAll();
        }
        #endregion
    }
}
