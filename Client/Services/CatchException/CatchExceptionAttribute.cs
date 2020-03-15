using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.DbCore;
using IceCoffee.DbCore.CatchException;
using PostSharp.Aspects;

namespace TianYiSdtdServerTools.Client.Services.CatchException
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    sealed class CatchExceptionAttribute : OnExceptionAspect
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }

        public CatchExceptionAttribute(string error)
        {
            Error = error;

            // 确定应用于迭代器或异步方法（编译为状态机）时方面的行为方式
            // 从PostSharp 5.0开始，FlowBehavior也适用于异步方法，ApplyToStateMachine默认为true
            // ApplyToStateMachine = true;
        }

        public override void OnException(MethodExecutionArgs args)
        {            
            args.FlowBehavior = FlowBehavior.Return;

            if(args.Instance is IExceptionCaughtSignal instance)
            {
                instance.EmitExceptionCaughtSignal(instance, new ServiceException(Error, args.Exception));
            }
        }

        public override Type GetExceptionType(MethodBase targetMethod)
        {
            return typeof(DbCoreException);
        }
        
    }
}
