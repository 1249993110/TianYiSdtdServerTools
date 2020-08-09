using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Shared.Models.NetDataObjects
{
    [Serializable]
    public class REQ_ClientInfo : NetDataObject
    {
        public REQ_ClientInfo() : base(NetDataType.REQ_ClientInfo)
        {
        }

        /// <summary>
        /// 客户版本
        /// </summary>
        public Version ClientVersion { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 密码哈希值
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// 是否已经授权
        /// </summary>
        public bool IsAuthorized { get; set; }
    }
}
