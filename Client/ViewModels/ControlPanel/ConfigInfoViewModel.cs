using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.Wpf.MvvmFrame;
using TianYiSdtdServerTools.Client.Models.ObservableClasses;
using TianYiSdtdServerTools.Client.Models.SdtdServerInfo;
using TianYiSdtdServerTools.Client.TelnetClient;

namespace TianYiSdtdServerTools.Client.ViewModels.ControlPanel
{
    public class ConfigInfoViewModel
    {
        #region 字段
        private RelayCommand _connectServer;

        private RelayCommand _disconnectServer;
        //private readonly PropertyObserver<SdtdServerPrefModel> _sdtdServerPrefObserver;
        #endregion

        #region 属性
        public SdtdServerPrefModel SdtdServerPrefs { get; set; } = new SdtdServerPrefModel();

        public SdtdServerStateModel SdtdServerStates { get; set; } = new SdtdServerStateModel();

        #region 命令
        public RelayCommand ConnectServer
        {
            get
            {
                return _connectServer ?? (_connectServer = new RelayCommand(() =>
                {
                    if (SdtdServerPrefs.TelnetPort.HasValue)
                    {
                        SdtdConsole.Instance.ConnectServer(SdtdServerPrefs.ServerIP, SdtdServerPrefs.TelnetPort.Value, SdtdServerPrefs.TelnetPassword);
                    }
                }, () => { return SdtdConsole.Instance.ConnectionState != ConnectionState.Connecting; }));
            }
        }

        public RelayCommand DisconnectServer
        {
            get
            {
                return _disconnectServer ?? (_disconnectServer = new RelayCommand(() =>
                {
                    SdtdConsole.Instance.Disconnect();
                }));
            }
        }
        #endregion

        #endregion

        #region 构造方法
        public ConfigInfoViewModel()
        {           
            SdtdConsole.Instance.ConnectionStateChanged += OnConnectionStateChanged;
            SdtdServerPrefs.ServerIP = "127.0.0.1";
            SdtdServerPrefs.TelnetPort = 8081;
            SdtdServerPrefs.TelnetPassword = "12345";
            //_sdtdServerPrefObserver = new PropertyObserver<SdtdServerPrefModel>(SdtdServerPrefs);
            //_sdtdServerPrefObserver.RegisterHandler(SdtdServerPref => SdtdServerPref.TelnetPassword,
            //    (sdtdServerPrefs) => {
            //        SdtdConsole.Instance.Password = sdtdServerPrefs.TelnetPassword;
            //    });
        }
        #endregion

        #region 私有方法
        private void OnConnectionStateChanged(ConnectionState connectionState)
        {
            this.SdtdServerStates.ConnectionState = connectionState;
        }
        #endregion
    }
}
