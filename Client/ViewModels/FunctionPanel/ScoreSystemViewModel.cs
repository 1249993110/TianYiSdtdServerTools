using IceCoffee.Common;
using IceCoffee.Common.LogManager;
using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using IceCoffee.Wpf.MvvmFrame.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using TianYiSdtdServerTools.Client.Models.Dtos;
using TianYiSdtdServerTools.Client.Models.EventArgs;
using TianYiSdtdServerTools.Client.Services;
using TianYiSdtdServerTools.Client.Services.Primitives.UI;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.FunctionPanel
{
    public class ScoreSystemViewModel : FunctionViewModelBase
    {
        private ScoreService _scoreService;

        private IDialogService _dialogService;

        public ObservableCollection<ScoreInfoDto> ScoreInfos { get; [NPCA_Method]set; }

        public int SelectedIndex { get; set; } = -1;

        public object SelectedItem { get; [NPCA_Method]set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public int ComboBoxSelectedIndex { get; [NPCA_Method]set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public string SearchText { get; set; }

        #region 命令

        public RelayCommand RefreshList { get; private set; }

        public RelayCommand SearchPlayer { get; private set; }

        public RelayCommand<DataGridItemChangedEventArgs> DataGridItemChanged { get; set; }

        public RelayCommand RemoveItem { get; set; }

        public RelayCommand ResetLastSignDate { get; set; }

        public RelayCommand RemoveAllScoreInfo { get; set; }

        #endregion

        public ScoreSystemViewModel(IDispatcherService dispatcherService, IDialogService dialogService, ScoreService scoreService, string functionTag) 
            : base(dispatcherService, functionTag)
        {
            _dialogService = dialogService;

            _scoreService = scoreService;

            ScoreService.ExceptionCaught += OnExceptionCaught;

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
            }, CanExecuteCmd_ScoreInfosNotNull);            

            DataGridItemChanged = new RelayCommand<DataGridItemChangedEventArgs>(OnDataGridItemChanged);

            RemoveItem = new RelayCommand(() =>
            {
                if (dialogService.ShowOKCancel("确定删除选中数据吗？"))
                {
                    var scoreInfo = SelectedItem as ScoreInfoDto;
                    ScoreInfos.Remove(scoreInfo);
                    _ = _scoreService.RemoveItem(scoreInfo);
                }
            }, () => { return SelectedIndex != -1 && SelectedItem != null; });

            ResetLastSignDate = new RelayCommand(() =>
            {
                if (dialogService.ShowOKCancel("确定重置所有签到天数吗？"))
                {
                    foreach (var item in ScoreInfos)
                    {
                        item.LastSignDate = 0;
                    }
                    _ = _scoreService.ResetLastSignDate();
                }
            }, CanExecuteCmd_ScoreInfosNotNull);

            RemoveAllScoreInfo = new RelayCommand(() =>
            {
                if (dialogService.ShowOKCancel("确定删除所有数据吗？"))
                {
                    ScoreInfos = null;
                    _ = _scoreService.RemoveAll();
                }
            }, CanExecuteCmd_ScoreInfosNotNull);

            PrivateRefreshList();
        }

        private bool CanExecuteCmd_ScoreInfosNotNull()
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
                    _dialogService.ShowInformation("无法更改SteamID\r\n请删除此条记录以添加新记录");
                    newItem.SteamID = oldItem.SteamID;// 此时新值还未更新至ui
                }
                else
                {
                    _ = _scoreService.UpdateScoreInfo(newItem);
                }
                
            }
        }

        private void OnExceptionCaught(object sender, Services.CatchException.ServiceException e)
        {
            StringBuilder messageBuilder = new StringBuilder();
            Exception exception = e.InnerException;
            while (exception != null)
            {
                messageBuilder.Append(exception.Message + Environment.NewLine);
                exception = exception.InnerException;
            }

            _dispatcherService.InvokeAsync(() => 
            {
                _dialogService.ShowInformation(messageBuilder.ToString(), e.Message);
            });

            PrivateRefreshList();
        }

        private async void PrivateRefreshList()
        {
            try
            {
                var scoreInfos = await _scoreService.GetAllScoreInfo();

                ScoreInfos = new ObservableCollection<ScoreInfoDto>(scoreInfos);
            }
            catch (Exception e)
            {
                string msg = "读取全部积分信息异常";
                Log.Error(msg, e);
                _dispatcherService.InvokeAsync(() =>
                {
                    _dialogService.ShowInformation(e.Message, msg);
                });
            }
        }

        protected override void DisableFunction()
        {
            
        }

        protected override void EnableFunction()
        {
            
        }
    }
}
