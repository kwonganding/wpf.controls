using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Util.Controls
{
    /// <summary>
    /// FrameworkElementË«»÷Command
    /// </summary>
    public sealed class DoubleClickCommandBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// ÃüÁî
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DoubleClickCommandBehavior), new PropertyMetadata(null));

        /// <summary>
        /// ÃüÁî²ÎÊý
        /// </summary>
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(DoubleClickCommandBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.DoCommand();
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
        }

        private void DoCommand()
        {
            if (this.Command == null)
                return;
            this.Command.Execute(this.CommandParameter);
        }


    }
}