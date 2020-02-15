using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.Common;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;

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
        public string GameDateTime { get; set; } = "Day 1,1";


        /// <summary>
        /// 游戏天数
        /// </summary>
        [NotNPC_Method]
        public int GameDays { get; set; }

        /// <summary>
        /// 在线玩家数
        /// </summary>
        public int OnlinePlayerCount { get; set; }

        /// <summary>
        /// 僵尸数
        /// </summary>
        public int ZombieCount { get; set; }

        /// <summary>
        /// 实体数
        /// </summary>
        public int EntityCount { get; set; }

        /// <summary>
        /// 连接状态
        /// </summary>
        public string ConnectStateStr { get; set; }
    }
}
