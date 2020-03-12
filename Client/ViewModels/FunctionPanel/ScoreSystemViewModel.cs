using IceCoffee.Common;
using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using IceCoffee.Wpf.MvvmFrame.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

        public List<ScoreInfoModel> ScoreInfos { get; [NPCA_Method]set; }

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
                    SelectedItem = ScoreInfos.Find(p => p.PlayerName == SearchText);
                }
                else if (ComboBoxSelectedIndex == 1)
                {
                    SelectedItem = ScoreInfos.Find(p => p.SteamID == SearchText);
                }

                if (SelectedItem == null)
                {
                    _dialogService.ShowInformation("没有找到此玩家");
                }                
            },()=> { return ScoreInfos != null; });            

            DataGridItemChanged = new RelayCommand<DataGridItemChangedEventArgs>(OnDataGridItemChanged);

            RemoveItem = new RelayCommand(()=>
            {
                if (dialogService.ShowOKCancel("确定删除选中数据吗？"))
                {
                    _scoreService.RemoveItem(SelectedItem as ScoreInfoModel);
                    ScoreInfos = _scoreService.GetAllScoreInfo();
                }
            }, CanExecuteCommand);

            ResetLastSignDate = new RelayCommand(() =>
            {
                if (dialogService.ShowOKCancel("确定重置所有签到天数吗？"))
                {
                    _scoreService.ResetLastSignDate();
                    ScoreInfos = _scoreService.GetAllScoreInfo();
                }
            }, CanExecuteCommand);

            RemoveAllScoreInfo = new RelayCommand(() =>
            {
                if (dialogService.ShowOKCancel("确定删除所有数据吗？"))
                {
                    _scoreService.RemoveAll();
                    ScoreInfos = _scoreService.GetAllScoreInfo();
                }
            }, CanExecuteCommand);

            PrivateRefreshList();
        }

        private bool CanExecuteCommand()
        {
            return SelectedIndex != -1 && SelectedItem != null;
        }

        private void OnDataGridItemChanged(DataGridItemChangedEventArgs eventArgs)
        {
            if(eventArgs.IsChanged == false)
            {
                return;
            }

            if (eventArgs.NewItem is ScoreInfoModel newItem)
            {
                if (eventArgs.IsNewItem)
                {
                    if (string.IsNullOrEmpty(newItem.SteamID) == false)
                    {
                        if (ScoreInfos.Find(p => p.SteamID == newItem.SteamID && object.ReferenceEquals(p, newItem) == false) != null)
                        {
                            _dialogService.ShowInformation("无法添加重复的SteamID");
                            newItem.SteamID = string.Empty;// 此时新值还未更新至ui
                        }
                        else
                        {
                            Task.Run(() =>
                            {
                                _scoreService.InsertScoreInfo(newItem);
                            });
                        }
                    }
                }
                else if (eventArgs.OldItem is ScoreInfoModel oldItem)
                {
                    if (newItem.SteamID != oldItem.SteamID)
                    {
                        _dialogService.ShowInformation("无法更改SteamID\r\n请删除此条记录以添加新记录");
                        newItem.SteamID = oldItem.SteamID;// 此时新值还未更新至ui
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            _scoreService.UpdateScoreInfo(newItem);                            
                        });
                    }
                }
            }
        }

        private void OnExceptionCaught(object sender, Services.CatchException.DALException e)
        {
            StringBuilder messageBuilder = new StringBuilder();
            Exception exception = e.InnerException;
            while (exception != null)
            {
                messageBuilder.Append(exception.Message + Environment.NewLine);
                exception = exception.InnerException;
            }

            _dispatcherService.Invoke(() => 
            {
                _dialogService.ShowInformation(messageBuilder.ToString(), e.Message);
            });            

            PrivateRefreshList();
        }

        private void PrivateRefreshList()
        {
            ScoreInfos = _scoreService.GetAllScoreInfo();
        }

        protected override void DisableFunction()
        {
            
        }

        protected override void EnableFunction()
        {
            
        }
    }
}
