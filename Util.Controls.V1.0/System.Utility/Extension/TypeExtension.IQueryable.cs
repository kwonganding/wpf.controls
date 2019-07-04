using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System
{
    /// <summary>
    /// IQueryable的操作扩展
    /// </summary>
    public static partial class TypeExtension
    {
        /// <summary>
        /// 如果条件condition为ture时执行查询谓词predicate 
        /// predicate 查询谓词['predɪkət]
        /// </summary>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> @this, Expression<Func<T, bool>> predicate, bool condition)
        {
            return condition ? @this.Where(predicate) : @this;
        }
    }
}
