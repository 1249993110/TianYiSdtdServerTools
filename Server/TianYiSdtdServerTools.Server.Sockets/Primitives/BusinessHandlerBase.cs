using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared.Models;
using TianYiSdtdServerTools.Shared.Models.NetDataObjects;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Server.Sockets.Primitives
{
    abstract class BusinessHandlerBase
    {
        protected readonly TcpSession tcpSession;

        protected BusinessHandlerBase(TcpSession tcpSession)
        {
            this.tcpSession = tcpSession;
        }

        /// <summary>
        /// 关闭客户端
        /// </summary>
        public virtual void CloseClient()
        {
            tcpSession.Send(new NetDataObject(NetDataType.RSP_CloseClient));
        }

        /// <summary>
        /// 弹出对话框
        /// </summary>
        public virtual void PopMessageBox(string content)
        {
            tcpSession.Send(new RSP_PopMessageBox()
            {
                MessageBoxBody = new MessageBoxBody(content)
            });
        }

        /// <summary>
        /// 弹出对话框
        /// </summary>
        public virtual void PopMessageBox(string content, string title, MessageBoxType messageBoxType)
        {
            tcpSession.Send(new RSP_PopMessageBox()
            {
                 MessageBoxBody = new MessageBoxBody(content, title, messageBoxType)
            });
        }
    }
}
