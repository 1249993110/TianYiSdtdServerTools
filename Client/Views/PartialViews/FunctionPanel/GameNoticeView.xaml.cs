using Autofac;
using System.Windows.Controls;
using TianYiSdtdServerTools.Client.ViewModels.FunctionPanel;

namespace TianYiSdtdServerTools.Client.Views.PartialViews.FunctionPanel
{
    /// <summary>
    /// GameNoticeView.xaml 的交互逻辑
    /// </summary>
    public partial class GameNoticeView : UserControl
    {
        public GameNoticeViewModel ViewModel { get; set; }

        public GameNoticeView(string functionTag)
        {
            InitializeComponent();

            ViewModel = IocContainer.Resolve<GameNoticeViewModel>(new NamedParameter(nameof(functionTag), functionTag));

            base.DataContext = ViewModel;
        }
    }
}
