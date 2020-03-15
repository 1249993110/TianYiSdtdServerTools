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
        /// 值是否改变
        /// </summary>
        public bool IsChanged { get; set; }

        /// <summary>
        /// 旧值
        /// </summary>
        public object OldItem { get; set; }

        /// <summary>
        /// 新值
        /// </summary>
        public object NewItem { get; set; }

        /// <summary>
        /// 绑定属性源的路径
        /// </summary>
        public string BindPath { get; set; }
    }
}
