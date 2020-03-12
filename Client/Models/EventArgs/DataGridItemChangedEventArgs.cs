using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.EventArgs
{
    public class DataGridItemChangedEventArgs
    {
        /// <summary>
        /// Item值是否改变
        /// </summary>
        public bool IsChanged { get; set; }

        /// <summary>
        /// 是否为新添加项
        /// </summary>
        public bool IsNewItem { get; set; }

        /// <summary>
        /// 旧值
        /// </summary>
        public object OldItem { get; set; }

        /// <summary>
        /// 新值
        /// </summary>
        public object NewItem { get; set; }
    }
}
