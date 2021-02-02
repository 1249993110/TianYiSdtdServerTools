using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.DbCore;
using PostSharp.Aspects;
using TianYiSdtdServerTools.Client.Services.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.CatchException
{
    /// <summary>
    /// 捕获异步方法异常，只能在UI线程使用，当在方法中调用多个service接口时使用此特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    sealed class CatchAsyncExceptionAttribute : OnMethodBoundaryAspect
    {
        public CatchAsyncExceptionAttribute()
        {

        }

        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.Return;
            args.Instance.GetType().GetMethod("OnAsyncExceptionCaught").Invoke(args.Instance, new object[] { args.Instance, args.Exception });
        }

    }
}
