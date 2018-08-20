using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
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

namespace Util.Controls
{
    /// <summary>
    /// MessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBox : WindowBase
    {
        private MMessage Message { get; set; }

        public MessageBox()
            : base(false)
        {
            InitializeComponent();
            this.Topmost = true;
            this.Message = new MMessage();
            this.DataContext = this.Message;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Message.Result = true;
            this.Close();
            e.Handled = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Message.Result = false;
            this.Close();
            e.Handled = true;
        }

        /********************* public static method **************************/

        /// <summary>
        /// 提示错误消息。支持跨UI线程使用
        /// </summary>
        public static void Error(string mes, Window owner = null)
        {
            Show(EnumMessageInfo.Error, mes, owner);
        }

        /// <summary>
        /// 提示错误消息。支持跨UI线程使用
        /// </summary>
        public static void Error(string formatMsg, params object[] parms)
        {
            var mess = string.Format(formatMsg, parms);
            Show(EnumMessageInfo.Error, mess);
        }

        /// <summary>
        /// 提示普通消息。支持跨UI线程使用
        /// </summary>
        public static void Info(string mes, Window owner = null)
        {
            Show(EnumMessageInfo.Info, mes, owner);
        }

        /// <summary>
        /// 提示警告消息。支持跨UI线程使用
        /// </summary>
        public static void Warning(string mes, Window owner = null)
        {
            Show(EnumMessageInfo.Warning, mes, owner);
        }
        /// <summary>
        /// 提示询问消息
        /// </summary>
        public static bool Question(string mes, Window owner = null)
        {
            return Show(EnumMessageInfo.Question, mes, owner);
        }

        /// <summary>
        /// 显示提示消息框，
        /// owner指定所属父窗体，默认参数值为null，则指定主窗体为父窗体。
        /// </summary>
        public static bool Show(EnumMessageInfo info, string mes, Window owner = null)
        {
            var res = false;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var win = new MessageBox();
                if (owner != null)
                {
                    win.Owner = owner;
                    win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                }
                else
                {
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
                //win.Owner = owner;
                win.Title = info.GetDescription();
                win.Message.Text = mes;
                win.Message.Info = info;
                win.Message.Result = false;
                System.Media.SystemSounds.Beep.Play();
                win.ShowDialog();
                res = win.Message.Result;
            }));
            return res;
        }
    }

    /// <summary>
    /// 通知消息类型
    /// </summary>
    public enum EnumMessageInfo
    {
        None = 0,
        [Description("错误")]
        Error = 1,
        [Description("警告")]
        Warning = 2,
        [Description("提示信息")]
        Info = 3,
        [Description("询问信息")]
        Question = 4,
    }
    /// <summary>
    /// 通知消息弹出的方式
    /// </summary>
    public enum EnumMessageShowType
    {
        PopBox = 0,
        MessageBox = 1,
    }
    internal class MMessage : BaseNotifyPropertyChanged
    {
        private string _FIcon;
        public string FIcon
        {
            get { return _FIcon; }
            set { _FIcon = value; base.OnPropertyChanged("FIcon"); }
        }

        private string _Text;
        public string Text
        {
            get { return _Text; }
            set { _Text = value; base.OnPropertyChanged("Text"); }
        }

        private Brush _Foreground;

        public Brush Foreground
        {
            get { return _Foreground; }
            set { _Foreground = value; base.OnPropertyChanged("Foreground"); }
        }

        private Visibility _CancleButtonVisibility;

        public Visibility CancleButtonVisibility
        {
            get { return _CancleButtonVisibility; }
            set { _CancleButtonVisibility = value; base.OnPropertyChanged("CancleButtonVisibility"); }
        }

        private EnumMessageInfo _info;

        public EnumMessageInfo Info
        {
            get { return _info; }
            set
            {
                if (value != _info)
                {
                    _info = value;
                    this.Update();
                }
            }
        }

        public bool Result { get; set; }

        private static Dictionary<EnumMessageInfo, Brush> _Brushes = new Dictionary<EnumMessageInfo, Brush>(4);
        private static Dictionary<EnumMessageInfo, string> _FIcons =
            new Dictionary<EnumMessageInfo, string> { { EnumMessageInfo.Info, "\ue626" }, { EnumMessageInfo.Question, "\ue625" }
                , { EnumMessageInfo.Warning, "\ue603" }, { EnumMessageInfo.Error, "\ue616" }};
        private void Update()
        {
            if (!_Brushes.ContainsKey(this._info))
            {
                string key = this._info.ToSafeString() + "Foreground";
                var b = Application.Current.TryFindResource(key) as Brush;
                _Brushes.Add(this._info, b);
            }
            this.Foreground = _Brushes[this._info];
            this.FIcon = _FIcons[this._info];
            this.CancleButtonVisibility = this._info == EnumMessageInfo.Question ? Visibility.Visible : Visibility.Collapsed;
        }
    }


}