using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Utility;
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
    /// Page_Waiting.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Waiting : Page
    {
        public Page_Waiting()
        {
            InitializeComponent();
            this.Asyn = new DefaultAsynNotify();
            this.pro1.DataContext = this.Asyn;
            this.pro2.DataContext = this.Asyn;
            
            this.pro3.DataContext = this.Asyn;

            this.pro4.DataContext = this.Asyn;
            this.pro5.DataContext = this.Asyn;
            this.pro6.DataContext = this.Asyn;
            this.proValue.DataContext = this.Asyn;
        }
        private IAsynNotify Asyn;

        private void FButton_Click(object sender, RoutedEventArgs e)
        {
            System.Utility.SingleThread _Thread;
            _Thread = new SingleThread();
            this.Asyn.Start(100);
            _Thread.Start(() =>
            {
                for (int i = 0; i < 100; i+=1)
                {
                    this.Asyn.Advance(1);
                    System.Threading.Thread.Sleep(50);
                }
                this.Asyn.IsSuccess = true;
            });
        }

        private void FButton1_Click(object sender, RoutedEventArgs e)
        {
            System.Utility.SingleThread _Thread;
            _Thread = new SingleThread();
            this.Asyn.Start(100);
            _Thread.Start(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    this.Asyn.Advance(1);
                    System.Threading.Thread.Sleep(50);
                    if (i >= 30)
                    {
                        this.Asyn.Cancel();
                        this.Asyn.IsSuccess = false;
                        break;
                    }
                }
            });
        }

        private void FButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Asyn.Start(0);
        }
    }
}
