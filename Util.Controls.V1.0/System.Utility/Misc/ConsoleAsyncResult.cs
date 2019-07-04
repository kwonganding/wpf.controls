namespace System.Utility
{
    /// <summary>
    /// 控制台输出异步通知实现
    /// </summary>
    public class ConsoleAsyncResult : AbstractSyncResult
    {
        #region Start
        /// <summary>
        /// 开始
        /// </summary>
        public override void Start(double total)
        {
            base.Start(total);
            Console.WriteLine(string.Format("Start   ,Total:{0}", total));
        }
        #endregion

        #region Advance
        /// <summary>
        /// 执行进度
        /// </summary>
        public override void Advance(double work, string message = "", object sender = null)
        {
            base.Advance(work, message, sender);
            Console.WriteLine("Total:{0};Completed:{1}.Message:{2}", this.Total, this.Completed, message);
        }
        #endregion

        #region Stop
        /// <summary>
        /// 完成或停止
        /// </summary>
        public override void Stop()
        {
            base.Stop();
            Console.WriteLine("Stop");
        }
        #endregion
    }
}
