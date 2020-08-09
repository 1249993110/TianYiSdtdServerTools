using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.ConsoleTempList
{
    public enum TempListDataType
    {
        /// <summary>
        /// 历史玩家列表
        /// </summary>
        HistoryPlayerList,

        /// <summary>
        /// 管理员
        /// </summary>
        AdminList,        

        /// <summary>
        /// 权限列表
        /// </summary>
        PermissionList,

        /// <summary>
        /// 黑名单列表
        /// </summary>
        BanList,

        /// <summary>
        /// 白名单列表
        /// </summary>
        WhiteList,

        /// <summary>
        /// 领地石列表
        /// </summary>
        KeystoneBlockList,

        /// <summary>
        /// 活动实体列表
        /// </summary>
        ActiveEntityList,

        /// <summary>
        /// 可用实体列表
        /// </summary>
        AvailableEntityList
    };
}
