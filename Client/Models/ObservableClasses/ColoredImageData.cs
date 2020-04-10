using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.ObservableClasses
{
    public class ColoredImageData : ObservableObject
    {
        public Uri Source { get; set; }

        public string HexColor { get; set; }

        public string ToolTip { get; set; }

        public string English { get; set; }

        public string Chinese { get; set; }

        public string MatchText { get; [NPCA_Method]set; }

        public bool Visibility { get; [NPCA_Method]set; } = true;
    }
}
