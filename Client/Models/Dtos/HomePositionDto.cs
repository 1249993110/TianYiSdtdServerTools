using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.DbCore.Primitives.Dto;
using TianYiSdtdServerTools.Client.Models.Entitys;

namespace TianYiSdtdServerTools.Client.Models.Dtos
{
    public class HomePositionDto : DtoBaseStr
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
        public String SteamID { get; set; }

        /// <summary>
        /// 三维坐标
        /// </summary>
        public string Pos { get; set; }
    }
}
