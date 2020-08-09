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

        public NetDataType NetDataType { get => _netDataType; set => _netDataType = value; }

        public NetDataObject(NetDataType netDataType)
        {
            _netDataType = netDataType;
        }
    }
}
