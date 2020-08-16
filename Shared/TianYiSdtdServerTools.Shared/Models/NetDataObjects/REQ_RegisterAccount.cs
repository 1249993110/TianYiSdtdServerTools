using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Shared.Models.NetDataObjects
{
    [Serializable]
    public class REQ_RegisterAccount : NetDataObject
    {
        public REQ_RegisterAccount() : base(NetDataType.REQ_RegisterAccount)
        {
        }

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
        /// 注册账户类型
        /// </summary>
        public RegisterAccountType RegisterAccountType { get; set; }
    }
}
