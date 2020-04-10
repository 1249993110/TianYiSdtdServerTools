using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Dtos;
using TianYiSdtdServerTools.Client.Models.Entitys;
using TianYiSdtdServerTools.Client.Services.CatchException;
using TianYiSdtdServerTools.Client.Services.Primitives;

namespace TianYiSdtdServerTools.Client.Services
{
    public class HomePositionService : MyServiceBase<HomePosition, HomePositionDto>
    {
        public override string IdColumnName { get { return "SteamID"; } }

        /// <summary>
        /// 通过SteamID和HomeName获取数据
        /// </summary>
        /// <returns></returns>
        [CatchAsyncException("通过SteamID和HomeName删除数据异常")]
        public async Task RemoveByKeyAsync(HomePositionDto homePositionDto)
        {
            await Repository.DeleteDataByIDAsync(TableName, KeyName, homePositionDto.UUID);
        }

        /// <summary>
        /// 通过SteamID和HomeName获取数据
        /// </summary>
        /// <returns></returns>
        [CatchSyncException("通过SteamID和HomeName获取数据异常")]
        public HomePositionDto GetDataBySteamIDAndHomeName(string steamID, string homeName)
        {
            return Repository.QueryOneData("WHERE SteamID=@SteamID AND HomeName=@HomeName",
                new HomePosition() { SteamID = steamID, HomeName = homeName })?.ToDto();
        }

        /// <summary>
        /// 通过SteamID获取数据
        /// </summary>
        /// <returns></returns>
        [CatchSyncException("通过SteamID获取数据异常")]
        public List<HomePositionDto> GetDataBySteamID(string steamID)
        {
            List<HomePositionDto> result = new List<HomePositionDto>();
            var entitys = Repository.QueryData("WHERE SteamID=@SteamID", new HomePosition() { SteamID = steamID });
            foreach (var entity in entitys)
            {
                result.Add(entity.ToDto());
            }
            return result;
        }
    }
}
