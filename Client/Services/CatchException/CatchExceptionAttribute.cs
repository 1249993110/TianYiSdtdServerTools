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
        }

        public override void OnException(MethodExecutionArgs args)
        {
            Exception e = args.Exception;
            args.FlowBehavior = FlowBehavior.Return;

            if(args.Instance is IExceptionCaughtSignal instance)
            {
                instance.EmitExceptionCaughtSignal(instance, new DALException(Error, e));
            }
        }

        public override Type GetExceptionType(MethodBase targetMethod)
        {
            return typeof(DbCoreException);
        }
    }
}
