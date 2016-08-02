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
    /// Page_ImageViewer.xaml 的交互逻辑
    /// </summary>
    public partial class Page_ImageViewer : Page
    {
        public Page_ImageViewer()
        {
            InitializeComponent();
        }

        private void FButton_Click(object sender, RoutedEventArgs e)
        {
            var str = this.txtFolder.Text.Trim();
            if (str.IsInvalid()) return;

            var fs1 = System.IO.Directory.GetFiles(str, "*.jpg", SearchOption.AllDirectories);
            var fs2 = System.IO.Directory.GetFiles(str, "*.png", SearchOption.AllDirectories);
            List<string> fs = new List<string>();
            fs.AddRange(fs1);
            fs.AddRange(fs2);
            List<FFile> users = new List<FFile>();
            foreach (var f in fs)
            {
                users.Add(new FFile { File = f, Name = System.IO.Path.GetFileName(f) });
            }

            this.timgViewer.ItemsSource = users;
        }

        internal class FFile
        {
            public string File { get; set; }
            public string Name { get; set; }
        }
    }
}
