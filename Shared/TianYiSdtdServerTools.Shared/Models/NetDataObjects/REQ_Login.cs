using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Shared.Models.NetDataObjects
{
    [Serializable]
    public class REQ_Login : NetDataObject
    {
        public REQ_Login() : base(NetDataType.REQ_Login)
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

        /// <summary>
        /// 登录类型
        /// </summary>
        public LoginType LoginType { get; set; }
    }
}
