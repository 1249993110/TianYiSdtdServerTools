using IceCoffee.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianYiSdtdServerTools.Server.Sockets
{
    internal static class SocketConfig
    {
        /// <summary>
        /// 补丁版本url
        /// </summary>
        public static readonly string UpdateXmlUrl_patch;
        /// <summary>
        /// 完整版url
        /// </summary>
        public static readonly string UpdateXmlUrl_complete;

        static SocketConfig()
        {
            UpdateXmlUrl_patch = CommonHelper.GetAppSettings("UpdateXmlUrl_patch");
            UpdateXmlUrl_complete = CommonHelper.GetAppSettings("UpdateXmlUrl_complete");
        }
    }
}
