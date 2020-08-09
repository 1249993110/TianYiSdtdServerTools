using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Shared.Models.NetDataObjects
{
    [Serializable]
    public class RSP_ClientInfo : NetDataObject
    {
        public RSP_ClientInfo() : base(NetDataType.RSP_ClientInfo)
        {

        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 上次登录IP
        /// </summary>
        public string LastLoginIP { get; set; }

        /// <summary>
        /// 用户角色ID
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 使用期限
        /// </summary>
        public DateTime ExpiryTime { get; set; }
    }
}
