using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Utility.Data.Validation
{
    /// <summary>
    /// 验证处理基类
    /// 提供基本属性验证方法
    /// </summary>
    public class ValidateableBase : IValidateable
    {
        /// <summary>
        /// 验证管理中心
        /// </summary>
        private ValidationManager _Manager;

        /// <summary>
        /// 验证基类
        /// </summary>
        public ValidateableBase()
        {
            this._Manager = new ValidationManager(this);
        }

        /// <summary>
        /// 验证数据合法性
        /// 验证通过返回true；验证失败返回false
        /// </summary>
        public virtual bool IsValid()
        {
            return this._Manager.DoIsValid();
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public virtual string ErrorMsg
        {
            get { return this._Manager.ErrorMsg; }
        }

        /// <summary>
        /// 自定义错误信息
        /// <para>(add: wangxi 2015-04-23 17:22:07)</para>
        /// </summary>
        public virtual string CustomError { get; set; }
    }
}
