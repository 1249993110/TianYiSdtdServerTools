using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.MvvmMessages
{
    public enum MyTcpClientMessageType
    {
        /// <summary>
        /// 首次登录成功
        /// </summary>
        FirstLoginSucceed,

        /// <summary>
        /// 收到用户信息
        /// </summary>
        ReceivedUserInfo,

    }

    public class MyTcpClientMessage
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public MyTcpClientMessageType MessageType { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public object Content { get; set; }
    }
}
