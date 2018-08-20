#region 引用命名空间
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace Util.Cache
{
    /// <summary>
    /// 缓存接口.
    /// </summary>
    public interface ICache
    {
        #region 检测缓存是否存在
        /// <summary>
        /// 检测缓存是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        bool Contains( string key );
        #endregion

        #region 添加缓存对象
        /// <summary>
        /// 添加缓存对象，time为缓存有效时间（单位：秒）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="target">缓存对象</param>
        void Add( string key, object target ,int time );
        #endregion

        #region 获取缓存对象
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        T Get<T>( string key ) where T : class;
        #endregion

        #region 移除缓存对象
        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key">缓存键</param>
        void Remove( string key );
        #endregion

        #region 清空所有缓存
        /// <summary>
        /// 清空所有缓存
        /// </summary>
        void ClearAll();
        #endregion
    }
}
