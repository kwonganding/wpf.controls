using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Util.Controls
{
    /// <summary>
    /// 实现了属性更改通知的基类
    /// </summary>
    [Serializable]
    public abstract class BaseNotifyPropertyChanged : System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// 属性值变化时发生
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Guard.ArgumentNotNull(propertyName, nameof(propertyName));
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 属性值变化时发生
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            Guard.ArgumentNotNull(propertyExpression, nameof(propertyExpression));
            var propertyName = (propertyExpression.Body as MemberExpression).Member.Name;
            this.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// 主动通知属性更改，参数为要通知的属性名称
        /// </summary>
        public void NotifyPropertyChanged(string propertyName)
        {
            Guard.ArgumentNotNull(propertyName, nameof(propertyName));
            this.OnPropertyChanged(propertyName);
        }

        [field: NonSerialized]
        public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}