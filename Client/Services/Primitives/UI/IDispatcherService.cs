using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Services.Primitives.UI
{
    public interface IDispatcherService
    {
        /// <summary>
        /// 在与 ui线程调度者 关联的线程上同步执行指定的Action。
        /// </summary>
        /// <param name="action"></param>
        void Invoke(Action action);

        /// <summary>
        /// 在与 ui线程调度者 关联的线程上异步执行指定的Action。
        /// </summary>
        /// <param name="action"></param>
        void InvokeAsync(Action action);

        /// <summary>
        /// 当 ui线程调度者 开始关闭时发生。
        /// </summary>
        event EventHandler ShutdownStarted;
    }
}
