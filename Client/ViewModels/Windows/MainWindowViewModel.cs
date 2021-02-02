using IceCoffee.LogManager;
using IceCoffee.Wpf.MvvmFrame.Messaging;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TianYiSdtdServerTools.Shared.Models.NetDataObjects;
using TianYiSdtdServerTools.Client.Models.Chat;
using TianYiSdtdServerTools.Client.Models.ObservableObjects;
using TianYiSdtdServerTools.Client.MyClient;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.ViewModels.Managers;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;
using TianYiSdtdServerTools.Shared.Models;
using TianYiSdtdServerTools.Client.Models.MvvmMessages;

namespace TianYiSdtdServerTools.Client.ViewModels.Windows
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IRichTextBoxService _outputLogService;

        public List<ListViewItemModel> ControlPanelItems { get; set; }

        public List<FunctionPanelViewItemModel> FunctionPanelItems { get; set; }

        public Uri HeaderIconUri { get; [NPCA_Method]set; }

        public string HeaderIconToolTip { get; [NPCA_Method]set; }

        public MainWindowViewModel(IDispatcherService dispatcherService, IRichTextBoxService richTextBoxService, IDialogService dialogService)
            : base(dispatcherService)
        {
            this._outputLogService = richTextBoxService;

            Log.LogRecorded += OnLogRecorded;

            ControlPanelItems = ViewItemManager.Instance.ControlPanelItems;

            FunctionPanelItems = ViewItemManager.Instance.FunctionPanelItems;

            OnReceivedUserInfo(new UserInfo() 
            { 
                DisplayName = "洛水天依",
                RoleName = "永久会员",
                ExpiryTime = DateTime.MinValue,
            });

            // OnReceivedUserInfo(MyClientManager.Instance.UserInfo);
            // Messenger.Default.Register<MyTcpClientMessage>(this, HandeMyTcpClientMessage);
        }

        private void HandeMyTcpClientMessage(MyTcpClientMessage msg)
        {
            switch (msg.MessageType)
            {
                case MyTcpClientMessageType.ReceivedUserInfo:
                    OnReceivedUserInfo(msg.Content as UserInfo);
                    break;
            }
        }


        private void OnReceivedUserInfo(UserInfo clientInfo)
        {
            HeaderIconUri = HeaderIconUri ?? new Uri("pack://application:,,,/Resources;component/Assets/Images/vip.png", UriKind.Absolute);
            HeaderIconToolTip = string.Format("用户: {0}\r\n会员等级: {1}\r\n有效期至: {2}",
                clientInfo.DisplayName, clientInfo.RoleName, clientInfo.ExpiryTime);
        }

        private void OnLogRecorded(string message, Exception exception, LogLevel logLevel)
        {
            List<RichText> richTexts = new List<RichText>();

            string color = null;

            switch (logLevel)
            {
                case LogLevel.Trace:
                    break;
                case LogLevel.Debug:
                    break;
                case LogLevel.Info:
                    break;
                case LogLevel.Warn:
                    color = "#FFD700";
                    break;
                case LogLevel.Error:
                    color = "#FF0000";
                    break;
                case LogLevel.Fatal:
                    color = "#FF0000";
                    break;
            }

            richTexts.Add(new RichText(message, color));

            if (exception != null)
            {
                richTexts.Add(new RichText(" "));
                richTexts.Add(new RichText(exception.Message, color));

                exception = exception.InnerException;
                if (exception != null)
                {
                    richTexts.Add(new RichText(" "));
                    richTexts.Add(new RichText(exception.Message, color));
                }
            }

            _outputLogService.AppendBlock(richTexts);
        }
    }
}
