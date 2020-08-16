using IceCoffee.DbCore.Primitives.Repository;
using IceCoffee.DbCore.Primitives.Service;
using LuoShuiTianYi.Sdtd.Domain.Aggregates;
using LuoShuiTianYi.Sdtd.Domain.Entitys;
using LuoShuiTianYi.Sdtd.Domain.IRepositorys;
using LuoShuiTianYi.Sdtd.Domain.Repositorys;
using LuoShuiTianYi.Sdtd.Services.Contracts;
using LuoShuiTianYi.Sdtd.Services.Dtos;
using LuoShuiTianYi.Sdtd.Services.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuoShuiTianYi.Sdtd.Services.Impl
{
    public class UserService : ServiceBaseGuid<T_User, UserDto>, IUserService
    {
        new protected IUserRepository Repository => base.Repository as IUserRepository;

        public UserService() : base(new UserRepository(DatabaseConfig.DefaultConnectionInfo))
        {
            
        }

        public bool InsertStandardAccount(SP_InsertStandardAccount_Params standardAccount)
        {
            return Repository.ExecProc_InsertStandardAccount(standardAccount);
        }

        public V_User GetByUserId(string userID)
        {
            return Repository.QueryByUserId_View(userID);
        }

        public IEnumerable<V_User> GetAllUser(string orderBy = "UserID")
        {
            return Repository.QueryAll_View(orderBy);
        }
    }
}
