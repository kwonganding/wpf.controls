#region 引用命名空间
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace System.Data
{
    /// <summary>
    /// 缓存接口.
    /// </summary>
    public interface ICache
    {
        #region 属性

        #region 过期时间(单位：秒)
        /// <summary>
        /// 过期时间(单位：秒)
        /// </summary>
        int ExpirationSenconds
        {
            get;
            set;
        } 
        #endregion

        #region 过期时间(单位：分)
        /// <summary>
        /// 过期时间(单位：分)
        /// </summary>
        int ExpirationMinutes
        {
            get;
            set;
        }
        #endregion

        #region 过期时间(单位：小时)
        /// <summary>
        /// 过期时间(单位：小时)
        /// </summary>
        int ExpirationHours
        {
            get;
            set;
        }
        #endregion

        #region 缓存依赖文件路径
        /// <summary>
        /// 缓存依赖文件绝对路径
        /// </summary>
        string DependencyFilePath
        {
            get;
            set;
        }
        #endregion

        #endregion

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
