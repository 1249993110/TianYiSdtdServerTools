using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Chat;
using TianYiSdtdServerTools.Client.Models.Players;
using TianYiSdtdServerTools.Client.Models.SdtdServerInfo;

namespace TianYiSdtdServerTools.Client.TelnetClient
{
    /// <summary>
    /// 收到一行事件处理器
    /// </summary>
    /// <param name="line"></param>
    public delegate void ReceiveLineEventHandler(string line);

    /// <summary>
    /// 连接状态改变事件处理器
    /// </summary>
    /// <param name="connectionState"></param>
    public delegate void ConnectionStateChangedEventHandler(ConnectionState connectionState);

    /// <summary>
    /// 游戏时间变化事件处理器
    /// </summary>
    /// <param name="gameDateTime"></param>
    public delegate void GameDateTimeChangedEventHandler(GameDateTime gameDateTime);

    /// <summary>
    /// 收到服务器部分首选项 事件处理器
    /// </summary>
    /// <param name="serverPartialPref"></param>
    public delegate void ReceivedServerPartialPrefEventHandler(ServerPartialPref serverPartialPref);

    /// <summary>
    /// 玩家聊天信息钩子事件处理器
    /// </summary>
    /// <param name="playerInfo"></param>
    /// <param name="chatType"></param>
    /// <param name="message"></param>
    public delegate void ChatHookEventHandler(PlayerInfo playerInfo, ChatType chatType, string message);

    /// <summary>
    /// 玩家进入游戏事件处理器
    /// </summary>
    /// <param name="playerInfo"></param>
    public delegate void PlayerEnterGameEventHandler(PlayerInfo playerInfo);

    /// <summary>
    /// 玩家离开游戏事件处理器
    /// </summary>
    /// <param name="playerInfo"></param>
    public delegate void PlayerLeftGameEventHandler(PlayerInfo playerInfo);

    /// <summary>
    /// 玩家死亡事件处理器
    /// </summary>
    /// <param name="playerInfo"></param>
    public delegate void PlayerDiedEventHandler(PlayerInfo playerInfo);

    /// <summary>
    /// 玩家被击杀事件处理器
    /// </summary>
    /// <param name="deadPlayerInfo"></param>
    /// <param name="killerPlayerInfo"></param>
    public delegate void PlayerKilledByEventHandler(PlayerInfo deadPlayerInfo, PlayerInfo killerPlayerInfo);
}
