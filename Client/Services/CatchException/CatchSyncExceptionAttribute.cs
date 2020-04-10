using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.DbCore;
using IceCoffee.DbCore.CatchException;
using PostSharp.Aspects;

namespace TianYiSdtdServerTools.Client.Services.CatchException
{
    /// <summary>
    /// 捕获同步方法异常
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    sealed class CatchSyncExceptionAttribute : OnExceptionAspect
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }

        public CatchSyncExceptionAttribute(string error)
        {
            Error = error;
        }

        public override void OnException(MethodExecutionArgs args)
        {
            throw new ServiceException(Error, args.Exception);
        }

        public override Type GetExceptionType(MethodBase targetMethod)
        {
            return typeof(DbCoreException);
        }

    }
}
