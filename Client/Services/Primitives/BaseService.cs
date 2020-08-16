using IceCoffee.DbCore;
using IceCoffee.DbCore.CatchServiceException;
using IceCoffee.DbCore.Primitives;
using IceCoffee.DbCore.Primitives.Dto;
using IceCoffee.DbCore.Primitives.Entity;
using IceCoffee.DbCore.Primitives.Repository;
using IceCoffee.DbCore.Primitives.Service;
using IceCoffee.DbCore.UnitWork;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TianYiSdtdServerTools.Client.Services.Primitives
{
    public abstract class BaseService<TEntity, TDto> : ServiceBaseStr<TEntity, TDto>
        where TEntity : EntityBaseStr, new()
        where TDto : DtoBaseStr, new()
    {
        public abstract string IdColumnName { get; }

        public BaseService()
            : base(new SQLiteRepositoryStr<TEntity>(DatabaseConfig.DefaultSQLiteConnectionInfo))
        {
        }


        #region 默认实现

        #region 同步
        /// <summary>
        /// 通过ID获取数据
        /// </summary>
        /// <returns></returns>
        [CatchSyncException("通过ID获取数据异常")]
        public TDto GetDataByID<TId>(TId id)
        {
            return EntityToDto(Repository.QueryById(id, IdColumnName).FirstOrDefault());
        }
        #endregion

        #region 异步
        /// <summary>
        /// 删除数据
        /// </summary>
        [CatchAsyncException("删除数据异常")]
        public async Task RemoveByIdAsync(TDto dto)
        {
            if(dto != null)
            {
                await Repository.DeleteByIdAsync(typeof(TDto).GetProperty(IdColumnName).GetValue(dto) as string, IdColumnName);
            }
        }
        #endregion

        #endregion
    }
}
