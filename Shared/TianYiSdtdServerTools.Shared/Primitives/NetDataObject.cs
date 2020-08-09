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
        private NetDataType _netDataType;

        public NetDataType NetDataType => _netDataType;

        public NetDataObject(NetDataType netDataType)
        {
            _netDataType = netDataType;
        }
    }
}
