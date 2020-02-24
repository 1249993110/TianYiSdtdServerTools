using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Chat;
using TianYiSdtdServerTools.Client.Models.ConsoleTempList;
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
    /// 收到服务器部分状态 事件处理器
    /// </summary>
    /// <param name="serverPartialState"></param>
    public delegate void ReceivedServerPartialStateEventHandler(ServerPartialState serverPartialState);

    /// <summary>
    /// 玩家聊天信息钩子事件处理器
    /// </summary>
    /// <param name="playerInfo"></param>
    /// <param name="message"></param>
    /// <param name="chatType"></param>
    /// <param name="senderType"></param>
    public delegate void ChatHookEventHandler(PlayerInfo playerInfo, string message, ChatType chatType, SenderType senderType);

    /// <summary>
    /// 玩家进入游戏事件处理器
    /// </summary>
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
    /// 实体被击杀事件处理器
    /// </summary>
    /// <param name="killerEntityID"></param>
    /// <param name="deadEntityID"></param>
    public delegate void EntityKilledEventHandler(int killerEntityID, int deadEntityID);

    /// <summary>
    /// 收到在线玩家信息事件处理器
    /// </summary>
    /// <param name="players"></param>
    public delegate void ReceivedOnlinePlayerInfoEventHandler(List<PlayerInfo> players);

    /// <summary>
    /// 收到临时列表数据事件处理器
    /// </summary>
    /// <param name="twoDimensionalList">二维列表</param>
    public delegate void ReceivedTempListDataEventHandler(object twoDimensionalList, TempListDataType tempListDataType);

    /// <summary>
    /// 服务器无玩家事件处理器
    /// </summary>
    public delegate void ServerNonePlayerEventHandler();

    /// <summary>
    /// 服务器再一次有玩家事件处理器
    /// </summary>
    public delegate void ServerHavePlayerAgainEventHandler();
}
