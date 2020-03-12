using TianYiSdtdServerTools.Client.Models.Dtos.Primitives;
using TianYiSdtdServerTools.Client.Models.Entitys;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;

namespace TianYiSdtdServerTools.Client.Models.Dtos
{
    public class ScoreInfoModel : MyDtoBase<ScoreInfoModel, ScoreInfo>
    {
        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// SteamID
        /// </summary>
        public string SteamID { get; set; }

        /// <summary>
        /// 拥有积分
        /// </summary>
        public int ScoreOwned { get; set; }

        /// <summary>
        /// 上次签到天数
        /// </summary>
        public int LastSignDate { get; set; }
    }
}
