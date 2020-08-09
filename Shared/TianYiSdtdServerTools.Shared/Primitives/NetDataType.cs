using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Shared.Primitives
{
    public enum NetDataType
    {
        /// <summary>
        /// 空信息
        /// </summary>
        NULL,

        #region 提交
        /// <summary>
        /// 提交客户信息
        /// </summary>
        REQ_ClientInfo,

        /// <summary>
        /// 提交cdkey
        /// </summary>
        REQ_CDkey,
        #endregion


        #region 返回
        /// <summary>
        /// 关闭客户端
        /// </summary>
        RSP_CloseClient,

        /// <summary>
        /// 弹出对话框
        /// </summary>
        RSP_PopDialogueBox,

        /// <summary>
        /// 返回客户信息
        /// </summary>
        RSP_ClientInfo,

        /// <summary>
        /// 返回自动更新器配置
        /// </summary>
        RSP_AutoUpdaterConfig
        #endregion
    }
}
