using IceCoffee.Common;
using IceCoffee.Common.Extensions;
using IceCoffee.Common.LogManager;
using IceCoffee.Wpf.MvvmFrame.Messaging;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TianYiSdtdServerTools.Client.Models.Chat;
using TianYiSdtdServerTools.Client.Models.MvvmMessages;
using TianYiSdtdServerTools.Client.Models.Players;
using TianYiSdtdServerTools.Client.Models.SdtdServerInfo;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.TelnetClient;

namespace TianYiSdtdServerTools.Client.ViewModels.Primitives
{
    public abstract class FunctionViewModelBase : ViewModelBase
    {
        private bool _isOpen;

        private bool _isDisabled = true;

        private readonly string _functionTag;

        private static readonly int _commandBufferMaxCount;

        /// <summary>
        /// 功能开关是否打开
        /// </summary>
        public bool IsOpen { get { return _isOpen; } }

        /// <summary>
        /// 功能是否已被禁用，防止重复禁用
        /// </summary>
        public bool IsDisabled { get { return _isDisabled; } }

        /// <summary>
        /// 功能标记
        /// </summary>
        public string FunctionTag { get { return _functionTag; } }

        private readonly Func<PlayerInfo, string, bool> _onPlayerChatHooked;

        private readonly static Dictionary<string, Func<PlayerInfo, string, bool>> commandBuffer;
        private readonly static List<Func<PlayerInfo, string, bool>> chatHookFuncs;

        static FunctionViewModelBase()
        {
            _commandBufferMaxCount = CommonHelper.GetAppSettings("CommandBufferMaxCount").ToInt();
            commandBuffer = new Dictionary<string, Func<PlayerInfo, string, bool>>();
            chatHookFuncs = new List<Func<PlayerInfo, string, bool>>();
            SdtdConsole.Instance.ChatHook += ChatHook;
        }

        public FunctionViewModelBase(IDispatcherService dispatcherService, string functionTag) : base(dispatcherService)
        {
            _functionTag = functionTag;

            SdtdConsole.Instance.ServerHavePlayerAgain += PrivateEnableFunction;
            SdtdConsole.Instance.ServerNonePlayer += PrivateDisableFunction;

            SdtdConsole.Instance.ConnectionStateChanged += OnConnectionStateChanged;

            Messenger.Default.Register<FunctionSwitchStateChangedMessage>(this, OnFunctionSwitchStateChanged);

            _onPlayerChatHooked = new Func<PlayerInfo, string, bool>(this.OnPlayerChatHooked);
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

        private void OnFunctionSwitchStateChanged(FunctionSwitchStateChangedMessage message)
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
            // 如果功能处于被禁用状态 且 功能开关已打开 且 已成功连接服务器 且 在线玩家数量大于0
            if (_isDisabled && _isOpen && SdtdConsole.Instance.IsConnected
                && SdtdConsole.Instance.OnlinePlayers != null && SdtdConsole.Instance.OnlinePlayers.Count > 0)
            {
                _isDisabled = false;
                EnableFunction();
            }
        }
        private void PrivateDisableFunction()
        {
            // 如果功能没有处于被禁用状态
            if (_isDisabled == false)
            {
                _isDisabled = true;
                DisableFunction();
            }
        }

        private static void ChatHook(ChatInfo chatInfo)
        {
            if (chatInfo.senderType != SenderType.Server && chatInfo.playerInfo != null)
            {
                if (commandBuffer.TryGetValue(chatInfo.message, out Func<PlayerInfo, string, bool> func)
                    && (func.Target as FunctionViewModelBase)._isDisabled == false)// 如果在命令缓冲区中 且 功能没有被禁用
                {                    
                    HandleChatMessage(func, chatInfo);
                    if (chatInfo.isHandled)// 如果命令被直接处理完成则返回
                    {
                        return;
                    }
                    else// 如果此命令已经被弃用
                    {
                        commandBuffer.Remove(chatInfo.message);
                    }
                }

                // 如果命令没有被直接处理
                foreach (var item in chatHookFuncs)
                {
                    HandleChatMessage(item, chatInfo);
                    if(chatInfo.isHandled)// 如果命令被其他模块接受
                    {                        
                        if(commandBuffer.Count > _commandBufferMaxCount)
                        {
                            Log.Info("清理命令缓存区");
                            commandBuffer.Clear();
                        }

                        commandBuffer.Add(chatInfo.message, item);
                        return;
                    }
                }                                
            }
        }

        private static void HandleChatMessage(Func<PlayerInfo, string, bool> func, ChatInfo chatInfo)
        {
            try
            {
                chatInfo.isHandled = func(chatInfo.playerInfo, chatInfo.message);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                SdtdConsole.Instance.SendMessageToPlayer(chatInfo.playerInfo.SteamID, "[FF0000]出现错误，请联系服务器管理员");
            }
        }


        /// <summary>
        /// 禁用功能，基类默认实现ChatHook
        /// </summary>
        protected virtual void DisableFunction()
        {
            chatHookFuncs.Remove(_onPlayerChatHooked);
        }

        /// <summary>
        /// 启用功能，基类默认实现ChatHook
        /// </summary>
        protected virtual void EnableFunction()
        {
            if(chatHookFuncs.Contains(_onPlayerChatHooked) == false)
            {
                chatHookFuncs.Add(_onPlayerChatHooked);
            }
        }

        /// <summary>
        /// 当捕获玩家聊天信息
        /// </summary>
        protected virtual bool OnPlayerChatHooked(PlayerInfo playerInfo, string message)
        {
            return false;
        }

        /// <summary>
        /// 可用变量，基类默认添加[玩家昵称]、[playerName]、[steamID]、[entityID]
        /// </summary>
        public List<string> AvailableVariables { get; private set; } = new List<string>()
        {
            "[玩家昵称]",
            "[playerName]",
            "[steamID]",
            "[entityID]",
        };

        protected virtual string FormatCmd(PlayerInfo playerInfo, string message, object otherParam = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                return string.Empty;
            }

            if(playerInfo == null)
            {
                return message;
            }

            return message.Replace("[玩家昵称]", playerInfo.PlayerName)
                .Replace("[playerName]", playerInfo.PlayerName)
                .Replace("[steamID]", playerInfo.SteamID)
                .Replace("[entityID]", playerInfo.EntityID.ToString());
        }
    }
}
