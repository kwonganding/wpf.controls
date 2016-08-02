using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace Util.Controls
{
    /// <summary>
    /// FImage.xaml 的交互逻辑
    /// 注意：目前绑定TextBlock是在Keyword值变更时绑定的。
    /// </summary>
    public partial class HighTextBlock : TextBlock
    {
        public static readonly DependencyProperty KeywordProperty = DependencyProperty.Register(
            "Keyword", typeof(string), typeof(HighTextBlock), new PropertyMetadata(string.Empty));
        public string Keyword
        {
            get { return (string)GetValue(KeywordProperty); }
            set { SetValue(KeywordProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(string), typeof(HighTextBlock), new PropertyMetadata(string.Empty, OnContentPropertyChanged));

        public string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty HighForegroundProperty = DependencyProperty.Register(
            "HighForeground", typeof(Brush), typeof(HighTextBlock), new PropertyMetadata(default(Brush)));

        public Brush HighForeground
        {
            get { return (Brush)GetValue(HighForegroundProperty); }
            set { SetValue(HighForegroundProperty, value); }
        }

        public static readonly DependencyProperty HighBackgroundProperty = DependencyProperty.Register(
            "HighBackground", typeof(Brush), typeof(HighTextBlock), new PropertyMetadata(default(Brush)));

        public Brush HighBackground
        {
            get { return (Brush)GetValue(HighBackgroundProperty); }
            set { SetValue(HighBackgroundProperty, value); }
        }


        static HighTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HighTextBlock), new FrameworkPropertyMetadata(typeof(HighTextBlock)));
        }

        public HighTextBlock()
        {
            this.Loaded += HighTextBlock_Loaded;
        }

        void HighTextBlock_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 绑定图片资源
        /// </summary>
        private static void OnContentPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            try
            {

            var tb = sender as HighTextBlock;
            var txt = tb.Content;
            var key = tb.Keyword;
            if (tb == null) return;
            tb.Inlines.Clear();
            tb.Text = string.Empty;
            if (txt.IsInvalid() || key.IsInvalid())
            {
                tb.Text = txt;
                return;
            }
            //分割txt，只处理找到的第一个
            var index = txt.IndexOf(key);
            if (index < 0)
            {
                tb.Text = txt;
                return;
            }
            if (index > 0)
            {
                tb.Inlines.Add(new Run(txt.Substring(0, index)));
            }
            Run rkey = new Run(txt.Substring(index, key.Length));
            rkey.Background = tb.HighBackground;
            rkey.Foreground = tb.HighForeground;
            tb.Inlines.Add(rkey);
            var pos = index + key.Length;
            var len = txt.Length - pos;
            if (len > 0)
            {
                tb.Inlines.Add(new Run(txt.Substring(pos, len)));
            }

            }
            catch (Exception)
            {
            }
        }
    }
}
