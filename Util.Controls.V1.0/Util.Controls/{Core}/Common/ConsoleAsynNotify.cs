using System;

namespace System
{
    /// <summary>
    /// console数据message消息的异步通知实现。
    /// 若需要输出更多（如进度、时间等）再扩展即可
    /// </summary>
    public class ConsoleAsynNotify : DefaultAsynNotify
    {
        #region ConsoleAsynNotify-构造函数（初始化）

        /// <summary>
        ///  ConsoleAsynNotify-构造函数（初始化）
        /// </summary>
        public ConsoleAsynNotify()
        {
            this.TimerEnable = false;
        }

        #endregion

        public override void Advance(double work, string message = "")
        {
            Console.WriteLine(message);
        }
    }
}