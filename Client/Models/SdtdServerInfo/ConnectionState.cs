using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.SdtdServerInfo
{
    /// <summary>
    /// 连接状态
    /// </summary>
    public enum ConnectionState
    {
        /// <summary>
        /// 未连接
        /// </summary>
        Disconnected,

        /// <summary>
        /// 正在断开
        /// </summary>
        Disconnecting,

        /// <summary>
        /// 正在连接
        /// </summary>
        Connecting,

        /// <summary>
        /// 已连接, 且密码正确
        /// </summary>
        Connected,

        /// <summary>
        /// 正在自动重连
        /// </summary>
        AutoReconnecting,

        /// <summary>
        /// 正在验证密码
        /// </summary>
        PasswordVerifying,

        /// <summary>
        /// 密码错误
        /// </summary>
        PasswordIncorrect
    }
}
