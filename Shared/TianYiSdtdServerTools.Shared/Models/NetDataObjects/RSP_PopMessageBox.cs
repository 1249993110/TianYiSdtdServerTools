using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Shared.Models.NetDataObjects
{
    [Serializable]
    public class RSP_PopMessageBox : NetDataObject
    {
        public MessageBoxBody MessageBoxBody { get; set; }

        public RSP_PopMessageBox() : base(NetDataType.RSP_PopMessageBox)
        {

        }
    }
}
