using Autofac;
using IceCoffee.Common;
using IceCoffee.Common.LogManager;
using IceCoffee.Network.Sockets;
using IceCoffee.Network.Sockets.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

        new public TcpServer SocketDispatcher => _socketDispatcher;

        public TcpSession()
        {
            _loginHandler = IocContainer.Resolve<LoginHandler>(new TypedParameter(typeof(TcpSession), this));
        }

        protected override void OnInitialized()
        {
            _socketDispatcher = base.SocketDispatcher as TcpServer;

            base.OnInitialized();
        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override void OnClosed(SocketError closedReason)
        {
            base.OnClosed(closedReason);
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
                if(_loginHandler.IsAuthorized == false)
                {
                    if(netObj.NetDataType == NetDataType.REQ_Login)
                    {
                        _loginHandler.RequestLogin(obj as REQ_Login);
                    }
                    else if(netObj.NetDataType == NetDataType.REQ_RegisterAccount)
                    {
                        _loginHandler.RegisterAccount(obj as REQ_RegisterAccount);
                    }
                    else
                    {
                        Log.Error("收到未授权认证的网络数据，来自IP：" + this.RemoteIPEndPoint);
                        this.Close();
                    }
                }
                else
                {
                    switch (netObj.NetDataType)
                    {
                        default:
                            Log.Error("错误的NetDataType：{0}，来自IP：{1}", netObj.NetDataType, this.RemoteIPEndPoint);
                            break;
                    }
                }
            }
        }
    }
}
