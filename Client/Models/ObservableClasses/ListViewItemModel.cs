using IceCoffee.Wpf.MvvmFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TianYiSdtdServerTools.Client.Models.ObservableClasses
{
    public class ListViewItemModel : ObservableObject
    {
        public ListViewItemModel(string header, string tag, string icon = null)
        {
            Header = header;
            Tag = tag;
            Icon = icon;
        }

        public string Icon { get; set; }

        public string Header { get; set; }

        public string Tag { get; set; }

        public Visibility Visibility
        {
            get { return _visibility; }
            set { _visibility = value; RaisePropertyChanged(); }
        }
        private Visibility _visibility = Visibility.Visible;
    }
}
