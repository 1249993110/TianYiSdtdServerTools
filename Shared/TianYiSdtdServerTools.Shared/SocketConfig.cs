using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Shared
{
    public static class SocketConfig
    {
#if DEBUG
        public const string IP = "127.0.0.1";

#else
        public const string IP = "7daystodie.top";

        //public const string LoginUrl = "https://7daystodie.top/ServerTools/Login";
#endif
        public const ushort Port = 20204;
    }
}
