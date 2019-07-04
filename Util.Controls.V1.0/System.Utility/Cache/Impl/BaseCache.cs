#region 引用命名空间
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
#endregion

namespace System.Data
{
    /// <summary>
    /// 缓存基类
    /// </summary>
    internal abstract class BaseCache : ICache
    {
        #region 字段
        /// <summary>
        /// 过期时间（单位：秒）
        /// </summary>
        private int _time;
        /// <summary>
        /// 缓存依赖文件路径
        /// </summary>
        private string _dependencyFilePath;
        #endregion

        #region 属性

        #region 过期时间(单位：秒)
        /// <summary>
        /// 过期时间(单位：秒)
        /// </summary>
        public int ExpirationSenconds
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
            }
        }
        #endregion

        #region 过期时间(单位：分)
        /// <summary>
        /// 过期时间(单位：分)
        /// </summary>
        public int ExpirationMinutes
        {
            get
            {
                return _time / 60;
            }
            set
            {
                _time = value * 60;
            }
        }
        #endregion

        #region 过期时间(单位：小时)
        /// <summary>
        /// 过期时间(单位：小时)
        /// </summary>
        public int ExpirationHours
        {
            get
            {
                return _time / 60 / 60;
            }
            set
            {
                _time = value * 60 * 60;
            }
        }
        #endregion

        #region 缓存依赖文件路径
        /// <summary>
        /// 缓存依赖文件路径
        /// </summary>
        public string DependencyFilePath
        {
            get
            {
                return _dependencyFilePath;
            }
            set
            {
                _dependencyFilePath = value;
            }
        }
        #endregion

        #endregion

        #region 构造方法
        /// <summary>
        /// 初始化
        /// </summary>
        public BaseCache()
        {
            _time = 3600;
            _dependencyFilePath = null;
        }
        #endregion

        #region 检测缓存是否存在
        /// <summary>
        /// 检测缓存是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        public bool Contains( string key )
        {
            //有效性验证
            if ( string.IsNullOrEmpty( key ) )
            {
                return false;
            }

            //清理缓存键
            key = key.Trim().ToLower();

            //检测缓存是否存在
            return ContainsKey( key );
        }

        /// <summary>
        /// 检测缓存是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        public abstract bool ContainsKey( string key );
        #endregion

        #region 添加缓存对象
        /// <summary>
        /// 添加缓存对象，time为缓存有效时间（单位：秒）
        /// </summary>        
        /// <param name="key">缓存键</param>
        /// <param name="target">缓存对象</param>
        public void Add( string key, object target, int time)
        {
            //有效性验证
            if ( string.IsNullOrEmpty( key ) || target == null )
            {
                return;
            }

            //清理缓存键
            key = key.Trim().ToLower();

            if ( string.IsNullOrEmpty( _dependencyFilePath ) )
            {
                //添加缓存对象
                AddCache( key, target, time, null );
            }
            else
            {
                //创建缓存依赖
                CacheDependency dependency = new CacheDependency( _dependencyFilePath );

                //添加缓存对象
                AddCache( key, target, time, dependency );
            }
        }

        /// <summary>
        /// 添加缓存对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="target">缓存对象</param>
        /// <param name="time">缓存过期时间，单位:秒</param>
        /// <param name="dependency">缓存依赖</param>
        protected abstract void AddCache( string key, object target, int time, CacheDependency dependency );
        #endregion

        #region 获取缓存对象
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        public T Get<T>( string key ) where T : class
        {
            //有效性验证
            if ( string.IsNullOrEmpty( key ) )
            {
                return null;
            }

            //清理缓存键
            key = key.Trim().ToLower();

            //获取缓存对象
            return GetCache<T>( key );
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        protected abstract T GetCache<T>( string key ) where T : class;
        #endregion

        #region 移除缓存对象
        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key">缓存键</param>
        public void Remove( string key )
        {
            //有效性验证
            if ( string.IsNullOrEmpty( key ) )
            {
                return;
            }

            //清理缓存键
            key = key.Trim().ToLower();

            //移除缓存对象
            RemoveCache( key );
        }

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key">缓存键</param>
        protected abstract void RemoveCache( string key );
        #endregion

        #region 清空所有缓存
        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public abstract void ClearAll();
        #endregion
    }
}
