using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Utility.Data.Validation
{
    /// <summary>
    /// 字符必填验证器
    /// </summary>
    public class StringRequireValidator : RequireValidator
    {
        #region StringRequireValidator：构造函数
        /// <summary>
        /// 字符必填验证器
        /// </summary>
        public StringRequireValidator(string errorMsg, string text, PropertyInfo propertyInfo)
            : base(errorMsg, text, propertyInfo)
        {
            if (string.IsNullOrEmpty(errorMsg))
            {
                this.ErrorMsg = string.Format(System.Utility.Resources.DefaultErrorMessages.StringRequired,
                    text);
            }
        }
        #endregion

        /// <summary>
        /// 执行验证
        /// 验证通过返回true；验证失败返回false
        /// </summary>
        protected override bool Validation(object value)
        {
            var res = base.Validation(value);
            if (!res)
            {
                return res;
            }
            string target = value as string;
            return !target.IsNullOrEmpty();
        }
    }
}
