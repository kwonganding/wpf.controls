using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace System.Utility.Logger
{
    /// <summary>
    /// 简单日志
    /// </summary>
    public class SimpleLogger
    {
        #region 日志基础配置

        /// <summary>
        /// 日志文件最大大小，默认（2048K)）
        /// </summary>
        public int MaxSize = 2048;

        /// <summary>
        /// 日志路径，默认（当前根路径\Log）
        /// </summary>
        public string LogPath = Helper.File.GetPhysicalPath("Log");

        /// <summary>
        /// 日志问名称，默认（Log）
        /// </summary>
        public string FileName = "Log";

        /// <summary>
        /// 是否启用异步日志写入，默认（true：启用异步）
        /// </summary>
        public bool IsAsyn = true;

        #endregion

        #region Private attributes（私有属性）
        /// <summary>
        /// 文件序号
        /// </summary>
        private int FileIndex = 1;
        /// <summary>
        /// 上次日志时间
        /// </summary>
        private DateTime LastDate = DateTime.Now;
        #endregion

        #region Private methods

        #region GetFilePath：获取文件路径
        /// <summary>
        /// 获取文件路径
        /// </summary>
        private string GetFilePath()
        {
            //如果是新的一天，文件序号重置
            if (DateTime.Now.Date > this.LastDate.Date)
            {
                this.FileIndex = 1;
            }

            this.LogPath = this.LogPath.EndsWith("\\") ? this.LogPath.Substring(0, this.LogPath.Length - 1) : this.LogPath;
            string file = string.Format("{0}\\{1}\\{2}{3}.log", this.LogPath, DateTime.Now.ToString("yyyy-MM-dd"),this.FileName, this.FileIndex);
            if (!System.IO.File.Exists(file))
            {
                //文件不存在，创建文件
                Helper.File.CreateFile(file, string.Empty);
            }

            if (this.GetFileSize(file) > this.MaxSize)
            {
                //文件大小超过最大设定，新增一个文件
                this.FileIndex++;
                return this.GetFilePath();
            }

            this.LastDate = DateTime.Now;
            return file;
        }

        private long GetFileSize(string filePath)
        {
            return new FileInfo(filePath).Length / 1024;
        }

        #endregion

        #region Write：写日志(入口）
        /// <summary>
        /// 写日志(入口）
        /// </summary>
        private void Write(string text)
        {
            if (this.IsAsyn)
            {
                this.AsynWriteLog(text);
            }
            else
            {
                this.WriteLog(text);
            }
        }
        #endregion

        #region WriteLog：写入日志
        /// <summary>
        /// 写入日志
        /// </summary>
        private void WriteLog(string text)
        {
            try
            {
                string filePath = this.GetFilePath();
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    //写入字符串     
                    writer.WriteLine(text);
                    //输出
                    writer.Flush();
                }
            }
            catch { }
        }
        #endregion

        #region SynWriteLog： 异步写入日志
        /// <summary>
        /// 异步写入日志
        /// </summary>
        private void AsynWriteLog(string value)
        {
            Executer.TryRunByThreadPool(() =>
                {
                    this.WriteLog(value);
                });
        }
        #endregion

        #region LogFormat：格式化日志内容
        /// <summary>
        /// 格式化日志内容
        /// </summary>
        private string LogFormat(string content, string logLevel, Exception exception)
        {
            var builder = new StringBuilder();
            builder.Append(Environment.NewLine);
            builder.AppendLine("//==========================================================================================================");
            builder.AppendFormat("系统时间：{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            //builder.Append("  ");
            //builder.AppendFormat("用户IP：{0}", Helper.Sys.GetClientHostIP());
            builder.Append("  ");
            builder.AppendFormat("当前线程ID：{0}", Thread.CurrentThread.ManagedThreadId);
            builder.Append(Environment.NewLine);
            builder.AppendFormat("日志等级：{0}", logLevel);
            builder.Append(Environment.NewLine);
            builder.AppendFormat("日志内容：{0}", content);

            if (exception != null)
            {
                builder.Append(Environment.NewLine);
                builder.AppendLine("异常信息：");
                builder.Append(exception.ToLogString());
            }
            return builder.ToString();
        }
        #endregion

        #endregion

        #region LogMessage
        /// <summary>
        /// 记录消息，无任何格式处理
        /// </summary>
        [Obsolete("记录消息，无任何格式处理，记录系统日志请勿使用该功能")]
        public void LogMessage(string content)
        {
            try
            {
                string filePath = this.GetFilePath();
                using (var writer = new StreamWriter(filePath, true))
                {
                    //写入字符串     
                    writer.WriteLine(content);
                    //输出
                    writer.Flush();
                }
            }
            catch { }
        }
        #endregion

        #region Public methods：记录日志

        /// <summary>
        /// 写日志
        /// </summary>
        public void Log(string content, string level, Exception ex)
        {
            var text = this.LogFormat(content, level, ex);
            this.Write(text);
        }
        #endregion
    }
}