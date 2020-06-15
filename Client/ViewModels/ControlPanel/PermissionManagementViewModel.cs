using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.ConsoleTempList;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.TelnetClient;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.ControlPanel
{
    public class PermissionManagementViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        public List<Administrator> Administrators { get; [NPCA_Method]set; }

        public List<CommandLevel> CommandLevels { get; [NPCA_Method]set; }

        public string SteamID { get; set; }

        public int PermissionLevel1 { get; set; } = -1;

        public string Command { get; set; }

        public int PermissionLevel2 { get; set; } = -1;

        /// <summary>
        /// 管理员列表当前选中索引
        /// </summary>
        public Administrator SelectedItem1 { get; set; }

        /// <summary>
        /// 命令权限列表当前选中索引
        /// </summary>
        public CommandLevel SelectedItem2 { get; set; }


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

        public RelayCommand AddAdministrator { get; private set; }

        public RelayCommand RemoveAdministrator { get; private set; }

        public RelayCommand RemoveAllAdministrator { get; private set; }

        public RelayCommand AddCommandLevel { get; private set; }

        public RelayCommand RemoveCommandLevel { get; private set; }

        public RelayCommand RefreshList { get; private set; }

        public RelayCommand ClearList { get; private set; }

        

        public PermissionManagementViewModel(IDispatcherService dispatcherService, IDialogService dialogService) : base(dispatcherService)
        {
            _dialogService = dialogService;

            AddAdministrator = new RelayCommand(()=> 
            {
                SdtdConsole.Instance.AddAdministrator(SteamID, PermissionLevel1);
                PrivateRefreshList();
            });
            
            RemoveAdministrator = new RelayCommand(() =>
            {
                SdtdConsole.Instance.RemoveAdministrator(SelectedItem1.SteamID);
                PrivateRefreshList();
            }, CanRemoveAdministrator);

            RemoveAllAdministrator = new RelayCommand(() =>
            {
                if(dialogService.ShowOKCancel("确定移除所有管理员吗？"))
                {
                    foreach (var item in Administrators)
                    {
                        SdtdConsole.Instance.RemoveAdministrator(item.SteamID);
                    }
                    PrivateRefreshList();
                }
            }, CanRemoveAdministrator);

            AddCommandLevel = new RelayCommand(() =>
            {
                SdtdConsole.Instance.AddCommandPermissionLevel(Command, PermissionLevel2);
                PrivateRefreshList();
            });

            RemoveCommandLevel = new RelayCommand(() =>
            {
                SdtdConsole.Instance.RemoveCommandPermissionLevel(SelectedItem2.Command);
                PrivateRefreshList();
            }, CanRemoveCommandLevel);

            RefreshList = new RelayCommand(PrivateRefreshList);

            ClearList = new RelayCommand(() => 
            {
                Administrators = null;
                CommandLevels = null;
            });
        }

        private void PrivateRefreshList()
        {
            SdtdConsole.Instance.SendCmd("admin list" + Environment.NewLine + SdtdConsole.CmdPlaceholder);
            SdtdConsole.Instance.SendCmd("cp list" + Environment.NewLine + SdtdConsole.CmdPlaceholder);
        }

        private bool CanRemoveAdministrator()
        {
            return SelectedItem1 != null;
        }

        private bool CanRemoveCommandLevel()
        {
            return SelectedItem2 != null;
        }

        private void OnReceivedTempListData(object twoDimensionalList, TempListDataType tempListDataType)
        {
            if (tempListDataType == TempListDataType.AdminList && twoDimensionalList is List<Administrator>)
            {
                Administrators = (List<Administrator>)twoDimensionalList;
            }
            else if (tempListDataType == TempListDataType.PermissionList && twoDimensionalList is List<CommandLevel>)
            {
                CommandLevels = (List<CommandLevel>)twoDimensionalList;
            }
        }
    }
}
