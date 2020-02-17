using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.Players
{
    public class PlayerBase
    {
        /// <summary>
        /// 17位steamID
        /// </summary>
        public string SteamID { get; set; }

        /// <summary>
        /// 短位ID|实体ID
        /// </summary>
        public int EntityID { get; set; }

        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string PlayerName { get; set; }
    }
}
