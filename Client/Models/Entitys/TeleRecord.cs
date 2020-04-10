using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Dtos;
using TianYiSdtdServerTools.Client.Models.Entitys.Primitives;

namespace TianYiSdtdServerTools.Client.Models.Entitys
{
    public class TeleRecord : MyEntityBase<TeleRecord, TeleRecordDto>
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
