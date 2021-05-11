using IceCoffee.Common;
using IceCoffee.Common.Extensions;
using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.Messaging;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.MvvmMessages;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.ViewModels.Managers;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.Windows
{
    public class LoginWindowViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        private string _passwordHash;

        public string UserID { get; [NPCA_Method]set; }

        public string PasswordHash 
        { 
            get => _passwordHash;
            [NPCA_Method]
            set => _passwordHash = value.ToBase64(); 
        }
        
        public RelayCommand Login { get; private set; }
        public RelayCommand Register { get; private set; }

        public LoginWindowViewModel(IDispatcherService dispatcherService, IDialogService dialogService) : base(dispatcherService)
        {
            _dialogService = dialogService;

            Login = new RelayCommand(()=> 
            {
                // MyClientManager.Instance.TryLogin(UserID, PasswordHash);
            });

            Register = new RelayCommand(() =>
            {
                string displayName = dialogService.ShowInputDialog("请输入您的昵称：");

                if(displayName != null)
                {
                    // MyClientManager.Instance.RegisterAccount(UserID, PasswordHash, displayName);
                }
            });
        }
    }
}
