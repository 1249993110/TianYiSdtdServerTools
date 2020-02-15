using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.Wpf.MvvmFrame;
using TianYiSdtdServerTools.Client.Models.ObservableClasses;
using TianYiSdtdServerTools.Client.TelnetClient;

namespace TianYiSdtdServerTools.Client.ViewModels.ControlPanel
{
    public class ConfigInfoViewModel
    {
        #region 字段
        private RelayCommand _connectServer;

        private readonly PropertyObserver<SdtdServerPrefModel> _sdtdServerPrefObserver;
        #endregion

        #region 属性
        public SdtdServerPrefModel SdtdServerPrefs { get; set; } = new SdtdServerPrefModel();

        public SdtdServerStateModel SdtdServerStates { get; set; } = new SdtdServerStateModel();

        public RelayCommand ConnectServer
        {
            get { return _connectServer ?? (_connectServer = new RelayCommand(PrivateConnectServer,
                () => { return SdtdConsole.Instance.ConnectionState != ConnectionState.Connecting; })); }
        }
        #endregion
        public ConfigInfoViewModel()
        {            
            SdtdServerStates.ConnectStateStr = "未连接";
            SdtdConsole.Instance.ConnectionStateChanged += OnConnectionStateChanged;

            _sdtdServerPrefObserver = new PropertyObserver<SdtdServerPrefModel>(SdtdServerPrefs);
            _sdtdServerPrefObserver.RegisterHandler(SdtdServerPref => SdtdServerPref.TelnetPassword,
                (sdtdServerPrefs) => {
                    SdtdConsole.Instance.Password = sdtdServerPrefs.TelnetPassword;
                });
        }

        private void PrivateConnectServer()
        {
            if (SdtdServerPrefs.TelnetPort.HasValue)
            {
                SdtdConsole.Instance.ConnectServer(SdtdServerPrefs.ServerIP, SdtdServerPrefs.TelnetPort.Value);
            }            
        }

        private void OnConnectionStateChanged(ConnectionState connectionState)
        {
            switch (connectionState)
            {
                case ConnectionState.AutoReconnecting:
                    SdtdServerStates.ConnectStateStr = "正在自动重连";
                    break;
                case ConnectionState.Connected:
                    SdtdServerStates.ConnectStateStr = "已连接";
                    break;
                case ConnectionState.Connecting:
                    SdtdServerStates.ConnectStateStr = "正在连接";
                    break;
                case ConnectionState.Disconnected:
                    SdtdServerStates.ConnectStateStr = "未连接";
                    break;
                case ConnectionState.Disconnecting:
                    SdtdServerStates.ConnectStateStr = "正在断开";
                    break;
                case ConnectionState.PasswordVerifying:
                    SdtdServerStates.ConnectStateStr = "正在验证密码";
                    break;
                case ConnectionState.PasswordIncorrect:
                    SdtdServerStates.ConnectStateStr = "密码错误";
                    break;
            }
        }
    }
}
