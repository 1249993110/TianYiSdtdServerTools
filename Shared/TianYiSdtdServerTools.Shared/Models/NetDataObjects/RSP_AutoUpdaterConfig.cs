using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Shared.Models.NetDataObjects
{
    [Serializable]
    public class RSP_AutoUpdaterConfig : NetDataObject
    {
        public RSP_AutoUpdaterConfig() : base(NetDataType.RSP_AutoUpdaterConfig)
        {
             
        }

        /// <summary>
        /// 更新配置文件的Url
        /// </summary>
        public string XmlUrl { get; set; }
    }
}
