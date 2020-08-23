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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TianYiSdtdServerTools.Client.ViewModels.FunctionPanel;

namespace TianYiSdtdServerTools.Client.Views.PartialViews.FunctionPanel
{
    /// <summary>
    /// LotterySystemView.xaml 的交互逻辑
    /// </summary>
    public partial class LotterySystemView : UserControl
    {
        public LotterySystemViewModel ViewModel { get; set; }

        public LotterySystemView(string functionTag)
        {
            InitializeComponent();

            ViewModel = IocContainer.Resolve<LotterySystemViewModel>(new NamedParameter(nameof(functionTag), functionTag));

            base.DataContext = ViewModel;
        }
    }
}
