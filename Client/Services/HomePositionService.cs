using IceCoffee.DbCore.CatchServiceException;
using IceCoffee.DbCore.Primitives.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Dtos;
using TianYiSdtdServerTools.Client.Models.Entitys;
using TianYiSdtdServerTools.Client.Services.Primitives;

namespace TianYiSdtdServerTools.Client.Services
{
    public class HomePositionService : BaseService<HomePosition, HomePositionDto>
    {
        public override string IdColumnName { get { return "SteamID"; } }
        
        /// <summary>
        /// 通过SteamID和HomeName获取数据
        /// </summary>
        /// <returns></returns>
        [CatchSyncException("通过SteamID和HomeName获取数据异常")]
        public HomePositionDto GetDataBySteamIDAndHomeName(string steamID, string homeName)
        {
            return EntityToDto(Repository.QueryAny(RepositoryBase<HomePosition,string>.Select_Statement,
                "SteamID=@SteamID AND HomeName=@HomeName LIMIT 1", null,
                new HomePosition() { SteamID = steamID, HomeName = homeName }).FirstOrDefault());
        }

        /// <summary>
        /// 通过SteamID获取数据
        /// </summary>
        /// <returns></returns>
        [CatchSyncException("通过SteamID获取数据异常")]
        public List<HomePositionDto> GetDataBySteamID(string steamID)
        {
            List<HomePositionDto> result = new List<HomePositionDto>();
            var entitys = Repository.QueryById(steamID, IdColumnName);
            foreach (var entity in entitys)
            {
                result.Add(EntityToDto(entity));
            }
            return result;
        }
    }
}
