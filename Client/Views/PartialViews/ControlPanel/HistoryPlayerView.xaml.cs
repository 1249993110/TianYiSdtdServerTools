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
    /// HistoryPlayerView.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryPlayerView : UserControl
    {
        public HistoryPlayerViewModel ViewModel { get; set; }

        public HistoryPlayerView()
        {
            InitializeComponent();
            ViewModel = Autofac.Resolve<HistoryPlayerViewModel>();
            base.DataContext = ViewModel;
        }

        private void OnDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.IsNewItem == false)
            {
                e.Row.Header = (e.Row.GetIndex() + 1).ToString();
            }
        }

        private void OnDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(this.dataGrid.SelectedIndex != -1)
            {
                this.dataGrid.ScrollIntoView(this.dataGrid.Items[this.dataGrid.SelectedIndex]);
            }
        }
    }
}
