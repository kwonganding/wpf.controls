using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace System.Utility.Win32
{
    public class Win32
    {
        /// <summary>
        /// 关闭句柄
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern void CloseHandle(IntPtr hObject);

        /// <summary>
        /// 写配置文件
        /// </summary>
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        /// <summary>
        /// 读取配置文件
        /// </summary>
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        [DllImport("kernel32")]
        public static extern uint GetPrivateProfileString(
            string lpAppName, // points to section name
            string lpKeyName, // points to key name
            string lpDefault, // points to default string
            byte[] lpReturnedString, // points to destination buffer
            uint nSize, // size of destination buffer
            string lpFileName  // points to initialization filename
        );

        /// <summary>
        /// 获取磁盘容量信息，参数C:
        /// </summary>
        [DllImport("kernel32.dll", EntryPoint = "GetDiskFreeSpaceExA")]
        public static extern int GetDiskFreeSpaceEx(string lpRootPathName, out long lpFreeBytesAvailable, out long lpTotalNumberOfBytes, out long lpTotalNumberOfFreeBytes);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        #region 查找窗口句柄

        #region 查找窗口句柄
        /// <summary>
        /// 查找窗口句柄，如果找到多个匹配窗口，则返回顶层窗口句柄
        /// </summary>
        /// <param name="className">窗口类名</param>
        /// <param name="windowName">窗口标题</param>
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string className, string windowName);
        #endregion

        #region 查找子窗口句柄
        /// <summary>
        /// 查找窗口句柄，如果找到多个匹配窗口，则返回顶层窗口句柄
        /// </summary>
        /// <param name="handlerParent">父窗口句柄, 如果为0, 则以桌面窗口为父窗口, 查找桌面窗口的所有子窗口</param>
        /// <param name="handlerChildAfter">子窗口句柄,
        /// 1. 子窗口必须是父窗口的直接子窗口.
        /// 2. 如果子窗口为0,则从父窗口的第一个子窗口开始查找</param>
        /// <param name="childClassName">子窗口类名</param>
        /// <param name="childWindowName">子窗口标题</param>
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr handlerParent, int handlerChildAfter, string childClassName, string childWindowName);
        #endregion

        #region 通过窗口标题查找窗口句柄
        /// <summary>
        /// 查找窗口句柄，如果找到多个匹配窗口，则返回顶层窗口句柄
        /// </summary>
        /// <param name="windowName">窗口标题</param>
        public static IntPtr FindWindowByText(string windowName)
        {
            return FindWindow(null, windowName);
        }

        /// <summary>
        /// 查找窗口句柄，如果找到多个匹配窗口，则返回顶层窗口句柄
        /// </summary>
        /// <param name="handlerParent">父窗口句柄, 如果为0, 则以桌面窗口为父窗口, 查找桌面窗口的所有子窗口</param>
        /// <param name="childWindowName">子窗口标题</param>
        public static IntPtr FindWindowByText(IntPtr handlerParent, string childWindowName)
        {
            return FindWindowEx(handlerParent, 0, null, childWindowName);
        }

        /// <summary>
        /// 查找窗口句柄，如果找到多个匹配窗口，则返回顶层窗口句柄
        /// </summary>
        /// <param name="handlerParent">父窗口句柄, 如果为0, 则以桌面窗口为父窗口, 查找桌面窗口的所有子窗口</param>
        /// <param name="handlerChildAfter">子窗口句柄,
        /// 1. 子窗口必须是父窗口的直接子窗口.
        /// 2. 如果子窗口为0,则从父窗口的第一个子窗口开始查找</param>
        /// <param name="childWindowName">子窗口标题</param>
        public static IntPtr FindWindowByText(IntPtr handlerParent, int handlerChildAfter, string childWindowName)
        {
            return FindWindowEx(handlerParent, handlerChildAfter, null, childWindowName);
        }
        #endregion

        #region 通过窗口类名查找窗口句柄
        /// <summary>
        /// 查找窗口句柄，如果找到多个匹配窗口，则返回顶层窗口句柄
        /// </summary>
        /// <param name="className">窗口类名</param>
        public static IntPtr FindWindowByClass(string className)
        {
            return FindWindow(null, className);
        }

        /// <summary>
        /// 查找窗口句柄，如果找到多个匹配窗口，则返回顶层窗口句柄
        /// </summary>
        /// <param name="handlerParent">父窗口句柄顺序, 如果为0, 则以桌面窗口为父窗口, 查找桌面窗口的所有子窗口</param>
        /// <param name="childClassName">子窗口类名</param>
        public static IntPtr FindWindowByClass(IntPtr handlerParent, string childClassName)
        {
            return FindWindowEx(handlerParent, 0, childClassName, null);
        }

        /// <summary>
        /// 查找窗口句柄，如果找到多个匹配窗口，则返回顶层窗口句柄
        /// </summary>
        /// <param name="handlerParent">父窗口句柄, 如果为0, 则以桌面窗口为父窗口, 查找桌面窗口的所有子窗口</param>
        /// <param name="handlerChildAfter">子窗口句柄顺序,
        /// 1. 子窗口必须是父窗口的直接子窗口.
        /// 2. 如果子窗口为0,则从父窗口的第一个子窗口开始查找</param>
        /// <param name="childClassName">子窗口类名</param>
        public static IntPtr FindWindowByClass(IntPtr handlerParent, int handlerChildAfter, string childClassName)
        {
            return FindWindowEx(handlerParent, handlerChildAfter, childClassName, null);
        }

        /// <summary>
        /// 查找窗口句柄，如果找到多个匹配窗口，则返回顶层窗口句柄
        /// </summary>
        /// <param name="handlerChildAfter">子窗口句柄顺序,
        /// 1. 子窗口必须是父窗口的直接子窗口.
        /// 2. 如果子窗口为0,则从父窗口的第一个子窗口开始查找</param>
        /// <param name="childClassName">子窗口类名</param>
        public static IntPtr FindWindowByClass(int handlerChildAfter, string childClassName)
        {
            return FindWindowEx(IntPtr.Zero, handlerChildAfter, childClassName, null);
        }
        #endregion


        #endregion

        #region 隐藏窗口
        /// <summary>
        /// 隐藏窗口
        /// </summary>
        /// <param name="handlerWindow">窗口句柄</param>
        public static bool HideWindow(IntPtr handlerWindow)
        {
            return ShowWindow(handlerWindow, 0);
        }
        #endregion

        #region 显示或隐藏窗口
        /// <summary>
        /// 显示或隐藏窗口
        /// </summary>
        /// <param name="handlerWindow">窗口句柄</param>
        /// <param name="cmd">命令</param>
        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern bool ShowWindow(IntPtr handlerWindow, uint cmd);
        #endregion

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern bool SetWindowLong(IntPtr handlerWindow, int nIndex, int newLong);

    }
}
