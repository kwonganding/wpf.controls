using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Utility.Data.Validation
{
    /// <summary>
    /// 必填验证器简单工厂
    /// </summary>
    public class RequireValidatorFactory
    {
        /// <summary>
        /// 创建必填验证器（根据数据类型）
        /// </summary>
        public static RequireValidator CreateValidator(string errorMsg, string text, PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;
            if (type.Equals(typeof(string)))
            {
                return new StringRequireValidator(errorMsg, text, propertyInfo);
            }
            if (type.Equals(typeof(DateTime)))
            {
                return new DatetimeRequireValidator(errorMsg, text, propertyInfo);
            }
            return new RequireValidator(errorMsg, text, propertyInfo);
        }
    }
}
