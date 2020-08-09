using IceCoffee.DbCore.Primitives;
using IceCoffee.DbCore.Primitives.Dto;
using IceCoffee.DbCore.Primitives.Entity;
using IceCoffee.DbCore.Primitives.Repository;
using IceCoffee.DbCore.Primitives.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuoShuiTianYi.Sdtd.Services.Primitives
{
    public abstract class ServiceBase<TEntity, TDto> : ServiceBaseGuid<TEntity, TDto>
        where TEntity : EntityBaseGuid, new()
        where TDto : DtoBaseGuid, new()
    {
        public ServiceBase()
            : base(new SqlServerRepositoryGuid<TEntity>(DatabaseConfig.DefaultConnectionInfo))
        {
        }

        public ServiceBase(IRepositoryBase<TEntity, Guid> repository)
            : base(repository)
        {
        }
    }
}
