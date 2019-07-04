using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NHibernate;
using NHibernate.Criterion;

namespace System.Data
{
    /// <summary>
    /// 查询类的基础类（封装了分页的处理）
    /// </summary>
    /// User:Ryan  CreateTime:2011-12-06 11:42.
    public abstract class BaseQuery
    {
        public BaseQuery()
        {
            this.PageSize = 20;
            this.PageIndex = 1;
            this.Total = 0;
            this.Orders = new List<Order>();
        }

        #region 分页属性

        /// <summary>
        /// 分页大小
        /// </summary>
        /// <value>The size of the page.</value>
        /// User:Ryan  CreateTime:2011-12-06 11:41.
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        /// <value>The index of the page.</value>
        /// User:Ryan  CreateTime:2011-12-06 11:41.
        public int PageIndex { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        /// <value>The total.</value>
        /// User:Ryan  CreateTime:2011-12-06 11:41.
        public int Total { get; set; }
        #endregion

        /// <summary>
        /// 排序规则
        /// </summary>
        public IList<Order> Orders { get; private set; }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        public abstract void SetCriteria(NHibernate.ICriteria criteria);

        #region SetOrder：设置排序规则
        /// <summary>
        /// 设置排序规则
        /// </summary>
        public virtual void SetOrder(ICriteria criteria)
        {
            if (this.Orders != null && this.Orders.Count > 0)
            {
                foreach (var order in this.Orders)
                {
                    criteria.AddOrder(order);
                }
            }
        }
        #endregion
    }
}
