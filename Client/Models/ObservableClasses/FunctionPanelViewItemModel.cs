using IceCoffee.Common.Xml;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TianYiSdtdServerTools.Client.Models.ObservableClasses
{
    public class FunctionPanelViewItemModel : ListViewItemModel
    {
        public FunctionPanelViewItemModel(string header, string tag, string icon = null) : base(header, tag, icon)
        {
        }

        /// <summary>
        /// 功能开关是否打开
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public bool IsOpen { get; [NPCA_Method]set; }
    }
}
