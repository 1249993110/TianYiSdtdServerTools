using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using IceCoffee.Common.Xml;
using TianYiSdtdServerTools.Client.Models.ObservableObjects;
using TianYiSdtdServerTools.Client.Models.Players;
using TianYiSdtdServerTools.Client.Models.SdtdServerInfo;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.TelnetClient;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.ControlPanel
{
    public class OnlinePlayerViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        public List<PlayerInfo> OnlinePlayers { get; [NPCA_Method]set; }

        private bool _autoRefresh;

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

        public PlayerInfo SelectedItem { get; set; }

        #region 命令
        public RelayCommand TelePlayer { get; private set; }

        public RelayCommand KickPlayer { get; private set; }

        public RelayCommand KillPlayer { get; private set; }

        public RelayCommand BanPlayer100Year { get; private set; }

        public RelayCommand RemoveLandclaims { get; private set; }

        public RelayCommand AddSuperAdministrator { get; private set; }

        public RelayCommand RemoveAdministrator { get; private set; }

        public RelayCommand RemovePlayerArchive { get; private set; }

        public RelayCommand ViewPlayerInventory { get; private set; }
        #endregion
        public OnlinePlayerViewModel(IDispatcherService dispatcherService, IDialogService dialogService) : base(dispatcherService)
        {
            this._dialogService = dialogService;
            if (SdtdConsole.Instance.OnlinePlayers != null)
            {
                this.OnlinePlayers = new List<PlayerInfo>(SdtdConsole.Instance.OnlinePlayers.Values);
            }

            TelePlayer = new RelayCommand(() =>
            {
                string result = _dialogService.ShowInputDialog("请输入目标：");
                if (result != null)
                {
                    SdtdConsole.Instance.TelePlayer(SelectedItem.SteamID, result);
                }
            }, CanExecuteCommand);
            KickPlayer = new RelayCommand(() =>
            {
                SdtdConsole.Instance.KickPlayer(SelectedItem.SteamID);
            }, CanExecuteCommand);
            KillPlayer = new RelayCommand(() =>
            {
                SdtdConsole.Instance.KillPlayer(SelectedItem.SteamID);
            }, CanExecuteCommand);
            BanPlayer100Year = new RelayCommand(() =>
            {
                string result = _dialogService.ShowInputDialog("请输入封禁原因：", "你因违规被管理员封禁");
                if (result != null)
                {
                    SdtdConsole.Instance.BanPlayerWithYear(SelectedItem.SteamID, 100, result);
                }
            }, CanExecuteCommand);
            RemoveLandclaims = new RelayCommand(() =>
            {
                SdtdConsole.Instance.RemovePlayerLandclaims(SelectedItem.SteamID);
            }, CanExecuteCommand);
            AddSuperAdministrator = new RelayCommand(() =>
            {
                SdtdConsole.Instance.AddAdministrator(SelectedItem.SteamID, 0);
            }, CanExecuteCommand);
            RemoveAdministrator = new RelayCommand(() =>
            {
                SdtdConsole.Instance.RemoveAdministrator(SelectedItem.SteamID);
            }, CanExecuteCommand);
            RemovePlayerArchive = new RelayCommand(() =>
            {
                SdtdConsole.Instance.RemovePlayerArchive(SelectedItem.SteamID);
            }, CanExecuteCommand);
            ViewPlayerInventory = new RelayCommand(() =>
            {
                _dialogService.ShowPlayerInventory(SelectedItem.SteamID);
            }, CanExecuteCommand);
        }

        private bool CanExecuteCommand()
        {
            return SelectedItem != null;
        }

        private void OnReceivedOnlinePlayerInfo(List<PlayerInfo> players)
        {
            // 拷贝一个副本，以避免用户修改在线玩家字典中的数据
            this.OnlinePlayers = new List<PlayerInfo>(players);
        }

        /// <summary>
        /// 关联自动刷新列表
        /// </summary>
        /// <param name="autoRefresh"></param>
        private void ConnectAutoRefresh()
        {
            if (this._autoRefresh)
            {
                SdtdConsole.Instance.ReceivedOnlinePlayerInfo -= OnReceivedOnlinePlayerInfo;
                SdtdConsole.Instance.ReceivedOnlinePlayerInfo += OnReceivedOnlinePlayerInfo;
            }
            else
            {
                SdtdConsole.Instance.ReceivedOnlinePlayerInfo -= OnReceivedOnlinePlayerInfo;
            }
        }
    }
}
