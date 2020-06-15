using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Shared.Models.NetDataObjects
{
    [Serializable]
    public class Return_ClientInfo : NetDataObject
    {
        public Return_ClientInfo() : base(NetDataType.Return_ClientInfo)
        {

        }

        /// <summary>
        /// 客户昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 大小为40×40像素的QQ头像URL
        /// </summary>
        public string Figureurl_40 { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 用户角色ID
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// 用户角色ID名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 会员过期时间
        /// </summary>
        public DateTime ExpiryTime { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }
    }
}
