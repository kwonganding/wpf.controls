using System.Utility.Logger;

namespace System.Utility
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
            Threading.ThreadPool.QueueUserWorkItem((obj) =>
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

        #region TryRunByThread
        /// <summary>
        /// 创建线程来执行某个方法（无返回值）
        /// </summary>
        public static void TryRunByThread(Action fun, bool skipException = true)
        {
            System.Threading.ThreadStart start = new Threading.ThreadStart(fun);
            System.Threading.Thread thread = new Threading.Thread(start);
            thread.IsBackground = true;
            if (skipException)
            {
                try
                {
                    thread.Start();
                }
                catch { }
            }
            else
            {
                thread.Start();
            }
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
                catch(Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
            }
            else
            {
                fun();
            }
        }
        #endregion

        #region TryRunLogExceptioin
        /// <summary>
        /// 执行指定代码，若发生异常，则记录异常日志
        /// </summary>
        public static void TryRunLogExceptioin(Action fun, string message = "")
        {
            try
            {
                fun();
            }
            catch (Exception ex)
            {
                string mes = string.Format("{0}，{1}错误详情：{2}", message, Environment.NewLine, ex.AllMessage());
                LogHelper.Error(message, ex);
            }
        }
        #endregion

        #region TryRunThrowExceptioin
        /// <summary>
        /// 执行指定代码，若发生异常，包装异常并抛出
        /// </summary>
        public static void TryRunThrowExceptioin(Action fun, string message = "")

        {
            try
            {
                fun();
            }
            catch (Exception ex)
            {
                string mes = string.Format("{0}，请尝试重新运行/重启程序，或者联系技术支持。{1}错误详情：{2}", message, Environment.NewLine, ex.AllMessage());
                throw new ApplicationException(mes, ex);
            }
        }
        #endregion
    }
}
