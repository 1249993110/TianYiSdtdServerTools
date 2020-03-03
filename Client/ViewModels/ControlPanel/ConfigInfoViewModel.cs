using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.Messaging;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using IceCoffee.Wpf.MvvmFrame.Utils;
using TianYiSdtdServerTools.Client.Models.MvvmMessages;
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
        private PropertyObserver<FunctionSwitchModel> _functionSwitchModelObserver;
        #endregion

        #region 属性
        [ConfigNode(XmlNodeType.Element)]
        public SdtdServerPrefModel SdtdServerPrefs { get; set; } = new SdtdServerPrefModel();

        public SdtdServerStateModel SdtdServerStates { get; set; } = new SdtdServerStateModel();

        [ConfigNode(XmlNodeType.Attribute)]
        public int AutoReconnectMaxCount { get; [NPCA_Method]set; } = 10;

        [ConfigNode(XmlNodeType.Attribute)]
        public int AutoReconnectInterval { get; [NPCA_Method]set; } = 20;

        [ConfigNode(XmlNodeType.Element)]
        public FunctionSwitchModel FunctionSwitchs { get; set; } = new FunctionSwitchModel();
        #region 命令
        public RelayCommand ConnectServer { get; private set; }

        public RelayCommand DisconnectServer { get; private set; }
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
                    if(_functionSwitchModelObserver == null)
                    {
                        InitFunctionSwitchModelObserver();
                        Messenger.Default.Send(CommonEnumMessage.InitControlPanelView);
                    }                    

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

        private void InitFunctionSwitchModelObserver()
        {
            _functionSwitchModelObserver = new PropertyObserver<FunctionSwitchModel>(FunctionSwitchs);

            foreach (var propertyInfo in FunctionSwitchs.GetType().GetProperties())
            {
                string propertyName = propertyInfo.Name;

                _functionSwitchModelObserver.RegisterHandler(propertyName, (propertySource) =>
                {
                    SendFunctionEnableChangedMessage(propertyName, propertySource);
                });

                SendFunctionEnableChangedMessage(propertyName, FunctionSwitchs);                
            }

        }

        private void SendFunctionEnableChangedMessage(string propertyName, FunctionSwitchModel propertySource)
        {
            Messenger.Default.Send(new FunctionEnableChangedMessage()
            {
                FunctionTag = propertyName,
                IsOpen = (bool)propertySource.GetType().GetProperty(propertyName).GetValue(propertySource)
            });
        }
        #endregion
    }
}
