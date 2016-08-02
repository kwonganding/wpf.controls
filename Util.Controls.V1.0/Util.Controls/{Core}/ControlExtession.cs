using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace System.Windows
{
    public static class ControlExtession
    {
        #region BindCommand

        /// <summary>
        /// 绑定命令和命令事件到宿主UI
        /// </summary>
        public static void BindCommand(this UIElement @ui, ICommand com, Action<object, ExecutedRoutedEventArgs> call)
        {
            var bind = new CommandBinding(com);
            bind.Executed += new ExecutedRoutedEventHandler(call);
            @ui.CommandBindings.Add(bind);
        }

        /// <summary>
        /// 绑定RelayCommand命令到宿主UI
        /// </summary>
        public static void BindCommand(this UIElement @ui, ICommand com)
        {
            var bind = new CommandBinding(com);
            @ui.CommandBindings.Add(bind);
        }

        #endregion

        #region TreeView操作扩展方法
        //code:http://www.codeproject.com/Articles/36193/WPF-TreeView-tools

        /// <summary>
        /// Returns the TreeViewItem of a data bound object.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <returns>The TreeViewItem of the data bound object or null.</returns>
        public static TreeViewItem GetItemFromObject(this TreeView treeView, object obj)
        {
            try
            {
                DependencyObject dObject = GetContainerFormObject(treeView, obj);
                TreeViewItem tvi = dObject as TreeViewItem;
                while (tvi == null)
                {
                    dObject = VisualTreeHelper.GetParent(dObject);
                    tvi = dObject as TreeViewItem;
                }
                return tvi;
            }
            catch
            {
            }
            return null;
        }

        private static DependencyObject GetContainerFormObject(ItemsControl item, object obj)
        {
            if (item == null)
                return null;

            DependencyObject dObject = null;
            dObject = item.ItemContainerGenerator.ContainerFromItem(obj);

            if (dObject != null)
                return dObject;

            var query = from childItem in item.Items.Cast<object>()
                        let childControl = item.ItemContainerGenerator.ContainerFromItem(childItem) as ItemsControl
                        select GetContainerFormObject(childControl, obj);

            return query.FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Selects a data bound object of a TreeView.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        public static void SelectObject(this TreeView treeView, object obj)
        {
            treeView.SelectObject(obj, true);
        }

        /// <summary>
        /// Selects or deselects a data bound object of a TreeView.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <param name="selected">select or deselect</param>
        public static void SelectObject(this TreeView treeView, object obj, bool selected)
        {
            var tvi = treeView.GetItemFromObject(obj);
            if (tvi != null)
            {
                tvi.IsSelected = selected;
            }
        }

        /// <summary>
        /// Returns if a data bound object of a TreeView is selected.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <returns>Returns true if the object is selected, and false if it is not selected or obj is not in the tree.</returns>
        public static bool IsObjectSelected(this TreeView treeView, object obj)
        {
            var tvi = treeView.GetItemFromObject(obj);
            if (tvi != null)
            {
                return tvi.IsSelected;
            }
            return false;
        }

        /// <summary>
        /// Returns if a data bound object of a TreeView is focused.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <returns>Returns true if the object is focused, and false if it is not focused or obj is not in the tree.</returns>
        public static bool IsObjectFocused(this TreeView treeView, object obj)
        {
            var tvi = treeView.GetItemFromObject(obj);
            if (tvi != null)
            {
                return tvi.IsFocused;
            }
            return false;
        }

        /// <summary>
        /// Expands a data bound object of a TreeView.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        public static void ExpandObject(this TreeView treeView, object obj)
        {
            treeView.ExpandObject(obj, true);
        }

        /// <summary>
        /// Expands or collapses a data bound object of a TreeView.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <param name="expanded">expand or collapse</param>
        public static void ExpandObject(this TreeView treeView, object obj, bool expanded)
        {
            var tvi = treeView.GetItemFromObject(obj);
            if (tvi != null)
            {
                tvi.IsExpanded = expanded;
                if (expanded)
                {
                    // update layout, so that following calls to f.e. SelectObject on child nodes will 
                    // find theire TreeViewNodes
                    treeView.UpdateLayout();
                }
            }
        }

        /// <summary>
        /// Returns if a douta bound object of a TreeView is expanded.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <returns>Returns true if the object is expanded, and false if it is collapsed or obj is not in the tree.</returns>
        public static bool IsObjectExpanded(this TreeView treeView, object obj)
        {
            var tvi = treeView.GetItemFromObject(obj);
            if (tvi != null)
            {
                return tvi.IsExpanded;
            }
            return false;
        }

        /// <summary>
        /// Retuns the parent TreeViewItem.
        /// </summary>
        /// <param name="item">TreeViewItem</param>
        /// <returns>Parent TreeViewItem</returns>
        public static TreeViewItem GetParentItem(this TreeViewItem item)
        {
            var dObject = VisualTreeHelper.GetParent(item);
            TreeViewItem tvi = dObject as TreeViewItem;
            while (tvi == null)
            {
                dObject = VisualTreeHelper.GetParent(dObject);
                tvi = dObject as TreeViewItem;
            }
            return tvi;
        }

        #endregion

    }

    public static class ControlHelper
    {
        //从Handle中获取Window对象
        private static Window GetWindowFromHwnd(IntPtr hwnd)
        {
            return (Window)HwndSource.FromHwnd(hwnd).RootVisual;
        }

        //GetForegroundWindow API
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        /////调用GetForegroundWindow然后调用GetWindowFromHwnd

        /// <summary>
        /// 获取当前顶级窗体，若获取失败则返回主窗体
        /// </summary>
        public static Window GetTopWindow()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                return Application.Current.MainWindow;

            return GetWindowFromHwnd(hwnd);
        }
    }
}