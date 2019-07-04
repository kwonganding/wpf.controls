using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 异常类型的扩展方法
    /// </summary>
    public static partial class TypeExtension
    {
        /// <summary>
        /// 转换为日志字符串
        /// </summary>
        public static string ToLogString(this Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }
            var builder = new StringBuilder();
            builder.Append("  >>异常消息（Message）：").AppendLine(ex.Message);
            builder.Append("  >>异常来源（Source）：").AppendLine(ex.Source);
            builder.Append("  >>异常类型（ExceptionType）：").AppendLine(ex.GetType().ToString());
            builder.Append("  >>原生异常类型（BaseExceptionType）：").AppendLine(ex.GetBaseException().GetType().ToString());
            builder.Append("  >>出错的方法签名（TargetSite）：").Append(ex.TargetSite);

            //若有自定义数据，输出到日志文件中。
            if (ex.Data.Count > 0)
            {
                builder.Append(Environment.NewLine);
                builder.Append("  >>自定义数据（Data）：");
                var dataString = new StringBuilder();
                foreach (DictionaryEntry de in ex.Data)
                {
                    dataString.Append("Key：" + de.Key + "，Value：" + de.Value +"; ");
                }

                builder.Append(dataString);
            }

            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                builder.Append(Environment.NewLine);
                builder.Append("  >>堆栈信息（StackTrace）：");
                builder.Append(Environment.NewLine);
                builder.Append(ex.StackTrace);
            }

            //appent inner exception
            if (ex.InnerException != null)
            {
                builder.Append(Environment.NewLine);
                builder.Append(">>========================== 内部异常（InnerException）==========================");
                builder.Append(Environment.NewLine);
                builder.Append(ex.InnerException.ToLogString());
            }
            return builder.ToString();
        }

        /// <summary>
        /// 转换为日志字符串
        /// </summary>
        public static string AllMessage(this Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(ex.Message);
            if (ex.InnerException != null)
            {
                sb.Append(Environment.NewLine);
                sb.Append(">").Append(ex.InnerException.AllMessage());
            }
            return sb.ToString();
        }
    }
}
