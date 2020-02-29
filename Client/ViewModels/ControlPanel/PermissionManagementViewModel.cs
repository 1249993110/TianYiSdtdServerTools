using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using IceCoffee.Wpf.MvvmFrame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.ConsoleTempList;
using TianYiSdtdServerTools.Client.Services.Primitives.UI;
using TianYiSdtdServerTools.Client.TelnetClient;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.ControlPanel
{
    public class PermissionManagementViewModel : ViewModelBase
    {
        public List<Administrator> Administrators { get; [NPCA_Method]set; }

        public List<CommandLevel> CommandLevels { get; [NPCA_Method]set; }

        public string SteamID { get; set; }

        public int PermissionLevel1 { get; set; } = -1;

        public string Command { get; set; }

        public int PermissionLevel2 { get; set; } = -1;

        /// <summary>
        /// 管理员列表当前选中索引
        /// </summary>
        public int SelectedIndex1 { get; set; } = -1;

        /// <summary>
        /// 命令权限列表当前选中索引
        /// </summary>
        public int SelectedIndex2 { get; set; } = -1;


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

        public RelayCommand AddCommandLevel { get; private set; }

        public RelayCommand RemoveCommandLevel { get; private set; }

        public RelayCommand RefreshList { get; private set; }

        public RelayCommand ClearList { get; private set; }

        

        public PermissionManagementViewModel(IDispatcherService dispatcherService) : base(dispatcherService)
        {           
            AddAdministrator = new RelayCommand(()=> 
            {
                SdtdConsole.Instance.AddAdministrator(SteamID, PermissionLevel1);
                PrivateRefreshList();
            });

            RemoveAdministrator = new RelayCommand(() =>
            {
                SdtdConsole.Instance.RemoveAdministrator(Administrators[SelectedIndex1].SteamID);
                PrivateRefreshList();
            }, CanAddAdministrator);

            AddCommandLevel = new RelayCommand(() =>
            {
                SdtdConsole.Instance.AddCommandPermissionLevel(Command, PermissionLevel2);
                PrivateRefreshList();
            });

            RemoveCommandLevel = new RelayCommand(() =>
            {
                SdtdConsole.Instance.RemoveCommandPermissionLevel(CommandLevels[SelectedIndex2].Command);
                PrivateRefreshList();
            }, CanAddCommandLevel);

            RefreshList = new RelayCommand(PrivateRefreshList);

            ClearList = new RelayCommand(() => 
            {
                Administrators = null;
                CommandLevels = null;
            });
        }

        private void PrivateRefreshList()
        {
            SdtdConsole.Instance.SendCmd("admin list" + Environment.NewLine + "0");
            SdtdConsole.Instance.SendCmd("cp list" + Environment.NewLine + "0");
        }

        private bool CanAddAdministrator()
        {
            return SelectedIndex1 != -1;
        }

        private bool CanAddCommandLevel()
        {
            return SelectedIndex2 != -1;
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
