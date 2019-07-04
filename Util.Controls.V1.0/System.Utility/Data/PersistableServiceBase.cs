using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transaction;

namespace System.Utility.Data
{
    /// <summary>
    /// 持久层基类
    /// </summary>
    public abstract class PersistableServiceBase<T>
    {
        #region  ISession对象

        /// <summary>
        /// ISession对象
        /// </summary>
        public abstract ISession Session { get; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public PersistableServiceBase()
        {
            ISession session = this.Session;
        }
        #endregion

        #region Save：保存实例对象
        /// <summary>
        /// 保存实例对象
        /// </summary>
        public virtual void Save(object entity)
        {
            if (entity.IsInstanceOfType(typeof(System.Utility.Data.Validation.ValidateableBase)))
            {
                var item = entity as System.Utility.Data.Validation.ValidateableBase;
                if (!item.IsValid())
                {
                    throw new Exception(item.ErrorMsg);
                }
            }
            ISession session = this.Session;
            session.SaveOrUpdate(entity);
            session.Flush();
        }
        #endregion

        #region Update：更新对象实例
        /// <summary>
        /// 更新对象实例
        /// </summary>
        public void Update(object entity)
        {
            if (entity.IsInstanceOfType(typeof(System.Utility.Data.Validation.ValidateableBase)))
            {
                var item = entity as System.Utility.Data.Validation.ValidateableBase;
                if (!item.IsValid())
                {
                    throw new Exception(item.ErrorMsg);
                }
            }
            ISession session = this.Session;
            session.Update(entity);
            session.Flush();
        }
        #endregion

        #region Delete：删除对象实例
        /// <summary>
        /// 删除对象实例
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(object entity)
        {
            ISession session = this.Session;
            try
            {
                session.Delete(entity);
                session.Flush();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region FindById：根据主键编号获取对象实例
        /// <summary>
        /// 根据主键编号获取对象实例
        /// </summary>
        public T FindById(object keyId)
        {
            ISession session = this.Session;
            return session.Get<T>(keyId);
        }
        #endregion

        #region FindAll：查询所有对象实例集合
        /// <summary>
        /// 查询所有对象实例集合
        /// </summary>
        public virtual IList<T> FindAll()
        {
            ISession session = this.Session;
            return session.Query<T>().ToList();
        }
        #endregion

        #region Find：根据表达式查询对象实例集合
        /// <summary>
        /// 根据表达式查询对象实例集合，无分页
        /// </summary>
        public virtual IList<T> Find(Func<T, bool> expression)
        {
            ISession session = this.Session;
            IList<T> list = session.Query<T>().Where(expression).ToList();
            return list;
        }
        #endregion

        #region Find：根据表达式查询对象实例集合,分页、有效行数
        /// <summary>
        /// 根据表达式查询对象实例集合,分页、有效行数
        /// </summary>
        public virtual IList<T> Find(Func<T, bool> expression, int pageIndex, int pageSize, out int total)
        {
            ISession session = this.Session;
            total = 0;
            int skip = (pageIndex - 1) * pageSize;
            total = session.Query<T>().Where(expression).Count();
            IList<T> list = session.Query<T>().Where(expression).Skip(skip).Take(pageSize).ToList();
            return list;
        }
        #endregion

        #region Find：根据表达式查询对象实例集合
        /// <summary>
        /// 根据表达式查询对象实例集合
        /// </summary>
        public virtual IList<T> Find(Func<T, bool> expression, int pageIndex, int pageSize)
        {
            ISession session = this.Session;
            int skip = (pageIndex - 1) * pageSize;
            IList<T> list = session.Query<T>().Where(expression).Skip(skip).Take(pageSize).ToList();
            return list;
        }
        #endregion

        #region FindByDesc：根据表达式、排序(倒序)条件查询对象实例集合
        /// <summary>
        /// 根据表达式、排序(倒序条件查询最新的对象实例集合
        /// </summary>
        public virtual IList<T> FindByDesc<TKey>(Func<T, bool> expression, Func<T, TKey> descOrder, int pageIndex, int pageSize)
        {
            ISession session = this.Session;
            int skip = (pageIndex - 1) * pageSize;
            IList<T> list = session.Query<T>().Where(expression).OrderByDescending<T, TKey>(descOrder).
                Skip(skip).Take(pageSize).ToList();
            return list;
        }
        #endregion

        #region FindByDesc：根据表达式、排序(倒序)条件查询对象实例集合,含分页
        /// <summary>
        /// 根据表达式、排序(倒序条件查询最新的对象实例集合,含分页
        /// </summary>
        public virtual IList<T> FindByDesc<TKey>(Func<T, bool> expression, Func<T, TKey> descOrder, int pageIndex, int pageSize, out int total)
        {
            ISession session = this.Session;
            int skip = (pageIndex - 1) * pageSize;
            total = session.Query<T>().Where(expression).Count();
            IList<T> list = session.Query<T>().Where(expression).OrderByDescending<T, TKey>(descOrder).
                Skip(skip).Take(pageSize).ToList();
            return list;
        }
        #endregion

        #region FindByAsc：根据表达式、排序条件查询对象实例集合
        /// <summary>
        /// 根据表达式、排序(倒序条件查询最新的对象实例集合
        /// </summary>
        public virtual IList<T> FindByAsc<TKey>(Func<T, bool> expression, Func<T, TKey> descOrder, int pageIndex, int pageSize)
        {
            ISession session = this.Session;
            int skip = (pageIndex - 1) * pageSize;
            IList<T> list = session.Query<T>().Where(expression).OrderBy<T, TKey>(descOrder).
                Skip(skip).Take(pageSize).ToList();
            return list;
        }
        #endregion

        #region FindByAsc：根据表达式、排序条件查询对象实例集合,含分页
        /// <summary>
        /// 根据表达式、排序(倒序条件查询最新的对象实例集合,含分页
        /// </summary>
        public virtual IList<T> FindByAsc<TKey>(Func<T, bool> expression, Func<T, TKey> descOrder, int pageIndex, int pageSize, out int total)
        {
            ISession session = this.Session;
            int skip = (pageIndex - 1) * pageSize;
            total = session.Query<T>().Where(expression).Count();
            IList<T> list = session.Query<T>().Where(expression).OrderBy<T, TKey>(descOrder).
                Skip(skip).Take(pageSize).ToList();
            return list;
        }
        #endregion

        #region GetCount：获取总行数：无条件
        /// <summary>
        /// 获取总行数：无条件
        /// </summary>
        public int GetCount()
        {
            ISession session = this.Session;
            int total = session.Query<T>().Count();
            return total;
        }
        #endregion

        #region GetCount：获取总行数：带条件
        /// <summary>
        /// 获取总行数：带条件
        /// </summary>
        public int GetCount(Func<T, bool> expression)
        {
            ISession session = this.Session;
            int total = session.Query<T>().Where(expression).Count();
            return total;
        }
        #endregion

        #region Search：查询系统数据列表
        /// <summary>
        /// 查询系统数据列表
        /// </summary>
        public virtual IList<T> Search(BaseQuery query)
        {
            ISession session = this.Session;
            var criteria = session.CreateCriteria(typeof(T));
            query.SetCriteria(criteria);
            var pageCriteria = CriteriaTransformer.Clone(criteria);
            //总行数
            query.Total = Convert.ToInt32(criteria.SetProjection(Projections.RowCount()).UniqueResult());
            //分页数据
            var start = (query.PageIndex - 1) * query.PageSize;
            query.SetOrder(pageCriteria);
            pageCriteria.SetFirstResult(start).SetMaxResults(query.PageSize);
            return pageCriteria.List<T>().ToList();
        }
        #endregion

        #region 配置过滤器
        /// <summary>
        /// 配置过滤器
        /// </summary>
        public virtual void SetFilter(ISession session)
        {
            session.EnableFilter("MarkDelete").SetParameter("deleteValue", 1);
        }
        #endregion
    }
}
