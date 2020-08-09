using IceCoffee.Common;
using IceCoffee.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.SdtdServerInfo;
using TianYiSdtdServerTools.Client.TelnetClient;

namespace TianYiSdtdServerTools.Client.ViewModels.Managers
{
    public sealed class SdtdServerInfoManager : Singleton3<SdtdServerInfoManager>
    {
        /// <summary>
        /// 服务器IP地址
        /// </summary>
        public string ServerIP { get; private set; }

        /// <summary>
        /// telnet端口
        /// </summary>
        public ushort TelnetPort { get; private set; }

        /// <summary>
        /// telnet密码
        /// </summary>
        public string TelnetPassword { get; private set; }

        /// <summary>
        /// GPS端口
        /// </summary>
        public ushort GPSPort { get; private set; }

        public WebUserToken WebUserToken { get; private set; }

        private SdtdServerInfoManager()
        {
            StringBuilder admintokenBuilder = new StringBuilder();
            foreach (var c in "1249993110".ToBase64())
            {
                if (char.IsLetterOrDigit(c))
                {
                    admintokenBuilder.Append(c);
                }
            }
            WebUserToken = new WebUserToken()
            {
                AdminUser = "admin_tianyi",
                AdminToken = admintokenBuilder.ToString(),
                PermissionLevel = 0
            };

            SdtdConsole.Instance.ConnectionStateChanged += OnConnectionStateChanged;
        }

        private void OnConnectionStateChanged(ConnectionState connectionState)
        {
            if(connectionState == ConnectionState.Connected)
            {
                SdtdConsole.Instance.SendCmd(string.Format("webtokens add {0} {1} {2}",
                    WebUserToken.AdminUser, WebUserToken.AdminToken, WebUserToken.PermissionLevel));
            }
        }

        public void SetServerInfo(string serverIP, ushort? telnetPort, string telnetPassword, ushort? gpsPort)
        {
            ServerIP = serverIP;
            TelnetPort = telnetPort.GetValueOrDefault();
            TelnetPassword = telnetPassword;
            GPSPort = gpsPort.GetValueOrDefault();
        }

    }
}
