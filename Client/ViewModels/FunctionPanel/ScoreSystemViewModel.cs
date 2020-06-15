using IceCoffee.Common.LogManager;
using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using IceCoffee.Common.Xml;
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
using IceCoffee.DbCore.CatchServiceException;

namespace TianYiSdtdServerTools.Client.ViewModels.FunctionPanel
{
    public class ScoreSystemViewModel : FunctionViewModelBase
    {
        #region 字段
        private readonly ScoreInfoService _scoreInfoService;

        private readonly IDialogService _dialogService;
        #endregion

        #region 属性

        public ObservableCollection<ScoreInfoDto> ScoreInfos { get; [NPCA_Method]set; }

        public ScoreInfoDto SelectedItem { get; [NPCA_Method]set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public int ComboBoxSelectedIndex { get; [NPCA_Method]set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public string SearchText { get; set; }

        /// <summary>
        /// 每日签到命令
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string SignCmd { get; set; }

        /// <summary>
        /// 签到获得积分
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public int GainScore { get; set; }

        /// <summary>
        /// 查询积分信息
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string QueryScore { get; set; }

        /// <summary>
        /// 签到成功提示
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string SignSucceedTips { get; set; }

        /// <summary>
        /// 签到失败提示
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string SignFailTips { get; set; }

        /// <summary>
        /// 查询积分提示
        /// </summary>
        [ConfigNode(XmlNodeType.Attribute)]
        public string QueryScoreTips { get; set; }
        #endregion

        #region 命令

        public RelayCommand RefreshList { get; private set; }

        public RelayCommand SearchPlayer { get; private set; }

        public RelayCommand<DataGridItemChangedEventArgs> DataGridItemChanged { get; private set; }

        public RelayCommand RemoveItem { get; private set; }

        public RelayCommand ResetLastSignDate { get; private set; }

        public RelayCommand RemoveAllScoreInfo { get; private set; }

        #endregion

        public ScoreSystemViewModel(IDispatcherService dispatcherService, string functionTag, IDialogService dialogService, ScoreInfoService scoreInfoService) 
            : base(dispatcherService, functionTag)
        {
            _dialogService = dialogService;

            _scoreInfoService = scoreInfoService;

            ScoreInfoService.AsyncExceptionCaught += OnAsyncExceptionCaught;

            DataGridItemChanged = new RelayCommand<DataGridItemChangedEventArgs>(OnDataGridItemChanged);

            RefreshList = new RelayCommand(PrivateRefreshList);     
            
            SearchPlayer = new RelayCommand(() =>
            {
                if (ComboBoxSelectedIndex == 0)
                {
                    SelectedItem = ScoreInfos.FirstOrDefault(p => p.PlayerName == SearchText);
                }
                else if (ComboBoxSelectedIndex == 1)
                {
                    SelectedItem = ScoreInfos.FirstOrDefault(p => p.SteamID == SearchText);
                }

                if (SelectedItem == null)
                {
                    _dialogService.ShowInformation("没有找到此玩家");
                }                
            }, CanExecuteCmd_ListNotNull);          

            RemoveItem = new RelayCommand(() =>
            {
                if (_dialogService.ShowOKCancel("确定删除选中数据吗？"))
                {                    
                    _ = _scoreInfoService.RemoveAsync(SelectedItem);
                    ScoreInfos.Remove(SelectedItem);
                }
            }, () => { return SelectedItem != null; });

            ResetLastSignDate = new RelayCommand(async () =>
            {
                if (_dialogService.ShowOKCancel("确定重置所有签到天数吗？"))
                {                    
                    await  _scoreInfoService.ResetLastSignDateAsync();
                    PrivateRefreshList();
                }
            }, CanExecuteCmd_ListNotNull);

            RemoveAllScoreInfo = new RelayCommand(() =>
            {
                if (_dialogService.ShowOKCancel("确定删除所有数据吗？"))
                {                    
                    _ = _scoreInfoService.RemoveAllAsync();
                    ScoreInfos = null;
                }
            }, CanExecuteCmd_ListNotNull);

            PrivateRefreshList();

            AddAvailableVariables();
        }

        private bool CanExecuteCmd_ListNotNull()
        {
            return ScoreInfos != null;
        }

        private void OnDataGridItemChanged(DataGridItemChangedEventArgs eventArgs)
        {
            if(eventArgs.IsChanged == false)
            {
                return;
            }

            if (eventArgs.NewItem is ScoreInfoDto newItem && eventArgs.OldItem is ScoreInfoDto oldItem)
            {
                if (newItem.SteamID != oldItem.SteamID)
                {
                    _dialogService.ShowInformation("无法更改SteamID");
                    newItem.SteamID = oldItem.SteamID;// 此时新值还未更新至ui
                }
                else
                {
                    _ = _scoreInfoService.UpdateAsync(newItem);
                }                
            }
        }

        private void OnAsyncExceptionCaught(object sender, ServiceException e)
        {
            ExceptionHandleHelper.ShowServiceException(_dialogService, e);
        }

        private async void PrivateRefreshList()
        {
            var result = await _scoreInfoService.GetAllAsync();

            ScoreInfos = result == null ? null : new ObservableCollection<ScoreInfoDto>(result);
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
                "[签到命令]",
                "[获得积分]",
                "[拥有积分]"
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
            return base.FormatCmd(playerInfo, message)
                .Replace("[签到命令]", SignCmd)
                .Replace("[获得积分]", GainScore.ToString())
                .Replace("[拥有积分]", otherParam.ToString());
        }

        protected override bool OnPlayerChatHooked(PlayerInfo playerInfo, string message)
        {
            if(message == SignCmd)
            {
                int currentDay = SdtdConsole.Instance.GameDateTime.Day;
                int lastSignDate = 0;
                ScoreInfoDto scoreInfoDto = _scoreInfoService.GetDataByID(playerInfo.SteamID);

                if (scoreInfoDto == null)// 如果无记录
                {
                    scoreInfoDto = new ScoreInfoDto()
                    {
                        PlayerName = playerInfo.PlayerName,
                        SteamID = playerInfo.SteamID,
                        ScoreOwned = GainScore,
                        LastSignDate = currentDay
                    };

                    _scoreInfoService.Insert(scoreInfoDto);                    
                }
                else
                {
                    if (scoreInfoDto.LastSignDate == currentDay)// 如果今日已签到
                    {
                        SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, FormatCmd(playerInfo, SignFailTips, scoreInfoDto.ScoreOwned));
                        return true;
                    }
                    else// 如果今日未签到
                    {
                        lastSignDate = scoreInfoDto.LastSignDate;

                        scoreInfoDto.ScoreOwned += GainScore;   // 增加积分
                        scoreInfoDto.LastSignDate = currentDay; // 更新签到天数

                        if (scoreInfoDto.PlayerName != playerInfo.PlayerName)
                        {
                            scoreInfoDto.PlayerName = playerInfo.PlayerName;
                            Log.Info("更新玩家昵称   " + playerInfo.PlayerName);
                        }

                        _scoreInfoService.Update(scoreInfoDto);
                    }
                }

                SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, FormatCmd(playerInfo, SignSucceedTips, scoreInfoDto.ScoreOwned));

                Log.Info(string.Format("玩家签到   steamID: {0} 当前服务器天数: {1} 上次签到天数: {2}", playerInfo.SteamID, currentDay, lastSignDate));
            }
            else if (message == QueryScore)
            {
                ScoreInfoDto scoreInfoDto = _scoreInfoService.GetDataByID(playerInfo.SteamID);
                SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, 
                    FormatCmd(playerInfo, QueryScoreTips, scoreInfoDto == null ? 0 : scoreInfoDto.ScoreOwned));
            }
            else
            {
                return false;
            }
            return true;
        }

    }
}
