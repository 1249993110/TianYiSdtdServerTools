using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using IceCoffee.Common;

namespace TianYiSdtdServerTools.Client.TelnetClient
{
    public partial class SdtdConsole
    {
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="cmd"></param>
        public void SendCmd(string cmd)
        {
            if(_tcpClient.Session != null && SdtdConsole.Instance.IsConnected)
            {
                _tcpClient.Session.SendCmd(cmd);
                if (cmd.StartsWith("say") || cmd.StartsWith("pm"))
                {
                    Task.Run(() =>
                    {
                        System.Threading.Thread.Sleep(25);
                        _tcpClient.Session.SendCmd(Environment.NewLine);
                    });
                }
            }                     
        }

        /// <summary>
        /// 发送公屏信息 Global
        /// </summary>
        /// <param name="msg"></param>
        public void SendGlobalMessage(string msg)
        {
            SendCmd(string.Format("say \"{0}\"", msg));
        }

        /// <summary>
        /// 发送私聊信息
        /// </summary>
        /// <param name="steamID"></param>
        /// <param name="msg"></param>
        public void SendMessageToPlayer(string steamID, string msg)
        {
            SendCmd(string.Format("pm {0} \"{1}\"", steamID, msg));
        }

        /// <summary>
        /// 发送私聊信息
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="msg"></param>
        public void SendMessageToPlayer(int entityID, string msg)
        {
            SendCmd(string.Format("pm {0} \"{1}\"", entityID.ToString(), msg));
        }

        #region 传送玩家

        /// <summary>
        /// 传送玩家
        /// </summary>
        /// <param name="steamID"></param>
        /// <param name="targetPos"></param>
        public void TelePlayer(string steamID, string targetPos)
        {

        }

        /// <summary>
        /// 传送玩家到朋友身边
        /// </summary>
        /// <param name="steamID"></param>
        /// <param name="targetSteamID"></param>
        public void TelePlayerToFriend(string steamID, string targetSteamID)
        {

        }

        /// <summary>
        /// 传送玩家到朋友身边
        /// </summary>
        /// <param name="steamID"></param>
        /// <param name="targetEntityID"></param>
        public void TelePlayerToFriend(string steamID, int targetEntityID)
        {

        }

        #endregion
    }
}
