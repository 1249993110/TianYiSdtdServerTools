using IceCoffee.Common.LogManager;
using System;
using System.Collections.Generic;
using System.Text;
using TianYiSdtdServerTools.Server.Sockets.Primitives;
using TianYiSdtdServerTools.Shared;
using TianYiSdtdServerTools.Shared.Models.NetDataObjects;

namespace TianYiSdtdServerTools.Server.Sockets.BusinessHandlers
{
    class LoginHandler : BusinessHandlerBase
    {
        public LoginHandler(TcpSession tcpSession) : base(tcpSession)
        {
        }

        public void RequestLogin(Submit_ClientInfo clientInfo)
        {
            Log.Info("客户请求登录，来自IP：" + tcpSession.RemoteIPEndPoint);

            if (VersionManager.CheckVersion(clientInfo.ClientVersion) == UpdateLevel.Necessary)
            {
                tcpSession.Send(new AutoUpdaterConfig() { XmlUrl = SocketConfig.UpdateXmlUrl_complete });
            }
            else if (VersionManager.CheckVersion(clientInfo.ClientVersion) == UpdateLevel.Optional)
            {
                tcpSession.Send(new AutoUpdaterConfig() { XmlUrl = SocketConfig.UpdateXmlUrl_patch });
            }

            if (clientInfo.IsAuthorized)
            {
                //Send()
            }
            else
            {
            }
        }
    }
}
