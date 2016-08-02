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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Util.Controls.WPFTest
{
    /// <summary>
    /// Page_Test.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Test : Page
    {
        public Page_Test()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            if (e.ClickCount == 2)
                da.To = 0d;
            else
                da.To = 180d;
            AxisAngleRotation3D aar =this.FindName("aar") as AxisAngleRotation3D;
            aar.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
        }
    }
}
