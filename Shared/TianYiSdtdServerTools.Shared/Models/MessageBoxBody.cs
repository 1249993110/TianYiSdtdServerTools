using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Shared.Models
{
    [Serializable]
    public class MessageBoxBody
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 对话框类型
        /// </summary>
        public MessageBoxType MessageBoxType { get; set; }

        public MessageBoxBody(string content, string title = "提示", MessageBoxType messageBoxType = MessageBoxType.Normal)
        {
            Title = title;
            Content = content;
            MessageBoxType = messageBoxType;
        }
    }
}
