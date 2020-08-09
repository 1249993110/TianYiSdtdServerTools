using AutoUpdaterDotNET;
using IceCoffee.Common.LogManager;
using Panuon.UI.Silver;
using System;
using System.Windows;
using TianYiSdtdServerTools.Client.MyClient;
using TianYiSdtdServerTools.Shared;

namespace TianYiSdtdServerTools.Client.Views.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Panuon.UI.Silver.WindowX
    {
        public LoginWindow()
        {
            InitMyTcpClient();

            InitializeComponent();
        }
        
        private void InitMyTcpClient()
        {            
            TcpClient.Instance.LoginSucceed += OnLoginSucceed;
            TcpClient.Instance.ReceivedAutoUpdaterConfig += OnReceivedAutoUpdaterConfig;
            TcpClient.Instance.ReconnectDefeated += OnReconnectDefeated;
            TcpClient.Instance.Connect(SocketConfig.IP, SocketConfig.Port);
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
    }
}
