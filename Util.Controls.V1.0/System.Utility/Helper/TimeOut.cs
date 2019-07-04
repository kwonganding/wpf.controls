using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace System.Utility.Helper
{
    /// <summary>
    /// 超时辅助类;
    /// By: WangXi 2014-11-21 10:22:12
    /// 用途：确定某些方法在一定时间内是否超时执行;
    /// </summary>
    public class TimeOut
    {
        private readonly ManualResetEvent _mTimeOut = new ManualResetEvent(true);
    }
}
