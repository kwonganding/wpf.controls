using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace System.Windows
{
    /// <summary>
    /// 广播命令：基本ICommand实现接口，带参数
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        public Action<T> ExecuteCommand { get; private set; }

        public Predicate<T> CanExecuteCommand { get; private set; }

        public RelayCommand(Action<T> executeCommand, Predicate<T> canExecuteCommand)
        {
            this.ExecuteCommand = executeCommand;
            this.CanExecuteCommand = canExecuteCommand;
        }

        public RelayCommand(Action<T> executeCommand)
            : this(executeCommand, null) { }

        /// <summary>
        /// 定义在调用此命令时调用的方法。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。如果此命令不需要传递数据，则该对象可以设置为 null。</param>
        public void Execute(object parameter)
        {
            if (this.ExecuteCommand != null) this.ExecuteCommand((T)parameter);
        }

        /// <summary>
        /// 定义用于确定此命令是否可以在其当前状态下执行的方法。
        /// </summary>
        /// <returns>
        /// 如果可以执行此命令，则为 true；否则为 false。
        /// </returns>
        /// <param name="parameter">此命令使用的数据。如果此命令不需要传递数据，则该对象可以设置为 null。</param>
        public bool CanExecute(object parameter)
        {
            return CanExecuteCommand == null || CanExecuteCommand((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { if (this.CanExecuteCommand != null) CommandManager.RequerySuggested += value; }
            remove { if (this.CanExecuteCommand != null) CommandManager.RequerySuggested -= value; }
        }
    }


    /// <summary>
    /// 广播命令：基本ICommand实现接口
    /// </summary>
    public class RelayCommand : ICommand
    {
        public Action ExecuteCommand { get; private set; }
        public Func<bool> CanExecuteCommand { get; private set; }

        public RelayCommand(Action executeCommand, Func<bool> canExecuteCommand)
        {
            this.ExecuteCommand = executeCommand;
            this.CanExecuteCommand = canExecuteCommand;
        }

        public RelayCommand(Action executeCommand)
            : this(executeCommand, null) { }

        /// <summary>
        /// 定义在调用此命令时调用的方法。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。如果此命令不需要传递数据，则该对象可以设置为 null。</param>
        public void Execute(object parameter)
        {
            if (this.ExecuteCommand != null) this.ExecuteCommand();
        }

        /// <summary>
        /// 定义用于确定此命令是否可以在其当前状态下执行的方法。
        /// </summary>
        /// <returns>
        /// 如果可以执行此命令，则为 true；否则为 false。
        /// </returns>
        /// <param name="parameter">此命令使用的数据。如果此命令不需要传递数据，则该对象可以设置为 null。</param>
        public bool CanExecute(object parameter)
        {
            return CanExecuteCommand == null || CanExecuteCommand();
        }

        public event EventHandler CanExecuteChanged
        {
            add { if (this.CanExecuteCommand != null) CommandManager.RequerySuggested += value; }
            remove { if (this.CanExecuteCommand != null) CommandManager.RequerySuggested -= value; }
        }
    }
}
