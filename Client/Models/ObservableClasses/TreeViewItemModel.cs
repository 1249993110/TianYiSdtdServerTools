using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IceCoffee.Wpf.MvvmFrame;

namespace TianYiSdtdServerTools.Client.Models.ObservableClasses
{
    public class TreeViewItemModel : ObservableObject
    {
        public TreeViewItemModel(string header, string tag, string icon = null)
        {
            Header = header;
            Tag = tag;
            Icon = icon;
            MenuItems = new ObservableCollection<TreeViewItemModel>();
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

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { _isExpanded = value; RaisePropertyChanged(); }
        }
        private bool _isExpanded = true;

        public ObservableCollection<TreeViewItemModel> MenuItems
        {
            get { return _menuItems; }
            set { _menuItems = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<TreeViewItemModel> _menuItems;
    }
}
