using Autofac;
using AutoUpdaterDotNET;
using IceCoffee.Common.LogManager;
using IceCoffee.Wpf.MvvmFrame.Messaging;
using Panuon.UI.Silver;
using System;
using System.Windows;
using System.Windows.Input;
using TianYiSdtdServerTools.Client.Models.MvvmMessages;
using TianYiSdtdServerTools.Client.ViewModels.Windows;
using TianYiSdtdServerTools.Shared;
using TianYiSdtdServerTools.Shared.Models.NetDataObjects;

namespace TianYiSdtdServerTools.Client.Views.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : PanuonWindowXBase
    {
        public LoginWindowViewModel ViewModel { get; set; }
        public LoginWindow()
        {           
            InitializeComponent();

            Messenger.Default.Register<MyTcpClientMessage>(this, HandeMyTcpClientMessage);

            ViewModel = IocContainer.Resolve<LoginWindowViewModel>();
            base.DataContext = ViewModel;
        }

        private void HandeMyTcpClientMessage(MyTcpClientMessage msg)
        {
            switch (msg.MessageType)
            {
                case MyTcpClientMessageType.FirstLoginSucceed:
                    OnFirstLoginSucceed();
                    break;
            }
        }
        private void OnFirstLoginSucceed()
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                MainWindow mainWindow = new MainWindow();

                Application.Current.MainWindow = mainWindow;

                this.Close();
                mainWindow.Show();
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            Messenger.Default.Unregister(this);
            base.OnClosed(e);
        }
    }
}
