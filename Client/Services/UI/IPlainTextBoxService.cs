using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Services.UI
{
    public interface IPlainTextBoxService
    {
        /// <summary>
        /// 添加纯文本至文档末尾
        /// </summary>
        /// <param name="plainText"></param>
        void AppendPlainText(string plainText);
    }
}
