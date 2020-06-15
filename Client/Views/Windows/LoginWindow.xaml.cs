using AutoUpdaterDotNET;
using IceCoffee.Common.LogManager;
using MiniBlinkPinvoke;
using Panuon.UI.Silver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TianYiSdtdServerTools.Client.MyClient;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Shared;

namespace TianYiSdtdServerTools.Client.Views.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Panuon.UI.Silver.WindowX
    {
        private BlinkBrowser blinkBrowser;

        public LoginWindow()
        {
            InitMyTcpClient();

            InitializeComponent();

            InitBlinkBrowser();
        }
        
        private void InitMyTcpClient()
        {            
            TcpClient.Instance.LoginSucceed += OnLoginSucceed;
            TcpClient.Instance.ReceivedAutoUpdaterConfig += OnReceivedAutoUpdaterConfig;
            TcpClient.Instance.ReconnectDefeated += OnReconnectDefeated;
            TcpClient.Instance.Connect(Config.IP, Config.Port);
        }

        private static void OnReconnectDefeated()
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                MessageBoxX.Show("连接工具服务端失败，请重新登陆", "提示");
                Environment.Exit(-1);
            });
        }

        private static void OnReceivedAutoUpdaterConfig(Shared.Models.NetDataObjects.AutoUpdaterConfig obj)
        {
            Application.Current.Dispatcher.InvokeAsync(() => 
            {
                AutoUpdater.ReportErrors = true;
                AutoUpdater.Mandatory = true;
                AutoUpdater.UpdateMode = Mode.Forced;
                AutoUpdater.Start(obj.XmlUrl);
            });
        }


        private void OnLoginSucceed()
        {
            Log.Info("登录成功");
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                MainWindow mainWindow = new MainWindow();

                Application.Current.MainWindow = mainWindow;

                this.Close();
                mainWindow.Show();
            });
        }

        private void InitBlinkBrowser()
        {
            // 实例化基于 WinForm 封装的 Miniblink浏览器
            blinkBrowser = new BlinkBrowser
            {
                Url = string.Format("{0}?clientToken={1}&version={2}", 
                    Config.LoginUrl, TcpClient.Instance.ClientToken,VersionManager.CurrentVersion.ToString())
            };
            blinkBrowser.OnCreateViewEvent += BlinkBrowser_OnCreateViewEvent;

            // 允许在 WPF 页面上承载 Windows 窗体控件的元素。
            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost
            {
                Child = blinkBrowser
            };
            
            this.grid1.Children.Add(host);
        }

        private IntPtr BlinkBrowser_OnCreateViewEvent(IntPtr webView, IntPtr param, wkeNavigationType navigationType, string url)
        {
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                {
                    Autofac.Resolve<IDialogService>().ShowInformation(noBrowser.Message);
                }
            }
            catch (Exception other)
            {
                Autofac.Resolve<IDialogService>().ShowInformation(other.Message);
            }
            return webView;
        }
    }
}
