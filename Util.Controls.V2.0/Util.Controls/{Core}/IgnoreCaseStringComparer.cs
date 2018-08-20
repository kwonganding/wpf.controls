using System;
using System.Collections.Generic;

namespace Util.Controls
{
    /// <summary>
    /// 不区分大小写的字符串比较器
    /// </summary>
    public sealed class IgnoreCaseStringComparer : IEqualityComparer<string>
    {
        public static IgnoreCaseStringComparer Instance
        {
            get { return Singleton<IgnoreCaseStringComparer>.GetInstance(); }
        }

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