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

namespace TianYiSdtdServerTools.Client.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : IceCoffee.WpfCustomControlLibrary.Windows.FramelessWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            base.DataContext = Customer;
            Binding bind = new Binding();
            bind.Source = Customer;
            bind.Path = new PropertyPath("Test");
            bind.Mode = BindingMode.TwoWay;
            //指定Binding 目标和目标属性
            but.SetBinding(TextBlock.FontSizeProperty, bind);
        }

        public Customer Customer = new Customer();
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            //Customer.Test = 12;
            this.but.FontSize = 5;
        }
    }

    public class Customer : System.ComponentModel.INotifyPropertyChanged
    {
        private int test = 1;
        public int Test
        {
            get { return test; }
            set
            {
                test = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Test"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }


}
