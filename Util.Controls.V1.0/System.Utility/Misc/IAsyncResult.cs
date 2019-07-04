using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Utility
{
    /// <summary>
    /// 异步任务的通知接口
    /// </summary>
    public interface IAsyncResult
    {
        #region Properties

        /// <summary>
        /// 是否完成
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// 消息
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 总任务刻度
        /// </summary>
        double Total { get; }

        /// <summary>
        /// 已完成数量
        /// </summary>
        double Completed { get; }

        /// <summary>
        /// 完成百分比
        /// </summary>
        double Percent { get; }

        /// <summary>
        /// 是否执行成功，默认执行成功
        /// </summary>
        bool IsSuccess { get; set; }

        /// <summary>
        /// 记录日志
        /// </summary>
        string Log { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// 开始
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void Start(double total);

        /// <summary>
        /// 执行进度
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void Advance(double work, string message = "", object sender = null);

        /// <summary>
        /// 执行进度
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void AdvanceCompleted(double completed, string message = "");

        /// <summary>
        /// 输出message信息
        /// </summary>
        /// <param name="mes"></param>
        void WriteMessage(string mes);

        /// <summary>
        /// 完成或停止
        /// 完成或停止
        /// </summary>
        void Stop(); 

        #endregion

        #region Events

        /// <summary>
        /// 当执行经度时的通知操作
        /// </summary>
        AdvanceEventHandler OnAdvance { get; set; }

        /// <summary>
        /// 完成时的通知操作
        /// </summary>
        Action OnCompleted { get; set; } 

        #endregion
    }

    /// <summary>
    /// 定义执行经度操作的委托
    /// </summary>
    /// <param name="work">完成的任务刻度</param>
    /// <param name="message">消息</param>
    public delegate void AdvanceEventHandler(double work, string message = "");
   
}
