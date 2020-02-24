using IceCoffee.Common;
using IceCoffee.Common.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Chat;
using TianYiSdtdServerTools.Client.Models.ConsoleTempList;
using TianYiSdtdServerTools.Client.Models.Players;
using TianYiSdtdServerTools.Client.Models.SdtdServerInfo;
using TianYiSdtdServerTools.Client.TelnetClient.Internal;

namespace TianYiSdtdServerTools.Client.TelnetClient
{
    /// <summary>
    /// 7daystodie控制台
    /// </summary>
    public partial class SdtdConsole : Singleton3<SdtdConsole>
    {
        #region 字段
        private TcpClient _tcpClient;

        private ConnectionState _connectionState = ConnectionState.Disconnected;
        #endregion

        #region 属性
        /// <summary>
        /// Telnet密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 连接状态
        /// </summary>
        public ConnectionState ConnectionState
        { 
            get { return _connectionState; }
        }

        /// <summary>
        /// 是否成功连接，且密码正确
        /// </summary>
        public bool IsConnected
        {
            get { return _connectionState == ConnectionState.Connected; }
        }

        /// <summary>
        /// 游戏时间
        /// </summary>
        public GameDateTime GameDateTime { get; private set; }

        /// <summary>
        /// 游戏玩家
        /// </summary>
        public IReadOnlyDictionary<string,PlayerInfo> OnlinePlayers { get { return _tcpClient.Session?.OnlinePlayers; } }

        /// <summary>
        /// 服务器版本
        /// </summary>
        public ServerVersion ServerVersion { get; internal set; } = ServerVersion.EarlierVersion;
        #endregion

        #region 事件
        /// <summary>
        /// 收到一行
        /// </summary>
        public event ReceiveLineEventHandler ReceiveLine;

        /// <summary>
        /// 连接状态改变
        /// </summary>
        public event ConnectionStateChangedEventHandler ConnectionStateChanged;

        /// <summary>
        /// 游戏时间变化
        /// </summary>
        public event GameDateTimeChangedEventHandler GameDateTimeChanged;

        /// <summary>
        /// 收到服务器部分首选项 
        /// </summary>
        public event ReceivedServerPartialPrefEventHandler ReceivedServerPartialPref;

        /// <summary>
        /// 收到服务器部分状态
        /// </summary>
        public event ReceivedServerPartialStateEventHandler ReceivedServerPartialState;

        /// <summary>
        /// 玩家聊天信息钩子
        /// </summary>
        public event ChatHookEventHandler ChatHook;

        /// <summary>
        /// 玩家进入游戏
        /// </summary>
        public event PlayerEnterGameEventHandler PlayerEnterGame;

        /// <summary>
        /// 玩家离开游戏
        /// </summary>
        public event PlayerLeftGameEventHandler PlayerLeftGame;

        /// <summary>
        /// 玩家死亡
        /// </summary>
        public event PlayerDiedEventHandler PlayerDied;

        /// <summary>
        /// 玩家被击杀
        /// </summary>
        public event EntityKilledEventHandler EntityKilled;

        /// <summary>
        /// 收到在线玩家信息
        /// </summary>
        public event ReceivedOnlinePlayerInfoEventHandler ReceivedOnlinePlayerInfo;

        /// <summary>
        /// 收到临时列表数据
        /// </summary>
        public event ReceivedTempListDataEventHandler ReceivedTempListData;

        /// <summary>
        /// 服务器无玩家
        /// </summary>
        public event ServerNonePlayerEventHandler ServerNonePlayer;

        /// <summary>
        /// 服务器再一次有玩家
        /// </summary>
        public event ServerHavePlayerAgainEventHandler ServerHavePlayerAgain;
        #endregion

        #region 方法

        #region 构造方法
        private SdtdConsole()
        {
            _tcpClient = new TcpClient();
            _tcpClient.ConnectionStateChanged += OnPrivateConnectionStateChanged;
            _tcpClient.ExceptionCaught += OnExceptionCaught;
        }

        #endregion

        #region 私有方法

        private void OnPrivateConnectionStateChanged(
            IceCoffee.Network.Sockets.MulitThreadTcpClient.ConnectionState connectionState)
        {
            if (connectionState != IceCoffee.Network.Sockets.MulitThreadTcpClient.ConnectionState.Connected)
            {
                this.RaiseConnectionStateChangedEvent((ConnectionState)connectionState);
            }
        }


        private void OnExceptionCaught(IceCoffee.Network.CatchException.NetworkException e)
        {
            Log.Error("Telnet异常捕获", e);
        }


        #endregion

        #region 公开方法
        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="ipStr"></param>
        /// <param name="port"></param>
        /// <param name="password"></param>
        public void ConnectServer(string ipStr, ushort port, string password)
        {
            Password = password;
            this.Disconnect();
            _tcpClient.Connect(ipStr, port);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if(IsConnected)
            {
                this.SendCmd("exit");
                _tcpClient.Disconnect();
            }
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 引发收到一行事件
        /// </summary>
        internal void RaiseRecvLineEvent(string data)
        {
            ReceiveLine?.Invoke(data);
        }

        /// <summary>
        /// 引发连接状态改变事件
        /// </summary>
        internal void RaiseConnectionStateChangedEvent(ConnectionState connectionState)
        {
            _connectionState = connectionState;
            ConnectionStateChanged?.Invoke(_connectionState);
        }

        /// <summary>
        /// 引发游戏时间变化事件
        /// </summary>
        internal void RaiseGameDateTimeChangedEvent(GameDateTime gameDateTime)
        {
            GameDateTime = gameDateTime;
            GameDateTimeChanged?.Invoke(gameDateTime);
        }

        /// <summary>
        /// 引发收到服务器部分首选项事件
        /// </summary>
        internal void RaiseReceivedServerPartialPrefEvent(ServerPartialPref serverPartialPref)
        {
            ReceivedServerPartialPref?.Invoke(serverPartialPref);
        }

        /// <summary>
        /// 引发收到服务器部分状态事件
        /// </summary>
        /// <param name="serverPartialState"></param>
        internal void RaiseReceivedServerPartialStateEvent(ServerPartialState serverPartialState)
        {
            ReceivedServerPartialState.Invoke(serverPartialState);
        }

        /// <summary>
        /// 引发玩家聊天信息钩子事件
        /// </summary>
        internal void RaiseChatHookEvent(PlayerInfo playerInfo, string message, ChatType chatType, SenderType senderType)
        {
            ChatHook?.Invoke(playerInfo, message, chatType, senderType);
        }

        /// <summary>
        /// 引发玩家进入游戏事件
        /// </summary>
        internal void RaisePlayerEnterGameEvent(PlayerInfo playerInfo)
        {
            PlayerEnterGame?.Invoke(playerInfo);
        }

        /// <summary>
        /// 引发玩家离开游戏事件
        /// </summary>
        internal void RaisePlayerLeftGameEvent(PlayerInfo playerInfo)
        {
            PlayerLeftGame?.Invoke(playerInfo);
        }

        /// <summary>
        /// 引发玩家死亡事件
        /// </summary>
        internal void RaisePlayerDiedEvent(PlayerInfo playerInfo)
        {
            PlayerDied?.Invoke(playerInfo);
        }

        /// <summary>
        /// 引发实体被击杀事件
        /// </summary>
        internal void RaiseEntityKilledEvent(int killerEntityID, int deadEntityID)
        {
            EntityKilled?.Invoke(killerEntityID, deadEntityID);
        }

        /// <summary>
        /// 引发收到在线玩家信息事件
        /// </summary>
        internal void RaiseReceivedOnlinePlayerInfoEvent(List<PlayerInfo> players)
        {
            ReceivedOnlinePlayerInfo?.Invoke(players);
        }

        /// <summary>
        /// 引发收到临时列表数据事件
        /// </summary>
        internal void RaiseReceivedTempListDataEvent(object twoDimensionalList, TempListDataType tempListDataType)
        {
            ReceivedTempListData?.Invoke(twoDimensionalList, tempListDataType);
        }

        /// <summary>
        /// 引发服务器无玩家事件
        /// </summary>
        internal void RaiseServerNonePlayerEvent()
        {
            ServerNonePlayer?.Invoke();
        }

        /// <summary>
        /// 引发服务器再一次有玩家事件
        /// </summary>
        internal void RaiseServerHavePlayerAgainEvent()
        {
            ServerHavePlayerAgain?.Invoke();
        }
        #endregion

        #endregion

    }
}
