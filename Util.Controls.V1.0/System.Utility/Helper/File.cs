
#region 引用命名空间
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
#endregion

namespace System.Utility.Helper
{
    /// <summary>
    /// 文件操作公共类
    /// </summary>
    public class File
    {
        #region 字段定义
        /// <summary>
        /// 同步标识
        /// </summary>
        private static readonly Object _sync = new object();
        /// <summary>
        /// windows中非法路径字符
        /// </summary>
        private static readonly char[] InvalidPathChars = new char[]
                                                              {
                                                                  '"', '<', '>', '|', '\0', '\x0001', '\x0002', '\x0003'
                                                                  , '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n'
                                                                  , '\v',
                                                                  '\f', '\r', '\x000e', '\x000f', '\x0010', '\x0011',
                                                                  '\x0012', '\x0013', '\x0014', '\x0015', '\x0016',
                                                                  '\x0017', '\x0018', '\x0019', '\x001a', '\x001b',
                                                                  '\x001c', '\x001d', '\x001e', '\x001f', '*', '?', ':'
                                                              };

        #endregion

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
        /// 外部不需要做异常捕获，内部已做处理
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
                Executer.TryRunLogExceptioin(() => Directory.CreateDirectory(directoryPath), string.Format("文件夹{0}创建失败", directoryPath));
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
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            try
            {
                //如果文件不存在则创建该文件
                if (!System.IO.File.Exists(filePath))
                {
                    //获取文件目录路径
                    string directoryPath = GetFilePath(filePath);

                    //如果文件的目录不存在，则创建目录
                    CreateDirectory(directoryPath);

                    lock (_sync)
                    {
                        //创建文件                    
                        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("创建文件异常", ex);
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
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            try
            {
                //如果文件不存在则创建该文件
                if (!System.IO.File.Exists(filePath))
                {
                    //创建一个FileInfo对象
                    FileInfo file = new FileInfo(filePath);

                    //创建文件
                    using (FileStream fs = file.Create())
                    {
                        //写入二进制流
                        fs.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            catch
            {
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
            CreateFile(filePath, text, Encoding.GetEncoding("utf-8"));
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

            //获取文件目录路径
            string directoryPath = GetFilePath(filePath);

            //如果目录不存在则创建该目录
            if (!System.IO.Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            //创建文件
            FileInfo file = new FileInfo(filePath);
            using (FileStream stream = file.Create())
            {
                using (StreamWriter writer = new StreamWriter(stream, encoding))
                {
                    //写入字符串     
                    writer.Write(text);

                    //输出
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
                Diagnostics.Process.Start("explorer.exe", nPath);
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

        #region ConvertLinuxPath 转换linux路径为合法的windows路径
        /// <summary>
        /// 转换linux路径为合法的windows路径
        /// 1.转换其中的非法字符为validChar(默认下划短线)
        /// 2.替换分隔符'/' to '\\'
        /// </summary>
        /// <param name="linuxpath"></param>
        /// <param name="validChar"></param>
        /// <returns></returns>
        public static string ConvertLinuxPath(string linuxpath, char validChar = '_')
        {
            var index = linuxpath.IndexOfAny(InvalidPathChars);
            if (index >= 0)
            {
                return ConvertLinuxPath(linuxpath.Replace(linuxpath[index], validChar));
            }
            return linuxpath.Replace('/', '\\');
        }
        #endregion

        #region ConnectLinuxPath（链接Linux文件格式的两个路径）

        /// <summary>
        /// 链接Linux文件格式的两个路径（自动处理其中的分隔符），默认分隔符'/'
        /// </summary>
        public static string ConnectLinuxPath(string path1, string path2)
        {
            return ConnectPath('/', path1, path2);
        }

        #endregion

        #region GetLinuxFileName
        /// <summary>
        /// 获取指定路径的linux格式文件或文件夹名称，分隔符‘/’
        /// </summary>
        public static string GetLinuxFileName(string source)
        {
            return GetFileName(source, '/');
        }
        #endregion

        #region GetLinuxFilePath
        /// <summary>
        /// 获取指定路径的linux格式文件或文件夹所在路径，分隔符‘/’
        /// </summary>
        public static string GetLinuxFilePath(string source)
        {
            return GetFilePath(source, '/');
        }
        #endregion

        #region 从文件路径中获取扩展名
        /// <summary>
        /// 从文件路径中获取扩展名,不包含扩展名前面的句点,绝对路径
        /// 范例：c:\test\test.jpg,返回jpg
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetExtension(string filePath)
        {
            //有效性验证
            Guard.ArgumentNotNullOrEmpty(filePath, "filePath");
            var extension = string.Empty;

            try
            {
                extension = Path.GetExtension(filePath).Replace(".", string.Empty);
            }
            catch (Exception)
            {
            }

            return extension;
        }


        /// <summary>
        /// 尝试从当前路径中获取文件的后缀; 如 123_jpg;
        /// 只是对这种格式做处理!
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="splitChar">拆分字符标识</param>
        /// <returns></returns>
        public static string TryGetExtension(string filename, char splitChar)
        {
            //有效性验证
            Guard.ArgumentNotNullOrEmpty(filename, "filePath");
            var temp = filename.Split(splitChar);
            var extension = string.Empty;
            if (temp.Any())
            {
                extension = temp.Last();
            }
            return extension;
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
            relativePath = relativePath.StartsWith("\\") ? relativePath.Substring(1, relativePath.Length - 1) : relativePath;
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var fullPath = System.IO.Path.Combine(path, relativePath);
            return fullPath;
        }
        #endregion

        #region GetFileSize 获取文件大小描述，绝对路径
        /// <summary>
        /// 获取文件大小描述，绝对路径
        /// </summary>
        public static string GetFileSize(string filePath)
        {
            //有效性验证
            Guard.ArgumentNotNullOrEmpty(filePath, "filePath");
            if (!System.IO.File.Exists(filePath))
            {
                return string.Empty;
            }
            FileInfo info = new FileInfo(filePath);
            return GetFileSize(info.Length);
        }
        #endregion

        /// <summary>
        /// 获取当前文件的大小
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static long GetFileLength(string filepath)
        {
            long length = 0;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                length = fs.Length;
            }
            return length;
        }

        #region 获取文件大小描述（参数：文件大小，字节数）

        /// <summary>
        /// 获取文件大小描述（参数：文件大小，字节数）
        /// </summary>
        public static string GetFileSize(long len, string format = "F2")
        {
            if (len <= 0)
            {
                return "0 KB";
            }

            string unit = " B";
            double res = len, rule = 1024D;
            //KB
            if (len >= rule)
            {
                res = len / rule;
                unit = " KB";
            }
            //M
            if (res > rule)
            {
                res = res / rule;
                unit = " MB";
            }
            //G
            if (res > rule)
            {
                res = res / rule;
                unit = " GB";
            }
            //去掉多余的0
            if (res - Math.Truncate(res) == 0)
            {
                return string.Concat(res.ToString("F2"), unit);
            }
            return string.Concat(res.ToString("F2"), unit);
        }

        /// <summary>
        /// 获取文件大小（参数：文件大小，字节数）
        /// </summary>
        public static string GetFileSize(int len)
        {
            return GetFileSize((long)len);
        }
        #endregion

        #region 获取制定文件夹(包括子文件夹)下的所有文件

        public static IList<FileInfo> GetFiles(string folder, string extension)
        {
            return GetFiles(folder, new[] { extension });
        }

        /// <summary>
        /// 获取非系统级目录的所有文件
        /// </summary>
        /// <param name="folder">非系统级目录。</param>
        /// <param name="extensions">筛选的后缀</param>
        /// <returns>返回满足条件的文件列表</returns>
        public static IList<FileInfo> GetFiles(string folder, string[] extensions)
        {
            var allFiles = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
            var allMeetFiles = (from file in allFiles
                                let extension = System.IO.Path.GetExtension(file)
                                where extension != null && extensions.Contains(extension.ToLower())
                                select new FileInfo(file)).ToList();

            return allMeetFiles;
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

        #region ReadFileHead 读取文件头指定长度的数据
        /// <summary>
        /// 读取文件头指定长度的数据
        /// </summary>
        public static byte[] ReadFileHead(string filePath, int index = 4)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buff = new byte[index];
                fs.Read(buff, 0, index);
                return buff;
            }
        }
        #endregion

        #region 过滤非法文件名

        private static readonly char[] _InvalidChars = System.IO.Path.GetInvalidFileNameChars();
        //private static readonly char[] _InvalidChars =new[] { '\r','\"', '<', '>', '|', '\0' };

        public static string FilterInvalidFileName(string oriName)
        {
            //大数据量性能差
            return _InvalidChars.Aggregate(oriName, (current, invalidChar) => current.Replace(invalidChar.ToString(), string.Empty));
        }

        #endregion

        /// <summary>
        /// 判断当前文件是否被暂用
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static bool IsFileInUsing(string path)
        {
            var isUse = true;
            try
            {
                using (new FileStream(path, FileMode.Open))
                {
                    isUse = false;
                }
            }
            catch
            {

            }
            return isUse;
        }


        #region 输入路径是否合法

        /// <summary>
        /// 输入路径是否合法
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>合法与否</returns>
        public static bool InputPathIsValid(string path)
        {
            return IsPositiveNumber(path) && IsHasUninvalidPathChars(path);
        }

        /// <summary>
        /// 是否是文件路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsPositiveNumber(string path)
        {
            return Regex.IsMatch(path,
                    @"^[a-zA-Z]:\\[^\/\:\*\?\""\<\>\|\,]+$",
                    RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 文件路径是否合法
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsHasUninvalidPathChars(string path)
        {
            return !Path.GetInvalidPathChars().Any(path.Contains);
        } 
        #endregion
    }
}
