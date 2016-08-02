using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System
{
    /// <summary>
    /// 普通异步通知消息实现类，支持计时、支持WPF属性值通知
    /// </summary>
    public class DefaultAsynNotify : BaseNotifyPropertyChanged, IAsynNotify
    {
        /// <summary>
        /// 该属性主要用于线程控制
        /// 当用户取消或停止操作时，但进度已经跑满，则保证“完成”事件通知执行完后，再结束线程。
        /// </summary>
        private bool _IsCompletedCallbackOver;

        /// <summary>
        /// 计时器
        /// </summary>
        protected System.Timers.Timer Timer { get; private set; }

        private string _Message;
        /// <summary>
        /// 消息
        /// </summary>
        public virtual string Message
        {
            get { return this._Message; }
            set
            {
                if (this._Message == value) return;
                this._Message = value;
                base.OnPropertyChanged("Message");
            }
        }

        private string _TimeMessage;
        /// <summary>
        /// /// <summary>
        /// 计时消息，格式如：已用时：4分5秒 估计剩余时间3分20秒
        /// </summary>
        /// </summary>
        public string TimeMessage
        {
            get { return this._TimeMessage; }
            set
            {
                this._TimeMessage = value;
                base.OnPropertyChanged("TimeMessage");
            }
        }

        public int UsedSecond { get; private set; }

        /// <summary>
        /// 是否启用计时器，默认ture启用。
        /// 只有启用后计时器Timer、计时消息TimeMessage才生效。
        /// </summary>
        public bool TimerEnable { get; set; }

        private double _Total;
        /// <summary>
        /// 总任务刻度
        /// </summary>
        public virtual double Total
        {
            get { return this._Total; }
            protected set
            {
                this._Total = value;
                base.OnPropertyChanged("Total");
            }
        }

        private double _Completed;
        /// <summary>
        /// 已完成数量
        /// </summary>
        public double Completed
        {
            get { return this._Completed; }
            private set
            {
                this._Completed = value;
                base.OnPropertyChanged("Completed");
            }
        }

        private bool _IsCompleted;
        /// <summary>
        /// 是否完成
        /// </summary>
        public virtual bool IsCompleted
        {
            get { return this._IsCompleted; }
            protected set
            {
                this._IsCompleted = value;
                base.OnPropertyChanged("IsCompleted");
            }
        }

        private double _Percent;
        /// <summary>
        /// 完成百分比
        /// </summary>
        public double Percent
        {
            get { return this._Percent; }
            private set
            {
                this._Percent = value;
                base.OnPropertyChanged("Percent");
            }
        }

        private bool _IsSuccess;
        /// <summary>
        /// 是否执行成功，默认执行成功。若失败则Message会输出错误消息
        /// </summary>
        public bool IsSuccess
        {
            get { return this._IsSuccess; }
            set
            {
                this._IsSuccess = value;
                base.OnPropertyChanged("IsSuccess");
            }
        }

        #region DefaultAsynNotify-构造函数（初始化）

        /// <summary>
        ///  DefaultAsynNotify-构造函数（初始化）
        /// </summary>
        public DefaultAsynNotify()
        {
            this.Completed = 0;
            this.Total = 1986;
            this.Percent = 0;
            this.UsedSecond = 0;
            this.IsSuccess = true;
            this.IsCompleted = false;
            this.TimerEnable = true;
        }

        #endregion

        /// <summary>
        /// 开始,total为刻度总数
        /// </summary>
        public void Start(double total)
        {
            this.Completed = 0;
            this.Total = total;
            this.Percent = 0;
            this.IsCompleted = false;
            this.IsSuccess = true;
            _IsCompletedCallbackOver = false;
            this.StartTimer();
        }

        public void ParseTime()
        {
            this.Timer.Enabled = false;
        }

        public void Continue()
        {
            this.Timer.Enabled = true;
        }

        /// <summary>
        /// 累加进度，work为更新的刻度（增量刻度值）
        /// 累加计算进度
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Advance(double work, string message = "")
        {
            if (work > 0)
            {
                this.Completed += work;
            }

            Notify(work, message);
        }

        /// <summary>
        /// 设置实际进度
        /// 与 Advance 方法不同，一个是直接设置进度
        /// 可以与Advance混合使用时，但需要谨慎使用。
        /// </summary>
        /// <param name="currProgress">当前实际进度</param>
        /// <param name="message">通知消息</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetProgress(double currProgress, string message = "")
        {
            this.Completed = currProgress;
            Notify(currProgress, message);
        }

        /// <summary>
        /// 完成
        /// </summary>
        public void Complete()
        {
            if (this.IsCompleted)
            {
                return;
            }

            _IsCompletedCallbackOver = false;
            this.IsCompleted = true;
            this.Completed = this.Total;
            this.Percent = 1;
            this.StopTimer();
            //完成时的事件通知
            if (this.OnCompleted != null)
            {
                this.OnCompleted();
            }

            _IsCompletedCallbackOver = true;
        }

        /// <summary>
        /// 取消操作，若已完成则不处理。
        /// mes：界面输出的取消提示信息
        /// </summary>
        public void Cancel(string mes = "用户取消了当前操作")
        {
            if (this.IsCompleted)
            {
                while (!_IsCompletedCallbackOver)
                {
                    Thread.Sleep(10);
                }

                return;
            }

            this.IsCompleted = true;
            this.IsSuccess = false;
            //完成时的事件通知
            if (this.OnCompleted != null)
            {
                this.OnCompleted();
            }

            _IsCompletedCallbackOver = true;
            this.Message = mes;
            this.StopTimer();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            this.Completed = 0;
            this.Total = 1986;
            this.Percent = 0;
            this.UsedSecond = 0;
            this.IsSuccess = true;
            this.IsCompleted = false;
            this.TimerEnable = true;
            this.Message = string.Empty;
        }

        /****************** private methods ******************/


        private void Notify(double work, string message = "")
        {
            this.SetMessage(message);
            this.Percent = this.Completed / this.Total;
            //完成时的事件通知
            if (this.Completed >= this.Total && !this.IsCompleted)
            {
                this.Complete();
            }

            //执行经度事件通知
            if (this.OnAdvance != null && !this.IsCompleted)
            {
                //这里的work应该没有用了，当不同方式反馈的时候意义也不同，界面不会用了。
                this.OnAdvance(work, message);
            }
        }

        /// <summary>
        /// 停止计时
        /// </summary>
        private void StopTimer()
        {
            if (!this.TimerEnable) return;
            this.TimeMessage = string.Format("共计耗时{0}",
                                             System.Utility.Helper.Format.FormatSecond((int)this.UsedSecond));

            if (this.Timer != null)
            {
                this.Timer.Stop();
                this.Timer.Dispose();
            }
        }

        /// <summary>
        /// 设置输出消息
        /// </summary>
        protected virtual void SetMessage(string mes)
        {
            this.Message = mes;
        }

        /// <summary>
        /// 设置计时消息
        /// </summary>
        protected virtual void SetTimeMessage()
        {
            if (!this.TimerEnable || this.UsedSecond == 0)
            {
                return;
            }

            string sf = "已用时 : {0}, 预计剩余时间 : {1}";
            var ct = this.UsedSecond;
            double rt;
            if (ct <= 0 || this.Completed <= 0)
            {
                rt = 0;
            }
            else
            {
                var sp = this.Completed / ct;
                rt = (this.Total - this.Completed) / sp;
            }

            //超长耗时操作，剩余时间会吓住客户，so，超过20个小时的操作，都显示为“未知”。
            string reStr;
            if (rt > 72000 || rt < 0)
            {
                reStr = "未知";
            }
            else
            {
                reStr = System.Utility.Helper.Format.FormatSecond((int)rt);
            }

            this.TimeMessage = string.Format(sf, System.Utility.Helper.Format.FormatSecond(ct), reStr);
        }

        /// <summary>
        /// 启动计时器
        /// </summary>
        private void StartTimer()
        {
            if (!this.TimerEnable) return;
            this.Timer = new System.Timers.Timer();
            this.Timer.Interval = 1000;//1 second
            this.Timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            //开始计时
            this.UsedSecond = 0;
            this.Timer.Start();

            this.SetTimeMessage();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.UsedSecond++;
            this.SetTimeMessage();
        }

        /// <summary>
        /// 当执行经度时的通知操作
        /// </summary>
        public AsynNotifyAdvanceEventHandler OnAdvance { get; set; }

        /// <summary>
        /// 完成时的通知操作
        /// </summary>
        public Action OnCompleted { get; set; }
    }
}