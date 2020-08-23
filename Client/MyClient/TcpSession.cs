using IceCoffee.Common;
using IceCoffee.Common.LogManager;
using IceCoffee.Network.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared;
using TianYiSdtdServerTools.Shared.Models;
using TianYiSdtdServerTools.Shared.Models.NetDataObjects;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Client.MyClient
{
    public class TcpSession : CustomSession<TcpSession>
    {
        private TcpClient _socketDispatcher;

        protected override void OnInitialized()
        {
            _socketDispatcher = base.SocketDispatcher as TcpClient;
            
            KeepAlive.Enable = true;

            base.OnInitialized();
        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }

        public void TryLogin(string userID, string passwordHash)
        {
            REQ_Login req = new REQ_Login()
            {
                ClientVersion = VersionManager.CurrentVersion,
                IsAuthorized = false,
                UserID = userID,
                PasswordHash = passwordHash,
                LoginType = LoginType.First
            };
            this.Send(req);
        }

        public void Reconnect(string userID, bool isAuthorized)
        {
            REQ_Login req = new REQ_Login()
            {
                ClientVersion = VersionManager.CurrentVersion,
                IsAuthorized = isAuthorized,
                UserID = userID,
                LoginType = LoginType.Reconnect
            };
            this.Send(req);
        }

        public void RegisterAccount(string userID, string passwordHash, string displayName)
        {
            REQ_RegisterAccount req = new REQ_RegisterAccount()
            {
                UserID = userID,
                PasswordHash = passwordHash,
                DisplayName = displayName,
                RegisterAccountType = RegisterAccountType.StandardAccount
            };
            this.Send(req);
        }

        protected override void OnReceived(object obj)
        {
            switch ((obj as NetDataObject).NetDataType)
            {
                case NetDataType.RSP_CloseClient:
                    Environment.Exit(-1);
                    break;
                case NetDataType.RSP_LoginResult:
                    _socketDispatcher.OnReceivedLoginResult(obj as RSP_LoginResult);
                    break;
                case NetDataType.RSP_AutoUpdaterConfig:
                    _socketDispatcher.OnReceivedAutoUpdaterConfig(obj as RSP_AutoUpdaterConfig);
                    break;
                case NetDataType.RSP_PopMessageBox:
                    _socketDispatcher.OnPopMessageBox(obj as RSP_PopMessageBox);
                    break;
                default:
                    Log.Error("错误的NetDataType");
                    break;
            }
        }
    }
}
