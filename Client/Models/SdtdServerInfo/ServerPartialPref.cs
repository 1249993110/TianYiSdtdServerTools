using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.SdtdServerInfo
{
    /// <summary>
    /// 服务器部分首选项
    /// </summary>
    public class ServerPartialPref
    {
        /// <summary>
        /// 服务器版本
        /// </summary>
        public string VersionStr { get; set; } = string.Empty;

        /// <summary>
        /// 专用服务器
        /// </summary>
        public bool DedicatedServer { get; set; } = false;

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
