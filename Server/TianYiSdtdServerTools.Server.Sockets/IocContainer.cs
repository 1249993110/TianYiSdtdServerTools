using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Autofac.Core;

namespace TianYiSdtdServerTools.Server.Sockets
{
    /// <summary>
    /// IocContainer
    /// </summary>
    static class IocContainer
    {
        private static readonly ContainerBuilder _builder;

        private static readonly IContainer _container;

        static IocContainer()
        {
            _builder = new ContainerBuilder();

            var assemblys = AppDomain.CurrentDomain.GetAssemblies();

            // 在程序集中自动查找类型
            _builder.RegisterAssemblyTypes(Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "LuoShuiTianYi.Sdtd.Services.dll")).AsImplementedInterfaces().SingleInstance();

            _builder.RegisterAssemblyTypes(assemblys
                .FirstOrDefault(x => x.FullName.StartsWith("TianYiSdtdServerTools.Server.Sockets")))
                .Where(t => t.Name.EndsWith("Handler"));

            
            //_builder.Register(c => new ContainerBuilder());//.SingleInstance();
            //_builder.RegisterType<JT809Serializer>()
            //    .WithParameter(new TypedParameter(typeof(IJT809Config), new JT809_2011_Config()));

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
