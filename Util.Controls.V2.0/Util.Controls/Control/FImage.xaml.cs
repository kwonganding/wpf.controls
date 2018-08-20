using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Util.Controls
{
    /// <summary>
    /// FImage，同时支持图片资源和字体图表的图片控件.
    /// 注意，但不支持切换，同一个地方只适用于一种情况
    /// </summary>
    public partial class FImage : UserControl
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(string), typeof(FImage), new PropertyMetadata(OnSourcePropertyChanged));
        /// <summary>
        /// 资源设置，支持FIcon图标，例如：&#xe64e;同FIcon使用方法一样
        /// 支持图片资源路径（相对路径、绝对路径）
        /// </summary>
        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        internal TextBlock FIcon { get { return this.Icon; } }
        internal Viewbox ContentBox { get { return this.vbox; } }

        public FImage()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.Loaded += FImage_Loaded;
        }

        void FImage_Loaded(object sender, RoutedEventArgs e)
        {
            BindSource(this);
        }

        /// <summary>
        /// 属性更改处理事件
        /// </summary>
        private static void OnSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            FImage img = sender as FImage;
            if (img == null) return;
            if (!img.IsLoaded) return;
            BindSource(img);
        }
        private static void BindSource(FImage img)
        {
            var value = img.Source;
            if (value.IsInvalid()) return;
            if (value.Length == 1)
            {
                img.FIcon.Text = value;
                return;
            }

            img.Dispatcher.BeginInvoke(new Action(() =>
            {
                //创建一个image对象
                var image = new Image() { Stretch = Stretch.Uniform };
                image.Source = Util.Controls.ImageHelper.CreateImageSourceThumbnia(value, (int)img.Width, (int)img.Height);
                img.ContentBox.Child = image;
            }), DispatcherPriority.ApplicationIdle);
        }
    }
}
