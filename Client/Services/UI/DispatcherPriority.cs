using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Services.UI
{
    /// <summary>
    /// 描述可通过 System.Windows.Threading.Dispatcher 调用操作的优先级。
    /// </summary>
    public enum DispatcherPriority
    {
        /// <summary>
        /// 枚举值为 -1。这是一个无效的优先级。
        /// </summary>
        Invalid = -1,

        /// <summary>
        /// 枚举值是 0。不处理操作。
        /// </summary>
        Inactive = 0,

        /// <summary>
        /// 枚举值是 1。在系统空闲时处理操作。
        /// </summary>
        SystemIdle = 1,

        /// <summary>
        /// 枚举值是 2。在应用程序空闲时处理操作。
        /// </summary>
        ApplicationIdle = 2,

        /// <summary>
        /// 枚举值是 3。在完成后台操作后处理操作。
        /// </summary>
        ContextIdle = 3,

        /// <summary>
        /// 枚举值是 4。在完成所有其他非空闲操作后处理操作。
        /// </summary>
        Background = 4,

        /// <summary>
        /// 枚举值是 5。按与输入相同的优先级处理操作。
        /// </summary>
        Input = 5,

        /// <summary>
        /// 枚举值是 6。在布局和呈现已完成，即将按输入优先级处理项之前处理操作。具体来说，此值在引发 Loaded 事件时使用。
        /// </summary>
        Loaded = 6,

        /// <summary>
        /// 枚举值是 7。按与呈现相同的优先级处理操作。
        /// </summary>
        Render = 7,

        /// <summary>
        /// 枚举值是 8。按与数据绑定相同的优先级处理操作。
        /// </summary>
        DataBind = 8,

        /// <summary>
        /// 枚举值是 9。按正常优先级处理操作。这是典型的应用程序优先级。
        /// </summary>
        Normal = 9,

        /// <summary>
        /// 枚举值是 10。在其他异步操作之前处理操作。这是最高优先级。
        /// </summary>
        Send = 10
    }
}
