using IceCoffee.DbCore.Primitives.Service;
using LuoShuiTianYi.Sdtd.Domain.Aggregates;
using LuoShuiTianYi.Sdtd.Domain.Entitys;
using LuoShuiTianYi.Sdtd.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuoShuiTianYi.Sdtd.Services.Contracts
{
    public interface IUserService : IServiceBaseGuid<UserDto>
    {
        /// <summary>
        /// 执行存储过程，插入多条与标准账户相关的数据
        /// </summary>
        /// <param name="standardAccount"></param>
        /// <returns></returns>
        bool InsertStandardAccount(SP_InsertStandardAccount_Params standardAccount);

        /// <summary>
        /// 使用视图查询用户多个属性
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        V_User GetByUserId(string userID);

        /// <summary>
        /// 使用视图查询所有用户
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IEnumerable<V_User> GetAllUser(string orderBy = "UserID");
    }
}
