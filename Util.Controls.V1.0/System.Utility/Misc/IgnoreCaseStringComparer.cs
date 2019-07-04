using System;
using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// 不区分大小写的字符串比较器
    /// </summary>
    public class IgnoreCaseStringComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return string.Compare(x, y, true) == 0;
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}