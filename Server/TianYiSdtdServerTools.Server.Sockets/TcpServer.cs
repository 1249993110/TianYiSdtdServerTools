using IceCoffee.Common.LogManager;
using IceCoffee.Network.CatchException;
using IceCoffee.Network.Sockets.MulitThreadTcpServer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Server.Sockets
{
    public class TcpServer : CustomServer<TcpSession>
    {
        public TcpServer()
        {
            this.ExceptionCaught += OnTcpServer_ExceptionCaught;
        }

        private void OnTcpServer_ExceptionCaught(object sender, NetworkException ex)
        {
            Log.Error("服务端异常捕获", ex);
        }

        protected override void OnStarted()
        {
            Log.Info("服务端已启动");
            base.OnStarted();
        }

        protected override void OnStopped()
        {
            Log.Info("服务端已停止");
            base.OnStopped();
        }
    }
}
