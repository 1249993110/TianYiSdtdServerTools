using IceCoffee.DbCore.Primitives;
using IceCoffee.DbCore.Primitives.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Dtos;

namespace TianYiSdtdServerTools.Client.Models.Entitys
{
    public class CityPosition : EntityBaseStr
    {
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 传送命令
        /// </summary>
        public string TeleCmd { get; set; }

        /// <summary>
        /// 传送需要积分
        /// </summary>
        public int TeleNeedScore { get; set; }

        /// <summary>
        /// 三维坐标
        /// </summary>
        public string Pos { get; set; }
    }
}
