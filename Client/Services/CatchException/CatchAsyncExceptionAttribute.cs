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
    /// 捕获异步方法异常
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    sealed class CatchAsyncExceptionAttribute : OnExceptionAspect
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }

        public CatchAsyncExceptionAttribute(string error)
        {
            Error = error;

            // 确定应用于迭代器或异步方法（编译为状态机）时方面的行为方式
            // 从PostSharp 5.0开始，FlowBehavior也适用于异步方法，ApplyToStateMachine默认为true
            // ApplyToStateMachine = true;
        }

        public override void OnException(MethodExecutionArgs args)
        {          
            if (args.Instance is IExceptionCaughtSignal instance)
            {
                if(instance.IsAutoHandleAsyncServiceException)
                {
                    args.FlowBehavior = FlowBehavior.Return;
                    instance.EmitAsyncExceptionCaughtSignal(instance, new ServiceException(Error, args.Exception));
                }
                else
                {
                    args.FlowBehavior = FlowBehavior.RethrowException;
                }                
            }
        }

        public override Type GetExceptionType(MethodBase targetMethod)
        {
            return typeof(DbCoreException);
        }
        
    }
}
