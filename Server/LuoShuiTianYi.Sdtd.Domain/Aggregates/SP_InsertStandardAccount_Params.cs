using System;
using System.Collections.Generic;
using System.Text;

namespace LuoShuiTianYi.Sdtd.Domain.Aggregates
{
    public class SP_InsertStandardAccount_Params
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 密码哈希值
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 上次登录IP
        /// </summary>
        public string LastLoginIP { get; set; }

        /// <summary>
        /// 用户角色ID
        /// </summary>
        public int RoleID { get; set; } = -1;

        /// <summary>
        /// 使用期限
        /// </summary>
        public DateTime? ExpiryTime { get; set; }

        public SP_InsertStandardAccount_Params()
        {
        }

        public SP_InsertStandardAccount_Params(string userID, string passwordHash,
             string displayName, DateTime expiryTime, int roleID)
        {
            UserID = userID;
            PasswordHash = passwordHash;
            DisplayName = displayName;
            ExpiryTime = expiryTime;
            RoleID = roleID;
        }
    }
}
