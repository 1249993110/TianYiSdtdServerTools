using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Dtos.Primitives;
using TianYiSdtdServerTools.Client.Models.Entitys;


namespace TianYiSdtdServerTools.Client.Models.Dtos
{
    public class LotteryDto : MyDtoBase<LotteryDto, Lottery>
    {
        /// <summary>
        /// 奖品名称
        /// </summary>
        public string LotteryName { get; set; }

        /// <summary>
        /// 内容 物品/实体ID/指令
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 品质
        /// </summary>
        public int Quality { get; set; }

        /// <summary>
        /// 奖品类型
        /// </summary>
        public string LotteryType { get; set; }
    }
}
