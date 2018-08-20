using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Util.Controls
{
    /// <summary>
    /// MultiComboBox.xaml 的交互逻辑
    /// </summary>
    [TemplatePart(Name = "PART_TreeView", Type = typeof(TreeView))]
    public partial class TreeComboBox : ComboBox
    {
        /// <summary>
        /// 是否只能选中叶子节点项
        /// </summary>
        public bool IsOnlyLeafItemSelectEnable
        {
            get { return (bool)GetValue(IsOnlyLeafItemSelectEnableProperty); }
            set { SetValue(IsOnlyLeafItemSelectEnableProperty, value); }
        }
        public static readonly DependencyProperty IsOnlyLeafItemSelectEnableProperty =
            DependencyProperty.Register("IsOnlyLeafItemSelectEnable", typeof(bool), typeof(TreeComboBox), new PropertyMetadata(false));

        /// <summary>
        /// 获取或设置选择项。
        /// 注意：对于树形选择控件，选中项有可能会设置不上，目前的一个即将方案就是：
        /// 先让树上的所有节点展开
        /// tcb.IsDropDownOpen = true;
        ///     tcb.IsDropDownOpen = false;
        /// 还有一种就是，设置TreeComboBox的Text值
        /// </summary>
        public new object SelectedItem
        {
            get
            {
                if (this._TreeView == null) return null;
                return this._TreeView.SelectedItem;
            }
            set
            {
                this._SelectedItem = value;
                this.Text = value.ToSafeString();
                if (this._TreeView != null)
                {
                    this._TreeView.SelectObject(value);
                }
            }
        }
        private object _SelectedItem;

        /// <summary>
        /// SelectedItem变化后的事件通知
        /// </summary>
        public Action<object> OnSelectedItemChanged { get; set; }

        public TreeView _TreeView;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this._TreeView = Template.FindName("PART_TreeView", this) as TreeView;
            this._TreeView.MouseDoubleClick += _TreeView_MouseDoubleClick;
            this._TreeView.PreviewMouseLeftButtonUp += _TreeView_PreviewMouseLeftButtonUp;
        }

        private void _TreeView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this._TreeView != null && this._TreeView.SelectedItem != null)
            {
                if (this.IsOnlyLeafItemSelectEnable && !this.IsLeafItem())
                    return;

                SelectedItem = this._TreeView.SelectedItem;
                if (this.OnSelectedItemChanged != null)
                {
                    this.OnSelectedItemChanged(this._TreeView.SelectedItem);
                }
            }
        }

        void _TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.IsOnlyLeafItemSelectEnable && !this.IsLeafItem())
                return;
            this.IsDropDownOpen = false;
        }

        private bool IsLeafItem()
        {
            var item = this._TreeView.GetItemFromObject(this._TreeView.SelectedItem);
            if (item == null)
                return false;
            return item.ItemsSource.IsInvalid();
        }

        protected override void OnDropDownOpened(EventArgs e)
        {
            base.OnDropDownOpened(e);
            this._TreeView.SelectObject(this._SelectedItem);
        }

        static TreeComboBox()
        {
        }
    }
}
