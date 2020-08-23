using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using IceCoffee.Common.Xml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.TelnetClient;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.ControlPanel
{
    public class TelnetConsoleViewModel : ViewModelBase
    {
        private const int maxRecordCommandCount = 10;

        private bool _autoRefresh;

        private readonly IPlainTextBoxService _plainTextBoxService;

        [ConfigNode(XmlNodeType.Attribute)]
        public bool AutoRefresh
        {
            get { return _autoRefresh; }
            set
            {
                _autoRefresh = value;
                ConnectAutoRefresh();
            }
        }

        public ObservableCollection<string> RecentCommands { get; set; } = new ObservableCollection<string>() { "help" };

        public string Command { get; set; }

        //public int ComboBoxSelectedIndex { get; [NPCA_Method]set; }
        
        public RelayCommand SendCommand { get; private set; }

        //public RelayCommand SkipCommand { get; set; }

        public TelnetConsoleViewModel(IDispatcherService dispatcherService,
            IPlainTextBoxService plainTextBoxService) : base(dispatcherService)
        {
            _plainTextBoxService = plainTextBoxService;
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

        private void OnReceiveLine(string line)
        {
            _plainTextBoxService.AppendPlainText(line);
        }

        /// <summary>
        /// 关联自动刷新
        /// </summary>
        private void ConnectAutoRefresh()
        {
            if (this._autoRefresh)
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
