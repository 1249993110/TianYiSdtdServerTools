using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Entitys;
using TianYiSdtdServerTools.Client.Models.Dtos;
using TianYiSdtdServerTools.Client.Services.Primitives;
using TianYiSdtdServerTools.Client.Services.CatchException;
using System.Threading;
using System.Diagnostics;

namespace TianYiSdtdServerTools.Client.Services
{
    public class ScoreService : MyServiceBase<ScoreInfo>
    {
        private const string tableName = nameof(ScoreInfo);

        private const string keyName = "SteamID";

        public ScoreService() : base(false)
        {

        }

        /// <summary>
        /// 插入积分信息
        /// </summary>
        /// <param name="scoreInfoDto"></param>
        [CatchException("插入积分信息异常")]
        public async Task InsertScoreInfo(ScoreInfoDto scoreInfoDto)
        {
            await Repository.InsertDataAsync(scoreInfoDto.CreateEntity());
        }

        /// <summary>
        /// 得到全部积分信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScoreInfoDto>> GetAllScoreInfo()
        {
            List<ScoreInfoDto> scoreInfoDtos = new List<ScoreInfoDto>();
            foreach (var item in await Repository.QueryAllDataAsync(tableName))
            {
                scoreInfoDtos.Add(item.ToDto());
            }
            return scoreInfoDtos;
        }

        /// <summary>
        /// 更新积分信息
        /// </summary>
        /// <param name="scoreInfoDto"></param>
        [CatchException("更新积分信息异常")]
        public async Task UpdateScoreInfo(ScoreInfoDto scoreInfoDto)
        {
            await Repository.UpdateDataByIDAsync(scoreInfoDto.ColumnNames, keyName, scoreInfoDto.ToEntity());
        }

        /// <summary>
        /// 删除积分信息
        /// </summary>
        /// <param name="scoreInfoDto"></param>
        [CatchException("删除积分信息异常")]
        public async Task RemoveItem(ScoreInfoDto scoreInfoDto)
        {
            await Repository.DeleteDataByIDAsync(tableName, keyName, scoreInfoDto.SteamID);
        }

        /// <summary>
        /// 重置签到天数
        /// </summary>
        [CatchException("重置签到天数异常")]
        public async Task ResetLastSignDate()
        {
            await Repository.UpdateDataAsync(tableName, "LastSignDate", 0);
        }

        /// <summary>
        /// 删除全部积分信息
        /// </summary>
        [CatchException("删除全部积分信息异常")]
        public async Task RemoveAll()
        {
            await Repository.DeleteAllDataAsync(tableName);
        }
    }
}
