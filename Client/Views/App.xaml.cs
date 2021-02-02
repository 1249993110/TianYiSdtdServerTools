using IceCoffee.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.ViewModels.Managers;

namespace TianYiSdtdServerTools.Client.Views
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // 当前应用程序域未处理异常处理事件，非主线程未捕获异常处理事件(例如自己创建的一个子线程)
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomain_UnhandledException;
			
			// Task线程内未处理异常处理事件
            TaskScheduler.UnobservedTaskException += OnTaskScheduler_UnobservedTaskException;
			
			// UI线程未处理异常处理事件
            this.DispatcherUnhandledException += OnApp_DispatcherUnhandledException;

            //MyClientManager.Instance.RegisterService(
            //    IocContainer.Resolve<IDispatcherService>(),
            //    IocContainer.Resolve<IDialogService>());

            base.OnStartup(e);
        }

        #region 捕获未处理异常
        private static void OnCurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal((Exception)e.ExceptionObject, "当前应用程序域未处理异常");
        }

        private static void OnTaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Log.Fatal(e.Exception.InnerException, "Task线程内未处理异常");
            e.SetObserved();
        }
		
		private void OnApp_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Fatal(e.Exception, "UI线程未处理异常");
            e.Handled = true;
            MessageBox.Show(e.Exception.Message + Environment.NewLine + "详情请查看日志文件："+AppDomain.CurrentDomain.BaseDirectory+"logs\\current\\Fatal.txt", "提示");
        }
        #endregion

    }
    
}
