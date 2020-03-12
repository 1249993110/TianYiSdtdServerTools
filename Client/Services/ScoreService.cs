using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Entitys;
using TianYiSdtdServerTools.Client.Models.Dtos;
using TianYiSdtdServerTools.Client.Services.Primitives;
using TianYiSdtdServerTools.Client.Services.CatchException;

namespace TianYiSdtdServerTools.Client.Services
{
    public class ScoreService : MyServiceBase<ScoreInfo>
    {
        private const string tableName = nameof(ScoreInfo);

        private const string keyName = "SteamID";

        public ScoreService()
        {
            
        }

        /// <summary>
        /// 插入积分信息
        /// </summary>
        /// <param name="scoreDataModel"></param>
        [CatchException("插入积分信息异常")]
        public void InsertScoreInfo(ScoreInfoModel scoreDataModel)
        {
            Repository.InsertData(scoreDataModel.CreateEntity());
        }

        /// <summary>
        /// 得到全部积分信息
        /// </summary>
        /// <returns></returns>
        [CatchException("读取全部积分信息异常")]
        public List<ScoreInfoModel> GetAllScoreInfo()
        {
            List<ScoreInfoModel> scoreInfoModels = new List<ScoreInfoModel>();
            foreach (var item in Repository.QueryAllData(tableName))
            {
                scoreInfoModels.Add(item.ToDto());
            }
            return scoreInfoModels;
        }

        /// <summary>
        /// 更新积分信息
        /// </summary>
        /// <param name="scoreDataModel"></param>
        [CatchException("更新积分信息异常")]
        public void UpdateScoreInfo(ScoreInfoModel scoreDataModel)
        {
            Repository.UpdateDataByID(scoreDataModel.ColumnNames, keyName, scoreDataModel.ToEntity());
        }

        /// <summary>
        /// 删除积分信息
        /// </summary>
        /// <param name="scoreDataModel"></param>
        [CatchException("删除积分信息异常")]
        public void RemoveItem(ScoreInfoModel scoreDataModel)
        {
            Repository.DeleteDataByID(tableName, keyName, scoreDataModel.SteamID);
        }

        /// <summary>
        /// 重置签到天数
        /// </summary>
        [CatchException("重置签到天数异常")]
        public void ResetLastSignDate()
        {
            Repository.UpdateData(tableName, "LastSignDate", 0);
        }

        /// <summary>
        /// 删除全部积分信息
        /// </summary>
        [CatchException("删除全部积分信息异常")]
        public void RemoveAll()
        {
            Repository.DeleteAllData(tableName);
        }
    }
}
