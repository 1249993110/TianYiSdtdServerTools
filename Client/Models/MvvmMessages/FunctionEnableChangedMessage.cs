using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.MvvmMessages
{
    public class FunctionEnableChangedMessage
    {
        /// <summary>
        /// 功能标记
        /// </summary>
        public string FunctionTag { get; set; }

        /// <summary>
        /// 功能开关是否打开
        /// </summary>
        public bool IsOpen { get; set; }
    }
}
