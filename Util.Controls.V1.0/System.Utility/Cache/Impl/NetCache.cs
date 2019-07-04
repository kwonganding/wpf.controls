#region 引用命名空间
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Collections;
#endregion

namespace System.Data
{
    /// <summary>
    /// .Net自带缓存
    /// </summary>
    internal class NetCache : BaseCache
    {
        #region 检测缓存是否存在
        /// <summary>
        /// 检测缓存是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        public override bool ContainsKey( string key )
        {
            //检测是否存在
            if ( HttpContext.Current == null )
            {
                //CS
                return HttpRuntime.Cache.Get( key ) == null ? false : true;
            }
            return HttpContext.Current.Cache.Get( key ) == null ? false : true;
        } 
        #endregion

        #region 添加缓存对象
        /// <summary>
        /// 添加缓存对象
        /// </summary>        
        /// <param name="key">缓存键</param>
        /// <param name="target">缓存对象</param>
        /// <param name="time">缓存过期时间，单位:秒</param>
        /// <param name="dependency">缓存依赖</param>
        protected override void AddCache( string key, object target, int time, CacheDependency dependency )
        {            
            if ( HttpContext.Current == null )
            {
                //CS
                AddCache_CS( key, target, time, dependency );
            }
            else
            {
                //BS
                AddCache_BS( key, target, time, dependency );
            }
        }

        #region 添加CS缓存
        /// <summary>
        /// 添加CS缓存
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="target">缓存对象</param>
        /// <param name="time">缓存过期时间，单位:秒</param>
        /// <param name="dependency">缓存依赖</param>
        private void AddCache_CS( string key, object target, int time, CacheDependency dependency )
        {
            if ( dependency == null )
            {
                HttpRuntime.Cache.Insert( key, target, null, DateTime.Now.AddSeconds( time ),
                    System.Web.Caching.Cache.NoSlidingExpiration );
            }
            else
            {
                HttpRuntime.Cache.Insert( key, target, dependency, System.Web.Caching.Cache.NoAbsoluteExpiration,
                System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null );
            }
        } 
        #endregion

        #region 添加BS缓存
        /// <summary>
        /// 添加BS缓存
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="target">缓存对象</param>
        /// <param name="time">缓存过期时间，单位:秒</param>
        /// <param name="dependency">缓存依赖</param>
        private void AddCache_BS( string key, object target, int time, CacheDependency dependency )
        {
            if ( dependency == null )
            {
                HttpContext.Current.Cache.Insert( key, target, null, DateTime.Now.AddSeconds( time ),
                    System.Web.Caching.Cache.NoSlidingExpiration );
            }
            else
            {
                HttpContext.Current.Cache.Insert( key, target, dependency, System.Web.Caching.Cache.NoAbsoluteExpiration,
                System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null );
            }
        } 
        #endregion

        #endregion

        #region 获取缓存对象
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        protected override T GetCache<T>( string key )
        {
            //获取缓存对象
            if ( HttpContext.Current == null )
            {
                //CS
                return HttpRuntime.Cache.Get( key ) as T;
            }
            return HttpContext.Current.Cache.Get( key ) as T;
        } 
        #endregion

        #region 移除缓存对象
        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key">缓存键</param>
        protected override void RemoveCache( string key )
        {
            //获取缓存对象
            if ( HttpContext.Current == null )
            {
                //CS
                HttpRuntime.Cache.Remove( key );
            }
            HttpContext.Current.Cache.Remove( key );
        } 
        #endregion

        #region 清空所有缓存
        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public override void ClearAll()
        {
            //获取枚举器
            IDictionaryEnumerator enumerator;                
            if ( HttpContext.Current == null )
            {
                //CS
                enumerator = HttpRuntime.Cache.GetEnumerator();
                while ( enumerator.MoveNext() )
                {
                    HttpRuntime.Cache.Remove( enumerator.Key.ToString() );
                }                
            }
            else
            {
                //BS
                enumerator = HttpContext.Current.Cache.GetEnumerator();
                while ( enumerator.MoveNext() )
                {
                    HttpContext.Current.Cache.Remove( enumerator.Key.ToString() );
                }
            }
        }


        #endregion
    }
}
