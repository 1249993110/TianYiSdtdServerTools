using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.ConsoleTempList
{
    public class CanUseEntity : ObservableObject
    {
        public int EntityID { get; set; }

        public string English { get; set; }

        public string Chinese { get; set; }

        public bool Visible { get; [NPCA_Method]set; } = true;
    }
}
