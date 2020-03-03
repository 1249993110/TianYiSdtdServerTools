using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.Common;
using TianYiSdtdServerTools.Client.Models.Players;

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

        /// <summary>
        /// 发送私聊信息
        /// </summary>
        /// <param name="playerInfo"></param>
        /// <param name="msg"></param>
        public void SendMessageToPlayer(PlayerInfo playerInfo, string msg)
        {
            SendMessageToPlayer(playerInfo.SteamID, msg);
        }

        #region 传送玩家

        /// <summary>
        /// 传送玩家
        /// </summary>
        /// <param name="steamID"></param>
        /// <param name="target"></param>
        public void TelePlayer(string steamID, string target)
        {
            SendCmd(string.Format("tele {0} {1}", steamID, target));
        }

        /// <summary>
        /// 踢出玩家
        /// </summary>
        /// <param name="steamID"></param>
        /// <param name="reason"></param>
        public void KickPlayer(string steamID, string reason = "")
        {
            SendCmd(string.Format("kick {0} {1}", steamID, reason));
        }

        /// <summary>
        /// 杀死玩家
        /// </summary>
        /// <param name="steamID"></param>
        public void KillPlayer(string steamID)
        {
            SendCmd(string.Format("kill {0}", steamID));
        }

        /// <summary>
        /// 封禁玩家n年
        /// </summary>
        /// <param name="steamID"></param>
        /// <param name="year"></param>
        /// <param name="reason"></param>
        public void BanPlayerWithYear(string steamID, int year,string reason = "")
        {
            SendCmd(string.Format("ban add {0} {1} year {2}", steamID, year, reason));
        }

        /// <summary>
        /// 移除玩家领地石
        /// </summary>
        /// <param name="steamID"></param>
        public void RemovePlayerLandclaims(string steamID)
        {
            SendCmd(string.Format("rlp {0}", steamID));
        }

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="level">0级最高，2000最低</param>
        public void AddAdministrator(string steamID, int level)
        {
            SendCmd(string.Format("admin add {0} {1}", steamID, level));
        }

        /// <summary>
        /// 移除管理员
        /// </summary>
        /// <param name="steamID"></param>
        public void RemoveAdministrator(string steamID)
        {
            SendCmd(string.Format("admin remove {0}", steamID));
        }

        /// <summary>
        /// 移除玩家存档
        /// </summary>
        /// <param name="steamID"></param>
        public void RemovePlayerArchive(string steamID)
        {
            SendCmd(string.Format("rp {0}", steamID));
        }

        /// <summary>
        /// 添加命令权限等级
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="level">0级最高，2000最低</param>
        public void AddCommandPermissionLevel(string cmd, int level)
        {
            SendCmd(string.Format("cp add {0} {1}", cmd, level));
        }

        /// <summary>
        /// 移除命令权限等级
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="level">0级最高，2000最低</param>
        public void RemoveCommandPermissionLevel(string cmd)
        {
            SendCmd(string.Format("cp remove {0}", cmd));
        }
        #endregion
    }
}
