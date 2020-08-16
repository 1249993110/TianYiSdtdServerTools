using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;
using TianYiSdtdServerTools.Client.Views.Services;

namespace TianYiSdtdServerTools.Client.Views
{
    /// <summary>
    /// IocContainer
    /// </summary>
    static class Autofac
    {
        private static ContainerBuilder _builder;

        private static IContainer _container;

        static Autofac()
        {
            _builder = new ContainerBuilder();

            var assemblys = AppDomain.CurrentDomain.GetAssemblies();
            
            

            // 在程序集 ViewModels.dll 中自动查找类型
            _builder.RegisterAssemblyTypes(assemblys.FirstOrDefault(x => x.FullName.StartsWith("TianYiSdtdServerTools.Client.ViewModels")));

            var assembly = assemblys.FirstOrDefault(x => x.FullName.StartsWith("TianYiSdtdServerTools.Client.Services"));

            var types = assembly.GetExportedTypes();

            // 在程序集 Services.dll 中自动查找类型
            _builder.RegisterAssemblyTypes(assembly).SingleInstance();

            //_builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            //_builder.RegisterType<DispatcherService>().As<IDispatcherService>().SingleInstance();

            _builder.RegisterAssemblyTypes(typeof(Autofac).Assembly)
                .Where(p => p.FullName.StartsWith("TianYiSdtdServerTools.Client.Views.Services"))
                .AsImplementedInterfaces().SingleInstance();

            _container = _builder.Build();
        }

        /// <summary>
        /// 从上下文中检索服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        /// <summary>
        /// 从上下文中检索服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T Resolve<T>(params Parameter[] parameters)
        {
            return _container.Resolve<T>(parameters);
        }
    }
}
