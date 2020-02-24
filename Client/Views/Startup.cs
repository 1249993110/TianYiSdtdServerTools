using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IceCoffee.Common.AntiDebug;
using IceCoffee.Common;

namespace TianYiSdtdServerTools.Client.Views
{
    public class Startup
    {
        static int? X { get; set; }
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
            App app = new App();
            X = 1;
            var xx = typeof(int?);
            var t = X.GetType(); 
            app.InitializeComponent();            
            app.Run();
        }
    }
}
