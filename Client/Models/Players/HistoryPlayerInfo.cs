using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.Players
{
    public class HistoryPlayerInfo : PlayerBase
    {
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 总游戏时长（分钟）
        /// </summary>
        public int TotalPlayTime { get; set; }

        /// <summary>
        /// 最后在线
        /// </summary>
        public string LastOnlineTime { get; set; }
    }
}
