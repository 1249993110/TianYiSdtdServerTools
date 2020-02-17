using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.SdtdServerInfo
{
    /// <summary>
    /// 服务器部分状态
    /// </summary>
    public class ServerPartialState
    {
        /// <summary>
        /// 在线玩家数
        /// </summary>
        public int OnlinePlayerCount { get; set; }

        /// <summary>
        /// 僵尸数
        /// </summary>
        public int ZombieCount { get; set; }

        /// <summary>
        /// 实体数
        /// </summary>
        public int EntityCount { get; set; }

    }
}
