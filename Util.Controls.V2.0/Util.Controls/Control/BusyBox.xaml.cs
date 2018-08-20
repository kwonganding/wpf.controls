using System.Windows;
using System.Windows.Controls;

namespace Util.Controls
{
    /// <summary>
    /// BusyBox.xaml 的交互逻辑
    /// </summary>
    public partial class BusyBox : UserControl
    {
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(BusyBox), new PropertyMetadata(false));
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        static BusyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyBox), new FrameworkPropertyMetadata(typeof(BusyBox)));
        }
    }
}
