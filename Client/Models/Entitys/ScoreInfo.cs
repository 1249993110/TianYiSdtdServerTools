using IceCoffee.DbCore.Primitives;

namespace TianYiSdtdServerTools.Client.Models.Entitys
{
    public class ScoreInfo : EntityBaseStr
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
