using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace Util.Controls
{
    public static class ControlHelper
    {
        #region GetTopWindow

        //GetForegroundWindow API
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();


        /// <summary>
        /// 获取当前顶级窗体，若获取失败则返回主窗体
        /// </summary>
        public static Window GetTopWindow()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                return Application.Current.MainWindow;

            var source = HwndSource.FromHwnd(hwnd);
            if (source != null) return source.RootVisual as Window;
            return null;
        }

        public static void SetOwnerWindow(Window win)
        {
            if (win == null) return;
            var top = GetTopWindow();
            if (top == null || win == top) return;
            //不能是WaitingBox
            if (top.GetType() == typeof(WaitingBox))
                win.Owner = top.Owner;
            else
                win.Owner = top;
        }

        #endregion

        #region ShowWindowNewThread

        /// <summary>
        /// 在一个新的线程中显示窗体，func为对窗体对象的初始操作回调
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        public static void ShowWindowNewThread<T>(Action<T> func) where T : Window, new()
        {
            Thread newWindowThread = new Thread(new ThreadStart(() =>
            {
                var win = new T();
                func(win);
                //关闭当前消息循环
                win.Closed += (s, e) => Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
                win.Show();
                //在当前线程中启用消息循环
                Dispatcher.Run();
            }));
            // Set the apartment state
            newWindowThread.SetApartmentState(ApartmentState.STA);
            // Make the thread a background thread
            newWindowThread.IsBackground = true;
            // Start the thread
            newWindowThread.Start();
        }

        private static void Win_Closed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region GetDpi

        /// <summary>
        /// 获取系统DPI
        /// </summary>
        /// <returns></returns>
        public static float GetDpi()
        {
            using (Graphics graph = Graphics.FromHwnd(IntPtr.Zero))
            {
                if (graph == null)
                {
                    throw new NullReferenceException("Graphics not found");
                }

                if (!graph.DpiX.Equals(graph.DpiY))
                {
                    throw new ArithmeticException("DpiX != DpiY");
                }

                return graph.DpiX;
            }
        }

        #endregion

        /// <summary>
        /// 让IInputElement控件强制获取焦点(调用线程优先级)
        /// </summary>
        /// <param name="element"></param>
        public static void SetFocus(IInputElement element)
        {
            // 调用操作优先级，强制让当前编辑框获取焦点
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                (Action)(() => { Keyboard.Focus(element); }));
        }
    }
}