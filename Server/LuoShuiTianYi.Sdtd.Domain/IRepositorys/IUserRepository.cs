using IceCoffee.DbCore.Primitives.Repository;
using LuoShuiTianYi.Sdtd.Domain.Aggregates;
using LuoShuiTianYi.Sdtd.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuoShuiTianYi.Sdtd.Domain.IRepositorys
{
    public interface IUserRepository: IRepositoryBase<T_User, Guid>
    {
        /// <summary>
        /// 执行存储过程，插入多条与标准账户相关的数据
        /// </summary>
        /// <param name="standardAccount"></param>
        /// <returns></returns>
        bool ExecProc_InsertStandardAccount(SP_InsertStandardAccount_Params standardAccount);

        /// <summary>
        /// 使用视图查询用户多个属性
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        V_User QueryByUserId_View(string userID);

        /// <summary>
        /// 使用视图查询所有用户
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IEnumerable<V_User> QueryAll_View(string orderBy = "UserID");
    }
}
