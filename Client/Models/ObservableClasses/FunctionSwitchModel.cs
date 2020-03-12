using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using IceCoffee.Wpf.MvvmFrame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TianYiSdtdServerTools.Client.Models.ObservableClasses
{
    /// <summary>
    /// 功能开关
    /// </summary>
    [NPCA_Class]
    public class FunctionSwitchModel : ObservableObject
    {
        /// <summary>
        /// 游戏公告
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public bool GameNotice { get; set; }

        /// <summary>
        /// 积分系统
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public bool ScoreSystem { get; set; }
    }
}
