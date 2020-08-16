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
        /// 请求登录
        /// </summary>
        REQ_Login,

        /// <summary>
        /// 注册账号
        /// </summary>
        REQ_RegisterAccount,

        /// <summary>
        /// 提交cdkey
        /// </summary>
        REQ_SubmitCDkey,
        #endregion


        #region 返回
        /// <summary>
        /// 关闭客户端
        /// </summary>
        RSP_CloseClient,

        /// <summary>
        /// 弹出消息框
        /// </summary>
        RSP_PopMessageBox,

        /// <summary>
        /// 返回登录结果
        /// </summary>
        RSP_LoginResult,

        /// <summary>
        /// 返回自动更新器配置
        /// </summary>
        RSP_AutoUpdaterConfig
        #endregion
    }
}
