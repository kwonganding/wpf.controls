using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ValidationCollection = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<
    System.Utility.Data.Validation.Validator>>;

namespace System.Utility.Data.Validation
{
    /// <summary>
    /// 验证中心，处理所有的验证
    /// </summary>
    public sealed class ValidationManager
    {
        #region Private attributes
        /// <summary>
        /// 静态的类型的验证器集合的容器，可缓存各个类型的验证器集合
        /// </summary>
        private static Dictionary<Type, ValidationCollection> _TypeValidators;
        /// <summary>
        /// 验证的目标
        /// </summary>
        private object _Target;
        /// <summary>
        /// 错误信息集合
        /// </summary>
        private List<string> _Errors;

        /// <summary>
        /// 验证结果
        /// </summary>
        private bool _Result;
        #endregion

        #region ValidationManager：静态构造函数
        /// <summary>
        /// 静态构造函数，初始化静态验证器容器
        /// </summary>
        static ValidationManager()
        {
            ValidationManager._TypeValidators = new Dictionary<Type, ValidationCollection>();
        }
        #endregion

        #region ValidationManager：构造函数
        /// <summary>
        /// 验证中心
        /// </summary>
        /// <param name="target"></param>
        public ValidationManager(object target)
        {
            this._Errors = new List<string>();
            this._Result = true;
            //argument
            Guard.ArgumentNotNull(target, "target");
            this._Target = target;
        }
        #endregion

        #region DoIsValid：执行数据验证
        /// <summary>
        /// 执行数据验证
        /// 验证通过返回true；验证失败返回false
        /// </summary>
        /// <returns></returns>
        public bool DoIsValid()
        {
            //重置
            this.Reset();
            var validators = this.FindValidators(this._Target.GetType());
            if (validators == null || validators.Count <= 0)
            {
                return this._Result;
            }
            //执行验证，有未通过验证，则返回
            validators.ForEach(s =>
                {
                    if (!this._Result)
                    {
                        return;
                    }
                    this.DoIsValid(s.Value);
                });
            return this._Result;
        }

        #region DoIsValid：执行验证
        /// <summary>
        /// 执行验证，有未通过验证，则返回
        /// </summary>
        private void DoIsValid(List<Validator> validators)
        {
            if (validators == null || validators.Count <= 0)
            {
                return;
            }
            validators.ForEach(s =>
            {
                if (!s.IsValid(this._Target))
                {
                    this._Errors.Add(s.ErrorMsg);
                    this._Result = false;
                    return;
                }
            });
        }
        #endregion
        #endregion

        #region Private methods

        #region FindValidators：查找当前对象的验证器集合
        /// <summary>
        /// 查找当前对象的验证器集合
        /// </summary>
        private ValidationCollection FindValidators(Type type)
        {
            Guard.ArgumentNotNull(type, "type");
            if (ValidationManager._TypeValidators.ContainsKey(type))
            {
                return ValidationManager._TypeValidators[type];
            }
            ValidationCollection items = new ValidationCollection();
            //获取所有公共成员
            var pis = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var pi in pis)
            {
                try
                {
                    //获取验证属性
                    List<ValidatorAttribute> vas = pi.GetCustomAttributes(typeof(ValidatorAttribute), true).
                        Cast<ValidatorAttribute>().ToList();
                    if (vas.IsInvalid())
                    {
                        continue;
                    }
                    var vds = vas.ConvertAll<Validator>(s => s.GetValidator(pi));
                    items.Add(pi.Name, vds);
                }
                catch { }

            }
            //缓存验证器集合
            ValidationManager._TypeValidators.Add(type, items);
            return items;
        }
        #endregion

        #region Reset：重置属性
        /// <summary>
        /// 重置属性
        /// </summary>
        private void Reset()
        {
            this._Errors.Clear();
            this._Result = true;
        }
        #endregion
        #endregion

        #region Properties

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg
        {
            get { return this._Errors.FirstOrDefault(); }
        }
        #endregion

    }
}
