using System;
using System.Threading;

/// <summary>
/// GUI线程同步辅助操作类。需要主GUI界面需要初始化GUISyncContext（GUI的线程上下文）
/// 示例：GUIThreadHelper.GUISyncContext = System.Threading.SynchronizationContext.Current;
/// </summary>
public static class GUIThreadHelper
{
    public static SynchronizationContext GUISyncContext
    {
        get { return _GUISyncContext; }
        set { _GUISyncContext = value; }
    }

    private static SynchronizationContext _GUISyncContext = SynchronizationContext.Current;

    /// <summary>
    /// 主要用于GUI线程的同步回调-同步调度
    /// </summary>
    /// <param name="callback"></param>
    public static void Invoke(Action callback)
    {
        if (callback == null) return;
        if (GUISyncContext == null)
        {
            callback();
            return;
        }
        GUISyncContext.Send(result =>
        {
            callback();
        }, null);
    }
    /// <summary>
    /// 主要用于GUI线程的同步回调-异步调度
    /// </summary>
    public static void BeginInvoke(Action callback)
    {
        if (callback == null) return;
        if (GUISyncContext == null)
        {
            callback();
            return;
        }
        GUISyncContext.Post(result =>
        {
            callback();
        }, null);
    }

    /// <summary>
    /// 支持APM异步编程模型的GUI线程的同步回调
    /// </summary>
    public static AsyncCallback Invoke(AsyncCallback callback)
    {
        if (callback == null) return callback;
        if (GUISyncContext == null) return callback;
        return asynresult => GUISyncContext.Post(result => callback(result as IAsyncResult), asynresult);
    }
}

