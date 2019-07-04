using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace System.Utility.Win32
{
    #region FileOperations / FO
    /// <summary>
    /// File Operations / FO
    /// </summary>
    public enum FileOperations : int
    {
        Move = 0x0001,
        Copy = 0x0002,
        Delete = 0x0003,
        Rename = 0x0004,
    }
    #endregion

    #region FileOperation Flags / FOF
    /// <summary>
    /// FileOperation Flag / FOF
    /// </summary>
    public enum FileOperationFlags : short
    {
        MULTIDESTFILES = 0x0001,
        CONFIRMMOUSE = 0x0002,
        /// <summary>
        /// Don't create progress/report
        /// </summary>
        SILENT = 0x0004,
        RENAMEONCOLLISION = 0x0008,
        /// <summary>
        /// Don't prompt the user.
        /// </summary>
        NOCONFIRMATION = 0x0010,
        /// <summary>
        /// Fill in SHFILEOPSTRUCT.hNameMappings
        /// </summary>
        WANTMAPPINGHANDLE = 0x0020,
        ALLOWUNDO = 0x0040,
        /// <summary>
        /// On *.*, do only files
        /// </summary>
        FILESONLY = 0x0080,
    }
    #endregion

    #region RECT
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public RECT(Drawing.Rectangle rectangle)
        {
            left = rectangle.Left; top = rectangle.Top;
            right = rectangle.Right; bottom = rectangle.Bottom;
        }
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
    #endregion

    #region SIZE
    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public int cx;
        public int cy;
    }
    #endregion

    #region OperatingSystem
    /// <summary>
    /// 操作系统信息
    /// </summary>
    public struct OperatingSystem
    {
        /// <summary>
        /// 操作系统
        /// </summary>
        public string System;

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version;

        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer;

        /// <summary>
        /// 计算机名称
        /// </summary>
        public string CSName;

        /// <summary>
        /// Windows目录
        /// </summary>
        public string WindowsDirectory;
    } 
    #endregion

    #region DirverInfo
    /// <summary>
    /// 磁盘信息
    /// </summary>
    public struct DirverInfo
    {
        public EnumDirverType Type;
        public string Name;
        public string VolumeName;
        public long Size;
        public long FreeSize;
    } 
    #endregion
}
