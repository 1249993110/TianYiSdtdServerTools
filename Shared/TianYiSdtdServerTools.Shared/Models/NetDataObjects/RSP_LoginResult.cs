using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Shared.Models.NetDataObjects
{
    [Serializable]
    public class RSP_LoginResult : NetDataObject
    {
        public RSP_LoginResult() : base(NetDataType.RSP_LoginResult)
        {

        }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        /// 登录类型
        /// </summary>
        public LoginType LoginType { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 是否弹出对话框
        /// </summary>
        public bool IsPopMessageBox { get; set; }

        /// <summary>
        /// 对话框应显示的消息
        /// </summary>
        public string Message { get; set; }
    }
}
