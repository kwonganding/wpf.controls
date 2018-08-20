using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Util.Controls;

namespace System.Windows
{
    public static class TreeViewExtension
    {
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
                if (dObject == null)
                {
                    return null;
                }

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


        /// <summary>
        /// 查找TreeView中TreeViewItem
        /// <para>警告:该方法只能用于TreeView.ItemsHost、TreeView.ItemContainerStyle中ItemsPanel的对象为 CustomVirtualizingStackPanel 方为有效,否则会发生异常</para>
        /// </summary>
        /// <param name="container"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static TreeViewItem GetTreeViewItem(this ItemsControl container, object item)
        {
            if (container != null)
            {
                if (container.DataContext == item)
                {
                    return container as TreeViewItem;
                }
                if (container.Items.Count == 0) return null;
                if (container is TreeViewItem && !((TreeViewItem)container).IsExpanded)
                {
                    container.SetValue(TreeViewItem.IsExpandedProperty, true);
                }
                container.ApplyTemplate();
                ItemsPresenter itemsPresenter = (ItemsPresenter)container.Template.FindName("ItemsHost", container);
                if (itemsPresenter != null)
                {
                    itemsPresenter.ApplyTemplate();
                }
                else
                {
                    itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                    if (itemsPresenter == null)
                    {
                        container.UpdateLayout();
                        itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                    }
                }

                Panel itemsHostPanel = (Panel)VisualTreeHelper.GetChild(itemsPresenter, 0);
                CustomVirtualizingStackPanel virtualizingPanel = itemsHostPanel as CustomVirtualizingStackPanel;
                for (int i = 0, count = container.Items.Count; i < count; i++)
                {
                    TreeViewItem subContainer;
                    if (virtualizingPanel != null)
                    {
                        virtualizingPanel.BringIntoView(i);
                        subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);
                    }
                    else
                    {
                        subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);
                        subContainer.BringIntoView();
                    }

                    if (subContainer != null)
                    {
                        TreeViewItem resultContainer = GetTreeViewItem(subContainer, item);
                        if (resultContainer != null)
                        {
                            return resultContainer;
                        }
                        else
                        {
                            subContainer.IsExpanded = false;
                        }
                    }
                }
            }
            return null;
        }

        private static T FindVisualChild<T>(Visual visual) where T : Visual
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                Visual child = (Visual)VisualTreeHelper.GetChild(visual, i);
                if (child != null)
                {
                    T correctlyTyped = child as T;
                    if (correctlyTyped != null)
                    {
                        return correctlyTyped;
                    }
                    T descendent = FindVisualChild<T>(child);
                    if (descendent != null)
                    {
                        return descendent;
                    }
                }
            }

            return null;
        }
    }
}
