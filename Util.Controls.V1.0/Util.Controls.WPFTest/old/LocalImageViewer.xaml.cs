using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class LocalImageViewer : Page
    {
        private List<User> Source;
        public LocalImageViewer()
        {
            InitializeComponent();
        }

        //load
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //var str = this.txtImage.Text.Trim();
            //if (str.IsInvalid()) return;

            //var fs1 = System.IO.Directory.GetFiles(str, "*.jpg", SearchOption.AllDirectories);
            //var fs2 = System.IO.Directory.GetFiles(str, "*.png", SearchOption.AllDirectories);
            //List<string> fs = new List<string>();
            //fs.AddRange(fs1);
            //fs.AddRange(fs2);
            //List<User> users = new List<User>();
            //foreach (var f in fs)
            //{
            //    users.Add(new User { FullPath = f, Name = System.IO.Path.GetFileName(f) });
            //}

            //this.timgViewer.ItemsSource = users;
        }

        internal class User
        {
            public string FullPath { get; set; }
            public string Name { get; set; }
        }

    }
}
