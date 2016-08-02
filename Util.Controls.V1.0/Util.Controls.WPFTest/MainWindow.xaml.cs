using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Util.Controls.WPFTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty OpenCommandProperty =
    DependencyProperty.Register("OpenCommand", typeof(RoutedCommand), typeof(MainWindow), new PropertyMetadata(null));

        public RoutedCommand OpenCommand
        {
            get { return (RoutedCommand)GetValue(OpenCommandProperty); }
            set { SetValue(OpenCommandProperty, value); }
        }

        public MainWindow()
        {
            InitializeComponent();
            //bind command
            this.OpenCommand = new RoutedCommand();
            var bin = new CommandBinding(this.OpenCommand);
            bin.Executed += bin_Executed;
            this.CommandBindings.Add(bin);
        }

        void bin_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var btn = e.Source as Button;
            this.PageContext.Source = new Uri(btn.Tag.ToString(), UriKind.Relative);
        }
    }
}
