using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System;
using System.Web;
using System.Net;
using Microsoft.Win32;

namespace System.Utility.Helper
{
    /// <summary>
    /// 系统、环境等操作辅助类
    /// </summary>
    public class Sys
    {
        #region GetClientIP
        /// <summary>
        /// 获取客户端IP,主机名
        /// </summary>
        /// <returns></returns>
        public static string GetClientHostIP()
        {
            if (HttpContext.Current == null)
            {
                //主机名
                string host = Dns.GetHostName();
                //获取IP列表,第1项是局域网IP
                var addressList = Dns.GetHostEntry(host).AddressList;
                if (addressList.Length > 0)
                {
                    var value = addressList.ToList().Where(s => !s.IsIPv6LinkLocal).Select(s => s.ToString()).ToArray();
                    //获取IP
                    return string.Format("{0}：{1}", host, string.Join(";", value));
                }
            }
            return HttpContext.Current.Request.UserHostAddress;
        }
        #endregion

        #region GetHostName
        /// <summary>
        /// 获取主机名
        /// </summary>
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }
        #endregion

        #region KillProcess
        /// <summary>
        /// 关闭指定名称的进程
        /// </summary>
        public static void KillProcess(string proName)
        {
            if (proName.IsInvalid())
            {
                return;
            }
            var pro = Diagnostics.Process.GetProcessesByName(proName);
            if (pro.IsInvalid())
            {
                return;
            }
            foreach (Diagnostics.Process p in pro)
            {
                try
                {
                    p.Kill();
                    p.Close();
                    p.Dispose();
                }
                catch { }

            }
        }
        #endregion

        #region KillProcess（关闭指定使用指定端口号的所有进程）

        /// <summary>
        /// 关闭指定使用指定端口号的所有进程，exceptPros为除外的进程名
        /// </summary>
        public static void KillProcess(int id, string[] exceptPros)
        {
            var pros = FindProcessByProtId(id);
            if (pros.IsInvalid())
            {
                return;
            }
            pros.ForEach(p =>
                             {
                                 if (!exceptPros.Contains(p.ProcessName))
                                 {
                                     p.Kill();
                                     p.Close();
                                     p.Dispose();
                                 }
                             });
        }
        #endregion

        #region FindProcessByProtId（获取指定使用了指定端口的所有进程）

        /// <summary>
        /// 获取指定使用了指定端口的所有进程
        /// </summary>
        public static List<Process> FindProcessByProtId(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            var pro = new Process();
            List<Process> pros = new List<Process>();
            try
            {
                pro.StartInfo.FileName = "cmd.exe";
                pro.StartInfo.CreateNoWindow = true;
                pro.StartInfo.UseShellExecute = false;
                pro.StartInfo.RedirectStandardOutput = true;
                pro.StartInfo.RedirectStandardInput = true;
                pro.StartInfo.RedirectStandardError = true;
                pro.Start();
                pro.StandardInput.WriteLine(string.Format("netstat -aon|findstr {0}", id));
                pro.StandardInput.WriteLine("exit");
                var reader = pro.StandardOutput;
                while (!reader.EndOfStream)
                {
                    var outline = reader.ReadLine();
                    if (outline.IsValid())
                    {
                        var p = ReadLine(outline);
                        if (p != null) pros.Add(p);
                    }
                }
                return pros;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                pro.Close();
            }
        }

        #region ReadLine
        private static Process ReadLine(string line)
        {
            line = Regex.Replace(line, @"\s+", ",").Trim(',');
            string[] temp = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (temp.Length == 0 || temp[0] != "TCP" || temp[3] != "LISTENING")
            {
                return null;
            }
            int tpid = temp[4].ToSafeInt();
            var pro = Process.GetProcessById(tpid);
            return pro;
        }
        #endregion

        #endregion

        #region IsConnectedInternet
        /// <summary>
        /// 通过Ping百度方式判定是否连接网络
        /// </summary>
        public static bool IsConnectedBiadu()
        {
            var ping = new Net.NetworkInformation.Ping();
            var res = ping.Send("www.baidu.com");
            if (res != null && res.Status == Net.NetworkInformation.IPStatus.Success)
            {
                return true;
            }
            return false;
        }
        

        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        /// <summary>
        /// Win32检测本机是否联网
        /// </summary>
        public static bool IsConnectedInternet()
        {
            int i = 0;
            return InternetGetConnectedState(out i, 0);
        }
        #endregion

        #region GetCurrentDesktop 获取当前用户的桌面路径
        /// <summary>
        /// 获取当前用户的桌面路径
        /// </summary>
        public static string GetCurrentDesktop()
        {
            var openSubKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders");
            if (openSubKey !=
                null)
                return openSubKey.
                    GetValue("Desktop").ToSafeString();
            return string.Empty;
        }
        #endregion

        #region IsRunAsAdmin 判断当前程序是否已管理员权限运行
        /// <summary>
        /// 判断当前程序是否已管理员权限运行
        /// </summary>
        /// <returns></returns>
        public static bool IsRunAsAdmin()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);
            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }
        #endregion

        #region SetPathEnvironment 设置（新增/修改）环境变量中PATH的值
        /// <summary>
        /// 设置（新增/修改）环境变量中PATH的值
        /// </summary>
        /// <param name="value"></param>
        public static void SetPathEnvironment(string value)
        {
            try
            {
                var path = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
                if (path.Contains(value)) return;
                if (path.EndsWith(";")) path += value;
                else path += ";" + value;
                Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Machine);
            }
            catch{}
            
        } 
        #endregion
    }
}
