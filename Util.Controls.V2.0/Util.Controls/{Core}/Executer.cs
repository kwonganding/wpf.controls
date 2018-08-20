using System;

namespace Util.Controls
{
    /// <summary>
    /// 执行器
    /// </summary>
    public class Executer
    {
        #region TryRunByAsyn:异步执行某个方法(无返回值)

        /// <summary>
        /// 试着异步执行某个方法(无返回值)
        /// skipException是否跳过异常扑捉，默认容错处理(skipException = true)
        /// </summary>
        public static void TryRunByAsyn(Action fun, bool skipException = true)
        {
            Executer.TryRunSkipExceptoin(() =>
            {
                fun.BeginInvoke(null, null);
            }, skipException);
        }

        #endregion

        #region TryRunByThreadPool

        /// <summary>
        /// 使用线程池来执行某个方法（无返回值）
        /// </summary>
        public static void TryRunByThreadPool(Action fun, bool skipException = true)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((obj) =>
            {
                Executer.TryRunSkipExceptoin(fun, skipException);
            });
        }

        #endregion

        #region TryRunByTask

        /// <summary>
        /// 使用任务来执行某个方法（无返回值）
        /// </summary>
        public static void TryRunByTask(Action fun, bool skipException = true)
        {
            var task = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Executer.TryRunSkipExceptoin(fun, skipException);
            });
        }

        #endregion

        #region TryRunSkipExceptoin：以同步方式容错执行方法

        /// <summary>
        /// 以同步方式容错执行方法，默认容错处理(skipException = true)
        /// </summary>
        public static void TryRunSkipExceptoin(Action fun, bool skipException = true)
        {
            if (skipException)
            {
                try
                {
                    fun();
                }
                catch
                {
                    // ignored
                }
            }
            else
            {
                fun();
            }
        }

        #endregion
    }
}