using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using IceCoffee.Wpf.MvvmFrame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.ConsoleTempList;
using TianYiSdtdServerTools.Client.Models.Players;
using TianYiSdtdServerTools.Client.Services.Primitives.UI;
using TianYiSdtdServerTools.Client.TelnetClient;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.ControlPanel
{
    public class HistoryPlayerViewModel : ViewModelBase
    {
        private IDialogService _dialogService;

        public List<HistoryPlayerInfo> HistoryPlayers { get; [NPCA_Method]set; }

        public int SelectedIndex { get; [NPCA_Method]set; } = -1;

        [ConfigNode(ConfigNodeType.Attribute)]
        public int ComboBoxSelectedIndex { get; [NPCA_Method]set; }

        [ConfigNode(ConfigNodeType.Attribute)]
        public string SearchText { get; set; }

        #region 命令
        public RelayCommand BanPlayer100Year { get; private set; }

        public RelayCommand RemoveLandclaims { get; private set; }

        public RelayCommand AddSuperAdministrator { get; private set; }

        public RelayCommand RemoveAdministrator { get; private set; }

        public RelayCommand RemovePlayerArchive { get; private set; }

        public RelayCommand ViewPlayerInventory { get; private set; }

        public RelayCommand RefreshList { get; private set; }

        public RelayCommand ClearList { get; private set; }

        public RelayCommand SearchPlayer { get; private set; }
        #endregion

        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                if (_isVisible)
                {
                    SdtdConsole.Instance.ReceivedTempListData -= OnReceivedTempListData;
                    SdtdConsole.Instance.ReceivedTempListData += OnReceivedTempListData;
                    PrivateRefreshList();
                }
                else
                {
                    SdtdConsole.Instance.ReceivedTempListData -= OnReceivedTempListData;
                }
            }
        }

        private void PrivateRefreshList()
        {
            SdtdConsole.Instance.SendCmd("lkp");
        }

        public HistoryPlayerViewModel(IDispatcherService dispatcherService, IDialogService dialogService) : base(dispatcherService)
        {
            this._dialogService = dialogService;

            SdtdConsole.Instance.ReceivedTempListData += OnReceivedTempListData;

            BanPlayer100Year = new RelayCommand(() =>
            {
                string result = _dialogService.ShowInputDialog("请输入封禁原因：", "你因违规被管理员封禁");
                SdtdConsole.Instance.BanPlayerWithYear(HistoryPlayers[SelectedIndex].SteamID, 100, result);
            }, CanExecuteCommand);
            RemoveLandclaims = new RelayCommand(() =>
            {
                SdtdConsole.Instance.RemovePlayerLandclaims(HistoryPlayers[SelectedIndex].SteamID);
            }, CanExecuteCommand);
            AddSuperAdministrator = new RelayCommand(() =>
            {
                SdtdConsole.Instance.AddAdministrator(HistoryPlayers[SelectedIndex].SteamID, 0);
            }, CanExecuteCommand);
            RemoveAdministrator = new RelayCommand(() =>
            {
                SdtdConsole.Instance.RemoveAdministrator(HistoryPlayers[SelectedIndex].SteamID);
            }, CanExecuteCommand);
            RemovePlayerArchive = new RelayCommand(() =>
            {
                SdtdConsole.Instance.RemovePlayerArchive(HistoryPlayers[SelectedIndex].SteamID);
            }, CanExecuteCommand);
            ViewPlayerInventory = new RelayCommand(() =>
            {
                string steamID = HistoryPlayers[SelectedIndex].SteamID;
            }, CanExecuteCommand);

            RefreshList = new RelayCommand(PrivateRefreshList);
            ClearList = new RelayCommand(() => { HistoryPlayers = null; });
            SearchPlayer = new RelayCommand(() => 
            {
                int i = 0;
                if (ComboBoxSelectedIndex == 0)
                {
                    for (; i < HistoryPlayers.Count; ++i)
                    {
                        if (HistoryPlayers[i].PlayerName == SearchText)
                        {
                            SelectedIndex = i;
                            break;
                        }
                    }
                }
                else if (ComboBoxSelectedIndex == 1)
                {
                    for (; i < HistoryPlayers.Count; ++i)
                    {
                        if (HistoryPlayers[i].SteamID == SearchText)
                        {
                            SelectedIndex = i;
                            break;
                        }
                    }
                }
                else if(ComboBoxSelectedIndex == 2)
                {
                    for (; i < HistoryPlayers.Count; ++i)
                    {
                        if (HistoryPlayers[i].EntityID.ToString() == SearchText)
                        {
                            SelectedIndex = i;
                            break;
                        }
                    }
                }

                if( i == HistoryPlayers.Count)
                {
                    _dialogService.ShowInformation("没有找到此玩家");
                }
            }, () => { return HistoryPlayers != null; });
        }

        private void OnReceivedTempListData(object twoDimensionalList, TempListDataType tempListDataType)
        {
            if (tempListDataType == TempListDataType.HistoryPlayerList && twoDimensionalList is List<HistoryPlayerInfo>)
            {
                HistoryPlayers = (List<HistoryPlayerInfo>)twoDimensionalList;
            }
        }

        private bool CanExecuteCommand()
        {
            return SelectedIndex != -1;
        }
    }
}
