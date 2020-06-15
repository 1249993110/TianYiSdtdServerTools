using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Entitys;
using TianYiSdtdServerTools.Client.Models.Dtos;
using TianYiSdtdServerTools.Client.Services.Primitives;

using System.Threading;
using System.Diagnostics;
using IceCoffee.DbCore.UnitOfWork;
using IceCoffee.Common.LogManager;
using IceCoffee.DbCore.CatchServiceException;

namespace TianYiSdtdServerTools.Client.Services
{
    public class ScoreInfoService : MyServiceBase<ScoreInfo, ScoreInfoDto>
    {
        public override string IdColumnName { get { return "SteamID"; } }

        /// <summary>
        /// 获取玩家拥有积分数量
        /// </summary>
        /// <param name="steamID"></param>
        /// <returns></returns>
        [CatchSyncException("获取玩家拥有积分数量异常")]
        public int GetPlayerScore(string steamID)
        {
            ScoreInfo scoreInfo = Repository.QueryOneById(steamID, IdColumnName);
            if (scoreInfo == null)
            {
                scoreInfo = ScoreInfo.Create<ScoreInfo>();
                scoreInfo.SteamID = steamID;
                Repository.InsertOne(scoreInfo);
                return 0;
            }
            else
            {
                return scoreInfo.ScoreOwned;
            }
        }

        /// <summary>
        /// 增加玩家积分
        /// </summary>
        /// <param name="steamID"></param>
        /// <param name="score"></param>
        [CatchSyncException("增加玩家积分异常")]
        public void IncreasePlayerScore(string steamID, int score)
        {
            if (Repository.UpdateAny("ScoreOwned=ScoreOwned+" + score.ToString(), "SteamID=" + steamID, null) == 0)
            {
                throw new Exception(steamID + ": 此玩家无积分记录");
            }
        }

        /// <summary>
        /// 扣除玩家积分
        /// </summary>
        /// <param name="steamID"></param>
        /// <param name="score"></param>
        [CatchSyncException("扣除玩家积分异常")]
        public void DeductPlayerScore(string steamID, int score)
        {
            if (Repository.UpdateAny("ScoreOwned=ScoreOwned-" + score.ToString(), "SteamID=" + steamID, null) == 0)
            {
                throw new Exception(steamID + ": 此玩家无积分记录");
            }
        }

        /// <summary>
        /// 重置签到天数
        /// </summary>
        [CatchAsyncException("重置签到天数异常")]
        public async Task ResetLastSignDateAsync()
        {
            await Repository.UpdateAnyAsync("LastSignDate=0", null, null);
        }
    }
}
