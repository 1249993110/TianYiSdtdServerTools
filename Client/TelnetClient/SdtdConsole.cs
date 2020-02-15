using IceCoffee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.Network.Sockets.MulitThreadTcpClient;
using TianYiSdtdServerTools.Client.TelnetClient.Internal;

namespace TianYiSdtdServerTools.Client.TelnetClient
{
    /// <summary>
    /// 7daystodie控制台
    /// </summary>
    public class SdtdConsole : Singleton3<SdtdConsole>
    {
        #region 字段
        private Internal.TcpClient _sdtdClient;

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
        /// 连接状态改变
        /// </summary>
        public event ConnectionStateChangedEventHandler ConnectionStateChanged;

        public event RecvDataEventHandler RecvData;
        #endregion

        #region 方法

        #region 构造方法
        private SdtdConsole()
        {
            _sdtdClient = new TcpClient();
            _sdtdClient.ConnectionStateChanged += PrivateConnectionStateChanged;
        }
        #endregion

        #region 私有方法
        private void PrivateConnectionStateChanged(IceCoffee.Network.Sockets.MulitThreadTcpClient.ConnectionState connectionState)
        {
            if (connectionState != IceCoffee.Network.Sockets.MulitThreadTcpClient.ConnectionState.Connected)
            {
                _connectionState = (ConnectionState)connectionState;
                ConnectionStateChanged?.Invoke(_connectionState);
            }
        }
        #endregion

        #region 保护方法

        #endregion

        #region 公开方法
        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="ipStr"></param>
        /// <param name="port"></param>
        public void ConnectServer(string ipStr, ushort port)
        {
            _sdtdClient.Connect(ipStr, port);
        }

        #endregion

        #region 其他方法
        /// <summary>
        /// 引发收到数据事件
        /// </summary>
        internal void RaiseRecvDataEvent(string data)
        {
            RecvData?.Invoke(data);
        }
        #endregion

        #endregion

    }
}
