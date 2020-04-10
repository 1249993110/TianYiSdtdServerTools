using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Dtos.Primitives;
using TianYiSdtdServerTools.Client.Models.Entitys;

namespace TianYiSdtdServerTools.Client.Models.Dtos
{
    public class TeleRecordDto : MyDtoBase<TeleRecordDto, TeleRecord>
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
