using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NHibernate;

namespace System.Utility.Data
{
    /// <summary>
    /// 业务层基类
    /// </summary>
    public abstract class ServiceBase<T>
    {
        #region Attributes

        /// <summary>
        /// 持久层对象
        /// </summary>
        protected PersistableServiceBase<T> PersistableService;

        /// <summary>
        /// 对象的名称，用于日志记载
        /// </summary>
        protected string Name;

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ServiceBase()
        {
        }
        #endregion

        #region Save：保存实例对象
        /// <summary>
        /// 保存实例对象
        /// </summary>
        public virtual void Save(T entity)
        {
            try
            {
                this.PersistableService.Save(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("保存", this.Name, "异常"), ex);
            }
        }
        #endregion

        #region Update：更新对象实例
        /// <summary>
        /// 更新对象实例
        /// </summary>
        public virtual void Update(T entity)
        {
            try
            {
                this.PersistableService.Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("更新", this.Name, "异常"), ex);
            }
        }
        #endregion

        #region Delete：根据主键删除对象实例
        /// <summary>
        /// 根据主键删除对象实例
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(int keyId)
        {
            try
            {
                var item = this.FindById(keyId);
                if (item != null)
                {
                    this.PersistableService.Delete(item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("删除", this.Name, "异常"), ex);
            }
        }
        #endregion

        #region Delete：删除对象实例
        /// <summary>
        /// 删除对象实例
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(object item)
        {
            try
            {
                if (item != null)
                {
                    this.PersistableService.Delete(item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("删除", this.Name, "异常"), ex);
            }
        }
        #endregion

        #region FindById：根据主键编号获取对象实例
        /// <summary>
        /// 根据主键编号获取对象实例
        /// </summary>
        public virtual T FindById(object keyId)
        {
            try
            {
                return this.PersistableService.FindById(keyId);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("获取", this.Name, "异常"), ex);
            }
        }
        #endregion

        #region FindAll：查询所有对象实例集合
        /// <summary>
        /// 查询所有对象实例集合
        /// </summary>
        public virtual IList<T> FindAll()
        {
            try
            {
                return this.PersistableService.FindAll();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("获取", this.Name, "列表异常"), ex);
            }
        }
        #endregion

        #region Find：根据表达式查询对象实例集合
        /// <summary>
        /// 根据表达式查询对象实例集合
        /// </summary>
        public virtual IList<T> Find(Func<T, bool> expression)
        {
            try
            {
                return this.PersistableService.Find(expression);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("查询", this.Name, "列表异常"), ex);
            }
        }
        #endregion

        #region Find：根据表达式查询对象实例集合,分页、有效行数
        /// <summary>
        /// 根据表达式查询对象实例集合,分页、有效行数
        /// </summary>
        public virtual IList<T> Find(Func<T, bool> expression, int pageIndex, int pageSize, out int total)
        {
            try
            {
                return this.PersistableService.Find(expression, pageIndex, pageSize, out total);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("获取", this.Name, "列表异常"), ex);
            }
        }
        #endregion

        #region Find：根据表达式查询对象实例集合,分页
        /// <summary>
        /// 根据表达式查询对象实例集合
        /// </summary>
        public virtual IList<T> Find(Func<T, bool> expression, int pageIndex, int pageSize)
        {
            try
            {
                return this.PersistableService.Find(expression, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("获取", this.Name, "列表异常"), ex);
            }
        }
        #endregion

        #region FindByDesc：根据表达式、排序(倒序)条件查询对象实例集合
        /// <summary>
        /// 根据表达式、排序(倒序)条件查询最新的对象实例集合
        /// </summary>
        public virtual IList<T> FindByDesc<TKey>(Func<T, bool> expression, Func<T, TKey> descOrder, int pageIndex, int pageSize)
        {
            try
            {
                return this.PersistableService.FindByDesc(expression, descOrder, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("获取", this.Name, "列表异常"), ex);
            }
        }
        #endregion

        #region FindByDesc：根据表达式、排序(倒序)条件查询对象实例集合,含分页
        /// <summary>
        /// 根据表达式、排序(倒序)条件查询最新的对象实例集合,含分页
        /// </summary>
        public virtual IList<T> FindByDesc<TKey>(Func<T, bool> expression, Func<T, TKey> descOrder, int pageIndex, int pageSize, out int total)
        {
            try
            {
                return this.PersistableService.FindByDesc(expression, descOrder, pageIndex, pageSize, out total);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("获取", this.Name, "列表异常"), ex);
            }
        }
        #endregion

        #region FindByAsc：根据表达式、排序条件查询对象实例集合
        /// <summary>
        /// 根据表达式、排序(倒序)条件查询最新的对象实例集合
        /// </summary>
        public virtual IList<T> FindByAsc<TKey>(Func<T, bool> expression, Func<T, TKey> descOrder, int pageIndex, int pageSize)
        {
            try
            {
                return this.PersistableService.FindByAsc(expression, descOrder, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("获取", this.Name, "列表异常"), ex);
            }
        }
        #endregion

        #region FindByAsc：根据表达式、排序条件查询对象实例集合,含分页
        /// <summary>
        /// 根据表达式、排序(倒序)条件查询最新的对象实例集合,含分页
        /// </summary>
        public virtual IList<T> FindByAsc<TKey>(Func<T, bool> expression, Func<T, TKey> descOrder, int pageIndex, int pageSize, out int total)
        {
            try
            {
                return this.PersistableService.FindByAsc(expression, descOrder, pageIndex, pageSize, out total);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("获取", this.Name, "列表异常"), ex);
            }
        }
        #endregion

        #region GetCount：获取总行数：无条件
        /// <summary>
        /// 获取总行数：无条件
        /// </summary>
        public virtual int GetCount()
        {
            try
            {
                return this.PersistableService.GetCount();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("获取", this.Name, "总行数(无条件)异常"), ex);
            }
        }
        #endregion

        #region GetCount：获取总行数：带条件
        /// <summary>
        /// 获取总行数：带条件
        /// </summary>
        public virtual int GetCount(Func<T, bool> expression)
        {
            try
            {
                return this.PersistableService.GetCount(expression);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("获取", this.Name, "总行数(带条件)异常"), ex);
            }
        }
        #endregion

        #region Search：查询系统数据列表
        /// <summary>
        /// 查询系统数据列表
        /// </summary>
        public virtual IList<T> Search(BaseQuery query)
        {
            try
            {
                return this.PersistableService.Search(query);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("查询", this.Name, "列表异常"), ex);
            }
        }
        #endregion

        #region 配置过滤器
        /// <summary>
        /// 配置过滤器
        /// </summary>
        protected virtual void SetFilter(ISession session)
        {
            this.PersistableService.SetFilter(session);
        }
        #endregion
    }
}
