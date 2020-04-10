using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Dtos.Primitives;
using TianYiSdtdServerTools.Client.Models.Entitys;

namespace TianYiSdtdServerTools.Client.Models.Dtos
{
    public class GoodsDto : MyDtoBase<GoodsDto, Goods>
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
        /// 售价
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public string GoodsType { get; set; }
    }
}
