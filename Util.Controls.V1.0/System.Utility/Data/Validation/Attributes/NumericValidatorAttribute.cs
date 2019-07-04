using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Utility.Data.Validation
{
    /// <summary>
    /// 数值大小验证属性
    /// </summary>
    public class NumericValidatorAttribute : ValidatorAttribute
    {
        #region private attribute
        /// <summary>
        /// 最小值
        /// </summary>
        private int _MinValue;
        /// <summary>
        /// 最大值
        /// </summary>
        private int _MaxValue;
        #endregion

        #region NumericValidatorAttribute：构造函数

        public NumericValidatorAttribute()
            : this(int.MinValue, int.MaxValue)
        {
        }

        public NumericValidatorAttribute(int max)
            : this(int.MinValue, max)
        {
        }

        public NumericValidatorAttribute(int min, int max)
        {
            this._MaxValue = max;
            this._MinValue = min;
        }
        #endregion

        #region GetValidator：获取验证器
        /// <summary>
        /// 获取验证器
        /// </summary>
        public override Validator GetValidator(PropertyInfo propertyInfo)
        {
            return new NumericValidator(this.ErrorMsg, this.Text, propertyInfo, this._MinValue, this._MaxValue);
        }
        #endregion

        #region Properties
        /// <summary>
        /// 最小值
        /// </summary>
        public int MinValue
        {
            get { return this._MinValue; }
            set { this._MinValue = value; }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public int MaxValue
        {
            get { return this._MaxValue; }
            set { this._MaxValue = value; }
        }
        #endregion
    }
}
