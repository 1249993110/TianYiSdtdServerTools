using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;

namespace TianYiSdtdServerTools.Client.Models.ObservableClasses
{
    /// <summary>
    /// 服务器首选项
    /// </summary>
    [NPCA_Class]
    public class SdtdServerPrefModel : ObservableObject
    {
        /// <summary>
        /// 服务器IP地址
        /// </summary>
        public string ServerIP { get; set; }

        /// <summary>
        /// telnet端口
        /// </summary>
        public ushort? TelnetPort { get; set; }

        /// <summary>
        /// telnet密码
        /// </summary>
        public string TelnetPassword { get; set; }

        /// <summary>
        /// GPS端口
        /// </summary>
        public ushort? GPSPort { get; set; }          

        /// <summary>
        /// 服务器版本
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 物理内存
        /// </summary>
        public string RSS { get; set; } = string.Empty;

        /// <summary>
        /// 游戏端口
        /// </summary>
        public ushort GamePort { get; set; }

        /// <summary>
        /// 最大玩家数
        /// </summary>
        public int MaxPlayerCount { get; set; }

        /// <summary>
        /// 游戏模式
        /// </summary>
        public string GameMode { get; set; } = string.Empty;

        /// <summary>
        /// 游戏地图
        /// </summary>
        public string GameWorld { get; set; } = string.Empty;

        /// <summary>
        /// 游戏名称
        /// </summary>
        public string GameName { get; set; } = string.Empty;

        /// <summary>
        /// 游戏难度  0 - 5, 0=简单, 5=困难
        /// </summary>
        public int GameDifficulty { get; set; }
    }
}
