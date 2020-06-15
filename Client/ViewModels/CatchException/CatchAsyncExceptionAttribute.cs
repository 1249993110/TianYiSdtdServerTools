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
    /// 捕获异步方法异常，只能在UI线程使用
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    sealed class CatchAsyncExceptionAttribute : OnMethodBoundaryAspect
    {
        //private static Dictionary<Type, List<PropertyInfo>> propertyInfosDict = new Dictionary<Type, List<PropertyInfo>>();

        //public override void OnEntry(MethodExecutionArgs args)
        //{
        //    Type type = args.Instance.GetType();
        //    if (propertyInfosDict.ContainsKey(type) == false)
        //    {
        //        var propertyInfos = new List<PropertyInfo>();
        //        propertyInfosDict[type] = propertyInfos;

        //        var fields = type.GetFields().Where(p => p.GetType().IsSubclassOf(typeof(MyServiceBase<>)));

        //        foreach (var item in fields)
        //        {
        //            propertyInfos.Add(item.GetType().GetProperty("IsAutoHandleAsyncServiceException"));
        //        }
        //    }

        //    propertyInfosDict[type].ForEach(p => p.SetValue(args.Instance, false));
        //}

        //public override void OnExit(MethodExecutionArgs args)
        //{
        //    propertyInfosDict[args.Instance.GetType()].ForEach(p => p.SetValue(args.Instance, true));
        //}

        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.Return;
            args.Instance.GetType().GetMethod("OnAsyncExceptionCaught").Invoke(args.Instance, new object[] { args.Instance, args.Exception });
        }

    }
}
