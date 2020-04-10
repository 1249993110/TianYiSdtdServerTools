using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Dtos;
using TianYiSdtdServerTools.Client.Models.Entitys.Primitives;

namespace TianYiSdtdServerTools.Client.Models.Entitys
{
    public class HomePosition : MyEntityBase<HomePosition, HomePositionDto>
    {
        /// <summary>
        /// Home名称
        /// </summary>
        public string HomeName { get; set; }

        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// SteamID
        /// </summary>
        public string SteamID { get; set; }

        /// <summary>
        /// 三维坐标
        /// </summary>
        public string Pos { get; set; }
    }
}
