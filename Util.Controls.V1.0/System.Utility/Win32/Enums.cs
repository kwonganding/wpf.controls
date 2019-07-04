using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace System.Utility.Win32
{
    #region EnumDirverType
    /// <summary>
    /// 磁盘类型
    /// </summary>
    public enum EnumDirverType
    {
        /// <summary>
        /// 未知设备
        /// </summary>
        [Description("未知设备")]
        None = 0,

        /// <summary>
        /// 未分区
        /// </summary>
        [Description("未分区")]
        NoPartition = 1,

        /// <summary>
        /// 可移动磁盘
        /// </summary>
        [Description("可移动磁盘")]
        MobileDisk = 2,

        /// <summary>
        /// "硬盘
        /// </summary>
        [Description("硬盘")]
        HardDisk = 3,

        /// <summary>
        /// 网络驱动器
        /// </summary>
        [Description("网络驱动器")]
        NetworkDisk = 4,

        /// <summary>
        /// 光驱
        /// </summary>
        [Description("光驱")]
        CD = 5,

        /// <summary>
        /// 内存磁盘
        /// </summary>
        [Description("内存磁盘")]
        MemoryDisk = 6,

        /// <summary>
        /// 未知类型
        /// </summary>
        [Description("未知类型")]
        Unknow = 10,
    } 
    #endregion
}
