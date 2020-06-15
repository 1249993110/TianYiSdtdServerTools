using IceCoffee.DbCore.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceCoffee.DbCore.Primitives.Entity;
using TianYiSdtdServerTools.Client.Models.Dtos;

namespace TianYiSdtdServerTools.Client.Models.Entitys
{
    public class Lottery : EntityBaseStr
    {
        /// <summary>
        /// 奖品名称
        /// </summary>
        public string LotteryName { get; set; }

        /// <summary>
        /// 内容 物品/方块/实体/指令/积分
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
