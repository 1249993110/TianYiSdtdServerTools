using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Shared.Models.NetDataObjects
{
    [Serializable]
    public class AutoUpdaterConfig : NetDataObject
    {
        public AutoUpdaterConfig() : base(NetDataType.Return_AutoUpdaterConfig)
        {
             
        }

        public string XmlUrl { get; set; }
    }
}
