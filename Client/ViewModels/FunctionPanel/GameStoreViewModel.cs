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
using System.Threading.Tasks;
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
    public class GameStoreViewModel : FunctionViewModelBase
    {
        #region 字段
        private readonly IDialogService _dialogService;

        private readonly ScoreInfoService _scoreInfoService;

        private readonly GoodsService _goodsService;
        #endregion

        #region 属性
        public ObservableCollection<GoodsDto> GoodsItems { get; [NPCA_Method]set; }

        public GoodsDto SelectedItem { get; [NPCA_Method]set; }

        public string GoodsName { get; set; }

        public string BuyCmd { get; set; }

        public string Content { get; set; }

        public int Amount { get; set; }

        public int Quality { get; set; }

        public int Price { get; set; }

        public string GoodsType { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public bool GiveItemBlockToBackpack { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public string QueryListCmd { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public string QueryListTips { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public string BuySucceedTips { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public string BuyFailTips { get; set; }
        #endregion

        #region 命令
        public RelayCommand<DataGridItemChangedEventArgs> DataGridItemChanged { get; private set; }

        public RelayCommand RefreshList { get; private set; }

        public RelayCommand RemoveItem { get; private set; }

        public RelayCommand RemoveAll { get; private set; }

        public RelayCommand AddData { get; private set; }
        #endregion

        public GameStoreViewModel(IDispatcherService dispatcherService, string functionTag,
            IDialogService dialogService, ScoreInfoService scoreInfoService,GoodsService goodsService) : base(dispatcherService, functionTag)
        {
            _dialogService = dialogService;
            _scoreInfoService = scoreInfoService;
            _goodsService = goodsService;

            GoodsService.AsyncExceptionCaught += OnAsyncExceptionCaught;

            DataGridItemChanged = new RelayCommand<DataGridItemChangedEventArgs>(OnDataGridItemChanged);

            RefreshList = new RelayCommand(PrivateRefreshList);

            RemoveItem = new RelayCommand(() =>
            {
                if (_dialogService.ShowOKCancel("确定删除选中数据吗？"))
                {
                    _ = _goodsService.RemoveAsync(SelectedItem);
                    GoodsItems.Remove(SelectedItem);
                }
            }, () => { return SelectedItem != null; });

            RemoveAll = new RelayCommand(() =>
            {
                if (_dialogService.ShowOKCancel("确定删除所有数据吗？"))
                {
                    _ = _goodsService.RemoveAllAsync();
                    GoodsItems = null;
                }
            }, () => { return GoodsItems != null; });

            AddData = new RelayCommand(() =>
            {
                if (string.IsNullOrEmpty(BuyCmd))
                {
                    _dialogService.ShowInformation("购买命令不能为空");
                    return;
                }
                if (GoodsItems.FirstOrDefault(p => p.BuyCmd == BuyCmd) != null)
                {
                    _dialogService.ShowInformation("购买命令重复");
                    return;
                }
                
                var dto = new GoodsDto()
                {
                    GoodsName = GoodsName,
                    BuyCmd = BuyCmd,
                    Content = Content,
                    Amount = Amount,
                    Quality = Quality,
                    Price = Price,
                    GoodsType = GoodsType
                };
                _ = _goodsService.InsertAsync(dto);
                GoodsItems.Add(dto);
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

            if (eventArgs.NewItem is GoodsDto newItem && eventArgs.OldItem is GoodsDto oldItem)
            {
                if (GoodsItems.FirstOrDefault(p => p.BuyCmd == newItem.BuyCmd && object.Equals(p, newItem) == false) != null)
                {
                    _dialogService.ShowInformation("购买命令重复");
                    newItem.BuyCmd = oldItem.BuyCmd;// 此时新值还未更新至ui
                }
                else
                {
                    _ = _goodsService.UpdateAsync(newItem);
                }
            }
        }

        private void OnAsyncExceptionCaught(object sender, ServiceException e)
        {
            ExceptionHandleHelper.ShowServiceException(_dialogService, e);
        }

        private async void PrivateRefreshList()
        {
            var result = await _goodsService.GetAllAsync("Price ASC");

            GoodsItems = result == null ? null : new ObservableCollection<GoodsDto>(result);
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
                "[商品名称]",
                "[购买命令]",
                "[数量]",
                "[品质]",
                "[售价]",
                "[商品类型]"
            });
        }
        protected override string FormatCmd(PlayerInfo playerInfo, string message, object otherParam = null)
        {
            if (otherParam is GoodsDto goodsDto)
            {
                return base.FormatCmd(playerInfo, message)
                    .Replace("[商品名称]", goodsDto.GoodsName)
                    .Replace("[购买命令]", goodsDto.BuyCmd)
                    .Replace("[数量]", goodsDto.Amount.ToString())
                    .Replace("[品质]", goodsDto.Quality.ToString())
                    .Replace("[售价]", goodsDto.Price.ToString())
                    .Replace("[商品类型]", goodsDto.GoodsType);
            }
            return base.FormatCmd(playerInfo, message);
        }

        protected override bool OnPlayerChatHooked(PlayerInfo playerInfo, string message)
        {
            if (message == QueryListCmd)
            {
                List<GoodsDto> dtos = _goodsService.GetAll("Price ASC");

                if (dtos.Count == 0)
                {
                    SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, "[00FF00]暂无商品信息");
                }
                else
                {
                    SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, "[00FF00]商品列表：");

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
                GoodsDto goodsDto = _goodsService.GetDataByID(message);
                if (goodsDto == null)
                {
                    return false;
                }
                else
                {
                    int playerScore = _scoreInfoService.GetPlayerScore(playerInfo.SteamID);
                    if (playerScore < goodsDto.Price)// 积分不足
                    {
                        SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, FormatCmd(playerInfo, BuyFailTips, goodsDto));
                    }
                    else
                    {
                        _scoreInfoService.DeductPlayerScore(playerInfo.SteamID, goodsDto.Price);

                        string cmdPrefix = "give";                        

                        switch(goodsDto.GoodsType)
                        {
                            case "物品":
                                if(GiveItemBlockToBackpack)
                                {
                                    cmdPrefix = "gi";
                                }
                                SdtdConsole.Instance.SendCmd(string.Format("{0} {1} {2} {3} {4}",
                                    cmdPrefix, playerInfo.EntityID, goodsDto.Content, goodsDto.Amount, goodsDto.Quality));
                                break;
                            case "方块":
                                if (GiveItemBlockToBackpack)
                                {
                                    cmdPrefix = "gb";
                                }
                                SdtdConsole.Instance.SendCmd(string.Format("{0} {1} {2} {3}",
                                    cmdPrefix, playerInfo.EntityID, goodsDto.Content, goodsDto.Amount));
                                break;
                            case "实体":
                                for (int i = 0; i < goodsDto.Amount; ++i)
                                {
                                    SdtdConsole.Instance.SendCmd(string.Format("se {0} {1}",
                                        playerInfo.EntityID, goodsDto.Content));
                                }
                                break;
                            case "指令":
                                for (int i = 0; i < goodsDto.Amount; ++i)
                                {
                                    SdtdConsole.Instance.SendCmd(FormatCmd(playerInfo, goodsDto.Content, goodsDto));
                                }
                                break;
                            default:
                                throw new Exception("无效商品类型");
                        }
                       
                        SdtdConsole.Instance.SendGlobalMessage(FormatCmd(playerInfo, BuySucceedTips, goodsDto));

                        // 记录购买
                        Log.Info(string.Format("玩家: {0} SteamID: {1} 购买了: {2}", playerInfo.PlayerName, playerInfo.SteamID, goodsDto.GoodsName));
                    }
                }
            }
            return true;
        }
    }
}
