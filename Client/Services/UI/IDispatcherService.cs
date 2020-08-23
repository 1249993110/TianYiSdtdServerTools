using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Services.UI
{
    public interface IDispatcherService
    {
        /// <summary>
        /// 使用postMessage向windows消息循环发送消息结束程序
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 在与 ui线程调度者 关联的线程上同步执行指定的Action。
        /// </summary>
        /// <param name="action"></param>
        void Invoke(Action action);

        /// <summary>
        /// 在与 ui线程调度者 关联的线程上同步执行指定的Action。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="dispatcherPriority"></param>
        void Invoke(Action action, DispatcherPriority dispatcherPriority);

        /// <summary>
        /// 在与 ui线程调度者 关联的线程上异步执行指定的Action。
        /// </summary>
        /// <param name="action"></param>
        void InvokeAsync(Action action);

        /// <summary>
        /// 在与 ui线程调度者 关联的线程上异步执行指定的Action。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="dispatcherPriority"></param>
        void InvokeAsync(Action action, DispatcherPriority dispatcherPriority);

        /// <summary>
        /// 当 ui线程调度者 开始关闭时发生。
        /// </summary>
        event EventHandler ShutdownStarted;
    }
}
