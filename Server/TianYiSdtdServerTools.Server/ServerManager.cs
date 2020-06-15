using IceCoffee.Common;
using IceCoffee.Common.LogManager;
using IceCoffee.Network.CatchException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Server.Internal;
using TianYiSdtdServerTools.Shared;
using TianYiSdtdServerTools.Shared.Models.NetDataObjects;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Server
{
    public class ServerManager : Singleton3<ServerManager>
    {
        private TcpServer _tcpServer;

        private ServerManager() { }

        public void StartServer()
        {
            _tcpServer = new TcpServer();

            _tcpServer.ExceptionCaught += OnExceptionCaught;

            _tcpServer.Start(Config.Port);
        }

        private void OnExceptionCaught(NetworkException e)
        {
            Log.Error("天依七日杀工具服务端异常捕获", e);
        }

        public bool ClientLogin(Return_ClientInfo clientInfo, string clientToken)
        {
            if(_tcpServer.tryLoginSessions.TryRemove(clientToken, out TcpSession tcpSession))
            {
                tcpSession.ClientToken = clientToken;
                tcpSession.Send(clientInfo);
                return _tcpServer.loggedSessions.TryAdd(clientToken, tcpSession);
            }
            return false;
        }
    }
}
