using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Utility.Data.Validation
{
    /// <summary>
    /// 验证属性抽象类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property), Serializable]
    public abstract class ValidatorAttribute : Attribute
    {
        /// <summary>
        /// 错误提示消息
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 属性文本名称
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 获取验证器
        /// </summary>
        public abstract Validator GetValidator(PropertyInfo propertyInfo);
    }
}
