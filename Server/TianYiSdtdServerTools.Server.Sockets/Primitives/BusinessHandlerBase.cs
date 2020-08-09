using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Server.Sockets.Primitives
{
    abstract class BusinessHandlerBase
    {
        protected readonly TcpSession tcpSession;

        protected BusinessHandlerBase(TcpSession tcpSession)
        {
            this.tcpSession = tcpSession;
        }
    }
}
