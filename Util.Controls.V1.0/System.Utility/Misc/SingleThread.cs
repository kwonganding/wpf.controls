using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace System.Utility
{
    /// <summary>
    /// 封装单线程的简单处理机制.
    /// </summary>
    public sealed class SingleThread
    {
        public Thread BackThread { get; private set; }

        /// <summary>
        /// 开始异步执行指定的方法体
        /// </summary>
        /// <param name="fun"></param>
        public void Start(Action fun)
        {
            this.BackThread = new Thread(new ThreadStart(fun));
            this.BackThread.IsBackground = true;
            this.BackThread.Start();
        }

        public void Parse()
        {
            this.BackThread.Suspend();
        }

        public void Continue()
        {
            if (BackThread.ThreadState == ThreadState.Suspended)
            {
                this.BackThread.Resume();
            }
        }

        /// <summary>
        /// 结束正在执行的方法，并执行指定的回调函数
        /// </summary>
        /// <param name="callback"></param>
        public void Stop(Action callback = null)
        {
            if (this.BackThread == null || !this.BackThread.IsAlive) return;
            try
            {
                if (callback != null) callback();

                if (this.BackThread.ThreadState == ThreadState.Suspended)
                {
                    this.BackThread.Resume();
                }

                this.BackThread.Abort();
            }
            catch (ThreadAbortException)
            {
                //do nothing
            }
            finally
            {
                this.BackThread.DisableComObjectEagerCleanup();
            }
        }

        /// <summary>
        /// Returns true if the thread is a threadpool thread.
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return BackThread != null && BackThread.IsAlive;
            }
        }

    }
}
