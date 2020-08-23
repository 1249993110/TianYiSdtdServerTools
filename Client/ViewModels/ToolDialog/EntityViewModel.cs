using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.ConsoleTempList;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.TelnetClient;
using TianYiSdtdServerTools.Client.ViewModels.Managers;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.ToolDialog
{
    public class EntityViewModel : ObservableObject
    {
        private readonly PropertyObserver<EntityViewModel> _currentPropertyObserver;

        private readonly IDialogService _dialogService;

        public List<AvailableEntity> AvailableEntitys { get; [NPCA_Method]set; }

        public string SearchText { get; [NPCA_Method]set; }

        public RelayCommand Search { get; private set; }

        public RelayCommand RefreshList { get; private set; }

        public EntityViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            Search = new RelayCommand(PrivateSearch);

            RefreshList = new RelayCommand(()=> 
            {
                if(SdtdConsole.Instance.IsConnected == false)
                {
                    _dialogService.ShowInformation("请先连接服务器");
                }
                else
                {
                    RequestData();
                }
            });

            _currentPropertyObserver = new PropertyObserver<EntityViewModel>(this);
            _currentPropertyObserver.RegisterHandler(p => p.SearchText, (vm) => { vm.PrivateSearch(); });

            SdtdConsole.Instance.ReceivedTempListData += OnReceivedTempListData;

            Task.Run(() => 
            {
                Thread.Sleep(100);
                RequestData();
            });
        }

        private void RequestData()
        {
            SdtdConsole.Instance.SendCmd("se" + Environment.NewLine + SdtdConsole.CmdPlaceholder);
        }

        private void OnReceivedTempListData(object twoDimensionalList, TempListDataType tempListDataType)
        {
            if(tempListDataType == TempListDataType.AvailableEntityList && twoDimensionalList is List<AvailableEntity> availableEntitys)
            {
                foreach (var item in availableEntitys)
                {
                    item.Chinese = SdtdLocalizationManager.Instance.GetTranslation(item.English);
                }
                AvailableEntitys = availableEntitys;
            }
        }

        private void PrivateSearch()
        {
            if (AvailableEntitys != null)
            {
                if (string.IsNullOrEmpty(SearchText))
                {
                    foreach (var item in AvailableEntitys)
                    {
                        item.Visible = true;
                    }
                }
                else
                {
                    foreach (var item in AvailableEntitys)
                    {
                        if (item.Chinese.Contains(SearchText) || item.English.Contains(SearchText))
                        {
                            item.Visible = true;
                        }
                        else
                        {
                            item.Visible = false;
                        }
                    }
                }
            }
        }

        public override void Clear()
        {
            SdtdConsole.Instance.ReceivedTempListData -= OnReceivedTempListData;
        }

    }
}
