using Autofac;
using System.Windows.Controls;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.ViewModels.ControlPanel;
using TianYiSdtdServerTools.Client.Views.Services;

namespace TianYiSdtdServerTools.Client.Views.PartialViews.ControlPanel
{
    /// <summary>
    /// TelnetConsoleView.xaml 的交互逻辑
    /// </summary>
    public partial class TelnetConsoleView : UserControl
    {
        public TelnetConsoleViewModel ViewModel { get; set; }

        public TelnetConsoleView()
        {
            InitializeComponent();

            ViewModel = IocContainer.Resolve<TelnetConsoleViewModel>(
                new TypedParameter(typeof(IPlainTextBoxService), new PlainTextBoxService(textBox_TelnetData)));

            base.DataContext = ViewModel;
        }
    }
}
