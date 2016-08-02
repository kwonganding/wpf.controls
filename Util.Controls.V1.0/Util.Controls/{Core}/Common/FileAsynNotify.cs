using System;

namespace System
{
    /// <summary>
    /// 文件数据操作异步消息，增加了数据处理速度消息处理
    /// </summary>
    public class FileAsynNotify : DefaultAsynNotify
    {
        /// <summary>
        /// 消息格式
        /// </summary>
        public string MessageFormat = "总大小{0}，已完成{1}，平均速度{2}/s";

        private string _TotalDesc;

        private double _Total;
        /// <summary>
        /// 总任务刻度
        /// </summary>
        public override double Total
        {
            get { return this._Total; }
            protected set
            {
                this._Total = value;
                this._TotalDesc = System.Utility.Helper.File.GetFileSize((long)value);
                base.OnPropertyChanged("Total");
            }
        }

        protected override void SetMessage(string mes)
        {
            var time = this.UsedSecond;
            var comsize = System.Utility.Helper.File.GetFileSize((long)this.Completed,"F4");
            string speed = "0KB";
            if (time > 0)
            {
                speed = System.Utility.Helper.File.GetFileSize((long)(this.Completed / time));
            }
            this.Message = string.Format(this.MessageFormat, this._TotalDesc, comsize, speed);
        }
    }
}