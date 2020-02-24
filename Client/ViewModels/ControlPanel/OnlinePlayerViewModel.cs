using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using TianYiSdtdServerTools.Client.Models.ObservableClasses;
using TianYiSdtdServerTools.Client.Models.Players;
using TianYiSdtdServerTools.Client.Models.SdtdServerInfo;
using TianYiSdtdServerTools.Client.TelnetClient;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.ControlPanel
{
    public class OnlinePlayerViewModel : ViewModelBase
    {
        private List<PlayerInfo> _onlinePlayers;

        public List<PlayerInfo> OnlinePlayers
        {
            get { return _onlinePlayers; }
            set
            {
                _onlinePlayers = value;
                RaisePropertyChanged();
            }
        }

        public OnlinePlayerViewModel()
        {
            this.OnlinePlayers = SdtdConsole.Instance.OnlinePlayers?.Values.ToList();
            SdtdConsole.Instance.ReceivedOnlinePlayerInfo += (onlinePlayers) => { this.OnlinePlayers = onlinePlayers; };
        }
    }
}
