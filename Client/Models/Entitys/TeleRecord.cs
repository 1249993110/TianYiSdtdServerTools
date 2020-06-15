using IceCoffee.DbCore.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.DbCore.Primitives.Entity;

namespace TianYiSdtdServerTools.Client.Models.Entitys
{
    public class TeleRecord : EntityBaseStr
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
