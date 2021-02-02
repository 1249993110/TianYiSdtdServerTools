using IceCoffee.Common;
using IceCoffee.LogManager;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using TianYiSdtdServerTools.Client.Models;
using TianYiSdtdServerTools.Client.Models.ObservableObjects;
using TianYiSdtdServerTools.Client.Models.PlayeInventory;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.ViewModels.Managers;

namespace TianYiSdtdServerTools.Client.ViewModels.Windows
{
    public class PlayerInventoryViewModel : ObservableObject
    {
        private readonly IDispatcherService _dispatcherService;
        private readonly IDialogService _dialogService;

        public ObservableCollection<PlayerInventoryData> BagItems { get; [NPCA_Method]set; } = new ObservableCollection<PlayerInventoryData>();

        public ObservableCollection<PlayerInventoryData> BeltItems { get; [NPCA_Method]set; } = new ObservableCollection<PlayerInventoryData>();

        public ObservableCollection<PlayerInventoryData> EquipmentItems { get; [NPCA_Method]set; } = new ObservableCollection<PlayerInventoryData>();

        public PlayerInventoryData SelectedItem { get; [NPCA_Method]set; }

        public string Title { get; set; }

        public string SteamID { get; set; }

        public PlayerInventoryViewModel(IDispatcherService dispatcherService, IDialogService dialogService, string steamID)
        {
            _dispatcherService = dispatcherService;
            _dialogService = dialogService;
            SteamID = steamID;

            Title = string.Format("玩家: {0} 的背包", SteamID);

            _ = GetPlayerInventory();
        }

        private async Task GetPlayerInventory()
        {
            try
            {
                string url = string.Format("http://{0}:{1}/api/getplayerinventory?steamid={2}&adminuser={3}&admintoken={4}",
                    SdtdServerInfoManager.Instance.ServerIP,
                    SdtdServerInfoManager.Instance.GPSPort,
                    SteamID,
                    SdtdServerInfoManager.Instance.WebUserToken.AdminUser,
                    SdtdServerInfoManager.Instance.WebUserToken.AdminToken);

                string json = await HttpHelper.Instance.GetStringAsync(url);

                PlayerInventoryInfo playerInventoryInfos = JsonConvert.DeserializeObject<PlayerInventoryInfo>(json);

                string itemIconBaseUrl = string.Format("http://{0}:{1}/itemicons/",
                    SdtdServerInfoManager.Instance.ServerIP,
                    SdtdServerInfoManager.Instance.GPSPort);

                LoadImage(BagItems, playerInventoryInfos.bag, itemIconBaseUrl);

                LoadImage(BeltItems, playerInventoryInfos.belt, itemIconBaseUrl);

                LoadImage(EquipmentItems, new List<ItemData>()
                    {
                        playerInventoryInfos.equipment.head,
                        playerInventoryInfos.equipment.eyes,
                        playerInventoryInfos.equipment.face,
                        playerInventoryInfos.equipment.armor,
                        playerInventoryInfos.equipment.jacket,
                        playerInventoryInfos.equipment.shirt,
                        playerInventoryInfos.equipment.legarmor,
                        playerInventoryInfos.equipment.pants,
                        playerInventoryInfos.equipment.boots,
                        playerInventoryInfos.equipment.gloves

                    }, itemIconBaseUrl);
            }
            catch (Exception ex)
            {
                string message = "获取玩家背包数据失败";
                Log.Error(ex, message);
                _dialogService.ShowInformation(ex.Message, message);
            }
        }

        private void LoadImage(ObservableCollection<PlayerInventoryData> imageDatas, IList<ItemData> itemDatas, string itemIconBaseUrl)
        {
            foreach (var item in itemDatas)
            {
                if (item != null)
                {
                    string iconUrl = string.Format("{0}{1}__{2}.png", itemIconBaseUrl, item.icon, item.iconcolor);

                    SdtdLocalizationManager.Instance.LocalizationDict.TryGetValue(item.name, out string chinese);
                    
                    PlayerInventoryData playerInventoryData = new PlayerInventoryData()
                    {
                        Source = new Uri(iconUrl, UriKind.Absolute),
                        //HexColor = null,
                        ToolTip = string.Format("{0}{1}{2}{3}数量: {4}{5}品质: {6}", 
                                    item.name, Environment.NewLine, chinese, Environment.NewLine,
                                    item.count, Environment.NewLine, item.quality == -1 ? "无" : item.quality.ToString()),
                        English = item.name,
                        Chinese = chinese,
                        Count = item.count,
                        QualityColor = item.qualitycolor
                    };

                    if(item.qualitycolor != null)
                    {
                        playerInventoryData.Count = null;
                    }

                    imageDatas.Add(playerInventoryData);
                }
            }
        }
    }
}
