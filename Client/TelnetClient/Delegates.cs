using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.TelnetClient
{
    public delegate void RecvDataEventHandler(string data);

    public delegate void ConnectionStateChangedEventHandler(ConnectionState connectionState);
}
