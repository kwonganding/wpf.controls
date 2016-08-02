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
using System.Windows.Shapes;

namespace System.Windows
{
    /// <summary>
    /// 简单等待框
    /// </summary>
    public partial class WaitingBox : Window
    {
        public string Text { get { return this.txtMessage.Text; } set { this.txtMessage.Text = value; } }

        private Action _Callback;

        public WaitingBox(Action callback)
        {
            InitializeComponent();
            this._Callback = callback;
            this.Loaded += WaitingBox_Loaded;
        }

        void WaitingBox_Loaded(object sender, RoutedEventArgs e)
        {
            this._Callback.BeginInvoke(this.OnComplate, null);
        }

        private void OnComplate(IAsyncResult ar)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.Close();
            }));
        }



        /// <summary>
        /// 显示等待框，callback为需要执行的方法体（需要自己做异常处理）。
        /// 目前等等框为模式窗体
        /// </summary>
        public static void Show(Action callback, string mes = "有一种幸福，叫做等待...")
        {
            WaitingBox win = new WaitingBox(callback);
            Window pwin = ControlHelper.GetTopWindow();
            win.Owner = pwin;
            win.Text = mes;
            win.ShowDialog();
        }
    }
}
