using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.Players
{
    public class PlayerInfo : PlayerBase
    {
        /// <summary>
        /// 玩家坐标，" "分隔
        /// </summary>
        public string Pos { get; set; }

        /// <summary>
        /// 生命值
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// 死亡次数
        /// </summary>
        public int Deaths { get; set; }

        /// <summary>
        /// 僵尸击杀数
        /// </summary>
        public int Zombies { get; set; }

        /// <summary>
        /// 玩家击杀数
        /// </summary>
        public int Players { get; set; }

        /// <summary>
        /// 分数
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 延迟
        /// </summary>
        public int Ping { get; set; }


    }
}
