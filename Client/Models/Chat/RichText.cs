using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.Chat
{
    public class RichText
    {
        public string content;
        public string color;

        public RichText()
        {

        }

        public RichText(string content)
        {
            this.content = content;
        }

        public RichText(string content, string color)
        {
            this.content = content;
            this.color = color;
        }
    }
}
