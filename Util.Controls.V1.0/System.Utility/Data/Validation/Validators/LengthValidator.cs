using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Utility.Data.Validation
{
    /// <summary>
    /// 字符长度验证器
    /// </summary>
    public class LengthValidator : Validator
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

        #region LengthValidator：构造函数
        /// <summary>
        /// 字符长度验证器
        /// </summary>
        public LengthValidator(string errorMsg, string text, PropertyInfo propertyInfo, int minLength, int maxLength)
            : base(errorMsg, text, propertyInfo)
        {
            if (string.IsNullOrEmpty(errorMsg))
            {
                this.ErrorMsg = string.Format(System.Utility.Resources.DefaultErrorMessages.Length,
                    text, minLength, maxLength);
            }
            this._MinLength = minLength;
            this._MaxLength = maxLength;
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
            var str = value.ToString();
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            int len = str.Length;
            return len >= this._MinLength && len <= this._MaxLength;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 最小长度
        /// </summary>
        public int MinLength
        {
            get { return this._MinLength; }
        }

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength
        {
            get { return this._MaxLength; }
        }
        #endregion
    }
}
