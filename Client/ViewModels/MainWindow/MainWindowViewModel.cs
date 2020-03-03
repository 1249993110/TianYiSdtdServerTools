using IceCoffee.Common.LogManager;
using IceCoffee.Wpf.MvvmFrame.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Chat;
using TianYiSdtdServerTools.Client.Models.ObservableClasses;
using TianYiSdtdServerTools.Client.Services.Primitives.UI;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.MainWindow
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IRichTextBoxService _outputLogService;

        public ObservableCollection<ListViewItemModel> ControlPanelItems { get; set; }

        public ObservableCollection<ListViewItemModel> FunctionPanelItems { get; set; }

        public MainWindowViewModel(IDispatcherService dispatcherService, IRichTextBoxService richTextBoxService) : base(dispatcherService)
        {
            this._outputLogService = richTextBoxService;

            Log.LogRecorded += OnLogRecorded;

            ControlPanelItems = new ObservableCollection<ListViewItemModel>()
            {
                new ListViewItemModel("配置信息", "ConfigInfo"),
                new ListViewItemModel("在线玩家", "OnlinePlayer"),
                new ListViewItemModel("聊天信息", "ChatMessage"),
                new ListViewItemModel("Telnet控制台", "TelnetConsole"),
                new ListViewItemModel("历史玩家", "HistoryPlayer"),
                new ListViewItemModel("权限管理", "PermissionManagement"),
            };

            FunctionPanelItems = new ObservableCollection<ListViewItemModel>()
            {
                new ListViewItemModel("游戏公告", "GameNotice"),
                //new ListViewItemModel("积分系统", "ScoreSystem"),                
            };
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
