using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Services.CatchException
{
    internal interface IExceptionCaughtSignal
    {
        /// <summary>
        /// 发射异常捕获信号
        /// </summary>
        /// <param name="e"></param>
        void EmitAsyncExceptionCaughtSignal(object sender, ServiceException e);

        /// <summary>
        /// 是否自动处理异步服务层异常
        /// </summary>
        bool IsAutoHandleAsyncServiceException { get; set; }
    }
}
