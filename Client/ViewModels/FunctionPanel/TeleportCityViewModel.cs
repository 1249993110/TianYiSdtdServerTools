using IceCoffee.LogManager;
using IceCoffee.Common.Xml;

using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using TianYiSdtdServerTools.Client.Models.Dtos;
using TianYiSdtdServerTools.Client.Models.EventArgs;
using TianYiSdtdServerTools.Client.Models.Players;
using TianYiSdtdServerTools.Client.Services;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.TelnetClient;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;
using TianYiSdtdServerTools.Client.ViewModels.Utils;

namespace TianYiSdtdServerTools.Client.ViewModels.FunctionPanel
{
    public class TeleportCityViewModel : FunctionViewModelBase
    {
        #region 字段
        private readonly IDialogService _dialogService;

        private readonly ScoreInfoService _scoreInfoService;

        private readonly CityPositionService _cityPositionService;

        private readonly TeleRecordService _teleRecordService;
        #endregion

        #region 属性

        public ObservableCollection<CityPositionDto> CityPositions { get; [NPCA_Method]set; }

        public CityPositionDto SelectedItem { get; [NPCA_Method]set; }

        public string CityName { get; set; }

        public string TeleCmd { get; set; }

        public int TeleNeedScore { get; set; }

        public string Pos { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public string QueryListCmd { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public int TeleInterval { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public string QueryListTips { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public string TeleSucceedTips { get; set; }

        /// <summary>
        /// 积分不足提示
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string TeleFailTips1 { get; set; }

        /// <summary>
        /// 正在冷却提示
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string TeleFailTips2 { get; set; }
        #endregion

        #region 命令
        public RelayCommand<DataGridItemChangedEventArgs> DataGridItemChanged { get; private set; }

        public RelayCommand RefreshList { get; private set; }       

        public RelayCommand RemoveItem { get; private set; }

        public RelayCommand RemoveAll { get; private set; }

        public RelayCommand AddData { get; private set; }
        #endregion

        #region 方法
        public TeleportCityViewModel(IDispatcherService dispatcherService, string functionTag, IDialogService dialogService, 
            ScoreInfoService scoreInfoService, CityPositionService cityPositionService, TeleRecordService teleRecordService) 
            : base(dispatcherService, functionTag)
        {
            _dialogService = dialogService;
            _scoreInfoService = scoreInfoService;
            _cityPositionService = cityPositionService;
            _teleRecordService = teleRecordService;

            DataGridItemChanged = new RelayCommand<DataGridItemChangedEventArgs>(OnDataGridItemChanged);

            RefreshList = new RelayCommand(PrivateRefreshList);

            RemoveItem = new RelayCommand(() =>
            {
                if (_dialogService.ShowOKCancel("确定删除选中数据吗？"))
                {
                    _ = _cityPositionService.RemoveAsync(SelectedItem);
                    CityPositions.Remove(SelectedItem);
                }
            }, () => { return SelectedItem != null; });

            RemoveAll = new RelayCommand(() =>
            {
                if (_dialogService.ShowOKCancel("确定删除所有数据吗？"))
                {
                    _ = _cityPositionService.RemoveAllAsync();
                    CityPositions = null;
                }
            }, () => { return CityPositions != null; });

            AddData = new RelayCommand(() =>
            {
                if (string.IsNullOrEmpty(TeleCmd))
                {
                    _dialogService.ShowInformation("传送命令不能为空");
                    return;
                }
                if (CityPositions.FirstOrDefault(p => p.TeleCmd == TeleCmd) != null)
                {
                    _dialogService.ShowInformation("传送命令重复");
                    return;
                }

                var cityPosition = new CityPositionDto()
                {
                    CityName = CityName,
                    TeleCmd = TeleCmd,
                    TeleNeedScore = TeleNeedScore,
                    Pos = Pos
                };
                _ = _cityPositionService.AddAsync(cityPosition);
                CityPositions.Add(cityPosition);
            });

            AddAvailableVariables();

            PrivateRefreshList();
        }

        private void OnDataGridItemChanged(DataGridItemChangedEventArgs eventArgs)
        {
            if (eventArgs.IsChanged == false)
            {
                return;
            }

            if (eventArgs.NewItem is CityPositionDto newItem && eventArgs.OldItem is CityPositionDto oldItem)
            {
                if (CityPositions.FirstOrDefault(p => p.TeleCmd == newItem.TeleCmd && object.Equals(p, newItem) == false) != null)
                {
                    _dialogService.ShowInformation("传送命令重复");
                    newItem.TeleCmd = oldItem.TeleCmd;// 此时新值还未更新至ui
                }
                else
                {
                    _ = _cityPositionService.UpdateAsync(newItem);
                }
            }
        }       

        private async void PrivateRefreshList()
        {
            var result = await _cityPositionService.GetAllAsync("CityName ASC");

            CityPositions = result == null ? null : new ObservableCollection<CityPositionDto>(result);
        }

        protected override void DisableFunction()
        {
            base.DisableFunction();
        }

        protected override void EnableFunction()
        {
            base.EnableFunction();
        }

        private void AddAvailableVariables()
        {
            AvailableVariables.AddRange(new List<string>()
            {
                "[城镇名称]",
                "[传送命令]",
                "[需要积分]",
                "[三维坐标]",
                "[剩余冷却]"
            });
        }

        /// <summary>
        /// 格式化命令
        /// </summary>
        /// <param name="playerInfo"></param>
        /// <param name="message"></param>
        /// <param name="otherParam"></param>
        /// <returns></returns>
        protected override string FormatCmd(PlayerInfo playerInfo, string message, object otherParam = null)
        {
            if (otherParam is CityPositionDto cityPositionDto)
            {
                return base.FormatCmd(playerInfo, message)
                    .Replace("[城镇名称]", cityPositionDto.CityName)
                    .Replace("[传送命令]", cityPositionDto.TeleCmd)
                    .Replace("[需要积分]", cityPositionDto.TeleNeedScore.ToString())
                    .Replace("[三维坐标]", cityPositionDto.Pos);
            }
            else if(otherParam is int timeSpan)
            {
                return base.FormatCmd(playerInfo, message)
                    .Replace("[剩余冷却]", timeSpan.ToString());
            }
            return base.FormatCmd(playerInfo, message);
        }

        protected override bool OnPlayerChatHooked(PlayerInfo playerInfo, string message)
        {
            if (message == QueryListCmd)
            {
                List<CityPositionDto> dtos = _cityPositionService.GetAll("CityName ASC");

                if(dtos.Count == 0) 
                {
                    SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, "[00FF00]暂无公共城镇信息");
                }
                else
                {
                    SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, "[00FF00]可用公共城镇：");

                    StringBuilder returnCmd = new StringBuilder();
                    int index = 1;
                    foreach (var item in dtos)
                    {
                        returnCmd.Append(string.Format("pm {0} \"[00FF00]{1}{2}\"\r\n", playerInfo.SteamID, index, FormatCmd(playerInfo, QueryListTips, item)));
                        ++index;
                    }
                    SdtdConsole.Instance.SendCmd(returnCmd.ToString());
                }
            }
            else
            {
                CityPositionDto cityPosition = _cityPositionService.GetDataByID(message);
                if (cityPosition == null)
                {
                    return false;
                }
                else
                {
                    var teleRecord = _teleRecordService.GetDataByID(playerInfo.SteamID);

                    if(teleRecord != null && string.IsNullOrEmpty(teleRecord.LastTeleDateTime) == false)
                    {
                        int timeSpan = (int)(DateTime.Now - DateTime.Parse(teleRecord.LastTeleDateTime)).TotalSeconds;
                        if (timeSpan < TeleInterval)// 正在冷却
                        {
                            SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, FormatCmd(playerInfo, TeleFailTips2, TeleInterval - timeSpan));

                            return true;
                        }
                    }

                    int playerScore = _scoreInfoService.GetPlayerScore(playerInfo.SteamID);
                    if (playerScore < cityPosition.TeleNeedScore)// 积分不足
                    {
                        SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID,FormatCmd(playerInfo, TeleFailTips1, cityPosition));
                    }
                    else
                    {                       
                        _scoreInfoService.DeductPlayerScore(playerInfo.SteamID, cityPosition.TeleNeedScore);

                        SdtdConsole.Instance.TelePlayer(playerInfo.SteamID, cityPosition.Pos);

                        SdtdConsole.Instance.SendGlobalMessage(FormatCmd(playerInfo, TeleSucceedTips, cityPosition));

                        // 记录传送日期
                        var dto = new TeleRecordDto() { SteamID = playerInfo.SteamID, LastTeleDateTime = DateTime.Now.ToString() };
                        if(teleRecord == null)
                        {
                            _teleRecordService.Add(dto);
                        }
                        else
                        {
                            _teleRecordService.Update(dto);
                        }

                        Log.Info(string.Format("玩家: {0} SteamID: {1} 传送到了: {2}", playerInfo.PlayerName, playerInfo.SteamID, cityPosition.CityName));
                    }
                }
            }
            return true;
        }
        #endregion
    }
}
