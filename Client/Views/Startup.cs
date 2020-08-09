using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IceCoffee.AntiDebugEngine;
using IceCoffee.Common;
using System.Configuration;
using System.IO;
using IceCoffee.Common.LogManager;

namespace TianYiSdtdServerTools.Client.Views
{
    public class Startup
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public static void Main()
        {
#if !DEBUG
            if (CommonHelper.GetMD5HashFromFile(AntiDebug.DllName) != AntiDebug.DllMD5 || AntiDebug.AntiDebug_DotNet())
            {
                //Environment.Exit(-1);// 强制退出，即使有其他的线程没有结束
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
#endif
            try
            {
                InitConfig();
            }
            catch (Exception ex)
            {
                Log.Error("初始化配置错误", ex);
            }

            App app = new App();
            app.InitializeComponent();
            app.Run();
        }


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
                        Directory.CreateDirectory(CommonHelper.GetAppSettings(directory));
                    }
                }
            }
            string viewModelConfigPath = CommonHelper.GetAppSettings("ViewModelConfigPath");
            if (File.Exists(viewModelConfigPath) == false)
            {
                File.Copy(CommonHelper.GetAppSettings("ViewModelConfigDefaultPath"), viewModelConfigPath);
            }

            Client.Services.DatabaseConfig.InitializeDatabase();
        }
    }
}
