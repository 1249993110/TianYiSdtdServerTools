using IceCoffee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Chat;
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

        private ConnectionState _connectionState;
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
            internal set
            {
                _connectionState = value;
                RaiseConnectionStateChangedEvent(_connectionState);
            }
            get { return _connectionState; }
        }
        /// <summary>
        /// 是否成功连接，且密码正确
        /// </summary>
        public bool IsConnected
        {
            get { return _connectionState == ConnectionState.Connected; }
        }
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
        public event PlayerKilledByEventHandler PlayerKilledBy;
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
                ConnectionState = (ConnectionState)connectionState;
            }
        }


        private void OnExceptionCaught(IceCoffee.Network.CatchException.NetworkException e)
        {

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
            if(_tcpClient.IsConnected)
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
        internal void RaiseRecvDataEvent(string data)
        {
            ReceiveLine?.Invoke(data);
        }

        /// <summary>
        /// 引发连接状态改变事件
        /// </summary>
        internal void RaiseConnectionStateChangedEvent(ConnectionState connectionState)
        {
            ConnectionStateChanged?.Invoke(connectionState);
        }

        /// <summary>
        /// 引发游戏时间变化事件
        /// </summary>
        internal void RaiseGameDateTimeChangedEvent(GameDateTime gameDateTime)
        {
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
        /// 引发玩家聊天信息钩子事件
        /// </summary>
        internal void RaiseChatHookEvent(PlayerInfo playerInfo, ChatType chatType, string message)
        {
            ChatHook?.Invoke(playerInfo, chatType, message);
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
        /// 引发玩家被击杀事件事件
        /// </summary>
        internal void RaisePlayerKilledByEvent(PlayerInfo deadPlayerInfo, PlayerInfo killerPlayerInfo)
        {
            PlayerKilledBy?.Invoke(deadPlayerInfo, killerPlayerInfo);
        }        
        #endregion

        #endregion

    }
}
