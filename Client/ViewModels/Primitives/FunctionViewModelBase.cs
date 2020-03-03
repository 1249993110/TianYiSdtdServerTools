using IceCoffee.Wpf.MvvmFrame.Messaging;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using IceCoffee.Wpf.MvvmFrame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TianYiSdtdServerTools.Client.Models.MvvmMessages;
using TianYiSdtdServerTools.Client.Models.SdtdServerInfo;
using TianYiSdtdServerTools.Client.Services.Primitives.UI;
using TianYiSdtdServerTools.Client.TelnetClient;

namespace TianYiSdtdServerTools.Client.ViewModels.Primitives
{
    public abstract class FunctionViewModelBase : ViewModelBase
    {
        /// <summary>
        /// 功能开关是否打开
        /// </summary>
        private bool _isOpen;

        /// <summary>
        /// 功能是否已被禁用，防止重复禁用
        /// </summary>
        private bool _isDisabled = true;

        private readonly string _functionTag;

        /// <summary>
        /// 功能开关是否打开
        /// </summary>
        public bool IsOpen { get { return _isOpen; } }

        /// <summary>
        /// 功能标记
        /// </summary>
        public string FunctionTag { get { return _functionTag; } }


        public FunctionViewModelBase(IDispatcherService dispatcherService, string functionTag) : base(dispatcherService)
        {
            _functionTag = functionTag;

            SdtdConsole.Instance.ServerHavePlayerAgain += PrivateEnableFunction;
            SdtdConsole.Instance.ServerNonePlayer += PrivateDisableFunction;

            SdtdConsole.Instance.ConnectionStateChanged += OnConnectionStateChanged;

            Messenger.Default.Register<FunctionEnableChangedMessage>(this, OnFunctionEnableChanged);
        }

        private void OnConnectionStateChanged(ConnectionState connectionState)
        {
            if (connectionState == ConnectionState.Connected)
            {
                PrivateEnableFunction();
            }
            else if (connectionState == ConnectionState.Disconnected)
            {
                PrivateDisableFunction();
            }
        }

        private void OnFunctionEnableChanged(FunctionEnableChangedMessage message)
        {
            if (message.FunctionTag == FunctionTag)
            {
                _isOpen = message.IsOpen;
                if (_isOpen)
                {
                    PrivateEnableFunction();
                }
                else
                {
                    PrivateDisableFunction();
                }
            }
        }

        private void PrivateEnableFunction()
        {
            // 如果功能开关已打开 且 没有被禁用 且 已成功连接服务器 且 在线玩家数量大于0
            if (_isOpen && _isDisabled && SdtdConsole.Instance.IsConnected
                && SdtdConsole.Instance.OnlinePlayers != null && SdtdConsole.Instance.OnlinePlayers.Count > 0)
            {
                _isDisabled = false;
                EnableFunction();
            }
        }
        private void PrivateDisableFunction()
        {
            // 如果功能没有被禁用
            if (_isDisabled == false)
            {
                _isDisabled = true;
                DisableFunction();
            }
        }
        /// <summary>
        /// 启用功能
        /// </summary>
        protected abstract void EnableFunction();

        /// <summary>
        /// 禁用功能
        /// </summary>
        protected abstract void DisableFunction();
    }
}
