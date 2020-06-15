using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IceCoffee.Common.Xml;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.Messaging;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;

using TianYiSdtdServerTools.Client.Models.MvvmMessages;
using TianYiSdtdServerTools.Client.Models.ObservableObjects;
using TianYiSdtdServerTools.Client.Models.SdtdServerInfo;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.TelnetClient;
using TianYiSdtdServerTools.Client.ViewModels.Managers;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.ControlPanel
{
    public class ConfigInfoViewModel : ViewModelBase
    {
        #region 字段
        private List<PropertyObserver<FunctionPanelViewItemModel>> _functionPanelViewItemModelObservers;
        #endregion

        #region 属性
        [ConfigNode(XmlNodeType.Element)]
        public SdtdServerPrefModel SdtdServerPrefs { get; set; } = new SdtdServerPrefModel();

        public SdtdServerStateModel SdtdServerStates { get; set; } = new SdtdServerStateModel();

        [ConfigNode(XmlNodeType.Attribute)]
        public int AutoReconnectMaxCount { get; set; } = 10;

        [ConfigNode(XmlNodeType.Attribute)]
        public int AutoReconnectInterval { get; set; } = 20;

        public List<FunctionPanelViewItemModel> FunctionPanelItems { get; set; }
        #endregion

        #region 命令
        public RelayCommand ConnectServer { get; private set; }

        public RelayCommand DisconnectServer { get; private set; }
        #endregion

        #region 构造方法
        public ConfigInfoViewModel(IDispatcherService dispatcherService) : base(dispatcherService)
        {
            SdtdConsole.Instance.ReceivedServerPartialPref += (serverPartialPref) => { SdtdServerPrefs.ServerPartialPref = serverPartialPref; };
            SdtdConsole.Instance.ConnectionStateChanged += (connectionState) => { SdtdServerStates.ConnectionState = connectionState; };            
            SdtdConsole.Instance.ReceivedServerPartialState += (serverPartialState) => { SdtdServerStates.ServerPartialState = serverPartialState; };
            SdtdConsole.Instance.GameDateTimeChanged += (gameDateTime) => { SdtdServerStates.GameDateTime = gameDateTime; };

            base._dispatcherService.ShutdownStarted += OnDispatcherService_ShutdownStarted;

            ConnectServer = new RelayCommand(() =>
            {
                if (_functionPanelViewItemModelObservers == null)
                {
                    Messenger.Default.Send(CommonEnumMessage.InitControlPanelView);
                    _dispatcherService.InvokeAsync(InitFunctionSwitchModelObservers, DispatcherPriority.ApplicationIdle);
                }

                SdtdServerInfoManager.Instance.SetServerInfo(
                    SdtdServerPrefs.ServerIP,
                    SdtdServerPrefs.TelnetPort,
                    SdtdServerPrefs.TelnetPassword,
                    SdtdServerPrefs.GPSPort);

                SdtdConsole.Instance.ConnectServer(
                    SdtdServerPrefs.ServerIP,
                    SdtdServerPrefs.TelnetPort.GetValueOrDefault(),
                    SdtdServerPrefs.TelnetPassword,
                    AutoReconnectMaxCount, AutoReconnectInterval);

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

        private void InitFunctionSwitchModelObservers()
        {
            _functionPanelViewItemModelObservers = new List<PropertyObserver<FunctionPanelViewItemModel>>();

            foreach (var item in FunctionPanelItems)
            {
                _functionPanelViewItemModelObservers.Add(new PropertyObserver<FunctionPanelViewItemModel>(item).
                    RegisterHandler(p => p.IsOpen, (propertySource) =>
                    {
                        SendFunctionEnableChangedMessage(propertySource.Tag, propertySource.IsOpen);
                    }));

                // 如果开关已经打开
                if (item.IsOpen)
                {
                    SendFunctionEnableChangedMessage(item.Tag, true);
                }
            }
        }

        private void SendFunctionEnableChangedMessage(string functionTag, bool isOpen)
        {
            Messenger.Default.Send(new FunctionSwitchStateChangedMessage()
            {
                FunctionTag = functionTag,
                IsOpen = isOpen
            });
        }

        protected override void OnLoadConfig(XmlDocument contextDoc, XmlNode baseNode)
        {
            FunctionPanelItems = ViewItemManager.Instance.FunctionPanelItems;

            baseNode = baseNode.GetSingleChildNode(contextDoc, nameof(FunctionPanelViewItemModel));
            foreach (var item in FunctionPanelItems)
            {
                XmlHelper.LoadConfig(item, baseNode.GetSingleChildNode(contextDoc, item.Tag));
            }
        }

        protected override void OnSaveConfig(XmlDocument contextDoc, XmlNode baseNode)
        {
            baseNode = baseNode.GetSingleChildNode(contextDoc, nameof(FunctionPanelViewItemModel));
            foreach (var item in FunctionPanelItems)
            {
                XmlHelper.SaveConfig(item, contextDoc, baseNode.GetSingleChildNode(contextDoc, item.Tag));
            }            
        }
        #endregion
    }
}
