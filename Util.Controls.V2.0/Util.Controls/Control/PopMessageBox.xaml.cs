using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Util.Controls
{
    /// <summary>
    /// PopMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class PopMessageBox
    {
        private MMessage Message { get; set; }
        private DispatcherTimer Timer { get; set; }
        public PopMessageBox()
        {
            InitializeComponent();
            this.Message = new MMessage();
            this.DataContext = this.Message;
            this.Timer = new DispatcherTimer();
            this.IsOpen = false;
            this.Timer.Tick += Timer_Tick;
            this.MouseEnter += PopMessageBox_MouseEnter;
            this.MouseLeave += PopMessageBox_MouseLeave;
        }

        void PopMessageBox_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Timer.Start();
        }

        void PopMessageBox_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Timer.Stop();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Stop();
        }

        private void Start(int second)
        {
            this.Timer.Interval = TimeSpan.FromSeconds(second);
            this.Timer.Start();
            this.IsOpen = true;
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            this.Stop();
        }
        private void Stop()
        {
            this.Timer.Stop();
            this.IsOpen = false;
        }

        //************************** static *******************************//

        /// <summary>
        /// 提示普通消息。支持跨UI线程使用
        /// </summary>
        public static void Info(string mes, int secend = 3, int width = 380, int height = 140)
        {
            Show(EnumMessageInfo.Info, mes, secend, width, height);
        }
        /// <summary>
        /// 提示警告消息。支持跨UI线程使用
        /// </summary>
        public static void Warning(string mes, int secend = 3, int width = 380, int height = 140)
        {
            Show(EnumMessageInfo.Warning, mes, secend, width, height);
        }
        /// <summary>
        /// 提示错误消息。支持跨UI线程使用
        /// </summary>
        public static void Error(string mes, int secend = 3, int width = 380, int height = 140)
        {
            Show(EnumMessageInfo.Error, mes, secend, width, height);
        }
        public static void Show(EnumMessageInfo info, string mes, int secend = 3, int width = 380, int height = 140)
        {
            GUIThreadHelper.BeginInvoke(() =>
            {
                PopMessageBox pop = PopBoxs.Where(s => !s.IsOpen).ToArray().IsValid() ? PopBoxs.First(s => !s.IsOpen) : new PopMessageBox();
                pop.HorizontalOffset = SystemParameters.PrimaryScreenWidth - width;
                var ops = PopBoxs.Where(s => s.IsOpen).ToArray();
                var strat = ops.IsValid() ? ops.Min(s => s.VerticalOffset) : _StartVerticalOffset;
                pop.VerticalOffset = strat - height;
                pop.Width = width;
                pop.Height = height;
                pop.Message.Info = info;
                pop.Message.Text = mes;
                pop.Start(secend);
                if (!PopBoxs.Contains(pop)) PopBoxs.Add(pop);
                //清理，最多缓存20个
                if (PopBoxs.Count > 20) PopBoxs.RemoveWhere(s => !s.IsOpen);
            });
        }
        private static List<PopMessageBox> PopBoxs = new List<PopMessageBox>();
        private static double _StartVerticalOffset = SystemParameters.WorkArea.Height - 40;
    }
}
