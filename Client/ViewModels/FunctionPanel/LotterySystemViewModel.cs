using IceCoffee.Common;
using IceCoffee.LogManager;
using IceCoffee.Common.Xml;

using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    public class LotterySystemViewModel : FunctionViewModelBase
    {
        #region 字段
        private PropertyObserver<LotterySystemViewModel> _currentViewModelObserver;

        private readonly IDialogService _dialogService;

        private readonly ScoreInfoService _scoreInfoService;

        private readonly LotteryService _lotteryService;

        private bool _isLotterying;

        /// <summary>
        /// 当前抽奖参与者
        /// </summary>
        private readonly List<PlayerInfo> _currentParticipant = new List<PlayerInfo>();
        #endregion

        #region 属性
        private Timer Timer1 { get; set; }

        private Timer Timer2 { get; set; }

        public ObservableCollection<LotteryDto> LotteryItems { get; [NPCA_Method]set; }

        public LotteryDto SelectedItem { get; [NPCA_Method]set; }

        public string LotteryName { get; set; }

        public string Content { get; set; }

        public int Amount { get; set; }

        public int Quality { get; set; }

        public string LotteryType { get; set; }
        
        [ConfigNode(XmlNodeType.Attribute)]
        public bool GiveItemBlockToBackpack { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public bool RandomCmd { get; set; } = true;

        [ConfigNode(XmlNodeType.Attribute)]
        public string LotteryCmd { get; [NPCA_Method]set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public int LotteryInterval { get; [NPCA_Method]set; } = 300;

        [ConfigNode(XmlNodeType.Attribute)]
        public int LotteryDuration { get; set; } = 30;

        [ConfigNode(XmlNodeType.Attribute)]
        public int MaxWinnerCount { get; set; } = 1;

        [ConfigNode(XmlNodeType.Attribute)]
        public string StartLotteryTips { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public string EndLotteryTips { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public string WinningTips { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public string NoWinningTips { get; set; }
        #endregion

        #region 命令
        public RelayCommand<DataGridItemChangedEventArgs> DataGridItemChanged { get; private set; }

        public RelayCommand RefreshList { get; private set; }

        public RelayCommand RemoveItem { get; private set; }

        public RelayCommand RemoveAll { get; private set; }

        public RelayCommand AddData { get; private set; }
        #endregion

        public LotterySystemViewModel(IDispatcherService dispatcherService, string functionTag,
            IDialogService dialogService, ScoreInfoService scoreInfoService, LotteryService lotteryService) : base(dispatcherService, functionTag)
        {
            _dialogService = dialogService;
            _scoreInfoService = scoreInfoService;
            _lotteryService = lotteryService;

            Timer1 = new Timer() { AutoReset = true, Interval = LotteryInterval * 1000 };
            Timer1.Elapsed += OnStartLottery;

            _currentViewModelObserver = new PropertyObserver<LotterySystemViewModel>(this);
            _currentViewModelObserver.RegisterHandler(currentViewModel => currentViewModel.LotteryInterval,
                (vm) =>
                {
                    if (LotteryInterval > 0)
                    {
                        Timer1.Interval = LotteryInterval * 1000;
                        Timer1.Enabled = true;
                    }
                });

            DataGridItemChanged = new RelayCommand<DataGridItemChangedEventArgs>(OnDataGridItemChanged);

            RefreshList = new RelayCommand(PrivateRefreshList);

            RemoveItem = new RelayCommand(() =>
            {
                if (_dialogService.ShowOKCancel("确定删除选中数据吗？"))
                {
                    _ = _lotteryService.RemoveAsync(SelectedItem);
                    LotteryItems.Remove(SelectedItem);
                }
            }, () => { return SelectedItem != null; });

            RemoveAll = new RelayCommand(() =>
            {
                if (_dialogService.ShowOKCancel("确定删除所有数据吗？"))
                {
                    _ = _lotteryService.RemoveAllAsync();
                    LotteryItems = null;
                }
            }, () => { return LotteryItems != null; });

            AddData = new RelayCommand(() =>
            {
                var dto = new LotteryDto()
                {
                    LotteryName = LotteryName,
                    Content = Content,
                    Amount = Amount,
                    Quality = Quality,
                    LotteryType = LotteryType
                };
                _ = _lotteryService.AddAsync(dto);
                LotteryItems.Add(dto);
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

            if (eventArgs.NewItem is LotteryDto newItem && eventArgs.OldItem is LotteryDto oldItem)
            {
                _ = _lotteryService.UpdateAsync(newItem);
            }
        }

        private async void PrivateRefreshList()
        {
            var result = await _lotteryService.GetAllAsync("LotteryName ASC");

            LotteryItems = result == null ? null : new ObservableCollection<LotteryDto>(result);
        }

        protected override void DisableFunction()
        {
            Timer1.Stop();
            base.DisableFunction();
        }

        protected override void EnableFunction()
        {       
            Timer1.Start();            
        }

        private void OnStartLottery(object sender, ElapsedEventArgs e)
        {
            if(_lotteryService.GetRecordCount() < 1)
            {
                return;
            }

            _isLotterying = true;
            base.EnableFunction();

            _currentParticipant.Clear();

            if(RandomCmd)
            {
                LotteryCmd = CommonHelper.GetRandomString(3, true, true);
            }            

            SdtdConsole.Instance.SendGlobalMessage(FormatCmd(null, StartLotteryTips));

            if(Timer2 == null)
            {
                Timer2 = new Timer() { AutoReset = false, Interval = LotteryDuration * 1000 };
                Timer2.Elapsed += OnEndLottery;

                _currentViewModelObserver = new PropertyObserver<LotterySystemViewModel>(this);
                _currentViewModelObserver.RegisterHandler(currentViewModel => currentViewModel.LotteryDuration,
                    (vm) =>
                    {
                        if (LotteryDuration > 0)
                        {
                            Timer2.Interval = LotteryDuration * 1000;
                        }
                    });
            }
            Timer2.Start();
        }

        private void OnEndLottery(object sender, ElapsedEventArgs e)
        {
            _isLotterying = false;
            base.DisableFunction();            

            Random Random = new Random();

            List<PlayerInfo> winners = new List<PlayerInfo>();

            int i = 0;
            int maxWinnerCount = _currentParticipant.Count > MaxWinnerCount ? MaxWinnerCount : _currentParticipant.Count;


            if (maxWinnerCount <= 0)
            {
                SdtdConsole.Instance.SendGlobalMessage(FormatCmd(null, EndLotteryTips).Replace("[中奖人员]", "无"));
                return;
            }

            while (i < maxWinnerCount)
            {
                int index = Random.Next(_currentParticipant.Count);
                if (winners.Contains(_currentParticipant[index]) == false)
                {
                    winners.Add(_currentParticipant[index]);
                    i++;
                }
            }

            List<string> winnersName = new List<string>();
            foreach (var playerInfo in winners)
            {
                winnersName.Add(playerInfo.PlayerName);
            }
            SdtdConsole.Instance.SendGlobalMessage(FormatCmd(null, EndLotteryTips).Replace("[中奖人员]", string.Join(",", winnersName.ToArray())));            

            foreach (var playerInfo in winners)
            {
                var lotteryDtos = _lotteryService.GetAll();
                if (lotteryDtos.Count == 0)
                {
                    SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, "暂无奖品，请等待管理员添加");
                }
                int index = Random.Next(lotteryDtos.Count);

                var lotteryDto = lotteryDtos[index];

                string cmdPrefix = "give";

                switch (lotteryDto.LotteryType)
                {
                    case "物品":
                        if (GiveItemBlockToBackpack)
                        {
                            cmdPrefix = "gi";
                        }
                        SdtdConsole.Instance.SendCmd(string.Format("{0} {1} {2} {3} {4}",
                            cmdPrefix, playerInfo.EntityID, lotteryDto.Content, lotteryDto.Amount, lotteryDto.Quality));
                        break;
                    case "方块":
                        if (GiveItemBlockToBackpack)
                        {
                            cmdPrefix = "gb";
                        }
                        SdtdConsole.Instance.SendCmd(string.Format("{0} {1} {2} {3}",
                            cmdPrefix, playerInfo.EntityID, lotteryDto.Content, lotteryDto.Amount));
                        break;
                    case "实体":
                        for (i = 0; i < lotteryDto.Amount; ++i)
                        {
                            SdtdConsole.Instance.SendCmd(string.Format("se {0} {1}",
                                playerInfo.EntityID, lotteryDto.Content));
                        }
                        break;
                    case "指令":
                        for (i = 0; i < lotteryDto.Amount; ++i)
                        {
                            SdtdConsole.Instance.SendCmd(FormatCmd(playerInfo, lotteryDto.Content, lotteryDto));
                        }
                        break;
                    case "积分":
                        _scoreInfoService.IncreasePlayerScore(playerInfo.SteamID, lotteryDto.Amount);
                        break;
                    default:
                        throw new Exception("无效商品类型");
                }

                SdtdConsole.Instance.SendGlobalMessage(FormatCmd(playerInfo, WinningTips, lotteryDto));

                // 记录购买
                Log.Info(string.Format("玩家: {0} SteamID: {1} 抽到了: {2}", playerInfo.PlayerName, playerInfo.SteamID, lotteryDto.LotteryName));
            }
        }

        private void AddAvailableVariables()
        {
            AvailableVariables.AddRange(new List<string>()
            {
                "[抽奖命令]",
                "[抽奖间隔]",
                "[持续时间]",
                "[奖品名称]",
                "[数量]",
                "[品质]",
                "[奖品类型]",
                "[中奖人员]"
            });
        }
        protected override string FormatCmd(PlayerInfo playerInfo, string message, object otherParam = null)
        {
            message = message.Replace("[抽奖命令]", LotteryCmd)
                            .Replace("[抽奖间隔]", LotteryInterval.ToString())
                            .Replace("[持续时间]", LotteryDuration.ToString());
            if (otherParam is LotteryDto lotteryDto)
            {
                return base.FormatCmd(playerInfo, message)
                    .Replace("[奖品名称]", lotteryDto.LotteryName)
                    .Replace("[数量]", lotteryDto.Amount.ToString())
                    .Replace("[品质]", lotteryDto.Quality.ToString())
                    .Replace("[奖品类型]", lotteryDto.LotteryType);
            }
            return base.FormatCmd(playerInfo, message);
        }

        protected override bool OnPlayerChatHooked(PlayerInfo playerInfo, string message)
        {
            if(_isLotterying == false)
            {
                return false;
            }
            else
            {
                if(message == LotteryCmd)
                {
                    _currentParticipant.Add(playerInfo);                    
                    return true;
                }
                return false;
            }
        }
    }
}
