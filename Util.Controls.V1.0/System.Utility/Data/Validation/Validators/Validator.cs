using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Utility.Data.Validation
{
    /// <summary>
    /// 验证器抽象类
    /// </summary>
    public abstract class Validator
    {
        #region Private attriutes
        /// <summary>
        /// 要验证的属性信息
        /// </summary>
        private PropertyInfo _PropertyInfo;
        /// <summary>
        /// 错误提示消息
        /// </summary>
        private string _ErrorMsg;
        #endregion

        #region Validator：构造函数
        /// <summary>
        /// 验证器抽象类
        /// </summary>
        protected Validator(string errorMsg, string text, PropertyInfo propertyInfo)
        {
            this._ErrorMsg = errorMsg;
            if (string.IsNullOrEmpty(errorMsg))
            {
                this._ErrorMsg = string.Format(System.Utility.Resources.DefaultErrorMessages.Default,
                    text);
            }
            this._PropertyInfo = propertyInfo;
        }
        #endregion

        #region IsValid：验证方法
        /// <summary>
        /// 验证属性值是否合法
        /// 验证通过返回true；验证失败返回false
        /// 
        /// </summary>
        public bool IsValid(object instance)
        {
            Guard.ArgumentNotNull(instance, "instance");
            object value = this._PropertyInfo.GetValue(instance, null);
            return this.Validation(value);
        }

        /// <summary>
        /// 执行验证
        /// 验证通过返回true；验证失败返回false
        /// </summary>
        protected abstract bool Validation(object value);
        #endregion

        #region Properties
        /// <summary>
        /// 错误提示消息
        /// </summary>
        public string ErrorMsg
        {
            get { return this._ErrorMsg; }
            protected set { this._ErrorMsg = value; }
        }
        /// <summary>
        /// 要验证的属性信息
        /// </summary>
        public PropertyInfo PropertyInfo
        {
            get { return this._PropertyInfo; }
        }
        #endregion
    }
}
