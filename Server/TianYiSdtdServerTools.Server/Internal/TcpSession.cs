using IceCoffee.Common;
using IceCoffee.Common.LogManager;
using IceCoffee.Network.Sockets;
using IceCoffee.Network.Sockets.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared;
using TianYiSdtdServerTools.Shared.Models.NetDataObjects;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Server.Internal
{
    internal class TcpSession : CustomSession<TcpSession>
    {
        private TcpServer _socketDispatcher;

        /// <summary>
        /// 客户令牌
        /// </summary>
        public string ClientToken { get; set; }

        protected override void OnInitialized()
        {
            _socketDispatcher = base.SocketDispatcher as TcpServer;

            //KeepAlive = new KeepAlive() {  Enable = true };

            base.OnInitialized();
        }

        protected override void OnReceived(object obj)
        {
            switch ((obj as NetDataObject).NetDataType)
            {
                case NetDataType.Submit_ClientInfo:
                    Submit_ClientInfo clientInfo = obj as Submit_ClientInfo;
                    if (VersionManager.CheckVersion(clientInfo.ClientVersion) == UpdateLevel.Necessary)
                    {
                        Send(new AutoUpdaterConfig() { XmlUrl = TcpServer.updateXmlUrl_complete });
                    }
                    else if (VersionManager.CheckVersion(clientInfo.ClientVersion) == UpdateLevel.Optional)
                    {
                        Send(new AutoUpdaterConfig() { XmlUrl = TcpServer.updateXmlUrl_patch });
                    }

                    if (clientInfo.IsAuthorized)
                    {
                        _socketDispatcher.loggedSessions.TryAdd(clientInfo.ClientToken, this);
                    }
                    else
                    {
                        _socketDispatcher.tryLoginSessions.TryAdd(clientInfo.ClientToken, this);
                    }
                    
                    break;
                default:
                    Log.Error("错误的NetDataType" + (int)((obj as NetDataObject).NetDataType));
                    break;
            }            
        }
    }
}
