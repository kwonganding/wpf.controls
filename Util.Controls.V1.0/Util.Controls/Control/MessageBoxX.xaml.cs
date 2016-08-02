using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using Util.Controls;

namespace System.Windows
{
    /// <summary>
    /// MessageBoxXxaml.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxX : WindowBase
    {
        /// <summary>
        /// 结果，用户点击确定Result=true;
        /// </summary>
        public bool Result { get; private set; }

        private static readonly Dictionary<string, Brush> _Brushes = new Dictionary<string, Brush>();

        public MessageBoxX(EnumNotifyType type, string mes)
        {
            InitializeComponent();
            this.txtMessage.Text = mes;
            //type
            btnCancel.Visibility = Visibility.Collapsed;
            this.SetForeground(type);
            switch (type)
            {
                case EnumNotifyType.Error:
                    this.ficon.Text = "\ue644";
                    break;
                case EnumNotifyType.Warning:
                    this.ficon.Text = "\ue60b";
                    break;
                case EnumNotifyType.Info:
                    this.ficon.Text = "\ue659";
                    break;
                case EnumNotifyType.Question:
                    this.ficon.Text = "\ue60e";
                    this.btnCancel.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void SetForeground(EnumNotifyType type)
        {
            string key = type.ToSafeString() + "Foreground";
            if (!_Brushes.ContainsKey(key))
            {
                var b = this.TryFindResource(key) as Brush;
                _Brushes.Add(key, b);
            }
            this.Foreground = _Brushes[key];
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Result = true;
            this.Close();
            e.Handled = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Result = false;
            this.Close();
            e.Handled = true;
        }

        /********************* public static method **************************/

        /// <summary>
        /// 提示错误消息
        /// </summary>
        public static void Error(string mes, Window owner = null)
        {
            Show(EnumNotifyType.Error, mes, owner);
        }

        /// <summary>
        /// 提示普通消息
        /// </summary>
        public static void Info(string mes, Window owner = null)
        {
            Show(EnumNotifyType.Info, mes, owner);
        }

        /// <summary>
        /// 提示警告消息
        /// </summary>
        public static void Warning(string mes, Window owner = null)
        {
            Show(EnumNotifyType.Warning, mes, owner);
        }

        /// <summary>
        /// 提示询问消息
        /// </summary>
        public static bool Question(string mes, Window owner = null)
        {
            return Show(EnumNotifyType.Question, mes, owner);
        }

        /// <summary>
        /// 显示提示消息框，
        /// owner指定所属父窗体，默认参数值为null，则指定主窗体为父窗体。
        /// </summary>
        private static bool Show(EnumNotifyType type, string mes, Window owner = null)
        {
            var res = true;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                MessageBoxX nb = new MessageBoxX(type, mes) { Title = type.GetDescription() };
                nb.Owner = owner ?? ControlHelper.GetTopWindow();
                nb.ShowDialog();
                res = nb.Result;
            }));
            return res;
        }

        /// <summary>
        /// 通知消息类型
        /// </summary>
        public enum EnumNotifyType
        {
            [Description("错误")]
            Error,
            [Description("警告")]
            Warning,
            [Description("提示信息")]
            Info,
            [Description("询问信息")]
            Question,
        }
    }
}
