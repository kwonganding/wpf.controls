using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Utility.Data.Validation
{
    /// <summary>
    /// 数值大小验证器
    /// </summary>
    public class NumericValidator : Validator
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

        #region NumericValidator：构造函数
        /// <summary>
        /// 数值大小验证器
        /// </summary>
        public NumericValidator(string errorMsg, string text, PropertyInfo propertyInfo, int minValue, int maxValue)
            : base(errorMsg,text, propertyInfo)
        {
            if (string.IsNullOrEmpty(errorMsg))
            {
                this.ErrorMsg = string.Format(System.Utility.Resources.DefaultErrorMessages.Compare,
                    text, minValue, maxValue);
            }
            this._MinValue = minValue;
            this._MaxValue = maxValue;
        }
        #endregion

        #region Validation：执行验证
        /// <summary>
        /// 执行验证
        /// 验证通过返回true；验证失败返回false
        /// </summary>
        protected override bool Validation(object value)
        {
            if (value == null)
            {
                return true;
            }
            decimal target;
            if (decimal.TryParse(value.ToString(), out target))
            {
                return target >= this._MinValue && target <= this._MaxValue;
            }
            return true;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 最小值
        /// </summary>
        public int MinValue
        {
            get { return this._MinValue; }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public int MaxValue
        {
            get { return this._MaxValue; }
        }
        #endregion
    }
}
