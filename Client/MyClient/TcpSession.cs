using IceCoffee.Common;
using IceCoffee.Common.LogManager;
using IceCoffee.Network.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared;
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
            Submit_ClientInfo clientInfo = new Submit_ClientInfo()
            {
                ClientToken = _socketDispatcher.ClientToken,
                ClientVersion = VersionManager.CurrentVersion,
                IsAuthorized = _socketDispatcher.IsAuthorized
            };
            Send(clientInfo);

            base.OnStarted();
        }

        protected override void OnReceived(object obj)
        {
            switch ((obj as NetDataObject).NetDataType)
            {
                case NetDataType.CloseClient:
                    Environment.Exit(-1);
                    break;
                case NetDataType.Return_ClientInfo:
                    _socketDispatcher.OnReceivedClientInfo(obj as Return_ClientInfo);                    
                    break;
                case NetDataType.Return_AutoUpdaterConfig:
                    _socketDispatcher.OnReceivedAutoUpdaterConfig(obj as AutoUpdaterConfig);
                    break;
                default:
                    Log.Error("错误的NetDataType");
                    break;
            }
        }
    }
}
