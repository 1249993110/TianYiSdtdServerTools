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
        public OnlinePlayerViewModel ViewModel { get; set; }

        public OnlinePlayerView()
        {
            InitializeComponent();
            ViewModel = Autofac.Resolve<OnlinePlayerViewModel>();
            base.DataContext = ViewModel;
        }
    }
}
