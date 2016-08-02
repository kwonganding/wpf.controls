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
    /// Page_MVVM.xaml 的交互逻辑
    /// </summary>
    public partial class Page_MVVM : Page
    {
        public Page_MVVM()
        {
            InitializeComponent();
            this.DataContext = new TestUserModel();
        }
    }

    public class TestUserModel : BaseNotifyPropertyChanged
    {
        private string _Name;
        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; base.OnPropertyChanged(() => Name); }
        }

        public RelayCommand<string> SetUserName { get; private set; }

        public TestUserModel()
        {
            this.SetUserName = new RelayCommand<string>(DoSetUserName);
        }

        private void DoSetUserName(string name)
        {
            if (MessageBoxX.Question(string.Format("Set Name is {0}?", name)))
            {
                this.Name = name;
            }
        }
    }
}
