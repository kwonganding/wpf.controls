using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 动态类型扩展提供接口
    /// </summary>
    public interface IDynamicEnable
    {
        /// <summary>
        /// 动态类型扩展
        /// </summary>
        DynamicX Dynamic { get; set; }
    }
}
