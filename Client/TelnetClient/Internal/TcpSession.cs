using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.Network.Sockets;

namespace TianYiSdtdServerTools.Client.TelnetClient.Internal
{
    internal class TcpSession : BaseSession<TcpSession>
    {
        #region 字段

        #endregion

        #region 属性

        #endregion

        #region 事件

        #endregion

        #region 方法

        #region 构造方法

        #endregion

        #region 私有方法

        #endregion

        #region 保护方法
        protected override void OnReceived()
        {
            while(ReadBuffer.CanReadLine)
            {                
                SdtdConsole.Instance.RaiseRecvDataEvent(Encoding.UTF8.GetString(ReadBuffer.ReadLine()));
            }
        }
        #endregion

        #region 公开方法

        #endregion

        #region 其他方法

        #endregion

        #endregion


    }
}
