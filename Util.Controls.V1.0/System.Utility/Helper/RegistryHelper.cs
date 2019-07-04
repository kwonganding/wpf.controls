using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace System.Utility.Helper
{
    public static class RegistryHelper
    {
        /// <summary>
        /// 读取指定名称的注册表的值
        /// </summary>
        /// <param name="root">注册表根:Registry.LocalMachine</param>
        /// <param name="subkey">子项节点名称:SOFTWARE\\Microsoft</param>
        /// <param name="name">要写入的节点名称</param>
        /// <returns>注册表值</returns>
        public static string GetRegistryData(RegistryKey root, string subkey, string name)
        {

            using (RegistryKey myKey = root.OpenSubKey(subkey, true))
            {
                string registData = "";
                if (myKey != null)
                {
                    registData = myKey.GetValue(name).ToString();
                }

                return registData;
            }

        }

        /// <summary>
        /// 写注册表数据
        /// </summary>
        /// <param name="root">注册表根:Registry.LocalMachine</param>
        /// <param name="subkey">子项节点名称:SOFTWARE\\Microsoft</param>
        /// <param name="name">要写入的节点名称</param>
        /// <param name="value">要写入的值</param>
        /// <param name="registryValueKind">待写入的值类型</param>
        public static void SetRegistryData(RegistryKey root, string subkey, string name, string value, RegistryValueKind registryValueKind)
        {
            using (RegistryKey aimdir = root.CreateSubKey(subkey))
            {
                if (aimdir != null)
                {
                    aimdir.SetValue(name, value, registryValueKind);
                }
            }


        }

        /// <summary>
        /// 删除注册表中指定的注册表项
        /// </summary>
        /// <param name="root">注册表根:Registry.LocalMachine</param>
        /// <param name="subkey">子项节点名称:SOFTWARE\\Microsoft</param>
        /// <param name="name">要删除的节点名称</param>
        public static void DeleteRegist(RegistryKey root, string subkey, string name)
        {
            using (RegistryKey myKey = root.OpenSubKey(subkey, true))
            {
                if (myKey == null)
                {
                    return;
                }
                var subkeyNames = myKey.GetSubKeyNames();

                foreach (string aimKey in subkeyNames)
                {
                    if (aimKey == name)
                        myKey.DeleteSubKeyTree(name);
                }
            }
        }

        /// <summary>
        /// 判断指定注册表项是否存在
        /// </summary>
        /// <param name="root">注册表根:Registry.LocalMachine</param>
        /// <param name="subkey">子项节点名称:SOFTWARE\\Microsoft</param>
        /// <param name="name">要判断的节点名称</param>
        /// <returns>存在与否</returns>
        public static bool IsRegistryExist(RegistryKey root, string subkey, string name)
        {
            using (RegistryKey myKey = root.OpenSubKey(subkey, true))
            {
                return myKey != null && myKey.GetValue(name) != null;
            }
        }
    }
}
