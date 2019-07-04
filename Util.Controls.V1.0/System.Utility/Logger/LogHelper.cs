using System;

namespace System.Utility.Logger
{
    public static class LogHelper
    {
        private static System.Utility.Logger.SimpleLogger _logger;

        static LogHelper()
        {
            _logger = new SimpleLogger();
            _logger.LogPath = System.Utility.Helper.File.GetPhysicalPath("Log");
        }

        /// <summary>
        /// 设置日志目录
        /// </summary>
        public static string LogPath { set { _logger.LogPath = value; } }

        #region Public methods：记录日志

        /// <summary>
        /// 向日志文件中写入 “运行信息”。
        /// </summary>
        /// <param name="message">消息内容。</param>
        /// <param name="exception">异常对象（此处建议不要有异常对象）。</param>
        public static void Info(string message, Exception exception = null)
        {
            _logger.Log(message, LogLevel.Info.GetDescription(), exception);
        }

        /// <summary>
        /// 向日志文件中写入 “调试内容”。
        /// </summary>
        /// <param name="message">消息内容。</param>
        /// <param name="exception">异常对象。</param>
        public static void Debug(string message, Exception exception = null)
        {
            _logger.Log(message, LogLevel.Debug.GetDescription(), exception);
        }

        /// <summary>
        /// 向日志文件中写入 “警告内容”。
        /// </summary>
        /// <param name="message">消息内容。</param>
        /// <param name="exception">异常对象。</param>
        public static void Warn(string message, Exception exception = null)
        {
            _logger.Log(message, LogLevel.Warn.GetDescription(), exception);
        }

        /// <summary>
        /// 向日志文件中写入 “错误信息”。
        /// </summary>
        /// <param name="message">消息内容。</param>
        /// <param name="exception">异常对象。</param>
        public static void Error(string message, Exception exception = null)
        {
            _logger.Log(message, LogLevel.Error.GetDescription(), exception);
        }

        /// <summary>
        /// 向日志文件中写入系统 “崩溃消息”。
        /// </summary>
        /// <param name="message">消息内容。</param>
        /// <param name="exception">异常对象。</param>
        public static void Fatal(string message, Exception exception = null)
        {
            _logger.Log(message, LogLevel.Fatal.GetDescription(), exception);
        }

        #endregion


        /// <summary>
        /// 日志级别。
        /// </summary>
        private enum LogLevel
        {
            /// <summary>
            /// 运行信息。
            /// </summary>
            Info,

            /// <summary>
            /// 调试。
            /// </summary>
            Debug,

            /// <summary>
            /// 警告。
            /// </summary>
            Warn,

            /// <summary>
            /// 错误。
            /// </summary>
            Error,

            /// <summary>
            /// 崩溃，死机。
            /// </summary>
            Fatal
        }
    }
}