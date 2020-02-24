using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using TianYiSdtdServerTools.Client.ViewModels.MainWindow;
using System.Collections.ObjectModel;
using TianYiSdtdServerTools.Client.Models.ObservableClasses;
using Panuon.UI.Silver;

namespace TianYiSdtdServerTools.Client.Views.Windows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : IceCoffee.Wpf.CustomControlLibrary.Windows.FramelessWindow
    {
        private static IDictionary<string, Type> _partialViewDic;

        public MainWindowViewModel ViewModel { get; set; }

        static MainWindow()
        {
            _partialViewDic = new Dictionary<string, Type>();
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.StartsWith("SdtdServerTools"));
            assembly.GetTypes().Where(x => x.Namespace.StartsWith("TianYiSdtdServerTools.Client.Views.PartialViews")
                                        && x.IsSubclassOf(typeof(UserControl)))
                                        .ToList().ForEach(x => _partialViewDic.Add(x.Name.Remove(x.Name.Length - 4), x));
        }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = Autofac.Resolve<MainWindowViewModel>();
            base.DataContext = ViewModel;
        }

        private void OnLeftListBox1SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewItemModel selectedItem = this.leftListBox1.SelectedItem as ListViewItemModel;

            foreach (TabItem tab in this.leftTabControl1.Items)
            {
                if (tab.Tag.ToString() == selectedItem.Tag)
                {
                    this.leftTabControl1.SelectedItem = tab;
                    return;
                }
            }

            System.Diagnostics.Debug.Assert(_partialViewDic.ContainsKey(selectedItem.Tag));

            TabItem tabItem = new TabItem()
            {
                Header = selectedItem.Header,
                Tag = selectedItem.Tag,
                Content = Activator.CreateInstance(_partialViewDic[selectedItem.Tag])
            };

            this.leftTabControl1.Items.Add(tabItem);
            this.leftTabControl1.SelectedItem = tabItem;            
        }

        private void OnMainWindowClosing(object sender, EventArgs e)
        {
            if (MessageBoxX.Show("确定退出程序吗？", "提示", this, MessageBoxButton.OKCancel, configKey: "CommonTheme") == MessageBoxResult.OK)
            {
                this.Close();
            }
        }
    }

}
