using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using IceCoffee.Wpf.MvvmFrame.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TianYiSdtdServerTools.Client.Services.Primitives.UI;
using TianYiSdtdServerTools.Client.TelnetClient;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.ControlPanel
{
    public class TelnetConsoleViewModel : ViewModelBase
    {
        private PropertyObserver<TelnetConsoleViewModel> _currentViewModelObserver;

        private const int maxRecordCommandCount = 10;

        private readonly StringBuilder _telnetDataStringBuilder = new StringBuilder();

        [ConfigNode(XmlNodeType.Attribute)]
        public bool AutoRefresh { get; [NPCA_Method]set; }

        public ObservableCollection<string> RecentCommands { get; set; } = new ObservableCollection<string>() { "help" };

        public string TelnetData { get { return _telnetDataStringBuilder.ToString(); } }

        public string Command { get; set; }

        //public int ComboBoxSelectedIndex { get; [NPCA_Method]set; }
        
        public RelayCommand SendCommand { get; private set; }

        //public RelayCommand SkipCommand { get; set; }

        public TelnetConsoleViewModel(IDispatcherService dispatcherService) : base(dispatcherService)
        {
            SendCommand = new RelayCommand(()=>
            {                
                SdtdConsole.Instance.SendCmd(Command);

                if(string.IsNullOrEmpty(Command) == false && Command != RecentCommands.FirstOrDefault())
                {
                    RecentCommands.Insert(0, Command);
                }
                
                if(RecentCommands.Count > maxRecordCommandCount)
                {
                    RecentCommands.RemoveAt(RecentCommands.Count - 1);
                }
            });

            //SkipCommand = new RelayCommand(()=>
            //{
            //    ComboBoxSelectedIndex += 1;
            //});
        }

        protected override void OnPrepareLoadConfig()
        {
            _currentViewModelObserver = new PropertyObserver<TelnetConsoleViewModel>(this);
            _currentViewModelObserver.RegisterHandler(currentViewModel => currentViewModel.AutoRefresh,
                (propertySource) =>
                {
                    ConnectAutoRefresh();
                });
        }

        private void OnReceiveLine(string line)
        {
            if(_telnetDataStringBuilder.Length > 40960)
            {
                _telnetDataStringBuilder.Remove(0, 4096);
            }

            _telnetDataStringBuilder.Append(line);

            RaisePropertyChanged("TelnetData");
        }

        /// <summary>
        /// 关联自动刷新
        /// </summary>
        private void ConnectAutoRefresh()
        {
            if (this.AutoRefresh)
            {
                SdtdConsole.Instance.ReceiveLine -= OnReceiveLine;
                SdtdConsole.Instance.ReceiveLine += OnReceiveLine;
            }
            else
            {
                SdtdConsole.Instance.ReceiveLine -= OnReceiveLine;
            }
        }
    }
}
