using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.ConsoleTempList
{
    public class CommandLevel
    {
        /// <summary>
        /// 命令
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// 命令所需权限等级
        /// </summary>
        public int PermissionLevel { get; set; }
    }
}
