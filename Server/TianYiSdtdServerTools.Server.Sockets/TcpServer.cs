using IceCoffee.Common;
using IceCoffee.LogManager;
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
            UserMaxLoginCount = CommonHelper.GetAppSettings("UserMaxLoginCount").ToInt();
            LoginGiveUseTime = CommonHelper.GetAppSettings("LoginGiveUseTime").ToInt();
        }

        public TcpServer()
        {
            this.HeartbeatEnable = true;
            this.ExceptionCaught += OnTcpServer_ExceptionCaught;
        }

        private void OnTcpServer_ExceptionCaught(object sender, NetworkException ex)
        {
            Log.Error(ex, "服务端异常捕获");
        }

        protected override void OnStarted()
        {
            IocContainer.Resolve<IOnlineUserService>().RemoveAny(null);

            base.OnStarted();

            Log.Info("服务端已启动");
        }

        protected override void OnStopped()
        {
            base.OnStopped();

            Log.Info("服务端已停止");
        }
    }
}
