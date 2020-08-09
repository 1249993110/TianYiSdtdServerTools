using IceCoffee.Common;
using IceCoffee.Common.Extensions;
using IceCoffee.Common.LogManager;
using IceCoffee.Network.CatchException;
using IceCoffee.Network.Sockets.MulitThreadTcpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared.Models.NetDataObjects;

namespace TianYiSdtdServerTools.Client.MyClient
{
    public class TcpClient : BaseClient<TcpSession>
    {
        private static readonly TcpClient _instance = new TcpClient();

        /// <summary>
        /// 获得实例
        /// </summary>
        public static TcpClient Instance { get { return _instance; } }

        public readonly string ClientToken = Guid.NewGuid().ToString().Replace("-", string.Empty).ToBase64();

        public bool IsAuthorized { get; set; }

        public Return_ClientInfo Return_ClientInfo { get; private set; }

        /// <summary>
        /// 登陆成功
        /// </summary>
        public event Action LoginSucceed;

        /// <summary>
        /// 收到返回的客户信息
        /// </summary>
        public event Action<Return_ClientInfo> ReceivedClientInfo;

        /// <summary>
        /// 收到自动更新器配置
        /// </summary>
        public event Action<AutoUpdaterConfig> ReceivedAutoUpdaterConfig;

        public TcpClient()
        {
            base.ExceptionCaught += OnExceptionCaught;
            base.AutoReconnectMaxCount = 3;
        }

        private void OnExceptionCaught(object sender, NetworkException ex)
        {
            Log.Error("MyClient异常捕获", ex);
        }

        protected override void OnReconnectDefeated()
        {
            Log.Info("自动重连工具服务器失败，已尝试次数：" + AutoReconnectMaxCount.ToString());
            base.OnReconnectDefeated();
        }

        internal void OnReceivedClientInfo(Return_ClientInfo return_ClientInfo)
        {
            Return_ClientInfo = return_ClientInfo;
            if (IsAuthorized == false)
            {
                IsAuthorized = true;
                LoginSucceed?.Invoke();
            }

            ReceivedClientInfo?.Invoke(return_ClientInfo);
        }

        internal void OnReceivedAutoUpdaterConfig(AutoUpdaterConfig autoUpdaterConfig)
        {
            ReceivedAutoUpdaterConfig?.Invoke(autoUpdaterConfig);
        }
    }
}
