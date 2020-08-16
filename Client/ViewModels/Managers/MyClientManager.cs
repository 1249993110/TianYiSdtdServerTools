using IceCoffee.Common;
using IceCoffee.Common.Extensions;
using IceCoffee.Common.LogManager;
using IceCoffee.Network.CatchException;
using IceCoffee.Wpf.MvvmFrame.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.MvvmMessages;
using TianYiSdtdServerTools.Client.MyClient;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Shared;
using TianYiSdtdServerTools.Shared.Models;
using TianYiSdtdServerTools.Shared.Models.NetDataObjects;

namespace TianYiSdtdServerTools.Client.ViewModels.Managers
{
    public sealed class MyClientManager : Singleton3<MyClientManager>
    {
        private readonly TcpClient _tcpClient;

        private IDispatcherService _dispatcherService;

        private IDialogService _dialogService;

        private UserInfo _userInfo;

        public bool IsConnected => _tcpClient.IsConnected;

        public UserInfo UserInfo => _userInfo;

        private MyClientManager()
        {
            _tcpClient = new TcpClient();
            _tcpClient.AutoReconnectMaxCount = 3;
            _tcpClient.ExceptionCaught += OnExceptionCaught;
            _tcpClient.ReconnectDefeated += ReconnectDefeated;

            _tcpClient.ReceivedAutoUpdaterConfig += OnReceivedAutoUpdaterConfig;
            _tcpClient.PopMessageBox += OnPopMessageBox;
            _tcpClient.ReceivedLoginResult += OnReceivedLoginResult;
            _tcpClient.Connect(SocketConfig.IP, SocketConfig.Port);
        }

        private void OnReceivedLoginResult(RSP_LoginResult obj)
        {
            _userInfo = obj.UserInfo;

            if (obj.IsPopMessageBox)
            {
                PopMessageBox(obj.Message);
            }

            if (obj.IsSuccess == false)
            {
                Log.Info("登录失败，原因：" + obj.Message);
                return;
            }

            if (obj.LoginType == LoginType.First)
            {
                Log.Info("登录成功");

                Messenger.Default.Send(new MyTcpClientMessage()
                {
                    MessageType = MyTcpClientMessageType.FirstLoginSucceed
                });
            }
            else if (obj.LoginType == LoginType.Reconnect)
            {
                Log.Info("重新连接成功");
            }

            Messenger.Default.Send(new MyTcpClientMessage()
            {
                MessageType = MyTcpClientMessageType.ReceivedUserInfo,
                Content = _userInfo
            });
        }

        private void ReconnectDefeated()
        {
            Log.Info("自动重连工具服务器失败，已尝试次数：" + _tcpClient.AutoReconnectMaxCount);

            CommonHelper.MessageBoxTimeout("连接工具服务端失败，请重新登陆");
            Environment.Exit(-1);
        }

        private void OnExceptionCaught(object sender, NetworkException ex)
        {
            Log.Error("MyClient异常捕获", ex);
        }

        private void OnReceivedAutoUpdaterConfig(RSP_AutoUpdaterConfig obj)
        {
            _dispatcherService.InvokeAsync(() =>
            {
                _dialogService.ShowAutoUpdater(obj.XmlUrl);
            });
        }

        private void OnPopMessageBox(RSP_PopMessageBox obj)
        {
            var messageBox = obj.MessageBoxBody;
            if (messageBox.MessageBoxType == MessageBoxType.Normal)
            {
                PopMessageBox(messageBox.Content, messageBox.Title);
            }
            else
            {
                CommonHelper.MessageBoxTimeout(messageBox.Content, messageBox.Title);
            }
        }

        private void PopMessageBox(string content, string title = "提示")
        {
            _dispatcherService.InvokeAsync(() =>
            {
                _dialogService.ShowInformation(content, title);
            });
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        public void RegisterService(IDispatcherService dispatcherService, IDialogService dialogService)
        {
            _dispatcherService = dispatcherService;
            _dialogService = dialogService;
        }

        private bool CheckConnection()
        {
            if (_tcpClient.IsConnected == false)
            {
                PopMessageBox("连接服务器失败，请尝试重新运行程序");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 尝试登录
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="passwordHash"></param>
        public void TryLogin(string userID, string passwordHash)
        {
            if (CheckConnection())
            {
                _tcpClient.Session.TryLogin(userID, passwordHash);
            }
        }

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="passwordHash"></param>
        public void RegisterAccount(string userID, string passwordHash, string displayName)
        {
            if (string.IsNullOrEmpty(userID)
                || string.IsNullOrEmpty(passwordHash)
                || string.IsNullOrEmpty(displayName))
            {
                _dialogService.ShowInformation("账号、密码或昵称不能为空");
            }
            else if(userID.Length < 6)
            {
                _dialogService.ShowInformation("账号长度不能小于6");
            }
            else if(StringExtension.FormBase64(passwordHash).Length < 6)
            {
                _dialogService.ShowInformation("密码长度不能小于6");
            }
            else if (CheckConnection())
            {
                _tcpClient.Session.RegisterAccount(userID, passwordHash, displayName);
            }
        }
    }
}
