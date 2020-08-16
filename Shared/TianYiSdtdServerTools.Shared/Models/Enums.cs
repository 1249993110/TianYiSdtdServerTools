using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Shared.Models
{
    /// <summary>
    /// 消息框类型
    /// </summary>
    public enum MessageBoxType
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,

        /// <summary>
        /// 超时关闭
        /// </summary>
        Timeout
    }

    /// <summary>
    /// 登录类型
    /// </summary>
    public enum LoginType
    {
        /// <summary>
        /// 第一次登录
        /// </summary>
        First,

        /// <summary>
        /// 重新连接
        /// </summary>
        Reconnect
    }

    public enum RegisterAccountType
    {
        /// <summary>
        /// 普通账户
        /// </summary>
        StandardAccount,

        /// <summary>
        /// QQ账户
        /// </summary>
        QQAccount,
    }

    /// <summary>
    /// 角色
    /// </summary>
    public enum Role : int
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
