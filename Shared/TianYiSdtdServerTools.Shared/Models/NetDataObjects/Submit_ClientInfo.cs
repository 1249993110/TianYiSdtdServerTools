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
        public VersionInfo ClientVersion { get; set; }

        /// <summary>
        /// 客户令牌
        /// </summary>
        public string ClientToken { get; set; }

        /// <summary>
        /// 是否已经授权
        /// </summary>
        public bool IsAuthorized { get; set; }
    }
}
