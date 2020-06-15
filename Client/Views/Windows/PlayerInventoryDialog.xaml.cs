using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TianYiSdtdServerTools.Client.ViewModels.Windows;

namespace TianYiSdtdServerTools.Client.Views.Windows
{
    /// <summary>
    /// PlayerInventoryDialog.xaml 的交互逻辑
    /// </summary>
    public partial class PlayerInventoryDialog : Window
    {
        public PlayerInventoryViewModel ViewModel { get; set; }

        public PlayerInventoryDialog(string steamID)
        {
            InitializeComponent();
            ViewModel = Autofac.Resolve<PlayerInventoryViewModel>(new NamedParameter(nameof(steamID), steamID));
            this.DataContext = ViewModel;
        }
    }
}
