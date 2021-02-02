using IceCoffee.Common;
using IceCoffee.Common.Extensions;
using IceCoffee.LogManager;
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
    public class TcpClient : CustomClient<TcpSession>
    {       
        /// <summary>
        /// 收到返回的登录结果
        /// </summary>
        public event Action<RSP_LoginResult> ReceivedLoginResult;

        /// <summary>
        /// 收到自动更新器配置
        /// </summary>
        public event Action<RSP_AutoUpdaterConfig> ReceivedAutoUpdaterConfig;

        /// <summary>
        /// 弹出对话框
        /// </summary>
        public event Action<RSP_PopMessageBox> PopMessageBox;

        public TcpClient()
        {
            
        }

        internal void OnReceivedLoginResult(RSP_LoginResult clientInfo)
        {
            ReceivedLoginResult?.Invoke(clientInfo);
        }

        internal void OnReceivedAutoUpdaterConfig(RSP_AutoUpdaterConfig autoUpdaterConfig)
        {
            ReceivedAutoUpdaterConfig?.Invoke(autoUpdaterConfig);
        }
        
        internal void OnPopMessageBox(RSP_PopMessageBox messageBox)
        {
            PopMessageBox?.Invoke(messageBox);
        }
    }
}
