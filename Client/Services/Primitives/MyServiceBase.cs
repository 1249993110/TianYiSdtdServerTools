using IceCoffee.DbCore;
using IceCoffee.DbCore.Primitives;
using IceCoffee.DbCore.Repositorys;
using IceCoffee.DbCore.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Dtos.Primitives;
using TianYiSdtdServerTools.Client.Models.Entitys.Primitives;
using TianYiSdtdServerTools.Client.Services.CatchException;

namespace TianYiSdtdServerTools.Client.Services.Primitives
{
    public abstract class MyServiceBase<TEntity, TDto> : ServiceBase<TEntity>, IExceptionCaughtSignal
        where TEntity : MyEntityBase<TEntity, TDto>, new()
        where TDto : MyDtoBase<TDto, TEntity>, new()
    {
        public virtual string TableName { get { return MyEntityBase<TEntity, TDto>.Default_Db_TableName; } }

        public virtual string KeyName { get { return MyEntityBase<TEntity, TDto>.KeyName; } }

        public abstract string IdColumnName { get; }

        public MyServiceBase(bool useConnectionPool = false)
            : base(new SQLiteRepository<TEntity>(Database.DefaultDbConnectionString, useConnectionPool))
        {
        }

        /// <summary>
        /// 捕获异步服务层异常
        /// </summary>
        public static event AsyncExceptionCaughtEventHandler AsyncExceptionCaught;

        void IExceptionCaughtSignal.EmitAsyncExceptionCaughtSignal(object sender, ServiceException e)
        {
            AsyncExceptionCaught?.Invoke(this, e);
        }

        /// <summary>
        /// 是否自动处理异步服务层异常
        /// </summary>
        public bool IsAutoHandleAsyncServiceException { get; set; } = true;

        #region 默认实现

        #region 同步
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="columnName"></param>
        /// <param name="asc">使用升序排序</param>
        /// <returns></returns>
        [CatchSyncException("获取全部数据异常")]
        public List<TDto> GetAllData(bool orderBy = false, string[] columnNames = null, bool asc = true)
        {
            List<TDto> dtos = new List<TDto>();
            foreach (var item in Repository.QueryAllData(TableName,
                orderBy ? string.Format("ORDER BY {0} {1}", 
                columnNames.Length == 0 ? columnNames[0] : string.Join(",", columnNames), asc ? "ASC" : "DESC") : null))
            {
                dtos.Add(item.ToDto());
            }
            return dtos;
        }

        /// <summary>
        /// 通过ID获取数据
        /// </summary>
        /// <returns></returns>
        [CatchSyncException("通过ID获取数据异常")]
        public TDto GetDataByID<TId>(TId id)
        {
            return Repository.QueryOneDataByID(TableName, IdColumnName, id)?.ToDto();
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <returns></returns>
        [CatchSyncException("插入数据异常")]
        public void InsertData(TDto dto)
        {
            Repository.InsertData(dto.CreateEntity());
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        [CatchSyncException("更新数据异常")]
        public void UpdateData(TDto dto)
        {
            Repository.UpdateDataByID(IdColumnName, dto.NeedUpdateColumnNames, dto.ToEntity());
        }

        #endregion

        #region 异步
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="columnName"></param>
        /// <param name="asc">使用升序排序</param>
        /// <returns></returns>
        [CatchAsyncException("获取全部数据异常")]
        public async Task<List<TDto>> GetAllDataAsync(bool orderBy = false, string[] columnNames = null, bool asc = true)
        {
            List<TDto> dtos = new List<TDto>();
            foreach (var item in await Repository.QueryAllDataAsync(TableName, 
                orderBy ? string.Format("ORDER BY {0} {1}", 
                columnNames.Length == 0 ? columnNames[0] : string.Join(",", columnNames), asc ? "ASC" : "DESC") : null))
            {
                dtos.Add(item.ToDto());
            }
            return dtos;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <returns></returns>
        [CatchAsyncException("插入数据异常")]
        public async Task InsertDataAsync(TDto dto)
        {
            await Repository.InsertDataAsync(dto.CreateEntity());
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        [CatchAsyncException("更新数据异常")]
        public async Task UpdateDataAsync(TDto dto)
        {
            await Repository.UpdateDataByIDAsync(IdColumnName, dto.NeedUpdateColumnNames, dto.ToEntity());
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        [CatchAsyncException("更新数据异常")]
        public async Task UpdateDataByKeyAsync(TDto dto)
        {
            await Repository.UpdateDataByIDAsync(KeyName, dto.NeedUpdateColumnNames, dto.ToEntity());
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        [CatchAsyncException("删除数据异常")]
        public async Task RemoveDataAsync(TDto dto)
        {
            await Repository.DeleteDataAsync(string.Format("WHERE {0}=@{0}", IdColumnName), dto.ToEntity());
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        [CatchAsyncException("删除数据异常")]
        public async Task RemoveDataByKeyAsync(TDto dto)
        {
            await Repository.DeleteDataAsync(string.Format("WHERE {0}=@{0}", KeyName), dto.ToEntity());
        }

        /// <summary>
        /// 删除全部数据
        /// </summary>
        [CatchAsyncException("删除全部数据异常")]
        public async Task RemoveAllDataAsync()
        {
            await Repository.DeleteAllDataAsync(TableName);
        }
        #endregion

        #endregion
    }
}
