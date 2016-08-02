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
using Util.Controls;

namespace Util.Controls.WPFTest
{
    /// <summary>
    /// Page_Button.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Button : Page
    {
        public Page_Button()
        {
            InitializeComponent();
        }

        private void FButton_Click_WindowBase(object sender, RoutedEventArgs e)
        {
            WindowBase win = new WindowBase();
            win.ShowDialog();
        }

        private void FButton_Click_Error(object sender, RoutedEventArgs e)
        {
            MessageBoxX.Error("你只看到我在不停的忙碌，却没看到我奋斗的热情。你有朝九晚五，我有通宵达旦。你否定我的现在，我决定我的未来。你可以轻视我的存在，我会用代码证明这是谁的时代！Coding是注定痛苦的旅行，路上少不了Bug和Change，但！那又怎样！我是程序猿，我为自己带眼");
        }

        private void FButton_Click_Info(object sender, RoutedEventArgs e)
        {
            MessageBoxX.Info("领域驱动设计、三体、极客与团队、技术领导之路、文明");
        }

        private void FButton_Click_Warning(object sender, RoutedEventArgs e)
        {
            MessageBoxX.Warning("架构之美、数学之美、.net 核心框架、异步编程的艺术、单元测试的艺术，代码，WPF");
        }

        private void FButton_Click_Question(object sender, RoutedEventArgs e)
        {
            var res = MessageBoxX.Question("你是最帅的嘛？");
            MessageBoxX.Info(res.ToString());
        }

        private void FButton_Click(object sender, RoutedEventArgs e)
        {
            WaitingBox.Show(() =>
            {
                System.Threading.Thread.Sleep(3000);
            },"正在玩命的加载，请稍后...");
            var res = MessageBoxX.Question("已经完了？");
        }
    }
}
