using IceCoffee.Common.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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

            //当前应用程序域未处理异常处理事件
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomain_UnhandledException;

            InitConfig();

            base.OnStartup(e);
        }

        #region 捕获未处理异常
        private void OnTaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Log.Error("Task线程内未处理异常", e.Exception);
        }

        private void OnApp_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error("当前应用程序域未处理异常", e.Exception);
        }

        private void OnCurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error("UI线程未处理异常", (Exception)e.ExceptionObject);
        }
        #endregion

        /// <summary>
        /// 初始化配置
        /// </summary>
        private static void InitConfig()
        {
            foreach (string directory in ConfigurationManager.AppSettings.Keys)
            {
                if (directory.EndsWith("Directory"))
                {
                    if (Directory.Exists(directory) == false)
                    {
                        Directory.CreateDirectory(ConfigurationManager.AppSettings[directory]);
                    }
                }
            }
        }
    }
    
}
