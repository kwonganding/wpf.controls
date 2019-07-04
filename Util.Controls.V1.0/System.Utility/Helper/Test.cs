#region 引用命名空间
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
#endregion

namespace System.Utility.Helper
{
    /// <summary>
    /// 测试公共操作类，主要提供运行时间、----托管内存的测量
    /// </summary>
    public class Test
    {
        #region 字段定义
        /// <summary>
        /// 测试运行时间
        /// </summary>
        private Stopwatch _watch;
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化
        /// </summary>
        public Test()
        {
            //创建Stopwatch实例
            _watch = new Stopwatch();
        }
        #endregion

        #region 开始计时
        /// <summary>
        /// 开启计时器,开始计算运行时间
        /// </summary>
        public void Start()
        {
            _watch.Reset();
            //开始计时
            _watch.Start();
        }
        #endregion

        #region 重置计时器
        /// <summary>
        /// 重置计时器
        /// </summary>
        public void Reset()
        {
            //重置计时器
            _watch.Reset();
        }
        #endregion

        #region 获取运行的时间间隔
        /// <summary>
        /// 获取运行的时间间隔
        /// </summary>
        public TimeSpan GetElapsed()
        {
            //返回时间间隔
            return _watch.Elapsed;
        }
        #endregion

        #region 获取运行的时间间隔,同时停止计时
        /// <summary>
        /// 获取运行的时间间隔,同时停止计时
        /// </summary>
        public TimeSpan GetElapsedAndStop()
        {
            //停止计时
            _watch.Stop();

            //返回时间间隔
            return _watch.Elapsed;
        }
        #endregion

        #region 停止计时
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _watch.Stop();
        }
        #endregion

        #region /*****************static TimeSpan RunTest 运行测试**********************/

        /// <summary>
        /// 运行测试，返回测试方法执行的时间间隔，单线程中使用
        /// </summary>
        public static TimeSpan RunTest(Action callBack)
        {
            Stopwatch sw = new Stopwatch();
            //start
            sw.Start();
            //execute
            callBack();
            //stop
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            return ts;
        } 
        #endregion
    }
}
