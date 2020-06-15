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
    public enum UserRole : uint
    {
        /// <summary>
        /// 黑名单用户
        /// </summary>
        BlacklistUser = 0,

        /// <summary>
        /// 普通用户
        /// </summary>
        NormalUser,
        
        /// <summary>
        /// 普通会员
        /// </summary>
        Vip,

        /// <summary>
        /// 优惠会员
        /// </summary>
        ConcessionVip,

        /// <summary>
        /// 管理员
        /// </summary>
        Administrator,

        /// <summary>
        /// 超级管理员
        /// </summary>
        SuperAdministrator
    }
}
