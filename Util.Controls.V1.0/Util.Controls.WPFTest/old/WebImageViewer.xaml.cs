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
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class WebImageViewer : Page
    {
        private List<User> Source;
        public WebImageViewer()
        {
            InitializeComponent();
            this.Ini();
        }

        //add
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var src = this.txtImage.Text.Trim();
            //if (src.IsInvalid()) return;
            //var times = this.txtTimes.Text.ToSafeInt();
            //if (times <= 0) return;
            //for (int i = 0; i < times; i++)
            //{
            //    this.Source.Add(new User { FullPath = src });
            //}
        }
        //load
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //this.timgViewer.ItemsSource = this.Source;
        }
        //reset
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Ini();
            //this.timgViewer.ItemsSource = this.Source;
        }
        private void Ini()
        {
            this.Source = new List<User>();
            this.Source.Add(new User { FullPath = "http://img0.bdstatic.com/img/image/shouye/fsxzqnghbxzzzz.jpg" });
            this.Source.Add(new User { FullPath = "http://ts4.mm.bing.net/th?id=HN.608002455653583158&pid=1.7" });
            this.Source.Add(new User { FullPath = "http://wallpaperpassion.com/upload_puzzle_thumb/16047/hot-girl-hd-wallpaper.jpg" });
            this.Source.Add(new User { FullPath = "http://fwallpapers.com/files/images/art-girl-1.jpg" });
            this.Source.Add(new User { FullPath = "http://pic.cnitblog.com/face/226868/20140302142647.png" });
            this.Source.Add(new User { FullPath = "http://static.cnblogs.com/images/friend_link/logo_aliyun.jpg" });
            this.Source.Add(new User { FullPath = "http://images.cnitblog.com/blog/383187/201412/080025524518442.png" });
            this.Source.Add(new User { FullPath = "http://ts1.mm.bing.net/th?id=HN.608004087736436677&pid=1.7" });
            this.Source.Add(new User { FullPath = "http://www.wallsave.com/wallpapers/1920x1080/beautiful-girl/733941/beautiful-girl-girls-hd-733941.jpg" });
        }

        internal class User
        {
            public string FullPath { get; set; }
        }

    }
}
