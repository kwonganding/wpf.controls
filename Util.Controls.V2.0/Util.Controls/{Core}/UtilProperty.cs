using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace Util.Controls
{
    /// <summary>
    /// 公共附加属性
    /// </summary>
    public static class UtilProperty
    {
        /************************************ Attach Property **************************************/

        #region TreeView允许右键选中

        public static bool GetIsMouseRightSelectedItem(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMouseRightSelectedItemProperty);
        }
        public static void SetIsMouseRightSelectedItem(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMouseRightSelectedItemProperty, value);
        }

        public static readonly DependencyProperty IsMouseRightSelectedItemProperty =
           DependencyProperty.RegisterAttached("IsMouseRightSelectedItem", typeof(bool), typeof(UtilProperty),
               new PropertyMetadata(OnIsMouseRightSelectedItemChanged));

        private static void OnIsMouseRightSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeview = d as TreeView;
            if (treeview == null) return;
            if ((bool)e.NewValue)
            {
                treeview.MouseRightButtonDown += Treeview_MouseRightButtonDown;
            }
            else
            {
                treeview.MouseRightButtonDown -= Treeview_MouseRightButtonDown;
            }
        }
        private static void Treeview_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = (e.OriginalSource as DependencyObject).FindParent<TreeViewItem>(s => s != null);
            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
            }
        }

        #endregion

        #region AllowDrop 拖拽操作

        public static readonly DependencyProperty AllowDropProperty =
            DependencyProperty.RegisterAttached("AllowDrop", typeof(bool), typeof(UtilProperty),
                new FrameworkPropertyMetadata(false, AllowDropProperty_OnChanged));
        private static void AllowDropProperty_OnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var element = obj as FrameworkElement;
            if (element == null) return;
            element.AllowDrop = (bool)e.NewValue;
            if (element.AllowDrop)
            {
                element.PreviewMouseMove += Element_PreviewMouseMove;
            }
            else
            {
                element.PreviewMouseMove -= Element_PreviewMouseMove;
            }
        }

        private static void Element_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop((FrameworkElement)sender, e.Source, DragDropEffects.Move);
            }
        }

        public static void SetAllowDrop(DependencyObject element, bool value)
        {
            element.SetValue(AllowDropProperty, value);
        }

        public static bool GetAllowDrop(DependencyObject element)
        {
            return (bool)element.GetValue(AllowDropProperty);
        }

        #endregion

        #region The PreviewDropCommand:  拖动Command
        /// <summary>
        /// 拖动Command
        /// </summary>
        private static readonly DependencyProperty PreviewDropCommandProperty =
                    DependencyProperty.RegisterAttached("PreviewDropCommand", typeof(ICommand),
                        typeof(UtilProperty), new PropertyMetadata(PreviewDropCommandPropertyChangedCallBack));

        public static void SetPreviewDropCommand(this UIElement inUIElement, ICommand inCommand)
        {
            inUIElement.SetValue(PreviewDropCommandProperty, inCommand);
        }
        private static ICommand GetPreviewDropCommand(UIElement inUIElement)
        {
            return (ICommand)inUIElement.GetValue(PreviewDropCommandProperty);
        }
        private static void PreviewDropCommandPropertyChangedCallBack(DependencyObject inDependencyObject, DependencyPropertyChangedEventArgs inEventArgs)
        {
            UIElement uiElement = inDependencyObject as UIElement;
            if (null == uiElement) return;

            uiElement.Drop += (sender, args) =>
            {
                GetPreviewDropCommand(uiElement).Execute(args.Data);
                args.Handled = true;
            };
        }
        #endregion

        #region HeaderHeightProperty 标题高度
        /// <summary>
        /// 标题高度
        /// </summary>
        public static readonly DependencyProperty HeaderHeightProperty = DependencyProperty.RegisterAttached(
            "HeaderHeight", typeof(double), typeof(UtilProperty), new FrameworkPropertyMetadata(0D));

        public static double GetHeaderHeight(DependencyObject d)
        {
            return (double)d.GetValue(HeaderHeightProperty);
        }

        public static void SetHeaderHeight(DependencyObject obj, double value)
        {
            obj.SetValue(HeaderHeightProperty, value);
        }
        #endregion

        #region AttachCommandProperty 附加命令，扩展
        public static ICommand GetAttachCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(AttachCommandProperty);
        }

        public static void SetAttachCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(AttachCommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for AttachCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AttachCommandProperty =
            DependencyProperty.RegisterAttached("AttachCommand", typeof(ICommand), typeof(UtilProperty), new PropertyMetadata(null));

        #endregion

        #region AttachStyle：附加样式

        public static Style GetAttachStyle(DependencyObject obj)
        {
            return (Style)obj.GetValue(AttachStyleProperty);
        }
        public static void SetAttachStyle(DependencyObject obj, Style value)
        {
            obj.SetValue(AttachStyleProperty, value);
        }

        public static readonly DependencyProperty AttachStyleProperty =
            DependencyProperty.RegisterAttached("AttachStyle", typeof(Style), typeof(UtilProperty), new PropertyMetadata(null));

        #endregion

        #region MouseDoubleClickCommandProperty 实现鼠标双击的命令，参数通过空间Tag传递
        public static ICommand GetMouseDoubleClickCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(MouseDoubleClickCommandProperty);
        }

        public static void SetMouseDoubleClickCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(MouseDoubleClickCommandProperty, value);
        }
        /// <summary>
        /// 实现鼠标双击的命令，参数通过空间Tag传递
        /// </summary>
        public static readonly DependencyProperty MouseDoubleClickCommandProperty =
            DependencyProperty.RegisterAttached("MouseDoubleClickCommand", typeof(ICommand), typeof(UtilProperty), new FrameworkPropertyMetadata(null, MouseDoubleClickCommand_OnChanged));

        private static void MouseDoubleClickCommand_OnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var element = obj as FrameworkElement;
            if (element == null || e.NewValue == null)
            {
                return;
            }
            ICommand com = e.NewValue as ICommand;
            element.MouseLeftButtonDown +=
                delegate(object sender, MouseButtonEventArgs me)
                {
                    if (me.ClickCount == 2)
                    {
                        com.Execute(element.Tag);
                    }
                };
        }
        #endregion

        #region FocusBackground 获得焦点背景色，如选中状态

        public static readonly DependencyProperty FocusBackgroundProperty = DependencyProperty.RegisterAttached(
            "FocusBackground", typeof(Brush), typeof(UtilProperty), new FrameworkPropertyMetadata(null));

        public static void SetFocusBackground(DependencyObject element, Brush value)
        {
            element.SetValue(FocusBackgroundProperty, value);
        }

        public static Brush GetFocusBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(FocusBackgroundProperty);
        }

        #endregion

        #region FocusForeground 获得焦点前景色，，如选中状态

        public static readonly DependencyProperty FocusForegroundProperty = DependencyProperty.RegisterAttached(
            "FocusForeground", typeof(Brush), typeof(UtilProperty), new FrameworkPropertyMetadata(null));

        public static void SetFocusForeground(DependencyObject element, Brush value)
        {
            element.SetValue(FocusForegroundProperty, value);
        }

        public static Brush GetFocusForeground(DependencyObject element)
        {
            return (Brush)element.GetValue(FocusForegroundProperty);
        }

        #endregion

        #region FocusBorderBrush 焦点边框色，如输入控件，选中状态

        public static readonly DependencyProperty FocusBorderBrushProperty = DependencyProperty.RegisterAttached(
            "FocusBorderBrush", typeof(Brush), typeof(UtilProperty), new FrameworkPropertyMetadata(null));
        public static void SetFocusBorderBrush(DependencyObject element, Brush value)
        {
            element.SetValue(FocusBorderBrushProperty, value);
        }
        public static Brush GetFocusBorderBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(FocusBorderBrushProperty);
        }

        #endregion

        #region MouseOverBackgroundProperty 鼠标悬浮背景色

        public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.RegisterAttached(
            "MouseOverBackground", typeof(Brush), typeof(UtilProperty), new FrameworkPropertyMetadata(null));

        public static void SetMouseOverBackground(DependencyObject element, Brush value)
        {
            element.SetValue(MouseOverBackgroundProperty, value);
        }

        public static Brush GetMouseOverBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(MouseOverBackgroundProperty);
        }

        #endregion

        #region MouseOverForegroundProperty 鼠标悬浮前景色

        public static readonly DependencyProperty MouseOverForegroundProperty = DependencyProperty.RegisterAttached(
            "MouseOverForeground", typeof(Brush), typeof(UtilProperty), new FrameworkPropertyMetadata(null));

        public static void SetMouseOverForeground(DependencyObject element, Brush value)
        {
            element.SetValue(MouseOverForegroundProperty, value);
        }

        public static Brush GetMouseOverForeground(DependencyObject element)
        {
            return (Brush)element.GetValue(MouseOverForegroundProperty);
        }

        #endregion

        #region MouseOverBorderBrush 鼠标进入边框色，输入控件

        public static readonly DependencyProperty MouseOverBorderBrushProperty =
            DependencyProperty.RegisterAttached("MouseOverBorderBrush", typeof(Brush), typeof(UtilProperty),
                new FrameworkPropertyMetadata(Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Sets the brush used to draw the mouse over brush.
        /// </summary>
        public static void SetMouseOverBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(MouseOverBorderBrushProperty, value);
        }

        /// <summary>
        /// Gets the brush used to draw the mouse over brush.
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(CheckBox))]
        [AttachedPropertyBrowsableForType(typeof(RadioButton))]
        [AttachedPropertyBrowsableForType(typeof(DatePicker))]
        [AttachedPropertyBrowsableForType(typeof(ComboBox))]
        [AttachedPropertyBrowsableForType(typeof(RichTextBox))]
        public static Brush GetMouseOverBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(MouseOverBorderBrushProperty);
        }

        #endregion

        #region PressBackgroundProperty 鼠标按下控件背景色

        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.RegisterAttached(
            "PressedBackground", typeof(Brush), typeof(UtilProperty), new FrameworkPropertyMetadata(null));

        public static void SetPressedBackground(DependencyObject element, Brush value)
        {
            element.SetValue(PressedBackgroundProperty, value);
        }

        public static Brush GetPressedBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(PressedBackgroundProperty);
        }

        #endregion

        #region PressedForegroundProperty 鼠标按下控件前景色

        public static readonly DependencyProperty PressedForegroundProperty = DependencyProperty.RegisterAttached(
            "PressedForeground", typeof(Brush), typeof(UtilProperty), new FrameworkPropertyMetadata(null));

        public static void SetPressedForeground(DependencyObject element, Brush value)
        {
            element.SetValue(PressedForegroundProperty, value);
        }

        public static Brush GetPressedForeground(DependencyObject element)
        {
            return (Brush)element.GetValue(PressedForegroundProperty);
        }

        #endregion

        #region AttachContentProperty 附加组件模板
        /// <summary>
        /// 附加组件模板
        /// </summary>
        public static readonly DependencyProperty AttachContentProperty = DependencyProperty.RegisterAttached(
            "AttachContent", typeof(ControlTemplate), typeof(UtilProperty), new FrameworkPropertyMetadata(null));

        public static ControlTemplate GetAttachContent(DependencyObject d)
        {
            return (ControlTemplate)d.GetValue(AttachContentProperty);
        }

        public static void SetAttachContent(DependencyObject obj, ControlTemplate value)
        {
            obj.SetValue(AttachContentProperty, value);
        }
        #endregion

        #region WatermarkProperty 水印
        /// <summary>
        /// 水印
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached(
            "Watermark", typeof(string), typeof(UtilProperty), new FrameworkPropertyMetadata(""));

        public static string GetWatermark(DependencyObject d)
        {
            return (string)d.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkProperty, value);
        }
        #endregion

        #region FIconProperty 字体图标
        /// <summary>
        /// 字体图标
        /// </summary>
        public static readonly DependencyProperty FIconProperty = DependencyProperty.RegisterAttached(
            "FIcon", typeof(string), typeof(UtilProperty), new FrameworkPropertyMetadata(""));

        public static string GetFIcon(DependencyObject d)
        {
            return (string)d.GetValue(FIconProperty);
        }

        public static void SetFIcon(DependencyObject obj, string value)
        {
            obj.SetValue(FIconProperty, value);
        }
        #endregion

        #region FIconSizeProperty 字体图标大小
        /// <summary>
        /// 字体图标
        /// </summary>
        public static readonly DependencyProperty FIconSizeProperty = DependencyProperty.RegisterAttached(
            "FIconSize", typeof(double), typeof(UtilProperty), new FrameworkPropertyMetadata(12D));

        public static double GetFIconSize(DependencyObject d)
        {
            return (double)d.GetValue(FIconSizeProperty);
        }

        public static void SetFIconSize(DependencyObject obj, double value)
        {
            obj.SetValue(FIconSizeProperty, value);
        }
        #endregion

        #region FIconMarginProperty 字体图标边距
        /// <summary>
        /// 字体图标
        /// </summary>
        public static readonly DependencyProperty FIconMarginProperty = DependencyProperty.RegisterAttached(
            "FIconMargin", typeof(Thickness), typeof(UtilProperty), new FrameworkPropertyMetadata(null));

        public static Thickness GetFIconMargin(DependencyObject d)
        {
            return (Thickness)d.GetValue(FIconMarginProperty);
        }

        public static void SetFIconMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(FIconMarginProperty, value);
        }
        #endregion

        #region BuildTextBoxCommand 修复TextBoxBase的剪切、复制bug
        public static bool GetBuildTextBoxCommand(DependencyObject obj)
        {
            return (bool)obj.GetValue(BuildTextBoxCommandProperty);
        }
        public static void SetBuildTextBoxCommand(DependencyObject obj, bool value)
        {
            obj.SetValue(BuildTextBoxCommandProperty, value);
        }
        /// <summary>
        /// 修复TextBoxBase的剪切、复制bug
        /// </summary>
        public static readonly DependencyProperty BuildTextBoxCommandProperty =
            DependencyProperty.RegisterAttached("BuildTextBoxCommand", typeof(bool), typeof(UtilProperty), new PropertyMetadata(false, BuildTextBoxCommandPropertyChanged));
        private static void BuildTextBoxCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var txt = d as TextBoxBase;
            if (txt == null) return;
            var value = (bool)e.NewValue;
            if (value)
            {
                CommandManager.AddPreviewExecutedHandler(txt, new ExecutedRoutedEventHandler(textBox_PreviewExecuted));
            }
            else
            {
                CommandManager.RemovePreviewExecutedHandler(txt, new ExecutedRoutedEventHandler(textBox_PreviewExecuted));
            }
        }
        private static void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var txt = sender as TextBox;
            var rtxt = sender as RichTextBox;
            if (txt == null && rtxt == null) return;
            if (e.Command == ApplicationCommands.Copy)
            {
                if (txt != null)
                    Clipboard.SetDataObject(txt.SelectedText);
                else if (rtxt != null)
                    rtxt.Copy();
                e.Handled = true;
            }
            if (e.Command == ApplicationCommands.Cut)
            {
                if (txt != null)
                {
                    Clipboard.SetDataObject(txt.SelectedText);
                    txt.SelectedText = "";
                }
                else if (rtxt != null)
                {
                    rtxt.Copy();
                    rtxt.Selection.Text = "";
                }
                e.Handled = true;
            }
        }

        #endregion

        #region KeyEnterToUpdateTextProperty Enter回车更新TextBox的Text值

        public static bool GetKeyEnterToUpdateTextProperty(DependencyObject obj)
        {
            return (bool)obj.GetValue(KeyEnterToUpdateTextPropertyProperty);
        }

        public static void SetKeyEnterToUpdateTextProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(KeyEnterToUpdateTextPropertyProperty, value);
        }

        /// <summary>
        /// 针对Text属性，开启键盘Enter更新TextProperty的值
        /// </summary>
        public static readonly DependencyProperty KeyEnterToUpdateTextPropertyProperty =
            DependencyProperty.RegisterAttached("KeyEnterToUpdateTextProperty", typeof(bool),
                typeof(UtilProperty),
                new PropertyMetadata(false, KeyEnterToUpdateTextPropertyChanged));
        /// <summary>
        /// Enter更新TextProperty的值的事件处理
        /// </summary>
        private static KeyEventHandler _KeyEnterToUpdateTextProperty = delegate(object sender, KeyEventArgs ke)
        {
            if (ke.Key == Key.Enter)
                (sender as TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        };
        private static void KeyEnterToUpdateTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var txt = d as TextBox;
            if (txt == null) return;
            var value = (bool)e.NewValue;
            if (value)
            {
                txt.KeyUp += _KeyEnterToUpdateTextProperty;
            }
            else
            {
                txt.KeyUp -= _KeyEnterToUpdateTextProperty;
            }
        }
        #endregion

        #region AllowsAnimationProperty 启用旋转动画
        /// <summary>
        /// 启用旋转动画，一般绑定到别的属性上，使用案例：{Binding IsMouseOver,RelativeSource={RelativeSource Self}}
        /// </summary>
        public static readonly DependencyProperty AllowsAnimationProperty = DependencyProperty.RegisterAttached("AllowsAnimation"
            , typeof(bool), typeof(UtilProperty), new FrameworkPropertyMetadata(false, AllowsAnimationChanged));

        public static bool GetAllowsAnimation(DependencyObject d)
        {
            return (bool)d.GetValue(AllowsAnimationProperty);
        }
        public static void SetAllowsAnimation(DependencyObject obj, bool value)
        {
            obj.SetValue(AllowsAnimationProperty, value);
        }

        /// <summary>
        /// 旋转动画刻度
        /// </summary>
        [ThreadStatic]
        private static DoubleAnimation RotateAnimation;

        /// <summary>
        /// 绑定动画事件
        /// </summary>
        private static void AllowsAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as FrameworkElement;
            if (uc == null) return;
            if (uc.RenderTransformOrigin == new Point(0, 0))
            {
                uc.RenderTransformOrigin = new Point(0.5, 0.5);
                RotateTransform trans = new RotateTransform(0);
                uc.RenderTransform = trans;
            }
            var value = (bool)e.NewValue;
            if (RotateAnimation == null)
            {
                RotateAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(200)));
                Timeline.SetDesiredFrameRate(RotateAnimation, 30);
            }
            if (value)
            {
                RotateAnimation.To = 180;
                uc.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, RotateAnimation);
            }
            else
            {
                RotateAnimation.To = 0;
                uc.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, RotateAnimation);
            }
        }
        #endregion

        #region FIconAnimationEnableProperty

        /// <summary>
        /// 是否启用FIcon的动画效果，目前用户Button内嵌的FIcon元素的动画设置
        /// </summary>
        public static readonly DependencyProperty FIconAnimationEnableProperty =
            DependencyProperty.RegisterAttached("FIconAnimationEnable", typeof(bool), typeof(UtilProperty),
                new PropertyMetadata(default(bool)));

        public static void SetFIconAnimationEnable(DependencyObject element, bool value)
        {
            element.SetValue(FIconAnimationEnableProperty, value);
        }

        public static bool GetFIconAnimationEnable(DependencyObject element)
        {
            return (bool)element.GetValue(FIconAnimationEnableProperty);
        }

        #endregion

        #region CornerRadiusProperty Border圆角
        /// <summary>
        /// Border圆角
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", typeof(CornerRadius), typeof(UtilProperty), new FrameworkPropertyMetadata(null));

        public static CornerRadius GetCornerRadius(DependencyObject d)
        {
            return (CornerRadius)d.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }
        #endregion

        #region LabelProperty TextBox的头部Label
        /// <summary>
        /// TextBox的头部Label
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty.RegisterAttached(
            "Label", typeof(string), typeof(UtilProperty), new FrameworkPropertyMetadata(null));

        public static string GetLabel(DependencyObject d)
        {
            return (string)d.GetValue(LabelProperty);
        }

        public static void SetLabel(DependencyObject obj, string value)
        {
            obj.SetValue(LabelProperty, value);
        }
        #endregion

        #region LabelWidthProperty

        public static double GetLabelWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(LabelWidthProperty);
        }

        public static void SetLabelWidth(DependencyObject obj, double value)
        {
            obj.SetValue(LabelWidthProperty, value);
        }

        /// <summary>
        /// 默认输入框label宽度，默认60，默认label模板有效
        /// </summary>
        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.RegisterAttached("LabelWidth", typeof(double), typeof(UtilProperty),
                new PropertyMetadata(60D));

        #endregion

        #region LabelTemplateProperty TextBox的头部Label模板
        /// <summary>
        /// TextBox的头部Label模板
        /// </summary>
        public static readonly DependencyProperty LabelTemplateProperty = DependencyProperty.RegisterAttached(
            "LabelTemplate", typeof(ControlTemplate), typeof(UtilProperty), new FrameworkPropertyMetadata(null));

        public static ControlTemplate GetLabelTemplate(DependencyObject d)
        {
            return (ControlTemplate)d.GetValue(LabelTemplateProperty);
        }

        public static void SetLabelTemplate(DependencyObject obj, ControlTemplate value)
        {
            obj.SetValue(LabelTemplateProperty, value);
        }
        #endregion

        #region TimeVisibilityProperty 是否显示时间

        public static readonly DependencyProperty TimeVisibilityProperty = DependencyProperty.RegisterAttached(
            "TimeVisibility", typeof(Visibility), typeof(UtilProperty), new FrameworkPropertyMetadata(Visibility.Collapsed));

        public static void SetTimeVisibility(DependencyObject element, Visibility value)
        {
            element.SetValue(TimeVisibilityProperty, value);
        }

        public static Visibility GetTimeVisibility(DependencyObject element)
        {
            return (Visibility)element.GetValue(TimeVisibilityProperty);
        }

        #endregion

        //Text,   

        /************************************ RoutedUICommand Behavior enable **************************************/

        #region IsClearTextButtonBehaviorEnabledProperty 清除输入框Text值按钮行为开关（设为ture时才会绑定事件）
        /// <summary>
        /// 清除输入框Text值按钮行为开关
        /// </summary>
        public static readonly DependencyProperty IsClearTextButtonBehaviorEnabledProperty = DependencyProperty.RegisterAttached("IsClearTextButtonBehaviorEnabled"
            , typeof(bool), typeof(UtilProperty), new FrameworkPropertyMetadata(false, IsClearTextButtonBehaviorEnabledChanged));

        public static bool GetIsClearTextButtonBehaviorEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(IsClearTextButtonBehaviorEnabledProperty);
        }

        public static void SetIsClearTextButtonBehaviorEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsClearTextButtonBehaviorEnabledProperty, value);
        }

        /// <summary>
        /// 绑定清除Text操作的按钮事件
        /// </summary>
        private static void IsClearTextButtonBehaviorEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if (e.OldValue == e.NewValue || button == null) return;
            var value = (bool)e.NewValue;
            if (value)
            {
                button.CommandBindings.Add(ClearTextCommandBinding);
            }
            else if (button.CommandBindings.Contains(ClearTextCommandBinding))
            {
                button.CommandBindings.Remove(ClearTextCommandBinding);
            }
        }

        #endregion

        #region IsOpenFileButtonBehaviorEnabledProperty 选择文件命令行为开关
        /// <summary>
        /// 选择文件命令行为开关
        /// </summary>
        public static readonly DependencyProperty IsOpenFileButtonBehaviorEnabledProperty = DependencyProperty.RegisterAttached("IsOpenFileButtonBehaviorEnabled"
            , typeof(bool), typeof(UtilProperty), new FrameworkPropertyMetadata(false, IsOpenFileButtonBehaviorEnabledChanged));

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetIsOpenFileButtonBehaviorEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(IsOpenFileButtonBehaviorEnabledProperty);
        }

        public static void SetIsOpenFileButtonBehaviorEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsOpenFileButtonBehaviorEnabledProperty, value);
        }

        private static void IsOpenFileButtonBehaviorEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if (e.OldValue == e.NewValue || button == null) return;
            var value = (bool)e.NewValue;
            if (value)
            {
                button.CommandBindings.Add(OpenFileCommandBinding);
            }
            else if (button.CommandBindings.Contains(OpenFileCommandBinding))
            {
                button.CommandBindings.Remove(OpenFileCommandBinding);
            }
        }

        #endregion

        #region IsOpenFolderButtonBehaviorEnabledProperty 选择文件夹命令行为开关
        /// <summary>
        /// 选择文件夹命令行为开关
        /// </summary>
        public static readonly DependencyProperty IsOpenFolderButtonBehaviorEnabledProperty = DependencyProperty.RegisterAttached("IsOpenFolderButtonBehaviorEnabled"
            , typeof(bool), typeof(UtilProperty), new FrameworkPropertyMetadata(false, IsOpenFolderButtonBehaviorEnabledChanged));

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetIsOpenFolderButtonBehaviorEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(IsOpenFolderButtonBehaviorEnabledProperty);
        }

        public static void SetIsOpenFolderButtonBehaviorEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsOpenFolderButtonBehaviorEnabledProperty, value);
        }

        private static void IsOpenFolderButtonBehaviorEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if (e.OldValue == e.NewValue || button == null) return;
            var value = (bool)e.NewValue;
            if (value)
            {
                button.CommandBindings.Add(OpenFolderCommandBinding);
            }
            else if (button.CommandBindings.Contains(OpenFolderCommandBinding))
            {
                button.CommandBindings.Remove(OpenFolderCommandBinding);
            }
        }

        #endregion

        #region IsSaveFileButtonBehaviorEnabledProperty 选择文件保存路径及名称
        /// <summary>
        /// 选择文件保存路径及名称
        /// </summary>
        public static readonly DependencyProperty IsSaveFileButtonBehaviorEnabledProperty = DependencyProperty.RegisterAttached("IsSaveFileButtonBehaviorEnabled"
            , typeof(bool), typeof(UtilProperty), new FrameworkPropertyMetadata(false, IsSaveFileButtonBehaviorEnabledChanged));

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetIsSaveFileButtonBehaviorEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(IsSaveFileButtonBehaviorEnabledProperty);
        }

        public static void SetIsSaveFileButtonBehaviorEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSaveFileButtonBehaviorEnabledProperty, value);
        }

        private static void IsSaveFileButtonBehaviorEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if (e.OldValue == e.NewValue || button == null) return;
            var value = (bool)e.NewValue;
            if (value)
            {
                button.CommandBindings.Add(SaveFileCommandBinding);
            }
            else if (button.CommandBindings.Contains(SaveFileCommandBinding))
            {
                button.CommandBindings.Remove(SaveFileCommandBinding);
            }
        }

        #endregion

        /************************************ RoutedUICommand **************************************/

        #region ClearTextCommand 清除输入框Text事件命令

        /// <summary>
        /// 清除输入框Text事件命令，需要使用IsClearTextButtonBehaviorEnabledChanged绑定命令
        /// </summary>
        public static RoutedUICommand ClearTextCommand { get; private set; }

        /// <summary>
        /// ClearTextCommand绑定事件
        /// </summary>
        private static readonly CommandBinding ClearTextCommandBinding;

        /// <summary>
        /// 清除输入框文本值
        /// </summary>
        private static void ClearButtonClick(object sender, ExecutedRoutedEventArgs e)
        {
            var tbox = e.Parameter as FrameworkElement;
            if (tbox == null) return;
            if (tbox is TextBox)
            {
                ((TextBox)tbox).Clear();
            }
            if (tbox is PasswordBox)
            {
                ((PasswordBox)tbox).Clear();
            }
            if (tbox is ComboBox)
            {
                var cb = tbox as ComboBox;
                cb.SelectedItem = null;
                cb.Text = string.Empty;
            }
            if (tbox is MultiComboBox)
            {
                var cb = tbox as MultiComboBox;
                cb.SelectedItem = null;
                cb.UnselectAll();
                cb.Text = string.Empty;
            }
            if (tbox is TreeComboBox)
            {
                var cb = tbox as TreeComboBox;
                cb.Text = string.Empty;
            }
            if (tbox is DatePicker)
            {
                var dp = tbox as DatePicker;
                dp.SelectedDate = null;
                dp.Text = string.Empty;
            }
            tbox.Focus();
        }

        #endregion

        #region OpenFileCommand 选择文件命令

        /// <summary>
        /// 选择文件命令，需要使用IsClearTextButtonBehaviorEnabledChanged绑定命令
        /// </summary>
        public static RoutedUICommand OpenFileCommand { get; private set; }

        /// <summary>
        /// OpenFileCommand绑定事件
        /// </summary>
        private static readonly CommandBinding OpenFileCommandBinding;

        /// <summary>
        /// 执行OpenFileCommand
        /// </summary>
        private static void OpenFileButtonClick(object sender, ExecutedRoutedEventArgs e)
        {
            var txt = e.Parameter as TextBox;
            Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
            fd.Title = "请选择文件";
            fd.Filter = txt.Tag.ToSafeString();
            fd.FileName = txt.Text.Trim();
            if (fd.ShowDialog() == true)
            {
                txt.Text = fd.FileName;
            }
            txt.Focus();
        }

        #endregion

        #region OpenFolderCommand 选择文件夹命令

        /// <summary>
        /// 选择文件夹命令
        /// </summary>
        public static RoutedUICommand OpenFolderCommand { get; private set; }

        /// <summary>
        /// OpenFolderCommand绑定事件
        /// </summary>
        private static readonly CommandBinding OpenFolderCommandBinding;

        /// <summary>
        /// 执行OpenFolderCommand
        /// </summary>
        private static void OpenFolderButtonClick(object sender, ExecutedRoutedEventArgs e)
        {
            var txt = e.Parameter as TextBox;
            if (txt == null) return;
            System.Windows.Forms.FolderBrowserDialog fd = new System.Windows.Forms.FolderBrowserDialog();
            fd.Description = "请选择文件路径";
            fd.SelectedPath = txt.Text.Trim();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt.Text = fd.SelectedPath;
            }
            txt.Focus();
        }

        #endregion

        #region SaveFileCommand 选择文件保存路径及名称

        /// <summary>
        /// 选择文件保存路径及名称
        /// </summary>
        public static RoutedUICommand SaveFileCommand { get; private set; }

        /// <summary>
        /// SaveFileCommand绑定事件
        /// </summary>
        private static readonly CommandBinding SaveFileCommandBinding;

        /// <summary>
        /// 执行OpenFileCommand
        /// </summary>
        private static void SaveFileButtonClick(object sender, ExecutedRoutedEventArgs e)
        {
            var txt = e.Parameter as TextBox;
            if (txt == null) return;
            System.Windows.Forms.SaveFileDialog fd = new System.Windows.Forms.SaveFileDialog();
            fd.Title = "文件保存路径";
            fd.Filter = txt.Tag.ToSafeString();
            fd.FileName = txt.Text.Trim();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt.Text = fd.FileName;
            }
            txt.Focus();
        }

        #endregion


        /// <summary>
        /// 静态构造函数
        /// </summary>
        static UtilProperty()
        {
            //ClearTextCommand
            ClearTextCommand = new RoutedUICommand();
            ClearTextCommandBinding = new CommandBinding(ClearTextCommand);
            ClearTextCommandBinding.Executed += ClearButtonClick;
            //OpenFileCommand
            OpenFileCommand = new RoutedUICommand();
            OpenFileCommandBinding = new CommandBinding(OpenFileCommand);
            OpenFileCommandBinding.Executed += OpenFileButtonClick;
            //OpenFolderCommand
            OpenFolderCommand = new RoutedUICommand();
            OpenFolderCommandBinding = new CommandBinding(OpenFolderCommand);
            OpenFolderCommandBinding.Executed += OpenFolderButtonClick;

            SaveFileCommand = new RoutedUICommand();
            SaveFileCommandBinding = new CommandBinding(SaveFileCommand);
            SaveFileCommandBinding.Executed += SaveFileButtonClick;
        }
    }
}
