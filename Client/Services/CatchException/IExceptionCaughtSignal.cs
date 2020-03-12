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
        void EmitExceptionCaughtSignal(object sender, DALException e);
    }
}
