using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Steam 扩展方法
    /// </summary>
    static partial class TypeExtension
    {
        #region 扩展流运用

        /// <summary>
        /// 扩展流运用.
        /// 注意：Stream使用者要注意资源的释放。
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="descSream"></param>
        public static void Append(this Stream stream, Stream descSream)
        {
            byte[] descbyte = new byte[descSream.Length];
            descSream.Read(descbyte, 0, descbyte.Length);
            descSream.Seek(0, SeekOrigin.Begin);
            stream.Write(descbyte, 0, descbyte.Length);
        }

        #endregion
    }
}
