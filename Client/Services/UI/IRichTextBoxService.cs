using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Chat;

namespace TianYiSdtdServerTools.Client.Services.UI
{
    public interface IRichTextBoxService
    {
        /// <summary>
        /// 添加一个富文本块，每个块占一行
        /// </summary>
        /// <param name="richTexts"></param>
        void AppendBlock(params RichText[] richTexts);

        /// <summary>
        /// 添加一个富文本块，每个块占一行
        /// </summary>
        /// <param name="richTexts"></param>
        void AppendBlock(IEnumerable<RichText> richTexts);

        /// <summary>
        /// 添加纯文本至文档末尾
        /// </summary>
        void AppendPlainText(string plainText);
    }
}
