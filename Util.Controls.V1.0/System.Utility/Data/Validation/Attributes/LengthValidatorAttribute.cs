using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Utility.Data.Validation
{
    /// <summary>
    /// 字符长度验证属性
    /// </summary>
    public class LengthValidatorAttribute : ValidatorAttribute
    {
        #region private attribute
        /// <summary>
        /// 最小长度
        /// </summary>
        private int _MinLength;
        /// <summary>
        /// 最大长度
        /// </summary>
        private int _MaxLength;
        #endregion

        #region LengthValidatorAttribute：构造函数

        public LengthValidatorAttribute()
            : this(0, int.MaxValue)
        {
        }

        public LengthValidatorAttribute(int max)
            : this(0, max)
        {
        }

        public LengthValidatorAttribute(int min, int max)
        {
            this._MaxLength = max;
            this._MinLength = min;
        }
        #endregion

        #region GetValidator：获取验证器
        /// <summary>
        /// 获取验证器
        /// </summary>
        public override Validator GetValidator(PropertyInfo propertyInfo)
        {
            return new LengthValidator(this.ErrorMsg, this.Text, propertyInfo, this.MinLength, this.MaxLength);
        }
        #endregion

        #region Properties
        /// <summary>
        /// 最小长度
        /// </summary>
        public int MinLength
        {
            get { return this._MinLength; }
            set { this._MinLength = value; }
        }

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength
        {
            get { return this._MaxLength; }
            set { this._MaxLength = value; }
        }

        #endregion
    }
}
