
#region 引用命名空间
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
#endregion

namespace Util.Controls
{
    /// <summary>
    /// 文件操作公共类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 同步标识
        /// </summary>
        private static readonly Object _sync = new object();

        #region 读取文件

        #region 读取文件到字符串：默认格式UTF8
        /// <summary>
        /// 读取文件到字符串(文件的绝对路径),默认格式UTF8
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static string FileToString(string filePath)
        {
            return FileToString(filePath, Encoding.UTF8);
        }
        #endregion

        #region 读取文件到字符串：指定格式
        /// <summary>
        /// 读取文件到字符串(文件的绝对路径)
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="encoding">字符编码</param>
        public static string FileToString(string filePath, Encoding encoding)
        {
            //检测文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                return string.Empty;
            }

            try
            {
                //创建流读取器
                using (StreamReader reader = new StreamReader(filePath, encoding))
                {
                    //读取流
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  读取文件到文件流FileStream
        /// <summary>
        /// 读取文件到文件流FileStream，注意使用Filestream完后必须释放资源
        /// </summary>
        public static FileStream FileToStream(string path)
        {
            Guard.ArgumentNotNullOrEmpty(path, "path");
            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            return fs;
        }

        #endregion

        #region FileToBytes：读取文件到缓冲区中
        /// <summary>
        /// 读取文件到缓冲区中，长度len默认为0，读取所有(最大为int.MaxValue或文件实际长度)。
        /// 注意：若读取大文件或读取大量数据不要用该方法，自己单独实现。
        /// </summary>
        public static byte[] FileToBytes(string filePath, int len = 0, int start = 0)
        {
            using (FileStream fs = FileToStream(filePath))
            {
                //文件长度最大支持int.MaxValue或文件实际长度
                int flen = fs.Length > int.MaxValue ? int.MaxValue : (int)fs.Length;
                //若未指定长度，则读取最大长度flen
                int readLen = len == 0 ? flen : len;
                //若读取长度大于最大长度，取实际长度
                readLen = readLen >= flen ? flen : readLen;
                var res = new byte[readLen];
                fs.Read(res, start, readLen);
                return res;
            }
        }

        #endregion

        #endregion

        #region 创建一个目录
        /// <summary>
        /// 创建一个目录(目录的绝对路径)
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        public static void CreateDirectory(string directoryPath)
        {
            //有效性验证
            if (string.IsNullOrEmpty(directoryPath))
            {
                return;
            }

            //如果目录不存在则创建该目录
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        /// <summary>
        /// 创建一个文件的上级目录(必须是个文件的路径)
        /// 传入示例:c:\\temp\\abc\\新建文本.txt;
        /// 返回结果:c:\\temp\\abc
        /// </summary>
        /// <param name="filePath">文本文件的路径</param>
        public static string CreateFileDirectory(string filePath)
        {
            var dir = GetFilePath(filePath);
            CreateDirectory(dir);
            return dir;
        }

        #endregion

        #region 创建一个文件

        #region 创建一个文件
        /// <summary>
        /// 创建一个文件(文件的绝对路径)
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void CreateFile(string filePath)
        {
            //有效性验证
            if (string.IsNullOrEmpty(filePath)) return;
            try
            {
                //如果文件不存在则创建该文件
                if (System.IO.File.Exists(filePath)) return;
                //获取文件目录路径
                string directoryPath = GetFilePath(filePath);
                //如果文件的目录不存在，则创建目录
                CreateDirectory(directoryPath);

                lock (_sync)
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate)) { }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("filePath", filePath);
                throw ex;
            }
        }
        #endregion

        #region 创建一个文件,并将字节流写入文件
        /// <summary>
        /// 创建一个文件,并将字节流写入文件。(文件的绝对路径)
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="buffer">二进制流数据</param>
        public static void CreateFile(string filePath, byte[] buffer)
        {
            //有效性验证
            if (string.IsNullOrEmpty(filePath)) return;
            try
            {
                //如果文件不存在则创建该文件
                if (System.IO.File.Exists(filePath)) return;
                //创建一个FileInfo对象
                FileInfo file = new FileInfo(filePath);

                //创建文件
                using (FileStream fs = file.Create())
                {
                    fs.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("filePath", filePath);
                throw ex;
            }
        }
        #endregion

        #region 创建一个文件,并将字符串写入文件

        #region 默认格式为utf-8
        /// <summary>
        /// 创建一个文件,并将字符串写入文件，如果文件已存在则覆盖。(文件的绝对路径)
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">字符串数据</param>
        public static void CreateFile(string filePath, string text)
        {
            CreateFile(filePath, text, Encoding.UTF8);
        }
        #endregion

        #region 指定格式
        /// <summary>
        /// 创建一个文件,并将字符串写入文件，如果文件已存在则覆盖。(文件的绝对路径)
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">字符串数据</param>
        /// <param name="encoding">字符编码</param>
        public static void CreateFile(string filePath, string text, Encoding encoding)
        {
            //有效性验证
            if (filePath == null || filePath == string.Empty)
            {
                return;
            }
            CreateFileDirectory(filePath);
            //创建文件
            FileInfo file = new FileInfo(filePath);

            using (FileStream stream = file.Open(FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(stream, encoding))
                {
                    writer.Write(text);
                    writer.Flush();
                }
            }
        }
        #endregion

        #endregion

        #endregion

        #region 打开一个文件路径

        /// <summary>
        /// 打开一个文件夹或者文件
        /// </summary>
        /// <param name="path">需要打开的文件、文件夹路径</param>
        public static void Open(params string[] path)
        {
            if (path.IsInvalid()) return;
            var nPath = ConnectPath(path);
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", nPath);
            }
            catch (Exception ex)
            {
                throw new Exception("打开文件出现异常:" + nPath, ex);
            }
        }

        #endregion

        #region DeleteFile
        /// <summary>
        /// 删除文件
        /// </summary>
        public static void DeleteFile(string filePath)
        {
            //有效性验证
            if (filePath.IsInvalid())
            {
                return;
            }
            try
            {
                System.IO.File.Delete(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception("删除文件异常:" + filePath, ex);
            }
        }
        #endregion

        #region GetFileName
        /// <summary>
        /// 获取指定文件或文件夹路径的名称，默认分隔符separate='\\'
        /// 如 d:\File\aa或d:\File\aa.rar 返回aa,aa.rar
        /// </summary>
        public static string GetFileName(string source, char separate = '\\')
        {
            if (source.IsInvalid())
            {
                return string.Empty;
            }
            source = source.TrimEnd(separate);
            var index = source.LastIndexOf(separate);
            if (index > 0)
            {
                return source.Substring(index + 1, source.Length - index - 1);
            }
            return source;
        }
        #endregion

        #region GetFilePath
        /// <summary>
        /// 获取指定文件或文件夹路径的所在文件路径，默认分隔符separate='\\'
        ///  如 d:\File\aa或d:\File\aa.rar 返回d:\File
        /// </summary>
        public static string GetFilePath(string source, char separate = '\\')
        {
            if (source.IsInvalid())
            {
                return string.Empty;
            }
            source = source.TrimEnd(separate);
            var index = source.LastIndexOf(separate);
            if (index > 0)
            {
                return source.Substring(0, index + 1);
            }
            return source;
        }
        #endregion

        #region ConnectPath（链接多个个路径）

        /// <summary>
        /// 链接多个个路径（自动处理其中的分隔符），
        /// </summary>
        public static string ConnectPath(char separate, params string[] path)
        {
            if (path.IsInvalid()) return string.Empty;
            if (path.Length == 2) return string.Format("{0}{1}{2}", path[0].TrimEnd(separate), separate, path[1].TrimStart(separate));
            if (path.Length == 1) return path[0];
            StringBuilder sb = new StringBuilder(32);
            foreach (var p in path)
            {
                sb.Append(p.TrimEnd(separate).TrimStart(separate)).Append(separate);
            }
            return sb.ToString().TrimEnd(separate);
        }

        /// <summary>
        /// 链接多个个路径（自动处理其中的分隔符），默认分隔符char separate = '\\'
        /// </summary>
        public static string ConnectPath(params string[] path)
        {
            return ConnectPath('\\', path);
        }

        #endregion

        #region 获取文件相对路径映射的物理路径
        /// <summary>
        /// 获取文件相对路径映射的物理路径，若文件为绝对路径则直接返回
        /// </summary>
        /// <param name="relativePath">文件的相对路径</param>        
        public static string GetPhysicalPath(string relativePath)
        {
            //有效性验证
            if (string.IsNullOrEmpty(relativePath))
            {
                return string.Empty;
            }
            //~,~/,/,\
            relativePath = relativePath.Replace("/", @"\").Replace("~", string.Empty).Replace("~/", string.Empty);
            //网络共享目录中的文件不应移除根路径
            if(!relativePath.StartsWith("\\\\"))
                relativePath = relativePath.StartsWith("\\") ? relativePath.Substring(1, relativePath.Length - 1) : relativePath;
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var fullPath = System.IO.Path.Combine(path, relativePath);
            return fullPath;
        }
        #endregion

        #region IsValid
        /// <summary>
        /// 判断文件是否可用，字符为空、文件不存在，文件大小为0都 表示文件不可用。
        /// true表示文件合法可用。
        /// </summary>
        public static bool IsValid(string file)
        {
            if (file.IsInvalid()) return false;
            if (!System.IO.File.Exists(file)) return false;
            System.IO.FileInfo info = new FileInfo(file);
            if (info.Length <= 0) return false;
            return true;
        }
        #endregion

        #region 是否合法的文件夹

        /// <summary>
        /// 是否合法的文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsValidDictory(string path)
        {
            return !path.IsInvalid() && Directory.Exists(path);
        }

        #endregion

        /// <summary>
        /// 路径合法性过滤。把windows不支持的文件路径字符替换为replaceChar
        /// </summary>
        public static string Filtration(string path, string replaceChar = "_")
        {
            StringBuilder rBuilder = new StringBuilder(path);
            foreach (char rInvalidChar in Path.GetInvalidFileNameChars())
                rBuilder.Replace(rInvalidChar.ToString(), replaceChar);
            return rBuilder.ToString();
        }
    }
}
