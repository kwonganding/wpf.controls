//-----------------------------------------------------------------------
// <copyright file="HitTest.cs" company="Vadeware">
//     Copyright (c) Vadeware Enterprises. All rights reserved.
//     枚举测试命中
// </copyright>
//-----------------------------------------------------------------------
namespace Util.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;

    /// <summary>
    /// 枚举测试命中
    /// </summary>
    public enum HitTest : int
    {
        /// <summary>
        /// 错误
        /// </summary>
        HTERROR = -2,

        /// <summary>
        /// 透明
        /// </summary>
        HTTRANSPARENT = -1,

        /// <summary>
        /// 任意位置
        /// </summary>
        HTNOWHERE = 0,

        /// <summary>
        /// 客户端
        /// </summary>
        HTCLIENT = 1,

        /// <summary>
        /// 标题
        /// </summary>
        HTCAPTION = 2,

        /// <summary>
        /// 系统菜单
        /// </summary>
        HTSYSMENU = 3,

        /// <summary>
        /// GroupBOx
        /// </summary>
        HTGROWBOX = 4,

        /// <summary>
        /// GroupBox的大小
        /// </summary>
        HTSIZE = HTGROWBOX,

        /// <summary>
        /// 菜单
        /// </summary>
        HTMENU = 5,

        /// <summary>
        /// 水平滚动条
        /// </summary>
        HTHSCROLL = 6,

        /// <summary>
        /// 垂直滚动条
        /// </summary>
        HTVSCROLL = 7,

        /// <summary>
        /// 最小化按钮
        /// </summary>
        HTMINBUTTON = 8,

        /// <summary>
        /// 最大化按钮
        /// </summary>
        HTMAXBUTTON = 9,

        /// <summary>
        /// 窗体左边
        /// </summary>
        HTLEFT = 10,

        /// <summary>
        /// 窗体右边
        /// </summary>
        HTRIGHT = 11,

        /// <summary>
        /// 窗体顶部
        /// </summary>
        HTTOP = 12,

        /// <summary>
        /// 窗体左上角
        /// </summary>
        HTTOPLEFT = 13,

        /// <summary>
        /// 窗体右上角
        /// </summary>
        HTTOPRIGHT = 14,

        /// <summary>
        /// 窗体底部
        /// </summary>
        HTBOTTOM = 15,

        /// <summary>
        /// 窗体左下角
        /// </summary>
        HTBOTTOMLEFT = 16,

        /// <summary>
        /// 窗体右下角
        /// </summary>
        HTBOTTOMRIGHT = 17,

        /// <summary>
        /// 窗体边框
        /// </summary>
        HTBORDER = 18,

        /// <summary>
        /// 窗体缩小
        /// </summary>
        HTREDUCE = HTMINBUTTON,

        /// <summary>
        /// 窗体填出
        /// </summary>
        HTZOOM = HTMAXBUTTON,

        /// <summary>
        /// 开始改变大小
        /// </summary>
        HTSIZEFIRST = HTLEFT,

        /// <summary>
        /// 结束改变大小
        /// </summary>
        HTSIZELAST = HTBOTTOMRIGHT,

        /// <summary>
        /// 对象
        /// </summary>
        HTOBJECT = 19,
        
        /// <summary>
        /// 关闭
        /// </summary>
        HTCLOSE = 20,

        /// <summary>
        /// 帮助
        /// </summary>
        HTHELP = 21,
    }
}