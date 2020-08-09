using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Shared.Models.UserGroups
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public enum UserRole : int
    {
        /// <summary>
        /// 黑名单用户
        /// </summary>
        BlacklistUser = -1,
  
        /// <summary>
        /// 超级管理员
        /// </summary>
        SuperAdministrator = 0,

        /// <summary>
        /// 管理员
        /// </summary>
        Administrator = 1,

        /// <summary>
        /// 优惠会员
        /// </summary>
        ConcessionVip = 2,

        /// <summary>
        /// 普通会员
        /// </summary>
        Vip = 3,

        /// <summary>
        /// 普通用户
        /// </summary>
        NormalUser = 4
    }
}
