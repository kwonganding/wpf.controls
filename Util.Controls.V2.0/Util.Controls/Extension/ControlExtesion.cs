using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Util.Controls;

public static class ControlExtension
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

    #region  Storyboard

    /// <summary>
    /// 重启画布动画 Restart Storyboard
    /// 先stop，再Begin
    /// </summary>
    /// <param name="this"></param>
    public static void Restart(this Storyboard @this)
    {
        if (@this == null) return;
        @this.Stop();

        Executer.TryRunByThreadPool(() =>
        {
            System.Threading.Thread.Sleep(100);
            GUIThreadHelper.BeginInvoke(@this.Begin);
        });
    }

    /// <summary>
    /// 重新开始或继续
    /// </summary>
    /// <param name="this"></param>
    [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
    public static void BeginOrResume(this Storyboard @this)
    {
        if (@this == null || @this.Children.IsInvalid()) return;
        try
        {
            var state = @this.GetCurrentState();
            if (state == ClockState.Stopped)
                @this.Begin();
            else
                @this.Resume();
        }
        catch
        {
            @this.Begin();
        }
    }

    /// <summary>
    /// 释放动画资源，containingObject为Storyboard关联的容器
    /// </summary>
    /// <param name="this"></param>
    /// <param name="containingObject"></param>
    public static void Dispose(this Storyboard @this, FrameworkElement containingObject = null)
    {
        if (@this == null)
            return;
        @this.Stop();
        if (containingObject == null)
            @this.Remove();
        else
            @this.Remove(containingObject);
        @this.Children.Clear();
    }

    #endregion

    #region FindParent，FindChildren
    /// <summary>
    /// 获取指定条件的父元素并转换为指定泛型参数的类型，若没找到，则返回null.
    /// </summary>
    public static T FindParent<T>(this DependencyObject obj, Func<DependencyObject, bool> predicate) where T : FrameworkElement
    {
        DependencyObject parent = VisualTreeHelper.GetParent(obj);
        while (parent != null)
        {
            if (parent is T && predicate(parent))
                return parent as T;
            parent = VisualTreeHelper.GetParent(parent);
        }
        return null;
    }

    /// <summary>
    /// 获取指定条件的父元素并转换为指定泛型参数的类型，若没找到，则返回null.
    /// </summary>
    public static T FindFirstParent<T>(this DependencyObject obj) where T : FrameworkElement
    {
        DependencyObject parent = VisualTreeHelper.GetParent(obj);
        while (parent != null)
        {
            if (parent is T)
                return parent as T;
            parent = VisualTreeHelper.GetParent(parent);
        }
        return null;
    }

    /// <summary>
    /// 查找指定条件的子控件，找到第一个就返回，如果没有找到，则返回null
    /// </summary>
    public static T FindChild<T>(this DependencyObject obj, Func<DependencyObject, bool> predicate) where T : FrameworkElement
    {
        if (obj == null) return null;
        T t = null;
        DependencyObject child = null;
        int count = VisualTreeHelper.GetChildrenCount(obj);
        //尝试从content获取
        if (count == 0 && obj is ContentControl)
        {
            var objc = obj as ContentControl;
            t = DoPredicate<T>(objc.Content, predicate);
        }
        if (count == 0 && obj is ContentPresenter)
        {
            var objc = obj as ContentPresenter;
            t = DoPredicate<T>(objc.Content, predicate);
        }
        if (t != null) return t;

        for (int i = 0; i < count; i++)
        {
            child = VisualTreeHelper.GetChild(obj, i);
            t = DoPredicate<T>(child, predicate);
            if (t != null) return t;
        }
        return null;
    }

    private static T DoPredicate<T>(object obj, Func<DependencyObject, bool> predicate) where T : FrameworkElement
    {
        if (obj == null) return null;
        var cc = obj as DependencyObject;
        if (cc == null) return null;
        if (cc is T && predicate(cc))
            return cc as T;
        return cc.FindChild<T>(predicate);
    }

    /// <summary>
    /// 查找指定条件的所有子控件集合，没找到则返回空元素
    /// </summary>
    public static List<T> FindChildren<T>(this DependencyObject obj, Func<DependencyObject, bool> predicate) where T : FrameworkElement
    {
        T t = null;
        List<T> children = new List<T>();
        int count = VisualTreeHelper.GetChildrenCount(obj);
        //尝试从content获取
        if (count == 0 && obj is ContentControl)
        {
            var objc = obj as ContentControl;
            DoPredicate<T>(objc.Content, predicate, children);
        }
        if (count == 0 && obj is ContentPresenter)
        {
            var objc = obj as ContentPresenter;
            DoPredicate<T>(objc.Content, predicate, children);
        }

        DependencyObject child = null;
        for (int i = 0; i < count; i++)
        {
            child = VisualTreeHelper.GetChild(obj, i);
            DoPredicate(child, predicate, children);
        }
        return children;
    }
    private static void DoPredicate<T>(object obj, Func<DependencyObject, bool> predicate, List<T> list) where T : FrameworkElement
    {
        if (obj == null) return;
        var cc = obj as DependencyObject;
        if (cc == null) return;
        if (cc is T && predicate(cc))
            list.Add(cc as T);
        list.AddRange(cc.FindChildren<T>(predicate));
    }
    #endregion
}