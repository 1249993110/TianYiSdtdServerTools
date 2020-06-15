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
    public class Goods : EntityBaseStr
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 购买命令
        /// </summary>
        public string BuyCmd { get; set; }

        /// <summary>
        /// 内容 物品/方块/实体/指令
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
        /// 售价
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public string GoodsType { get; set; }
    }
}
