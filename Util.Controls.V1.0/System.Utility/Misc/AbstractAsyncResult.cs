using System.Runtime.CompilerServices;

namespace System.Utility
{
    /// <summary>
    /// 抽象异步通知实现
    /// </summary>
    public abstract class AbstractSyncResult : IAsyncResult
    {
        #region Properties

        private bool _IsCompleted;
        /// <summary>
        /// 是否完成
        /// </summary>
        public virtual bool IsCompleted
        {
            get { return this._IsCompleted; }
        }

        private string _Message;
        /// <summary>
        /// 通知消息
        /// </summary>
        public virtual string Message
        {
            get { return this._Message; }
            set
            {
                this._Message = value;
            }
        }

        private double _Total;
        /// <summary>
        /// 任务刻度总数
        /// </summary>
        public virtual double Total
        {
            get { return this._Total; }
        }

        private double _Completed;
        /// <summary>
        /// 当前已完成的刻度总数
        /// </summary>
        public virtual double Completed
        {
            get { return this._Completed; }
        }

        /// <summary>
        /// 完成百分数
        /// </summary>
        public virtual double Percent
        {
            get { return this.Total > 0 ? (this.Completed * 100d / this.Total) : 0; }
        }

        /// <summary>
        /// 是否执行成功，默认执行成功
        /// </summary>
        public virtual bool IsSuccess { get; set; }

        private string _Log;
        /// <summary>
        /// 记录日志
        /// </summary>
        public string Log
        {
            get { return _Log; }
            set { _Log = value; }
        }


        #endregion

        #region AbstractSyncResult 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AbstractSyncResult()
        {
            this._Completed = 0;
            this._Total = 0;
            this._IsCompleted = false;
            this.IsSuccess = true;
        }
        #endregion

        #region Start
        /// <summary>
        /// 开始
        /// </summary>
        public virtual void Start(double total)
        {
            this._Total = total;
            this._IsCompleted = false;
            this._Completed = 0;
            this.IsSuccess = true;
        }
        #endregion

        #region Advance
        /// <summary>
        /// 执行进度
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Advance(double work, string message = "", object sender = null)
        {
            this._Completed += work;
            this._Message = message;
            //执行经度事件通知
            if (this.OnAdvance != null && !this._IsCompleted)
            {
                this.OnAdvance(work, message);
            }

            if (this._Completed >= this._Total && !this.IsCompleted)
            {
                this._Completed = this._Total;
                this._IsCompleted = true;
                //完成时的事件通知
                if (this.OnCompleted != null)
                {
                    this.OnCompleted();
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AdvanceCompleted(double completed, string message = "")
        {
            this._Completed = completed;
            this._Message = message;

            if (this.OnAdvance != null && !this._IsCompleted)
            {
                this.OnAdvance(0, message);
            }

            if (this._Completed >= this._Total && !this.IsCompleted)
            {
                this._Completed = this._Total;
                this._IsCompleted = true;
                //完成时的事件通知
                if (this.OnCompleted != null)
                {
                    this.OnCompleted();
                }
            }
        }


        /// <summary>
        /// 输出message信息
        /// </summary>
        /// <param name="mes"></param>
        public void WriteMessage(string mes)
        {
            this._Message = mes;
            if (this.OnAdvance != null)
            {
                this.OnAdvance(0, mes);
            }
        }

        #endregion

        #region Stop
        /// <summary>
        /// 完成或停止
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Stop()
        {
            if (!this.IsCompleted)
            {
                this._IsCompleted = true;
                //完成时的事件通知
                if (this.OnCompleted != null)
                {
                    this.OnCompleted();
                }
            }
        }
        #endregion

        /// <summary>
        /// 完成时的通知操作
        /// </summary>
        public Action OnCompleted { get; set; }

        /// <summary>
        /// 当执行经度时的通知操作
        /// </summary>
        public AdvanceEventHandler OnAdvance { get; set; }
    }
}
