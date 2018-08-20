using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Util.Controls
{
    /// <summary>
    /// WindowBase.xaml 的交互逻辑
    /// </summary>
    public class WindowBase : Window
    {
        private const string PART_Header = "PART_Header";

        private FrameworkElement headerElement;


        #region 默认Header：窗体字体图标FIcon

        public static readonly DependencyProperty FIconProperty =
            DependencyProperty.Register("FIcon", typeof(string), typeof(WindowBase), new PropertyMetadata("\ue62e"));

        /// <summary>
        /// 按钮字体图标编码
        /// </summary>
        public string FIcon
        {
            get { return (string)GetValue(FIconProperty); }
            set { SetValue(FIconProperty, value); }
        }

        #endregion

        #region  默认Header：窗体字体图标大小

        public static readonly DependencyProperty FIconSizeProperty =
            DependencyProperty.Register("FIconSize", typeof(double), typeof(WindowBase), new PropertyMetadata(20D));

        /// <summary>
        /// 按钮字体图标大小
        /// </summary>
        public double FIconSize
        {
            get { return (double)GetValue(FIconSizeProperty); }
            set { SetValue(FIconSizeProperty, value); }
        }

        #endregion

        #region CaptionHeight 标题栏高度

        public static readonly DependencyProperty CaptionHeightProperty =
            DependencyProperty.Register("CaptionHeight", typeof(double), typeof(WindowBase), new PropertyMetadata(26D));

        /// <summary>
        /// 标题高度
        /// </summary>
        public double CaptionHeight
        {
            get { return (double)GetValue(CaptionHeightProperty); }
            set
            {
                SetValue(CaptionHeightProperty, value);
                //this._WC.CaptionHeight = value;
            }
        }

        #endregion

        #region CaptionBackground 标题栏背景色

        public static readonly DependencyProperty CaptionBackgroundProperty = DependencyProperty.Register(
            "CaptionBackground", typeof(Brush), typeof(WindowBase), new PropertyMetadata(null));

        public Brush CaptionBackground
        {
            get { return (Brush)GetValue(CaptionBackgroundProperty); }
            set { SetValue(CaptionBackgroundProperty, value); }
        }

        #endregion

        #region CaptionForeground 标题栏前景景色

        public static readonly DependencyProperty CaptionForegroundProperty = DependencyProperty.Register(
            "CaptionForeground", typeof(Brush), typeof(WindowBase), new PropertyMetadata(null));

        public Brush CaptionForeground
        {
            get { return (Brush)GetValue(CaptionForegroundProperty); }
            set { SetValue(CaptionForegroundProperty, value); }
        }

        #endregion

        #region Header 标题栏内容模板，以提供默认模板，可自定义

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(ControlTemplate), typeof(WindowBase), new PropertyMetadata(null));

        public ControlTemplate Header
        {
            get { return (ControlTemplate)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        #endregion

        #region MaxboxEnable 是否显示最大化按钮

        public static readonly DependencyProperty MaxboxEnableProperty = DependencyProperty.Register(
            "MaxboxEnable", typeof(bool), typeof(WindowBase), new PropertyMetadata(true));

        public bool MaxboxEnable
        {
            get { return (bool)GetValue(MaxboxEnableProperty); }
            set { SetValue(MaxboxEnableProperty, value); }
        }

        #endregion

        #region MinboxEnable 是否显示最小化按钮

        public static readonly DependencyProperty MinboxEnableProperty = DependencyProperty.Register(
            "MinboxEnable", typeof(bool), typeof(WindowBase), new PropertyMetadata(true));

        public bool MinboxEnable
        {
            get { return (bool)GetValue(MinboxEnableProperty); }
            set { SetValue(MinboxEnableProperty, value); }
        }

        #endregion

        /// <summary>
        /// 窗体标题控件元素
        /// </summary>
        public FrameworkElement HeaderElement { get { return this.headerElement; } }

        /// <summary>
        /// 是否启用鼠标左键移动窗体，默认启用
        /// </summary>
        public bool LeftMouseMoveWindowEnable { get; set; }
        /// <summary>
        /// 是否启用键盘Esc关闭窗口，默认启用
        /// </summary>
        public bool KeyEscColseWindowEnable { get; set; }

        /// <summary>
        /// 类似DialogResult的状态标示，在缓存模式下使用
        /// </summary>
        public bool ShowDialogResult { get; set; }

        /****************** commands ******************/
        public ICommand CloseWindowCommand { get; protected set; }
        public ICommand MaximizeWindowCommand { get; protected set; }
        public ICommand MinimizeWindowCommand { get; protected set; }

        public WindowBase()
        {
            ControlHelper.SetOwnerWindow(this);
            this.WindowStartupLocation = this.Owner == null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;

            this.Style = this.FindResource("DefaultWindowStyle") as Style;
            //bind command
            this.CloseWindowCommand = new RoutedUICommand();
            this.MaximizeWindowCommand = new RoutedUICommand();
            this.MinimizeWindowCommand = new RoutedUICommand();
            this.BindCommand(CloseWindowCommand, this.CloseCommand_Execute);
            this.BindCommand(MaximizeWindowCommand, this.MaxCommand_Execute);
            this.BindCommand(MinimizeWindowCommand, this.MinCommand_Execute);
            this.LeftMouseMoveWindowEnable = true;
            this.KeyEscColseWindowEnable = true;
            this.KeyDown += WindowBase_KeyDown;
            Win32Helper.SetWindowSizeHook(this);
        }

        /// <summary>
        /// 参数cacheEnable为true，则支持缓存当前窗体(窗体不会被关闭，而是隐藏)，下次使用就会复用。
        /// 如果启用缓存，注意事项：
        /// 1.需要通过GetCacheWindow方法获取缓存窗体；
        /// 2.窗口状态建议使用ShowDialogResult
        /// 3.推荐频繁使用、且独占焦点的模式窗体实现该构造函数。可以参考基础控件中MessageBox的使用
        /// </summary>
        public WindowBase(bool cacheEnable)
            : this()
        {
            this._CacheEnable = cacheEnable;
            if (cacheEnable)
            {
                this.Closing +=
                delegate (object sender, CancelEventArgs e)
                {
                    if (_CacheEnable)
                    {
                        this.Hide();
                        e.Cancel = true;
                        //尝试让宿主获取到窗口焦点
                        if (this.Owner != null)
                            this.Owner.Focus();
                    }
                };
                this.Activated += WindowBase_Activated;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.headerElement = this.GetTemplateChild(PART_Header) as FrameworkElement;
        }

        void WindowBase_Activated(object sender, EventArgs e)
        {
            this.ShowDialogResult = false;
        }
        private bool _CacheEnable = false;

        private void WindowBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.KeyEscColseWindowEnable && e.Key == Key.Escape)
            {
                this.Close();
                e.Handled = true;
            }
        }

        private void CloseCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void MaxCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            e.Handled = true;
        }

        private void MinCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            e.Handled = true;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (this.WindowState == WindowState.Maximized || this.LeftMouseMoveWindowEnable == false) return;
            //e.RoutedEvent
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
