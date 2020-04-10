using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Players;

namespace TianYiSdtdServerTools.Client.Models.Chat
{
    public class ChatInfo
    {
        public PlayerInfo playerInfo;

        public string message;

        public ChatType chatType;

        public SenderType senderType;

        /// <summary>
        /// 该值指示事件在传递过程中的当前处理状态
        /// </summary>
        public bool isHandled;
    }
}
