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
        Submit_ClientInfo,

        /// <summary>
        /// 提交cdkey
        /// </summary>
        Submit_CDkey,
        #endregion


        #region 返回
        /// <summary>
        /// 关闭客户端
        /// </summary>
        CloseClient,

        /// <summary>
        /// 弹出对话框
        /// </summary>
        PopDialogueBox,

        /// <summary>
        /// 返回客户信息
        /// </summary>
        Return_ClientInfo,

        /// <summary>
        /// 返回自动更新器配置
        /// </summary>
        Return_AutoUpdaterConfig
        #endregion
    }
}
