using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Shared.Primitives
{
    [Serializable]
    public class NetDataObject
    {
        public NetDataType NetDataType { get; private set; }

        public NetDataObject(NetDataType netDataType)
        {
            NetDataType = netDataType;
        }
    }
}
