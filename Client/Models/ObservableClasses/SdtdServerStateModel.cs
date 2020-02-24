using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.Common;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using TianYiSdtdServerTools.Client.Models.SdtdServerInfo;

namespace TianYiSdtdServerTools.Client.Models.ObservableClasses
{
    /// <summary>
    /// 服务器状态
    /// </summary>
    [NPCA_Class]
    public class SdtdServerStateModel : ObservableObject
    {
        /// <summary>
        /// 游戏时间
        /// </summary>
        public GameDateTime GameDateTime { get; set; } = new GameDateTime() { Day = 1, Hour = 7, Minute = 0 };

        /// <summary>
        /// 连接状态
        /// </summary>
        public ConnectionState ConnectionState { get; set; } = ConnectionState.Disconnected;

        /// <summary>
        /// 服务器部分状态
        /// </summary>
        public ServerPartialState ServerPartialState { get; set; } = new ServerPartialState();
    }
}
