//-----------------------------------------------------------------------
// <copyright file="ChangeWindowSize.cs" company="Vadeware">
//     Copyright (c) Vadeware Enterprises. All rights reserved.
//     窗体
// </copyright>
//-----------------------------------------------------------------------
namespace Util.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Interop;

    /// <summary>
    /// 拖动窗体四角，可以改变窗体的大小
    /// </summary>
    public class ChangeWindowSize
    {
        /// <summary>
        /// 边框宽度
        /// </summary>
        private readonly int Thickness = 4;

        /// <summary>
        /// 改变大小的通知消息
        /// </summary>
        private const int WMNCHITTEST = 0x0084;

        /// <summary>
        /// 窗口的大小和位置将要被改变时的消息
        /// </summary>
        private const int WMWINDOWPOSCHANGING = 0x0046;

        /// <summary>
        /// 拐角宽度
        /// </summary>
        private readonly int angelWidth = 12;

        /// <summary>
        /// 要改变窗体大小的对象
        /// </summary>
        private Window window = null;

        /// <summary>
        /// 鼠标坐标
        /// </summary>
        private Point mousePoint = new Point();

        /// <summary>
        /// 构造函数，初始化目标窗体对象
        /// </summary>
        /// <param name="window">目标窗体</param>
        public ChangeWindowSize(Window window)
        {
            this.window = window;
        }

        /// <summary>
        /// 进行注册钩子
        /// </summary>
        public void RegisterHook()
        {
            HwndSource hwndSource = PresentationSource.FromVisual(this.window) as HwndSource;
            if (hwndSource != null)
            {
                hwndSource.AddHook(new HwndSourceHook(this.WndProc));
            }
        }

        /// <summary>
        /// 窗体回调程序
        /// </summary>
        /// <param name="hwnd">窗体句柄</param>
        /// <param name="msg">消息</param>
        /// <param name="wideParam">附加参数1</param>
        /// <param name="longParam">附加参数2</param>
        /// <param name="handled">是否处理</param>
        /// <returns>返回句柄</returns>
        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wideParam, IntPtr longParam, ref bool handled)
        {
            // 获得窗体的 样式
            int oldstyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_STYLE);
            switch (msg)
            {
                case WMNCHITTEST:
                    this.mousePoint.X = longParam.ToInt32() & 0xFFFF;
                    this.mousePoint.Y = longParam.ToInt32() >> 16;

                    // 更改窗体的样式为无边框窗体
                    NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, oldstyle & ~NativeMethods.WS_CAPTION);

                    // 窗口位置  // 窗口左上角
                    if (this.mousePoint.Y - this.window.Top <= this.angelWidth
                       && this.mousePoint.X - this.window.Left <= this.angelWidth)
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTTOPLEFT);
                    }
                    else if (this.window.ActualHeight + this.window.Top - this.mousePoint.Y <= this.angelWidth
                       && this.mousePoint.X - this.window.Left <= this.angelWidth) // 窗口左下角
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTBOTTOMLEFT);
                    }
                    else if (this.mousePoint.Y - this.window.Top <= this.angelWidth
                       && this.window.ActualWidth + this.window.Left - this.mousePoint.X <= this.angelWidth) // 窗口右上角
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTTOPRIGHT);
                    }
                    else if (this.window.ActualWidth + this.window.Left - this.mousePoint.X <= this.angelWidth
                       && this.window.ActualHeight + this.window.Top - this.mousePoint.Y <= this.angelWidth) // 窗口右下角
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTBOTTOMRIGHT);
                    }
                    else if (this.mousePoint.X - this.window.Left <= this.Thickness) // 窗口左侧
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTLEFT);
                    }
                    else if (this.window.ActualWidth + this.window.Left - this.mousePoint.X <= this.Thickness) // 窗口右侧
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTRIGHT);
                    }
                    else if (this.mousePoint.Y - this.window.Top <= this.Thickness) // 窗口上方
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTTOP);
                    }
                    else if (this.window.ActualHeight + this.window.Top - this.mousePoint.Y <= this.Thickness) // 窗口下方
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTBOTTOM);
                    }
                    else // 窗口移动
                    {
                        // handled = true;
                        // 更改窗体的样式为无边框窗体
                        return new IntPtr((int)HitTest.HTCAPTION);
                    }

                case WMWINDOWPOSCHANGING:

                    // 在将要改变的时候，是样式添加系统菜单
                    NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, oldstyle & ~NativeMethods.WS_CAPTION | NativeMethods.WS_SYSMENU);
                    break;
            }

            return IntPtr.Zero;
        }
    }
}
