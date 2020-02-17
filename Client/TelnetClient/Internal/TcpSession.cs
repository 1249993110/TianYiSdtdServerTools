using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.Common;
using IceCoffee.Network.Sockets;
using TianYiSdtdServerTools.Client.Models.SdtdServerInfo;

namespace TianYiSdtdServerTools.Client.TelnetClient.Internal
{
    internal class TcpSession : BaseSession<TcpSession>
    {
        #region 字段
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        private string cmdBuffer;//_line          

        /// <summary>
        /// 数据缓冲区列表
        /// </summary>
        private readonly List<string> cmdBufferList = new List<string>();//_lineList = new List<string>();

        /// <summary>
        /// 正在执行的命令
        /// </summary>
        private string executingCmd;

        /// <summary>
        /// INF 开始坐标
        /// </summary>
        private int index_INF;// startIndexINF;

        /// <summary>
        /// 是否正在往数据缓冲区列表中写入数据
        /// </summary>
        private bool isWritingToBufferList;

        /// <summary>
        /// 是否正在执行本机请求的命令
        /// </summary>
        private bool isExecutingCmd;
        #endregion

        #region 属性

        #endregion

        #region 方法

        #region 构造方法

        #endregion

        #region 私有方法
        /// <summary>
        /// 重置字段
        /// </summary>
        private void ResetField()
        {
            isWritingToBufferList = false;
            isExecutingCmd = false;
            executingCmd = string.Empty;
            cmdBufferList.Clear();
        }

        /// <summary>
        /// 发送密码
        /// </summary>
        private void SendPassword()
        {
            // "0\r\n"确保密码发送成功，被服务端接受
            SdtdConsole.Instance.SendCmd("0\r\n" + SdtdConsole.Instance.Password);
        }

        /// <summary>
        /// 读取一行到缓冲区
        /// </summary>
        private void ReadLineToBuffer()
        {
            cmdBuffer = Encoding.UTF8.GetString(ReadBuffer.ReadLine());
            System.Diagnostics.Debug.Write(cmdBuffer);
            SdtdConsole.Instance.RaiseRecvDataEvent(cmdBuffer);
        }

        /// <summary>
        /// 处理正在执行的命令
        /// </summary>
        private void HandleExecutingCommand()
        {

        }

        /// <summary>
        /// 内部处理数据
        /// </summary>
        private void InternalHandleData()
        {
            if (cmdBuffer.Length > 36)
            {
                index_INF = cmdBuffer.IndexOf(' ', 25);
                if (index_INF != -1 && cmdBuffer.Substring(index_INF, 5) == " INF ")
                {
                    HandleINF();
                    return;
                }
            }

            if(SdtdConsole.Instance.IsConnected == false)
            {
                if (cmdBuffer.StartsWith("Please enter password:"))
                {
                    SdtdConsole.Instance.ConnectionState = ConnectionState.PasswordVerifying;
                    SendPassword();
                }
                else if (cmdBuffer.StartsWith("Press 'help' to get a list of all commands."))
                {
                    SdtdConsole.Instance.ConnectionState = ConnectionState.Connected;
                }
                else if (cmdBuffer.StartsWith("Password incorrect, please enter password:"))
                {
                    SdtdConsole.Instance.ConnectionState = ConnectionState.PasswordIncorrect;
                }
            }           
            
        }

        /// <summary>
        /// 处理INF信息
        /// </summary>
        private void HandleINF()
        {
            // 定位到信息真实起点
            index_INF += 5;
            if (cmdBuffer.Substring(index_INF, 10) == "Chat (from")
            {
                HandleChatMessage();
            }
            else if (cmdBuffer.Substring(index_INF, 9) == "Executing")
            {
                executingCmd = cmdBuffer.GetMidStr("Executing command '", "' by", index_INF);
                isExecutingCmd = true;
                return;
            }
        }

        /// <summary>
        /// 处理聊天信息
        /// </summary>
        private void HandleChatMessage()
        {

        }
        #endregion

        #region 保护方法
        protected override void OnReceived()
        {
            while(ReadBuffer.CanReadLine)
            {
                //if (isExecutingCmd)
                //{
                //    HandleExecutingCommand();
                //}
                //else
                {
                    ReadLineToBuffer();
                    InternalHandleData();
                }
            }
        }

        protected override void OnStarted()
        {
            SdtdConsole.Instance.ConnectionState = ConnectionState.PasswordVerifying;
            base.OnStarted();
        }

        protected override void OnClosed(SocketError closedReason)
        {            
            base.OnClosed(closedReason);
            ResetField();
        }
        #endregion

        #region 公开方法

        #endregion

        #region 其他方法

        #endregion

        #endregion


    }
}
