using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.SdtdServerInfo
{
    public class WebUserToken
    {
        public string AdminUser { get; set; }

        public string AdminToken { get; set; }

        public int PermissionLevel { get; set; }
    }
}
