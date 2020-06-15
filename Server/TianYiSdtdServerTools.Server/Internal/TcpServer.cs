using IceCoffee.Network.Sockets.MulitThreadTcpServer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Server.Internal
{
    internal class TcpServer : BaseServer<TcpSession>
    {
        /// <summary>
        /// 补丁版本url
        /// </summary>
        internal const string updateXmlUrl_patch = "https://oss.7daystodie.top/TianYiSdtdServertools/AutoUpdate/patch.xml";
        /// <summary>
        /// 完整版url
        /// </summary>
        internal const string updateXmlUrl_complete = "https://oss.7daystodie.top/TianYiSdtdServertools/AutoUpdate/complete.xml";

        internal readonly ConcurrentDictionary<string, TcpSession> loggedSessions = new ConcurrentDictionary<string, TcpSession>();
        internal readonly ConcurrentDictionary<string, TcpSession> tryLoginSessions = new ConcurrentDictionary<string, TcpSession>();

        protected override void OnSessionClosed(TcpSession session, SocketError closedReason)
        {
            if(string.IsNullOrEmpty(session.ClientToken) == false)
            {
                loggedSessions.TryRemove(session.ClientToken, out TcpSession s1);
                tryLoginSessions.TryRemove(session.ClientToken, out TcpSession s2);
            }

            base.OnSessionClosed(session, closedReason);
        }
    }
}
