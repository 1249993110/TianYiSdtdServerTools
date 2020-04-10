using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Dtos.Primitives;
using TianYiSdtdServerTools.Client.Models.Entitys;

namespace TianYiSdtdServerTools.Client.Models.Dtos
{
    public class CityPositionDto : MyDtoBase<CityPositionDto, CityPosition>
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
