using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.Chat
{
    public enum ChatType
    {
        /// <summary>
        /// 公屏
        /// </summary>
        Global,    

        /// <summary>
        /// 好友
        /// </summary>
        Friends,   

        /// <summary>
        /// 队伍
        /// </summary>
        Party,     

        /// <summary>
        /// 私聊
        /// </summary>
        Whisper
    }
}
