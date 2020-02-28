using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
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
        //private readonly PropertyObserver<SdtdServerPrefModel> _sdtdServerPrefObserver;
        #endregion

        #region 属性
        [ConfigNode(ConfigNodeType.Element)]
        public SdtdServerPrefModel SdtdServerPrefs { get; set; } = new SdtdServerPrefModel();

        public SdtdServerStateModel SdtdServerStates { get; set; } = new SdtdServerStateModel();

        [ConfigNode(ConfigNodeType.Attribute)]
        public int AutoReconnectMaxCount { get; [NPCA_Method]set; } = 10;

        [ConfigNode(ConfigNodeType.Attribute)]
        public int AutoReconnectInterval { get; [NPCA_Method]set; } = 20;

        #region 命令
        public RelayCommand ConnectServer { get; set; }

        public RelayCommand DisconnectServer { get; set; }
        #endregion

        #endregion

        #region 构造方法
        public ConfigInfoViewModel(IDispatcherService dispatcherService, IDialogService dialogService) : base(dispatcherService)
        {
            SdtdConsole.Instance.ConnectionStateChanged += (connectionState) => { SdtdServerStates.ConnectionState = connectionState; };
            SdtdConsole.Instance.ReceivedServerPartialPref += (serverPartialPref) => { SdtdServerPrefs.ServerPartialPref = serverPartialPref; };
            SdtdConsole.Instance.ReceivedServerPartialState += (serverPartialState) => { SdtdServerStates.ServerPartialState = serverPartialState; };

            base.dispatcherService.ShutdownStarted += OnDispatcherService_ShutdownStarted;

            ConnectServer = new RelayCommand(() =>
            {
                if (SdtdServerPrefs.TelnetPort.HasValue)
                {
                    SdtdConsole.Instance.ConnectServer(
                        SdtdServerPrefs.ServerIP, 
                        SdtdServerPrefs.TelnetPort.Value, 
                        SdtdServerPrefs.TelnetPassword,
                        AutoReconnectMaxCount, AutoReconnectInterval);
                }
            }, () => { return SdtdConsole.Instance.ConnectionState != ConnectionState.Connecting; });

            DisconnectServer = new RelayCommand(() =>
            {
                SdtdConsole.Instance.Disconnect();
            });
        }

        private void OnDispatcherService_ShutdownStarted(object sender, EventArgs e)
        {
            SdtdConsole.Instance.Disconnect();
        }

        #endregion
    }
}
