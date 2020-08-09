using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Shared.Models.NetDataObjects
{
    [Serializable]
    public class Submit_ClientInfo : NetDataObject
    {
        public Submit_ClientInfo() : base(NetDataType.Submit_ClientInfo)
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
        /// 是否已经授权
        /// </summary>
        public bool IsAuthorized { get; set; }
    }
}
