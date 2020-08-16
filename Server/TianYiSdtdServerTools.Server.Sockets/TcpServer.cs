using IceCoffee.Common;
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
using IceCoffee.Common.Extensions;
using LuoShuiTianYi.Sdtd.Services.Contracts;

namespace TianYiSdtdServerTools.Server.Sockets
{
    public class TcpServer : CustomServer<TcpSession>
    {
        /// <summary>
        /// 用户最大登录数量
        /// </summary>
        public static readonly int UserMaxLoginCount;

        /// <summary>
        /// 登录赠送使用时长 单位：天
        /// </summary>
        public static readonly int LoginGiveUseTime;

        static TcpServer()
        {
            UserMaxLoginCount = CommonHelper.GetAppSettings("userMaxLoginCount").ToInt();
            LoginGiveUseTime = CommonHelper.GetAppSettings("loginGiveUseTime").ToInt();
        }

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

            _ = IocContainer.Resolve<IOnlineUserService>().RemoveAny(null);

            base.OnStarted();
        }

        protected override void OnStopped()
        {
            Log.Info("服务端已停止");
            base.OnStopped();
        }
    }
}
