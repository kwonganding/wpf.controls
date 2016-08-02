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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Util.Controls.WPFTest
{
    /// <summary>
    /// Page_ListView.xaml 的交互逻辑
    /// </summary>
    public partial class Page_ListView : Page
    {
        public Page_ListView()
        {
            InitializeComponent();
            
        }

        private void btnBind_Click(object sender, RoutedEventArgs e)
        {
            var len = this.txtNumber.Text.ToSafeInt();
            List<Device> ds = new List<Device>();
            for (int i = 0; i < len; i++)
            {
                var d1 = new Device()
                {
                    Name = "MX33333333333333333333333331_" + i,
                    Manufacturer = "Meizu--" + i,
                    Mode = "M303",
                    OSType = EnumOSType.Android,
                    State = EnumDeviceState.Online,
                    Version = "4,2,1",
                    SerialNumber = "0123456789",
                    IsRoot = true,
                };
                ds.Add(d1);
            }
            this.gridList.ItemsSource = ds;
            this.gridList0.ItemsSource = ds;
            this.gridList2.ItemsSource = ds;
        }
    }
}
