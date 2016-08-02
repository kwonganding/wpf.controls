using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Util.Controls.WPFTest
{
    /// <summary>
    /// Page_Image.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Image : Page
    {
        public Page_Image()
        {
            InitializeComponent();
        }

        private void FButton_Click(object sender, RoutedEventArgs e)
        {
            this.ImageCache.ThumbnailSource =
                "http://www.wallsave.com/wallpapers/1920x1080/beautiful-girl/733941/beautiful-girl-girls-hd-733941.jpg";
        }

        private void FButton_BindClick(object sender, RoutedEventArgs e)
        {
            this.Gif.GIFSource = this.gifSource.Text.Trim();
        }
        private void FButton_StartClick(object sender, RoutedEventArgs e)
        {
            this.Gif.StartAnimate();
        }

        private void FButton_EndClick(object sender, RoutedEventArgs e)
        {
            this.Gif.StopAnimate();
        }

        private void FButton_ChangeSizeClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
