using IceCoffee.Common.LogManager;
using IceCoffee.Common.Xml;
using IceCoffee.DbCore.CatchServiceException;
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
    public class TeleportHomeViewModel : FunctionViewModelBase
    {
        #region 字段
        private readonly IDialogService _dialogService;

        private readonly ScoreInfoService _scoreInfoService;

        private readonly HomePositionService _homePositionService;

        private readonly TeleRecordService _teleRecordService;

        private int _currentPlayerOwnedHomeCount;
        #endregion

        #region 属性

        public ObservableCollection<HomePositionDto> HomePositions { get; [NPCA_Method]set; }

        public HomePositionDto SelectedItem { get; [NPCA_Method]set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public string QueryListCmd { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public string SetHomeCmdPrefix { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public int MaxCanSetCount { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public int SetHomeNeedScore { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public int ComboBoxSelectedIndex { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public string TeleHomeCmdPrefix { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public int TeleNeedScore { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public int TeleInterval { get; set; }

        #region 提示
        /// <summary>
        /// 尚未设定Home提示
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string Tips1 { get; set; }

        /// <summary>
        /// 获取列表提示
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string Tips2 { get; set; }

        /// <summary>
        /// 超过最大设置数量上限提示
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string Tips3{ get; set; }

        /// <summary>
        /// 设置需要积分不足提示
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string Tips4 { get; set; }

        /// <summary>
        /// Home名称已存在覆盖提示
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string Tips5 { get; set; }

        /// <summary>
        /// 首次设置Home成功提示
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string Tips6 { get; set; }

        /// <summary>
        /// 传送需要积分不足提示
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string Tips7 { get; set; }

        /// <summary>
        /// Home不存在提示
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string Tips8 { get; set; }

        /// <summary>
        /// 正在冷却提示
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string Tips9 { get; set; }

        /// <summary>
        /// 传送成功提示
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string Tips10 { get; set; }
        #endregion

        #endregion

        #region 命令
        public RelayCommand<DataGridItemChangedEventArgs> DataGridItemChanged { get; private set; }

        public RelayCommand RefreshList { get; private set; }

        public RelayCommand RemoveItem { get; private set; }

        public RelayCommand RemoveAll { get; private set; }
        #endregion

        #region 方法
        public TeleportHomeViewModel(IDispatcherService dispatcherService, string functionTag, IDialogService dialogService,
            ScoreInfoService scoreInfoService, HomePositionService homePositionService, TeleRecordService teleRecordService)
            : base(dispatcherService, functionTag)
        {
            _dialogService = dialogService;
            _scoreInfoService = scoreInfoService;
            _homePositionService = homePositionService;
            _teleRecordService = teleRecordService;

            CityPositionService.AsyncExceptionCaught += OnAsyncExceptionCaught;

            DataGridItemChanged = new RelayCommand<DataGridItemChangedEventArgs>(OnDataGridItemChanged);

            RefreshList = new RelayCommand(PrivateRefreshList);

            RemoveItem = new RelayCommand(() =>
            {
                if (_dialogService.ShowOKCancel("确定删除选中数据吗？"))
                {
                    _ = _homePositionService.RemoveAsync(SelectedItem);
                    HomePositions.Remove(SelectedItem);
                }
            }, () => { return SelectedItem != null; });

            RemoveAll = new RelayCommand(() =>
            {
                if (_dialogService.ShowOKCancel("确定删除所有数据吗？"))
                {
                    _ = _homePositionService.RemoveAllAsync();
                    HomePositions = null;
                }
            }, () => { return HomePositions != null; });

            AddAvailableVariables();

            PrivateRefreshList();
        }

        private void OnDataGridItemChanged(DataGridItemChangedEventArgs eventArgs)
        {
            if (eventArgs.IsChanged == false)
            {
                return;
            }

            if (eventArgs.NewItem is HomePositionDto newItem && eventArgs.OldItem is HomePositionDto oldItem)
            {
                if (newItem.SteamID != oldItem.SteamID)
                {
                    _dialogService.ShowInformation("无法更改SteamID");
                    newItem.SteamID = oldItem.SteamID;// 此时新值还未更新至ui
                }
                else
                {
                    _ = _homePositionService.UpdateAsync(newItem);
                }                
            }
        }

        private void OnAsyncExceptionCaught(object sender, ServiceException e)
        {
            ExceptionHandleHelper.ShowServiceException(_dialogService, e);
        }

        private async void PrivateRefreshList()
        {
            var result = await _homePositionService.GetAllAsync("SteamID,HomeName ASC");

            HomePositions = result == null ? null : new ObservableCollection<HomePositionDto>(result);
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
                "[Home名称]",
                "[三维坐标]",
                "[获取Home命令]",
                "[设置Home命令]",
                "[最大设置数量]",
                "[设置需要积分]",
                "[私人回家命令]",
                "[传送需要积分]",
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
            message = message.Replace("[获取Home命令]", QueryListCmd)
                            .Replace("[设置Home命令]", SetHomeCmdPrefix)
                            .Replace("[最大设置数量]", MaxCanSetCount.ToString())
                            .Replace("[设置需要积分]", GetSetHomeNeedScore().ToString())
                            .Replace("[传送需要积分]", TeleNeedScore.ToString());
            if (otherParam is HomePositionDto homePositionDto)
            {
                return base.FormatCmd(playerInfo, message)
                    .Replace("[Home名称]", homePositionDto.HomeName)
                    .Replace("[三维坐标]", homePositionDto.Pos)
                    .Replace("[私人回家命令]", TeleHomeCmdPrefix + " " + homePositionDto.HomeName);
            }
            else if (otherParam is int timeSpan)
            {
                return base.FormatCmd(playerInfo, message)
                    .Replace("[剩余冷却]", timeSpan.ToString());
            }
            return base.FormatCmd(playerInfo, message);
        }

        private int GetSetHomeNeedScore()
        {
            int setHomeNeedScore = this.SetHomeNeedScore;
            switch (ComboBoxSelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    setHomeNeedScore = (_currentPlayerOwnedHomeCount + 1) * setHomeNeedScore;
                    break;
                case 2:
                    if (_currentPlayerOwnedHomeCount > MaxCanSetCount / 2)
                    {
                        setHomeNeedScore *= 2;
                    }
                    break;
            }
            return setHomeNeedScore;
        }

        private void QueryList(PlayerInfo playerInfo)
        {
            List<HomePositionDto> dtos = _homePositionService.GetAll("SteamID,HomeName ASC");
            _currentPlayerOwnedHomeCount = dtos.Count;
            if (_currentPlayerOwnedHomeCount == 0)
            {
                SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, FormatCmd(playerInfo, Tips1));
            }
            else
            {
                SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, "[00FF00]Home：");

                StringBuilder returnCmd = new StringBuilder();
                int index = 1;
                foreach (var item in dtos)
                {
                    returnCmd.Append(string.Format("pm {0} \"[00FF00]{1}{2}\"\r\n", playerInfo.SteamID, index, FormatCmd(playerInfo, Tips2, item)));
                    ++index;
                }
                SdtdConsole.Instance.SendCmd(returnCmd.ToString());
            }
        }

        protected override bool OnPlayerChatHooked(PlayerInfo playerInfo, string message)
        {
            if (message == QueryListCmd)
            {
                QueryList(playerInfo);
            }
            else if(message.StartsWith(SetHomeCmdPrefix) && message.Length > SetHomeCmdPrefix.Length)
            {
                string homeName = message.Substring(SetHomeCmdPrefix.Length + 1);

                if (string.IsNullOrEmpty(homeName))
                {
                    SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, "Home名称不能为空");
                    return true;
                }

                List<HomePositionDto> dtos = _homePositionService.GetDataBySteamID(playerInfo.SteamID);
                _currentPlayerOwnedHomeCount = dtos.Count;

                if (_currentPlayerOwnedHomeCount >= MaxCanSetCount)
                {
                    SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, FormatCmd(playerInfo, Tips3));
                }
                else
                {
                    int playerScore = _scoreInfoService.GetPlayerScore(playerInfo.SteamID);
                    if (playerScore < GetSetHomeNeedScore())// 设置需要积分不足提示
                    {
                        SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, FormatCmd(playerInfo, Tips4));
                    }
                    else
                    {
                        var dto = dtos.Find(p => p.HomeName == homeName);
                        if (dto == null)// 首次设置Home成功提示
                        {
                            dto = new HomePositionDto()
                            {
                                HomeName = homeName,
                                PlayerName = playerInfo.PlayerName,
                                SteamID = playerInfo.SteamID,
                                Pos = playerInfo.Pos
                            };
                            _homePositionService.Insert(dto);

                            SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, FormatCmd(playerInfo, Tips6, dto));
                        }
                        else// Home名称已存在覆盖提示
                        {
                            dto.HomeName = homeName;
                            dto.PlayerName = playerInfo.PlayerName;
                            dto.SteamID = playerInfo.SteamID;
                            dto.Pos = playerInfo.Pos;
                            _homePositionService.Update(dto);
                            SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, FormatCmd(playerInfo, Tips5, dto));
                        }

                        Log.Info(string.Format("玩家: {0} SteamID: {1} 设置了Home: {2} 三维坐标: {3}",
                            playerInfo.PlayerName, playerInfo.SteamID, homeName, playerInfo.Pos));
                    }
                }
            }
            else if (message.StartsWith(TeleHomeCmdPrefix) && message.Length > TeleHomeCmdPrefix.Length)
            {
                string homeName = message.Substring(TeleHomeCmdPrefix.Length + 1);
                HomePositionDto dto = _homePositionService.GetDataBySteamIDAndHomeName(playerInfo.SteamID, homeName);
                if(dto == null)
                {
                    SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, FormatCmd(playerInfo, Tips8));
                }
                else
                {
                    var teleRecord = _teleRecordService.GetDataByID(playerInfo.SteamID);

                    if (teleRecord != null && string.IsNullOrEmpty(teleRecord.LastTeleDateTime) == false)
                    {
                        int timeSpan = (int)(DateTime.Now - DateTime.Parse(teleRecord.LastTeleDateTime)).TotalSeconds;
                        if (timeSpan < TeleInterval)// 正在冷却
                        {
                            SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, FormatCmd(playerInfo, Tips9, TeleInterval - timeSpan));
                            return true;
                        }
                    }

                    int playerScore = _scoreInfoService.GetPlayerScore(playerInfo.SteamID);
                    if (playerScore < TeleNeedScore)// 传送需要积分不足提示
                    {
                        SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, FormatCmd(playerInfo, Tips7));
                    }
                    else
                    {
                        _scoreInfoService.DeductPlayerScore(playerInfo.SteamID, TeleNeedScore);

                        SdtdConsole.Instance.TelePlayer(playerInfo.SteamID, dto.Pos);

                        SdtdConsole.Instance.SendGlobalMessage(FormatCmd(playerInfo, Tips10, dto));

                        // 记录传送日期
                        var tempDto = new TeleRecordDto() { SteamID = playerInfo.SteamID, LastTeleDateTime = DateTime.Now.ToString() };
                        if (teleRecord == null)
                        {
                            _teleRecordService.Insert(tempDto);
                        }
                        else
                        {
                            _teleRecordService.Update(tempDto);
                        }

                        Log.Info(string.Format("玩家: {0} SteamID: {1} 传送到了: {2}", playerInfo.PlayerName, playerInfo.SteamID, dto.HomeName));
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
