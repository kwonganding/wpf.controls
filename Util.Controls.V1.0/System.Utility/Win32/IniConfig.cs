using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace System.Utility.Win32
{
    /// <summary>
    /// ini配置文件操作
    /// </summary>
    public class IniConfig
    {
        private string _FilePath;

        /// <summary> 
        /// 构造方法 
        /// </summary> 
        /// <param name="filePath">文件路径</param> 
        public IniConfig(string filePath)
        {
            _FilePath = filePath;
        }
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            if (this.ExistINIFile())
            {
                Win32.WritePrivateProfileString(Section, Key, Value, this._FilePath);
            }
            else
            {
                throw new Exception("指定的配置文件读写错误！");
            }
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key, string sdef="")
        {
            if (this.ExistINIFile())
            {
                StringBuilder temp = new StringBuilder(500);
                int i = Win32.GetPrivateProfileString(Section, Key, sdef, temp, 500, this._FilePath);
                return temp.ToString().Trim().Replace(",", string.Empty);
            }
            else
            {
                throw new Exception("指定的配置文件读写错误！");
            }
        }

        /// <summary>
        /// 读取ini文件中所有Section
        /// </summary>
        /// <returns></returns>
        public List<string> ReadAllSection()
        {
            List<string> result = new List<string>();
            try
            {
                long count = 0;
                using (var fs = new FileStream(_FilePath, FileMode.Open, FileAccess.Read))
                {
                    count = fs.Length;
                }


                byte[] buf = new byte[count];

                uint len = Win32.GetPrivateProfileString(null, null, null, buf, (uint)buf.Length, this._FilePath);
                int k = 0;
                for (int i = 0; i < len; i++)
                    if (buf[i] == 0)
                    {
                        result.Add(Encoding.Default.GetString(buf, k, i - k));
                        k = i + 1;
                    }
            }
            catch { throw new Exception("指定的配置文件读写错误！"); }
            return result;
        }

        /// <summary>
        /// 读取section下所有keyvalue
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ReadSectionKeys(string section)
        {
            var result = new Dictionary<string, string>();
            try
            {
                long count = 0;
                using (var fs = new FileStream(_FilePath, FileMode.Open, FileAccess.Read))
                {
                    count = fs.Length;
                }

                byte[] buf = new byte[count];
                uint lenf = Win32.GetPrivateProfileString(section, null, null, buf, (uint)buf.Length, this._FilePath);
                int j = 0;
                for (int i = 0; i < lenf; i++)
                    if (buf[i] == 0)
                    {
                        string key = Encoding.Default.GetString(buf, j, i - j);
                        string value = IniReadValue(section, key, this._FilePath);
                        result.Add(key, value);
                        j = i + 1;
                    }
            }
            catch { throw new Exception("指定的配置文件读写错误！"); }
            return result;
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            //return false;
            return File.Exists(_FilePath);
        }
    }
}
