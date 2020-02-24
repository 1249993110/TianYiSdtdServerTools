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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TianYiSdtdServerTools.Client.ViewModels.ControlPanel;

namespace TianYiSdtdServerTools.Client.Views.PartialViews.ControlPanel
{
    /// <summary>
    /// OnlinePlayerView.xaml 的交互逻辑
    /// </summary>
    public partial class OnlinePlayerView : UserControl
    {
        public OnlinePlayerView()
        {
            InitializeComponent();
            ViewModel = new OnlinePlayerViewModel();
            base.DataContext = ViewModel;
        }

        public OnlinePlayerViewModel ViewModel { get; set; }

        private void OnDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.IsNewItem == false)
            {
                e.Row.Header = (e.Row.GetIndex() + 1).ToString();
            }
        }
    }
}
