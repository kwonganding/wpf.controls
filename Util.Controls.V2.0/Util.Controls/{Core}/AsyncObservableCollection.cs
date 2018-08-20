using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Util.Controls
{
    /// <summary>
    /// 支持跨UI线程、少量并发的ObservableCollection集合。
    /// 注意如果有多线程并发，需要自己控制
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        private bool _IsUpdate;

        public AsyncObservableCollection()
        {
            this._IsUpdate = false;
        }

        public AsyncObservableCollection(IEnumerable<T> list)
            : base(list)
        {
            this._IsUpdate = false;
        }


        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (_IsUpdate) return;
            GUIThreadHelper.Invoke(() => RaiseCollectionChanged(e));
        }

        private void RaiseCollectionChanged(object param)
        {
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            GUIThreadHelper.BeginInvoke(() => RaisePropertyChanged(e));
        }

        private void RaisePropertyChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }

        public new void Clear()
        {
            if (this.IsInvalid())
                return;
            this.BeginUpdate(() =>
            {
                base.Clear();
            });
        }

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="items"></param>

        public void AddRange(IEnumerable<T> items)
        {
            if (items.IsInvalid()) return;
            this.BeginUpdate(() =>
            {
                foreach (var item in items)
                {
                    base.Add(item);
                }
            });
        }

        /// <summary>
        /// 更新集合内容，更新完成后统一通知集合变更。
        /// </summary>
        /// <param name="call"></param>
        public void BeginUpdate(Action call)
        {
            try
            {
                _IsUpdate = true;
                GUIThreadHelper.Invoke(() =>
                {
                    call();
                    base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                });
            }
            finally
            {
                _IsUpdate = false;
            }
        }
    }
}