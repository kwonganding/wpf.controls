using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Utility;

namespace System
{
    /// <summary>
    /// 指针扩展方法
    /// </summary>
    public static class IntptrExtension
    {
        /// <summary>
        /// IntPtr转换成Struct
        /// </summary>
        /// <param name="intptr">指针地址</param>
        /// <returns>Struct 对象</returns>
        public static T ToStruct<T>(this IntPtr intptr) where T : struct
        {
            if (intptr.Equals(IntPtr.Zero))
            {
                return default(T);
            }
            return (T)Marshal.PtrToStructure(intptr, typeof(T));
        }

        /// <summary>
        /// IntPtr 类型转换成 Struct数组
        /// </summary>
        /// <typeparam name="T">结构体类型</typeparam>
        /// <param name="count">数组大小</param>
        /// <param name="intptr">第一个数组地址</param>
        /// <returns>Struct数组列表</returns>
        public static T[] IntPtrToStructs<T>(this IntPtr intptr, int count) where T : struct
        {
            var temps = new T[count];
            var temp = new T();
            for (int i = 0; i < count; i++)
            {
                temp = (T)Marshal.PtrToStructure(intptr, typeof(T));
                temps[i] = (temp);
                intptr = (IntPtr)(Marshal.SizeOf(temp) + intptr.ToInt32());
            }
            return temps;
        }

        /// <summary>
        /// IntPtr 自增一个 T 长度的大小。
        /// </summary>
        /// <typeparam name="T">T 泛型参数。</typeparam>
        /// <param name="intptr">指针地址。</param>
        /// <returns>返回新地址。</returns>
        public static IntPtr Increment<T>(this IntPtr intptr)
        {
            return (IntPtr)(intptr.ToInt32() + Marshal.SizeOf(typeof(T)));
        }

        /// <summary>
        /// 本方法只有适合于结构体对象中，含有中文字符对象时，根据指针转换。
        /// 使用该方法，避免了Windows8 64位机器在Utf8转换为Ansi格式时不定时出错。
        /// </summary>
        /// <param name="intPtr">指针地址。</param>
        /// <returns>返回转换后的字符串。</returns>
        public static string ToAnsiString(this IntPtr intPtr)
        {
            return Encoding.Default.GetString(Encoding.Convert(Encoding.UTF8, Encoding.Default, PtrtoByteArray(intPtr)));
        }

        /// <summary>
        /// 本方法只有适合于结构体对象中，含有中文字符对象时，根据指针转换。
        /// 使用该方法，避免了Windows8 64位机器在Utf8转换为Ansi格式时不定时出错。
        /// </summary>
        /// <param name="intPtr">指针地址。</param>
        /// <returns>返回转换后的字符串。</returns>
        public static string ToGB2312String(this IntPtr intPtr)
        {
            return Encoding.Default.GetString(PtrtoByteArray(intPtr));
        }

        /// <summary>
        /// 本方法只有适合于结构体对象中，含有中文字符对象时，根据指针转换。
        /// </summary>
        /// <param name="intPtr">指针地址。</param>
        /// <returns>返回转换后的字符串。</returns>
        public static string ToUTF8String(this IntPtr intPtr)
        {
            return Encoding.UTF8.GetString(PtrtoByteArray(intPtr));
        }

        /// <summary>
        /// 将Intprt转换成 byte[]
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        private static byte[] PtrtoByteArray(this IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return new byte[0];
            }
            var bytes = new List<byte>();
            unsafe
            {
                int length = 0;
                byte* p = (byte*)ptr;
                while (*p != 0 && length < 10000000)
                {
                    bytes.Add(*p);
                    length++;
                    p++;
                }
            }
            return bytes.ToArray();
        }

        /// <summary>
        /// 获取指针区域的Byte数组;
        /// </summary>
        /// <param name="ptr">指针地址</param>
        /// <returns>byte数组</returns>
        public static Byte[] Ptr2Bytes(this IntPtr ptr)
        {
            return ptr.PtrtoByteArray();
        }

        /// <summary>
        /// 将byte[]转换成 T 结构体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataBuffer"></param>
        /// <returns></returns>
        public static T ByteToStructure<T>(this byte[] dataBuffer)
        {
            object structure = null;
            int size = Marshal.SizeOf(typeof(T));
            IntPtr allocIntPtr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(dataBuffer, 0, allocIntPtr, size);
                structure = Marshal.PtrToStructure(allocIntPtr, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(allocIntPtr);
            }
            return (T)structure;
        }
    }
}
