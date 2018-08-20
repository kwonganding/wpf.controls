using System;
using System.Windows;
using Util.Controls;

namespace Util.Controls
{
    /// <summary>
    /// 支持三种状态的询问窗口。
    /// </summary>
    public partial class QuestionBox : WindowBase
    {
        private EnumQuestionBox Result { get; set; }

        public QuestionBox()
            : base(false)
        {
            InitializeComponent();
            this.Topmost = true;
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            this.Result = EnumQuestionBox.State1;
            this.Close();
            e.Handled = true;
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            this.Result = EnumQuestionBox.State2;
            this.Close();
            e.Handled = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Result = EnumQuestionBox.Cancel;
            this.Close();
            e.Handled = true;
        }

        /********************* public static method **************************/

        /// <summary>
        /// 提示询问消息，指定按钮名称
        /// </summary>
        public static EnumQuestionBox Question(string mes, string btn1Text, string btn2Text, Window owner = null)
        {
            return Show(mes, btn1Text, btn2Text, owner);
        }

        /// <summary>
        /// 显示提示消息框，
        /// owner指定所属父窗体，默认参数值为null，则指定主窗体为父窗体。
        /// </summary>
        private static EnumQuestionBox Show(string mes, string btn1Text, string btn2Text, Window owner = null)
        {
            var res = EnumQuestionBox.Cancel;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var win = new QuestionBox();
                if (owner != null) win.Owner = owner;
                win.txtMessage.Text = mes;
                win.btn1.Content = btn1Text;
                win.btn2.Content = btn2Text;
                System.Media.SystemSounds.Beep.Play();
                win.ShowDialog();
                res = win.Result;
            }));
            return res;
        }
    }

    public enum EnumQuestionBox
    {
        State1 = 0,
        State2 = 1,
        Cancel = 2,
    }
}