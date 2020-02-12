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
using TianYiSdtdServerTools.Client.ViewModels;
using System.Collections.ObjectModel;
using TianYiSdtdServerTools.Client.Models.ObservableObjects;

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
            ViewModel = new MainWindowViewModel();
            base.DataContext = ViewModel;

        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewItemModel selectedItem = this.leftListBox1.SelectedItem as ListViewItemModel;

            foreach (TabItem tab in this.leftTabControl1.Items)
            {
                if (tab.Header.ToString() == selectedItem.Header)
                {
                    this.leftTabControl1.SelectedItem = tab;
                    return;
                }
            }

            TabItem tabItem = new TabItem()
            {
                Header = selectedItem.Header,
                Content = Activator.CreateInstance(_partialViewDic[selectedItem.Tag])
            };

            this.leftTabControl1.Items.Add(tabItem);
            this.leftTabControl1.SelectedItem = tabItem;
            
        }
    }

}
