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
using TianYiSdtdServerTools.Client.ViewModels.ToolDialog;

namespace TianYiSdtdServerTools.Client.Views.PartialViews.ToolDialog
{
    /// <summary>
    /// ItemView.xaml 的交互逻辑
    /// </summary>
    public partial class ItemView : UserControl
    {
        public ItemViewModel ViewModel { get; set; }

        public ItemView()
        {
            InitializeComponent();
            ViewModel = Autofac.Resolve<ItemViewModel>();
            this.DataContext = ViewModel;
        }

        protected ItemView(bool setDataContext)
        {

        }
    }
}
