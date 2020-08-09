using Dapper;
using IceCoffee.DbCore.Domain;
using IceCoffee.DbCore.Primitives.Repository;
using LuoShuiTianYi.Sdtd.Domain.Aggregates;
using LuoShuiTianYi.Sdtd.Domain.Entitys;
using LuoShuiTianYi.Sdtd.Domain.IRepositorys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LuoShuiTianYi.Sdtd.Domain.Repositorys
{
    public class UserRepository : SqlServerRepositoryGuid<T_User>, IUserRepository
    {
        public UserRepository(DbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }

        public bool ExecProc_InsertStandardAccount(SP_InsertStandardAccount_Params standardAccount)
        {
            string procName = "SP_InsertStandardAccount";
            SP_InsertStandardAccount_Params sa = standardAccount;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", sa.UserID, DbType.String, ParameterDirection.Input);
            parameters.Add("@DisplayName", sa.DisplayName, DbType.String, ParameterDirection.Input);
            parameters.Add("@LastLoginTime", sa.LastLoginTime, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@LastLoginIP", sa.LastLoginIP, DbType.String, ParameterDirection.Input);
            parameters.Add("@PasswordHash", sa.PasswordHash, DbType.String, ParameterDirection.Input);
            parameters.Add("@RoleID", sa.RoleID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ExpiryTime", sa.ExpiryTime, DbType.DateTime, ParameterDirection.Input);
            return base.ExecProcedure<int>(procName, parameters).FirstOrDefault() == 1;
        }

        public V_User QueryByUserId_View(string userID)
        {
            return base.QueryAny<V_User>("SELECT * FROM V_User WHERE UserID=@UserID",
                new { UserID = userID }).FirstOrDefault();
        }

        public IEnumerable<V_User> QueryAll_View(string orderBy = "UserID")
        {
            string sql = "SELECT * FROM V_User";
            if(string.IsNullOrEmpty(orderBy) == false)
            {
                sql = "SELECT * FROM V_User ORDER BY " + orderBy;
            }

            return base.QueryAny<V_User>(sql);
        }

    }
}
