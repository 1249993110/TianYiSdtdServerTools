using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.DbCore.Primitives.Dto;
using TianYiSdtdServerTools.Client.Models.Entitys;

namespace TianYiSdtdServerTools.Client.Models.Dtos
{
    public class TeleRecordDto : DtoBaseStr
    {
        /// <summary>
        /// SteamID
        /// </summary>
        public string SteamID { get; set; }

        /// <summary>
        /// 上次传送日期
        /// </summary>
        public string LastTeleDateTime { get; set; }
    }
}
