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

namespace Util.Controls.Control
{
    /// <summary>
    /// ProgressForValue.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressForValue : UserControl
    {
        public static readonly DependencyProperty ValueProperty = 
            DependencyProperty.Register("Value", typeof(double), typeof(ProgressForValue));

        public static readonly DependencyProperty MaxValueProperty = 
            DependencyProperty.Register("MaxValue", typeof(double), typeof(ProgressForValue));

        public static readonly DependencyProperty MessageProperty = 
            DependencyProperty.Register("Message", typeof(string), typeof(ProgressForValue));

        /// <summary>
        /// 当前刻度值
        /// </summary>
        public double Value
        {
            get => (double) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxValue
        {
            get => (double)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        /// <summary>
        /// 进度消息
        /// </summary>
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }


        public ProgressForValue()
        {
            InitializeComponent();
        }
    }
}
