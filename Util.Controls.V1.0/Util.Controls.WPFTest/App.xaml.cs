using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Util.Controls.WPFTest
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            App.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //MessageBox.Show("未知异常", e.Exception.AllMessage(), MessageBoxButton.OK, MessageBoxImage.Error);
            //LogHelper.Error(e.Exception.Message, e.Exception);
            //e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                //MessageBox.Show("未知异常", ex.AllMessage(), MessageBoxButton.OK, MessageBoxImage.Error);
                //LogHelper.Error(ex.Message, ex);
            }
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            //Xceed.Wpf.Toolkit.Licenser.LicenseKey = "DGF37-deds5-23dgv-we3dr";
            //Xceed.Wpf.Toolkit.Licenser.
            base.OnStartup(e);
        }
    }
}
