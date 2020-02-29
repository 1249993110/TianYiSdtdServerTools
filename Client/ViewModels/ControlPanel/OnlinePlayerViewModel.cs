using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using IceCoffee.Wpf.MvvmFrame.Utils;
using TianYiSdtdServerTools.Client.Models.ObservableClasses;
using TianYiSdtdServerTools.Client.Models.Players;
using TianYiSdtdServerTools.Client.Models.SdtdServerInfo;
using TianYiSdtdServerTools.Client.Services.Primitives.UI;
using TianYiSdtdServerTools.Client.TelnetClient;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.ControlPanel
{
    public class OnlinePlayerViewModel : ViewModelBase
    {
        private IDialogService _dialogService;

        private PropertyObserver<OnlinePlayerViewModel> _currentViewModelObserver;

        public List<PlayerInfo> OnlinePlayers { get; [NPCA_Method]set; }

        [ConfigNode(ConfigNodeType.Attribute)]
        public bool AutoRefresh { get; [NPCA_Method]set; }

        public int SelectedIndex { get; set; } = -1;
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
                SdtdConsole.Instance.TelePlayer(OnlinePlayers[SelectedIndex].SteamID, result);
            }, CanExecuteCommand);
            KickPlayer = new RelayCommand(() =>
            {
                SdtdConsole.Instance.KickPlayer(OnlinePlayers[SelectedIndex].SteamID);
            }, CanExecuteCommand);
            KillPlayer = new RelayCommand(() =>
            {
                SdtdConsole.Instance.KillPlayer(OnlinePlayers[SelectedIndex].SteamID);
            }, CanExecuteCommand);
            BanPlayer100Year = new RelayCommand(() =>
            {
                string result = _dialogService.ShowInputDialog("请输入封禁原因：", "你因违规被管理员封禁");
                SdtdConsole.Instance.BanPlayerWithYear(OnlinePlayers[SelectedIndex].SteamID, 100, result);
            }, CanExecuteCommand);
            RemoveLandclaims = new RelayCommand(() =>
            {
                SdtdConsole.Instance.RemovePlayerLandclaims(OnlinePlayers[SelectedIndex].SteamID);
            }, CanExecuteCommand);
            AddSuperAdministrator = new RelayCommand(() =>
            {
                SdtdConsole.Instance.AddAdministrator(OnlinePlayers[SelectedIndex].SteamID, 0);
            }, CanExecuteCommand);
            RemoveAdministrator = new RelayCommand(() =>
            {
                SdtdConsole.Instance.RemoveAdministrator(OnlinePlayers[SelectedIndex].SteamID);
            }, CanExecuteCommand);
            RemovePlayerArchive = new RelayCommand(() =>
            {
                SdtdConsole.Instance.RemovePlayerArchive(OnlinePlayers[SelectedIndex].SteamID);
            }, CanExecuteCommand);
            ViewPlayerInventory = new RelayCommand(() =>
            {
                string steamID = OnlinePlayers[SelectedIndex].SteamID;
            }, CanExecuteCommand);
        }

        protected override void OnPrepareLoadConfig()
        {
            _currentViewModelObserver = new PropertyObserver<OnlinePlayerViewModel>(this);
            _currentViewModelObserver.RegisterHandler(currentViewModel => currentViewModel.AutoRefresh,
                (propertySource) =>
                {
                    ConnectAutoRefresh();
                });
        }

        private bool CanExecuteCommand()
        {
            return SelectedIndex != -1;
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
            if (this.AutoRefresh)
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
