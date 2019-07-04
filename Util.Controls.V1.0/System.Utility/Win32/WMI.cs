using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace System.Utility.Win32
{
    public class WMI
    {
        public static List<DirverInfo> GetDirverInfo()
        {
            var list = ExecuteSearch("select * from win32_logicaldisk");
            //var list = ExecuteSearch("select * from Win32_DiskDrive");
            List<DirverInfo> items = new List<DirverInfo>();
            foreach (var obj in list)
            {
                DirverInfo item = new DirverInfo();
                try
                {
                    item.Name = obj["Name"].ToString();
                    item.VolumeName = obj["VolumeName"].ToString();
                    item.Size = obj["Size"].ToString().ToInt64();
                    item.FreeSize = obj["FreeSpace"].ToString().ToInt64();
                }
                catch
                {
                    item.VolumeName = "设备未准备好";
                }
                try
                {
                    item.Type = obj["DriveType"].ToEnumByValue<EnumDirverType>();
                }
                catch
                {
                    item.Type = EnumDirverType.Unknow;
                }
                items.Add(item);
            }
            return items;
        }

        #region GetOperatingSystem
        /// <summary>
        /// 获取操作系统信息
        /// </summary>
        /// <returns></returns>
        public static OperatingSystem GetOperatingSystem()
        {
            var list = ExecuteCommand("Win32_OperatingSystem");
            OperatingSystem item = new OperatingSystem();
            foreach (var obj in list)
            {
                item.System = obj["Caption"].ToString();
                item.Version = obj["Version"].ToString();
                item.Manufacturer = obj["Manufacturer"].ToString();
                item.CSName = obj["csname"].ToString();
                item.WindowsDirectory = obj["WindowsDirectory"].ToString();
                return item;
            }
            return item;
        }
        #endregion

        /// <summary>
        /// 获取指定设备序列号的USB磁盘分区信息
        /// </summary>
        public static ManagementObjectCollection FindUSBDevicePartitions(string serialNumber)
        {
            var list = ExecuteSearch("select * from Win32_DiskDrive where InterfaceType='USB' ");

            foreach (var obj in list)
            {
                Print(obj);
                var s1 = "ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + obj["DeviceID"] +
                         "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition";
                var partitons = ExecuteSearch(s1);
                foreach (var p in partitons)
                {
                    var s2 = "ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + p["DeviceID"] +
                             "'} WHERE AssocClass =  Win32_LogicalDiskToPartition";
                    var ds = ExecuteSearch(s2);
                    return ds;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取指定USB设备的第一个磁盘分区名称
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static string FindUSBDevicesPartition(string serialNumber)
        {
            var ps = FindUSBDevicePartitions(serialNumber);
            if (ps == null || ps.Count <= 0) return string.Empty;
            foreach (var obj in ps)
            {
                return obj["Name"].ToSafeString();
            }
            return string.Empty;
        }

        private static void Print(ManagementBaseObject obj)
        {
            foreach (var item in obj.Properties)
            {
                Console.WriteLine("Name: " + item.Name + "\t Value: " + item.Value);
            }
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        private static ManagementObjectCollection ExecuteSearch(string path)
        {
            Guard.ArgumentNotNullOrEmpty(path, "path");
            ManagementObjectSearcher search = new ManagementObjectSearcher(path);
            ManagementObjectCollection list = search.Get();
            return list;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        private static ManagementObjectCollection ExecuteCommand(string command)
        {
            Guard.ArgumentNotNullOrEmpty(command, "command");
            ManagementClass mc = new ManagementClass(command);
            ManagementObjectCollection list = mc.GetInstances();
            return list;
        }
    }
}
