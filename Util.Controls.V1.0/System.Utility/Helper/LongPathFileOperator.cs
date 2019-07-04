using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace System.Utility.Helper
{
    /// <summary>
    /// 路径超长文件操作辅助类
    /// 文件夹路径长度超过248、文件路径超过260处理
    /// </summary>
    public class LongPathFileOperator : IDisposable
    {
        private const uint GENERIC_READ = 0x80000000;
        private const uint OPEN_EXISTING = 3;
        private System.IntPtr handle;
        private const string PathHead = @"\\?\";

        public string TryParse(string filePath, out bool isSucess)
        {
            int len = -1;
            var ptr = GetTextFromFile(filePath, ref len);
            if (len <= 0)
            {
                isSucess = false;
                return string.Empty;
            }

            var str = ptr.ToAnsiString();
            Marshal.FreeHGlobal(ptr);
            isSucess = true;
            return str;
        }

        [DllImport("XFileParser.dll", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr GetTextFromFile(string pFileName, ref int pCount);

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="FileName">文件路径</param>
        /// <returns>返回是否能正常打开，成功返回true</returns>
        public bool Open(string FileName)
        {
            // open the existing file for reading       
            handle = CreateFile
            (
                FileName,
                GENERIC_READ,
                0,
                0,
                OPEN_EXISTING,
                0,
                0
            );

            if (handle != System.IntPtr.Zero)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="buffer">读取文件大小</param>
        /// <param name="index">文件读取位置</param>
        /// <param name="count">读取的长度</param>
        /// <returns>正常读取返回读取到长度（大于0），读取到结尾返回0</returns>
        public unsafe int Read(byte[] buffer, int index, int count)
        {
            int n = 0;
            fixed (byte* p = buffer)
            {
                if (!ReadFile(handle, p + index, count, &n, 0))
                {
                    return 0;
                }
            }
            return n;
        }

        /// <summary>
        /// 长文件名的文件拷贝
        /// </summary>
        /// <param name="source">源路径</param>
        /// <param name="target">目标路径</param>
        /// <param name="overwrite">是否覆盖目标路径</param>
        public static bool Copy(string source, string target, bool overwrite)
        {
            string formattedName_source = PathHead + source;
            string formattedName_target = PathHead + target;

            // CopyFile 第三个参数是 FALSE 的时候自动覆盖 所以写成 !overwrite
            return CopyFile(formattedName_source, formattedName_target, !overwrite);
        }

        /// <summary>
        /// 关闭文件
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// 关闭文件
        /// </summary>
        /// <returns>成功关闭返回true</returns>
        public bool Close()
        {
            return CloseHandle(handle);
        }

        #region Windows kernel32 API

        [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true)]
        private static extern unsafe System.IntPtr CreateFile
        (
            string FileName,          // file name
            uint DesiredAccess,       // access mode
            uint ShareMode,           // share mode
            uint SecurityAttributes,  // Security Attributes
            uint CreationDisposition, // how to create
            uint FlagsAndAttributes,  // file attributes
            int hTemplateFile         // handle to template file
        );

        [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true)]
        private static extern unsafe bool ReadFile
        (
            System.IntPtr hFile,      // handle to file
            void* pBuffer,            // data buffer
            int NumberOfBytesToRead,  // number of bytes to read
            int* pNumberOfBytesRead,  // number of bytes read
            int Overlapped            // overlapped buffer
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CopyFile(string source, string target, bool overwrite);

        [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true)]
        private static extern unsafe bool CloseHandle
        (
            System.IntPtr hObject // handle to object
        );

        #endregion


    }
}
