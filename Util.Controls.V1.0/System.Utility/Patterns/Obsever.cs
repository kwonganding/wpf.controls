using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Utility.Patterns
{
    #region AbstractObseverable：观察者模式-主体抽象基类
    /// <summary>
    /// 观察者模式-主体抽象基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractObseverable
    {
        /// <summary>
        /// 观察者列表
        /// </summary>
        protected List<IObsever> ObSevers = new List<IObsever>();

        /// <summary>
        /// 注册观察者
        /// </summary>
        /// <param name="obsever"></param>
        public void Register(IObsever obsever)
        {
            this.ObSevers.Add(obsever);
        }

        /// <summary>
        /// 注销观察者的注册
        /// </summary>
        /// <param name="obsever"></param>
        public void UnRegister(IObsever obsever)
        {
            this.ObSevers.Remove(obsever);
        }
    } 
    #endregion

    #region IObsever： 观察者模式-观察者抽象接口
    /// <summary>
    /// 观察者模式-观察者抽象接口
    /// </summary>
    public interface IObsever
    {
        /// <summary>
        /// 响应观察者主体的事件
        /// </summary>
        void Response();
    } 
    #endregion
}
