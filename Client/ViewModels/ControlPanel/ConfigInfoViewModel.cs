using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.Utils;
using TianYiSdtdServerTools.Client.Models.ObservableClasses;
using TianYiSdtdServerTools.Client.Models.SdtdServerInfo;
using TianYiSdtdServerTools.Client.Services.Primitives.UI;
using TianYiSdtdServerTools.Client.TelnetClient;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.ControlPanel
{
    public class ConfigInfoViewModel : ViewModelBase
    {
        #region 字段
        private RelayCommand _connectServer;

        private RelayCommand _disconnectServer;
        //private readonly PropertyObserver<SdtdServerPrefModel> _sdtdServerPrefObserver;
        #endregion

        #region 属性
        [ConfigNode(ConfigNodeType.Element)]
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
        public ConfigInfoViewModel(IDispatcherService dispatcherService, IDialogService dialogService) : base(dispatcherService, dialogService)
        {
            SdtdConsole.Instance.ConnectionStateChanged += (connectionState) => { SdtdServerStates.ConnectionState = connectionState; };
            SdtdConsole.Instance.ReceivedServerPartialPref += (serverPartialPref) => { SdtdServerPrefs.ServerPartialPref = serverPartialPref; };
            SdtdConsole.Instance.ReceivedServerPartialState += (serverPartialState) => { SdtdServerStates.ServerPartialState = serverPartialState; };

            //var _sdtdServerPrefObserver = new PropertyObserver<SdtdServerPrefModel>(SdtdServerPrefs);
            //_sdtdServerPrefObserver.RegisterHandler(SdtdServerPref => SdtdServerPref.TelnetPassword,
            //    (sdtdServerPrefs) =>
            //    {
            //        SdtdConsole.Instance.Password = sdtdServerPrefs.TelnetPassword;
            //    });

            base.dispatcherService.ShutdownStarted += OnDispatcherService_ShutdownStarted;
        }

        private void OnDispatcherService_ShutdownStarted(object sender, EventArgs e)
        {
            SdtdConsole.Instance.Disconnect();
        }

        #endregion
    }
}
