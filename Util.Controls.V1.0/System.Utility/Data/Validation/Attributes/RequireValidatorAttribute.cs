using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Utility.Data.Validation
{
    /// <summary>
    /// 必填属性抽象类
    /// </summary>
    public class RequireValidatorAttribute : ValidatorAttribute
    {
        /// <summary>
        /// 获取验证器
        /// </summary>
        public override Validator GetValidator(PropertyInfo propertyInfo)
        {
            return RequireValidatorFactory.CreateValidator(this.ErrorMsg, this.Text, propertyInfo);
        }
    }
}
