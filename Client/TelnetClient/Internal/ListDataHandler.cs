using IceCoffee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Players;

namespace TianYiSdtdServerTools.Client.TelnetClient.Internal
{
    internal static class ListDataHandler
    {
        /// <summary>
        /// 解析在线玩家数据
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static void ParseOnlinePlayers(List<string> lines, ref Dictionary<string, PlayerInfo> players)
        {
            List<string> steamIDs = new List<string>();
            List<string> keys = players.Keys.ToList();

            int end1, end2;
            string playerSteamID,playerPos, x, y, z;
            PlayerInfo player = null;

            foreach (var line in lines)
            {
                playerSteamID = line.GetMidStr("steamid=", ",", 50);// 取出玩家17位SteamID;

                steamIDs.Add(playerSteamID);

                if (players.ContainsKey(playerSteamID))
                {
                    player = players[playerSteamID];
                }
                else
                {
                    player = new PlayerInfo();
                    player.SteamID = playerSteamID;
                }

                player.EntityID = line.GetMidStr("id=", ",", out end1, 1).ToInt();          // 取出玩家实体ID
                player.PlayerName = line.GetMidStr(", ", ", pos=(", out end1, end1);        // 取出玩家昵称

                playerPos = line.GetMidStr("pos=(", ")", out end1, end1);                   // 取出玩家坐标
                end2 = playerPos.IndexOf(".");
                x = playerPos.Substring(0, end2);
                y = playerPos.GetMidStr(", ", ".", out end2, end2);
                z = playerPos.GetMidStr(", ", ".", end2);
                playerPos = x + " " + y + " " + z;
                player.Pos = playerPos;

                player.Health = line.GetMidStr("health=", ",", out end1, end1).ToInt();     // 取出玩家生命值
                player.Deaths = line.GetMidStr("deaths=", ",", out end1, end1).ToInt();     // 取出玩家死亡次数
                player.Zombies = line.GetMidStr("zombies=", ",", out end1, end1).ToInt();   // 取出玩家击杀僵尸数
                player.Players = line.GetMidStr("players=", ",", out end1, end1).ToInt();   // 取出玩家击杀玩家数
                player.Score = line.GetMidStr("score=", ",", out end1, end1).ToInt();       // 取出玩家分数
                player.Level = line.GetMidStr("level=", ",", out end1, end1).ToInt();       // 取出玩家等级
                
                player.IP = line.GetMidStr("ip=", ",", out end1, end1);                     // 取出玩家IP地址
                player.Ping = line.GetMidStr("ping=", Environment.NewLine, end1).ToInt();   // 取出玩家网络延迟

                players[playerSteamID] = player;
            }

            // 移除离线玩家
            foreach (var key in keys)
            {
                if (steamIDs.Contains(key) == false)
                {
                    players.Remove(key);
                }
                else
                {
                    steamIDs.Remove(key);
                }
            }

            SdtdConsole.Instance.RaiseReceivedOnlinePlayerInfoEvent(players.Values.ToList());
        }

        /// <summary>
        /// 解析历史玩家数据
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static void ParseHistoryPlayers(List<string> lines)
        {

        }

        /// <summary>
        /// 解析管理员数据
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static void ParseAdmins(List<string> lines)
        {
        }

        /// <summary>
        /// 解析权限数据
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static void ParsePermissions(List<string> lines)
        {
        }

        /// <summary>
        /// 解析白名单数据
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static void ParseWhitelist(List<string> lines)
        {
        }

        /// <summary>
        /// 解析黑名单数据
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static void ParseBanlist(List<string> lines)
        {
        }

        /// <summary>
        /// 解析领地石列表数据
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static void ParseKeystoneBlockList(List<string> lines)
        {
        }

        /// <summary>
        /// 解析活动实体列表数据
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static void ParseActiveEntityList(List<string> lines)
        {
        }

        /// <summary>
        /// 解析可用实体列表数据
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static void ParseCanUseEntityList(List<string> lines)
        {
        }
    }
}
