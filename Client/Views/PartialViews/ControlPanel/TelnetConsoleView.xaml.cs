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

            ViewModel = Autofac.Resolve<TelnetConsoleViewModel>();

            base.DataContext = ViewModel;
        }

        private void OnTextBox_TelnetData_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.textBox_TelnetData.IsVisible && this.textBox_TelnetData.IsFocused == false)
            {
                this.textBox_TelnetData.ScrollToEnd();
            }
        }
    }
}
