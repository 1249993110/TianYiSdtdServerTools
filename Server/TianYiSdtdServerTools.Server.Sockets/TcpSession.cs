using Autofac;
using IceCoffee.Common;
using IceCoffee.Common.LogManager;
using IceCoffee.Network.Sockets;
using IceCoffee.Network.Sockets.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Server.Sockets.BusinessHandlers;
using TianYiSdtdServerTools.Shared;
using TianYiSdtdServerTools.Shared.Models.NetDataObjects;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Server.Sockets
{
    public class TcpSession : CustomSession<TcpSession>
    {
        private TcpServer _socketDispatcher;

        private readonly LoginHandler _loginHandler;

        public TcpSession()
        {
            _loginHandler = IocContainer.Resolve<LoginHandler>(new TypedParameter(typeof(TcpSession), this));
        }

        protected override void OnInitialized()
        {
            _socketDispatcher = base.SocketDispatcher as TcpServer;

            base.OnInitialized();
        }

        protected override void OnReceived(object obj)
        {           
            NetDataObject netObj = obj as NetDataObject;
            if (netObj == null)
            {
                Log.Error("网络数据解析失败，来自IP：" + this.RemoteIPEndPoint);
                this.Close();
            }
            else
            {
                switch (netObj.NetDataType)
                {
                    case NetDataType.Submit_ClientInfo:
                        _loginHandler.RequestLogin(obj as Submit_ClientInfo);
                        break;
                    default:
                        Log.Error("错误的NetDataType" + netObj.NetDataType);
                        break;
                }
            }               
        }
    }
}
