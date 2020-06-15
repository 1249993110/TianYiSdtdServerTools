using IceCoffee.Common.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TianYiSdtdServerTools.Client.Services.UI;

namespace TianYiSdtdServerTools.Client.Views
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //UI线程未处理异常处理事件
            this.DispatcherUnhandledException += OnApp_DispatcherUnhandledException;

            //Task线程内未处理异常处理事件
            TaskScheduler.UnobservedTaskException += OnTaskScheduler_UnobservedTaskException;

            //当前应用程序域未处理异常处理事件，非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomain_UnhandledException;            

            base.OnStartup(e);
        }

        #region 捕获未处理异常

        private void OnApp_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Fatal("UI线程未处理异常", e.Exception);
            e.Handled = true;
            MessageBox.Show(e.Exception.Message + Environment.NewLine + "详情请查看日志文件：logs\\current\\Fatal.txt", "提示");
        }

        private void OnTaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Log.Fatal("Task线程内未处理异常", e.Exception.InnerException);
            e.SetObserved();
        }

        private void OnCurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal("当前应用程序域未处理异常", (Exception)e.ExceptionObject);
        }
        #endregion

    }
    
}
